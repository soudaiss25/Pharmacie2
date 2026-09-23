using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.views
{
    public partial class FormVente : Form
    {
        private List<LigneVente> _lignes = new List<LigneVente>();
        private List<Mutuel> _mutuels = new List<Mutuel>();
        // Compteur initialisé depuis la base au premier accès
        private static int _compteur = -1;

        private static int GetProchainCompteur()
        {
            if (_compteur < 0)
            {
                try
                {
                    using (var ctx = new AppDbContext())
                    {
                        // Chercher le plus grand numéro V-XXXXXX en base
                        var derniere = ctx.ventes
                            .Select(v => v.numeroVente)
                            .OrderByDescending(n => n)
                            .FirstOrDefault();

                        if (derniere != null && derniere.StartsWith("V-")
                            && int.TryParse(derniere.Substring(2), out int num))
                            _compteur = num + 1;
                        else
                            _compteur = 1;
                    }
                }
                catch { _compteur = 1; }
            }
            return _compteur++;
        }

        // ID de la dernière vente sauvegardée (pour impression immédiate)
        private int _derniereVenteId = -1;

        public FormVente()
        {
            InitializeComponent();
            InitListView();
            ChargerMutuelles();
            InitComboMoyenPaiement();
            MettreAJourAffichagePaiement();
        }

        private void InitListView()
        {
            lvProduits.View = View.Details;
            lvProduits.FullRowSelect = true;
            lvProduits.Columns.Add("Produit", 160);
            lvProduits.Columns.Add("Unité", 70);
            lvProduits.Columns.Add("Qté", 50);
            lvProduits.Columns.Add("Prix U.", 90);
            lvProduits.Columns.Add("Sous-total", 90);
        }

        private void ChargerMutuelles()
        {
            using (var ctx = new AppDbContext())
                _mutuels = ctx.mutuels.ToList();
        }

        private void InitComboMoyenPaiement()
        {
            cbPaiement.Items.Clear();
            cbPaiement.Items.AddRange(new object[] {
                "Comptant", "Crédit", "Mutuelle",
                "Chèque", "Carte bancaire", "Mvolo", "Huri Money"
            });
            cbPaiement.SelectedIndex = 0;
        }

        // ── Produits ──────────────────────────────────────────────────────

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            using (var form = new FormChoixProduit())
            {
                if (form.ShowDialog() != DialogResult.OK) return;

                var produit = form.ProduitSelectionne;
                int qte = form.QuantiteSelectionnee;
                string unite = form.UniteSelectionnee;

                // ── Calcul des boîtes nécessaires ─────────────────────────
                // Ex : vendre 3 plaquettes d'une boîte de 4 plaquettes = ceil(3/4) = 1 boîte
                int boitesNecessaires;
                if (unite != "Boîte" && produit.NbUniteParBoite > 1)
                    boitesNecessaires = (int)Math.Ceiling((double)qte / produit.NbUniteParBoite);
                else
                    boitesNecessaires = qte;

                if (boitesNecessaires > produit.QuantiteEnStock)
                {
                    int unitesDispo = produit.QuantiteEnStock * produit.NbUniteParBoite;
                    MessageBox.Show(
                        $"Stock insuffisant pour « {produit.Nom} ».\n" +
                        $"Disponible : {produit.QuantiteEnStock} boîte(s)" +
                        (produit.NbUniteParBoite > 1
                            ? $" = {unitesDispo} {produit.UniteVente}(s)"
                            : ""),
                        "Stock insuffisant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Prix unitaire selon l'unité choisie
                decimal prixU = unite == "Boîte"
                    ? produit.PrixVente
                    : produit.PrixUnitaireVente;

                // Si le produit est déjà dans la liste avec la même unité → cumul
                var existant = _lignes.FirstOrDefault(l =>
                    l.ProduitId == produit.Id && l.UniteVendue == unite);

                if (existant != null)
                    existant.Quantite += qte;
                else
                    _lignes.Add(new LigneVente
                    {
                        ProduitId = produit.Id,
                        Produit = produit,
                        Quantite = qte,
                        PrixUnitaire = prixU,
                        UniteVendue = unite
                    });

                RafraichirListView();
                CalculerTotal();
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (lvProduits.SelectedItems.Count == 0) return;
            _lignes.RemoveAt(lvProduits.SelectedItems[0].Index);
            RafraichirListView();
            CalculerTotal();
        }

        private void RafraichirListView()
        {
            lvProduits.Items.Clear();
            foreach (var l in _lignes)
            {
                var item = new ListViewItem(l.Produit?.Nom ?? "");
                item.SubItems.Add(l.UniteVendue);
                item.SubItems.Add(l.Quantite.ToString());
                item.SubItems.Add(l.PrixUnitaire.ToString("0.00"));
                item.SubItems.Add(l.SousTotal.ToString("0.00"));
                lvProduits.Items.Add(item);
            }
        }

        private void CalculerTotal()
        {
            decimal total = _lignes.Sum(l => l.SousTotal);
            txtMontantTotal.Text = total.ToString("0.00");
            MettreAJourAffichagePaiement();
        }

        // ── Paiement ──────────────────────────────────────────────────────

        private void cbPaiement_SelectedIndexChanged(object sender, EventArgs e)
            => MettreAJourAffichagePaiement();

        private void MettreAJourAffichagePaiement()
        {
            decimal total = 0;
            decimal.TryParse(txtMontantTotal.Text, out total);
            string mode = cbPaiement.SelectedItem?.ToString() ?? "Comptant";

            pnlMutuelle.Visible = false;
            pnlEspeces.Visible = false;
            pnlMatricule.Visible = false;
            txtMontantVerse.Text = "0.00";

            switch (mode)
            {
                case "Comptant":
                    pnlEspeces.Visible = true;
                    lblEspeces.Text = "Espèces :";
                    txtMontantEspeces.Text = total.ToString("0.00");
                    lblMontantRendu.ForeColor = System.Drawing.Color.DarkGreen;
                    CalculerRendu();
                    break;

                case "Crédit":
                    // ✅ Avance partielle possible en crédit
                    pnlEspeces.Visible = true;
                    lblEspeces.Text = "Avance (optionnel) :";
                    txtMontantEspeces.Text = "0.00";   // 0 = crédit total
                    txtMontantVerse.Text = "0.00";
                    lblMontantRendu.Text = "Laisser 0 = crédit total  |  Saisir une avance = crédit partiel";
                    lblMontantRendu.ForeColor = System.Drawing.Color.FromArgb(25, 118, 210);
                    break;

                case "Mutuelle":
                    pnlMutuelle.Visible = true;
                    pnlEspeces.Visible = true;
                    pnlMatricule.Visible = true;
                    cbMutuelle.DataSource = null;
                    cbMutuelle.DataSource = _mutuels;
                    cbMutuelle.DisplayMember = "NomEmployeur";
                    cbMutuelle.ValueMember = "IdMutuel";
                    AppliquerTauxMutuelle();
                    break;

                case "Chèque":
                case "Carte bancaire":
                case "Mvolo":
                case "Huri Money":
                    txtMontantVerse.Text = total.ToString("0.00");
                    break;
            }
        }

        private void cbMutuelle_SelectedIndexChanged(object sender, EventArgs e)
            => AppliquerTauxMutuelle();

        private void AppliquerTauxMutuelle()
        {
            if (cbMutuelle.SelectedItem is Mutuel m)
            {
                decimal total = 0;
                decimal.TryParse(txtMontantTotal.Text, out total);

                decimal partMutuelle = Math.Round(total * m.TauxPriseEnCharge / 100m, 2);
                decimal partPatient = total - partMutuelle;

                txtTauxMutuelle.Text = m.TauxPriseEnCharge.ToString("0.00");
                lblPartMutuelle.Text = $"Pris en charge mutuelle : {partMutuelle:0.00} KMF (crédit entreprise)";
                lblRestePatient.Text = $"À payer par le patient : {partPatient:0.00} KMF";
                txtMontantEspeces.Text = partPatient.ToString("0.00");
            }
        }

        private void txtMontantEspeces_TextChanged(object sender, EventArgs e)
        {
            string mode = cbPaiement.SelectedItem?.ToString() ?? "";

            if (mode == "Mutuelle")
            {
                decimal total = 0; decimal.TryParse(txtMontantTotal.Text, out total);
                decimal taux = 0; decimal.TryParse(txtTauxMutuelle.Text, out taux);
                decimal partPatient = total - Math.Round(total * taux / 100m, 2);
                CalculerRenduMutuelle(partPatient);
            }
            else if (mode == "Crédit")
            {
                // En crédit : afficher le reste après avance
                decimal.TryParse(txtMontantTotal.Text, out decimal total);
                decimal.TryParse(txtMontantEspeces.Text, out decimal avance);
                avance = Math.Max(0, Math.Min(avance, total));
                decimal reste = total - avance;

                txtMontantVerse.Text = avance.ToString("0.00");
                lblMontantRendu.Text = reste > 0
                    ? $"Reste en crédit : {reste:N0} KMF"
                    : "✅ Payé en totalité";
                lblMontantRendu.ForeColor = reste > 0
                    ? System.Drawing.Color.OrangeRed
                    : System.Drawing.Color.ForestGreen;
            }
            else
                CalculerRendu();
        }

        private void CalculerRendu()
        {
            if (!decimal.TryParse(txtMontantTotal.Text, out decimal total)) return;
            if (!decimal.TryParse(txtMontantEspeces.Text, out decimal especes))
            { lblMontantRendu.Text = "Rendu : 0.00 KMF"; return; }
            decimal rendu = especes > total ? especes - total : 0;
            lblMontantRendu.Text = $"Rendu : {rendu:0.00} KMF";
            txtMontantVerse.Text = (especes >= total ? total : especes).ToString("0.00");
        }

        private void CalculerRenduMutuelle(decimal partPatient)
        {
            if (!decimal.TryParse(txtMontantEspeces.Text, out decimal especes))
            { lblMontantRendu.Text = "Rendu : 0.00 KMF"; return; }
            decimal rendu = especes > partPatient ? especes - partPatient : 0;
            lblMontantRendu.Text = $"Rendu patient : {rendu:0.00} KMF";
            txtMontantVerse.Text = (especes >= partPatient ? partPatient : especes).ToString("0.00");
        }

        // ── Validation ────────────────────────────────────────────────────

        private void btnValider_Click(object sender, EventArgs e)
        {
            if (_lignes.Count == 0)
            {
                MessageBox.Show("Ajoutez au moins un médicament.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mode = cbPaiement.SelectedItem?.ToString() ?? "Comptant";

            if (!decimal.TryParse(txtMontantTotal.Text, out decimal total))
            {
                MessageBox.Show("Montant total invalide.", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal especes = 0, rendu = 0, verse = 0, partMutuelle = 0;

            if (mode == "Comptant")
            {
                if (!decimal.TryParse(txtMontantEspeces.Text, out especes) || especes < total)
                {
                    MessageBox.Show("Le montant en espèces est insuffisant.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                rendu = especes - total;
                verse = total;
            }
            else if (mode == "Mutuelle")
            {
                if (cbMutuelle.SelectedItem is Mutuel m)
                {
                    partMutuelle = Math.Round(total * m.TauxPriseEnCharge / 100m, 2);
                    decimal partPatient = total - partMutuelle;

                    if (!decimal.TryParse(txtMontantEspeces.Text, out especes))
                        especes = 0;

                    if (especes < partPatient)
                    {
                        MessageBox.Show(
                            $"Espèces insuffisantes pour la part patient.\nAttendu : {partPatient:0.00} KMF",
                            "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    rendu = especes - partPatient;
                    verse = partPatient;
                }
            }
            else if (mode == "Crédit")
            {
                // Avance optionnelle — le client peut verser une partie maintenant
                decimal.TryParse(txtMontantEspeces.Text, out especes);
                especes = Math.Max(0, especes);

                if (especes > total)
                {
                    MessageBox.Show(
                        $"L'avance ({especes:N0} KMF) ne peut pas dépasser le total ({total:N0} KMF).",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                verse = especes;     // ce qui est versé maintenant
                rendu = 0;           // pas de rendu en crédit
                // MontantRestant = total - verse sera calculé automatiquement
            }
            else
            {
                // Chèque, CB, Mvolo, Huri Money → versé en totalité
                decimal.TryParse(txtMontantVerse.Text, out verse);
                especes = verse;
            }

            try
            {
                using (var ctx = new AppDbContext())
                {
                    var vente = new Vente
                    {
                        numeroVente = "V-" + GetProchainCompteur().ToString("D6"),
                        NomClient = txtNom.Text.Trim(),
                        PrenomClient = txtPrenom.Text.Trim(),
                        TelephoneClient = txtTelephone.Text.Trim(),
                        MotifAchat = txtMotif.Text.Trim(),
                        MatriculeEmploye = mode == "Mutuelle" ? txtMatricule.Text.Trim() : "N/A",
                        MoyenPaiement = mode,
                        Type = mode,
                        MotifAnnulation = "null",
                        DateAnnulation = DateTime.Now,
                        MontantTotal = total,
                        MontantEspeces = especes,
                        MontantRendu = rendu,
                        MontantMutuelle = partMutuelle,
                        // ✅ La part mutuelle est un CRÉDIT ENTREPRISE — pas encore réglée
                        MutuelleReglee = false,
                        Statut = "Active",
                        DateVente = DateTime.Now,
                        UserId = SessionUtilisateur.IdCourant
                    };

                    if (mode == "Mutuelle" && cbMutuelle.SelectedItem is Mutuel m)
                    {
                        vente.MutuelId = m.IdMutuel;
                        vente.TauxMutuelle = m.TauxPriseEnCharge;
                    }

                    ctx.ventes.Add(vente);
                    ctx.SaveChanges();

                    // ── Lignes + décrémentation stock ─────────────────────
                    foreach (var ligne in _lignes)
                    {
                        ctx.LigneVentes.Add(new LigneVente
                        {
                            VenteId = vente.IdVente,
                            ProduitId = ligne.ProduitId,
                            Quantite = ligne.Quantite,
                            PrixUnitaire = ligne.PrixUnitaire,
                            UniteVendue = ligne.UniteVendue
                        });

                        var produit = ctx.produits.Find(ligne.ProduitId);
                        if (produit != null)
                        {
                            // Stock stocké en UNITÉS DE BASE (plaquettes/comprimés/boîtes)
                            if (ligne.UniteVendue == "Boîte" && produit.NbUniteParBoite > 1)
                            {
                                // On vend des boîtes entières → enlever NbUniteParBoite par boîte vendue
                                // Ex : vendre 2 boîtes de 5 plaquettes → enlever 10 unités
                                produit.QuantiteEnStock -= ligne.Quantite * produit.NbUniteParBoite;
                            }
                            else
                            {
                                // On vend à l'unité (plaquette/comprimé) OU NbUniteParBoite = 1
                                // → enlever exactement la quantité vendue
                                // Ex : vendre 3 plaquettes → enlever 3 (pas 5 !)
                                produit.QuantiteEnStock -= ligne.Quantite;
                            }

                            // Sécurité : jamais en dessous de 0
                            if (produit.QuantiteEnStock < 0)
                                produit.QuantiteEnStock = 0;
                        }
                    }

                    // ── Paiement initial ──────────────────────────────────
                    if (verse > 0)
                    {
                        ctx.paiement.Add(new Paiement
                        {
                            NumeroPaiement = "P-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                            VenteId = vente.IdVente,
                            Montant = verse,
                            DatePaiement = DateTime.Now,
                            UserId = SessionUtilisateur.IdCourant
                        });
                    }

                    ctx.SaveChanges();
                    _derniereVenteId = vente.IdVente;
                }

                // ── Message de succès + proposition d'impression ──────────
                string msg = "✅ Vente enregistrée avec succès !";
                if (rendu > 0) msg += $"\n\n💵 Rendu au client : {rendu:0.00} KMF";
                if (mode == "Crédit" && verse > 0) msg += $"\n\n💵 Avance reçue : {verse:N0} KMF\n⚠️ Reste à payer : {(total - verse):N0} KMF";
                if (mode == "Crédit" && verse == 0) msg += $"\n\n⚠️ Vente entièrement à crédit : {total:N0} KMF à récupérer";
                if (mode == "Mutuelle") msg += $"\n\n🏢 Part entreprise (crédit) : {partMutuelle:0.00} KMF";
                msg += "\n\n🖨️ Voulez-vous imprimer la facture ?";

                var res = MessageBox.Show(msg, "Vente enregistrée",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (res == DialogResult.Yes && _derniereVenteId > 0)
                    ImprimerFacture(_derniereVenteId);

                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "\n\nDétail : " + ex.InnerException.Message;
                if (ex.InnerException?.InnerException != null)
                    msg += "\n\n" + ex.InnerException.InnerException.Message;
                MessageBox.Show(msg, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Impression Facture ────────────────────────────────────────────

        /// <summary>
        /// Charge la vente depuis la DB et affiche une prévisualisation d'impression.
        /// </summary>
        private void ImprimerFacture(int venteId)
        {
            try
            {
                Vente vente;
                List<LigneVente> lignes;

                using (var ctx = new AppDbContext())
                {
                    vente = ctx.ventes
                        .Include(v => v.Lignes).ThenInclude(l => l.Produit)
                        .Include(v => v.Paiements)   // ← INDISPENSABLE pour MontantRestant correct
                        .Include(v => v.Mutuel)
                        .Include(v => v.User)
                        .FirstOrDefault(v => v.IdVente == venteId);

                    if (vente == null) return;
                    lignes = vente.Lignes.ToList();
                }

                var pd = new PrintDocument();
                pd.DefaultPageSettings.PaperSize =
                    new PaperSize("A4", 827, 1169);   // A4 en centièmes de pouce

                pd.PrintPage += (s, ev) =>
                    DessinerFacture(ev.Graphics, vente, lignes);

                using (var preview = new PrintPreviewDialog())
                {
                    preview.Document = pd;
                    preview.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                    preview.Text = $"Facture — {vente.numeroVente}";

                    // Zoom out pour voir toute la page A4 d'un coup
                    preview.PrintPreviewControl.Zoom = 0.75;

                    preview.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'impression : " + ex.Message,
                    "Erreur impression", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Dessine la facture dans le contexte graphique GDI+ fourni par PrintDocument.
        /// </summary>
        private void DessinerFacture(Graphics g, Vente vente, List<LigneVente> lignes)
        {
            // A4 : 827 × 1169 centièmes de pouce — on utilise toute la largeur
            float marge = 40f;
            float x = marge;
            float y = marge;
            float largeur = 827f - (marge * 2);  // 747 pts utiles

            // ── Ressources ────────────────────────────────────────────────
            using var penVert = new Pen(Color.FromArgb(20, 60, 20), 1.5f);
            using var penGris = new Pen(Color.FromArgb(180, 180, 180), 0.7f);
            using var brushVert = new SolidBrush(Color.FromArgb(20, 60, 20));
            using var brushVert2 = new SolidBrush(Color.FromArgb(46, 100, 46));
            using var brushGris = new SolidBrush(Color.FromArgb(90, 90, 90));
            using var brushLG = new SolidBrush(Color.FromArgb(235, 245, 235));
            using var brushAlt = new SolidBrush(Color.FromArgb(248, 252, 248));
            using var brushRed = new SolidBrush(Color.FromArgb(183, 28, 28));
            using var brushRedL = new SolidBrush(Color.FromArgb(255, 235, 235));
            var brushNoir = Brushes.Black;
            var brushBlanc = Brushes.White;

            using var fontNom = new Font("Segoe UI", 26F, FontStyle.Bold);
            using var fontSlogan = new Font("Segoe UI", 9F, FontStyle.Italic);
            using var fontST = new Font("Segoe UI", 10F, FontStyle.Bold);
            using var fontGras = new Font("Segoe UI", 9F, FontStyle.Bold);
            using var fontNormal = new Font("Segoe UI", 9F);
            using var fontPetit = new Font("Segoe UI", 7.5F);
            using var fontTotal = new Font("Segoe UI", 13F, FontStyle.Bold);

            // ══════════════════════════════════════════════════════════════
            // 1. EN-TÊTE — bande verte pleine largeur
            // ══════════════════════════════════════════════════════════════
            g.FillRectangle(brushVert, x, y, largeur, 80f);
            g.DrawString("LIGUAPHARME", fontNom, brushBlanc, x + 16, y + 8);
            g.DrawString("Votre santé, notre priorité — Pharmacie agréée",
                fontSlogan, brushBlanc, x + 16, y + 56);
            g.DrawString($"Facture  {vente.numeroVente ?? "—"}",
                fontGras, brushBlanc, x + largeur - 220, y + 14);
            g.DrawString(vente.DateVente.ToString("dd/MM/yyyy  HH:mm"),
                fontNormal, brushBlanc, x + largeur - 220, y + 34);
            y += 96;

            // ══════════════════════════════════════════════════════════════
            // 2. BLOC INFO — deux colonnes
            // ══════════════════════════════════════════════════════════════
            float colG = x;
            float colD = x + largeur / 2 + 10;

            g.FillRectangle(brushLG, x, y, largeur, 76);

            void Info(string lbl, string val, float lx, float ly)
            {
                g.DrawString(lbl, fontGras, brushGris, lx, ly);
                g.DrawString(val, fontNormal, brushNoir, lx + 105f, ly);
            }

            string nomVendeur = vente.User != null
                ? $"{vente.User.Prenom} {vente.User.Nom}" : "—";
            string nomClient = $"{vente.PrenomClient} {vente.NomClient}".Trim();
            if (string.IsNullOrWhiteSpace(nomClient)) nomClient = "Client anonyme";
            string tel = string.IsNullOrWhiteSpace(vente.TelephoneClient)
                ? "—" : vente.TelephoneClient;
            string motif = !string.IsNullOrWhiteSpace(vente.MotifAchat)
                && vente.MotifAchat != "null" ? vente.MotifAchat : "—";

            Info("Vendeur :", nomVendeur, colG, y + 8);
            Info("Paiement :", vente.MoyenPaiement, colG, y + 28);
            Info("Motif :", motif, colG, y + 48);
            Info("Client :", nomClient, colD, y + 8);
            Info("Téléphone :", tel, colD, y + 28);
            if (!string.IsNullOrWhiteSpace(vente.MatriculeEmploye)
                && vente.MatriculeEmploye != "N/A")
                Info("Matricule :", vente.MatriculeEmploye, colD, y + 48);

            y += 84;
            g.DrawLine(penVert, x, y, x + largeur, y);
            y += 12;

            // ══════════════════════════════════════════════════════════════
            // 3. TABLEAU PRODUITS
            // ══════════════════════════════════════════════════════════════
            // Colonnes : Produit | Unité | Qté | Prix U. | Sous-total
            float[] cx = { x + 5, x + 360, x + 450, x + 530, x + 630 };
            float rh = 24f;

            // En-tête
            g.FillRectangle(brushVert2, x, y, largeur, rh);
            string[] ent = { "Produit", "Unité", "Qté", "Prix unitaire", "Sous-total" };
            for (int i = 0; i < ent.Length; i++)
                g.DrawString(ent[i], fontGras, brushBlanc, cx[i], y + 4);
            y += rh + 2;

            // Lignes produits
            bool alt = false;
            foreach (var l in lignes)
            {
                // Hauteur variable : +18 si posologie présente
                bool hasPoso = l.Produit != null &&
                    (!string.IsNullOrWhiteSpace(l.Produit.Indication)
                  || !string.IsNullOrWhiteSpace(l.Produit.Posologie)
                  || l.Produit.NbFoisParJour > 0);

                float ligneH = hasPoso ? rh + 18f : rh;

                if (alt) g.FillRectangle(brushAlt, x, y, largeur, ligneH);

                string nom = l.Produit?.Nom ?? $"#{l.ProduitId}";
                if (nom.Length > 52) nom = nom.Substring(0, 50) + "…";

                g.DrawString(nom, fontNormal, brushNoir, cx[0], y + 3);
                g.DrawString(l.UniteVendue, fontNormal, brushNoir, cx[1], y + 3);
                g.DrawString(l.Quantite.ToString(), fontNormal, brushNoir, cx[2], y + 3);
                g.DrawString($"{l.PrixUnitaire:N0} KMF", fontNormal, brushNoir, cx[3], y + 3);
                g.DrawString($"{l.SousTotal:N0} KMF", fontGras, brushNoir, cx[4], y + 3);

                // Posologie sous le nom du médicament
                if (hasPoso)
                {
                    var parts = new System.Collections.Generic.List<string>();
                    if (!string.IsNullOrWhiteSpace(l.Produit.Indication))
                        parts.Add(l.Produit.Indication);
                    if (!string.IsNullOrWhiteSpace(l.Produit.Posologie))
                        parts.Add(l.Produit.Posologie);
                    if (l.Produit.NbFoisParJour > 0)
                        parts.Add($"{l.Produit.NbFoisParJour}x/jour");

                    string poso = "   → " + string.Join("  |  ", parts);
                    using var fontPoso = new Font("Segoe UI", 7.5F, FontStyle.Italic);
                    using var brushPoso = new SolidBrush(Color.FromArgb(60, 100, 60));
                    g.DrawString(poso, fontPoso, brushPoso, cx[0], y + rh - 2);
                }

                y += ligneH;
                alt = !alt;
            }

            y += 8;
            g.DrawLine(penVert, x, y, x + largeur, y);
            y += 14;

            // ══════════════════════════════════════════════════════════════
            // 4. TOTAUX
            // ══════════════════════════════════════════════════════════════
            float xLbl = x + largeur - 340;
            float xVal = x + largeur - 110;

            void LigneT(string lbl, string val, Font fl, Font fv,
                        System.Drawing.Brush cl, System.Drawing.Brush cv)
            {
                g.DrawString(lbl, fl, cl, xLbl, y);
                g.DrawString(val, fv, cv, xVal, y);
                y += 22f;
            }

            // Total principal
            g.FillRectangle(brushLG, x + largeur - 360, y - 4, 360, 32);
            g.DrawString("TOTAL", fontTotal, brushNoir, xLbl, y);
            g.DrawString($"{vente.MontantTotal:N0} KMF", fontTotal, brushVert2, xVal, y);
            y += 36;

            // Mutuelle
            if (vente.MontantMutuelle > 0)
            {
                string nomMut = vente.Mutuel?.NomEmployeur ?? "Mutuelle";
                decimal partPat = vente.MontantTotal - vente.MontantMutuelle;
                LigneT($"Part {nomMut} :", $"{vente.MontantMutuelle:N0} KMF",
                    fontNormal, fontGras, brushGris, brushGris);
                LigneT("Part patient :", $"{partPat:N0} KMF",
                    fontNormal, fontGras, brushNoir, brushNoir);
            }

            // Avance crédit
            if (vente.Type == "Crédit" && vente.MontantVerse > 0)
                LigneT("Avance reçue :", $"{vente.MontantVerse:N0} KMF",
                    fontNormal, fontGras, brushGris, brushGris);

            // Reste à payer
            if (vente.MontantRestant > 0)
            {
                g.FillRectangle(brushRedL, x + largeur - 360, y - 2, 360, 28);
                LigneT("RESTE À PAYER :", $"{vente.MontantRestant:N0} KMF",
                    fontGras, fontGras, brushRed, brushRed);
            }
            else if (vente.Type == "Crédit")
            {
                g.DrawString("✅  Soldé intégralement", fontGras, brushVert2, xLbl, y);
                y += 22;
            }

            // ══════════════════════════════════════════════════════════════
            // 5. PIED DE PAGE — bande verte en bas de page
            // ══════════════════════════════════════════════════════════════
            float yPied = 1169f - marge - 36f;
            g.FillRectangle(brushVert, x, yPied, largeur, 36f);
            g.DrawString(
                "Merci pour votre confiance.   Conservez cette facture pour tout remboursement.",
                fontPetit, brushBlanc,
                x + largeur / 2 - 230, yPied + 6);
            g.DrawString(
                $"Document généré le {DateTime.Now:dd/MM/yyyy à HH:mm}  —  LIGUAPHARME",
                fontPetit, brushBlanc,
                x + largeur / 2 - 175, yPied + 20);
        }

        private void btnAnnuler_Click(object sender, EventArgs e) => this.Close();
    }
}