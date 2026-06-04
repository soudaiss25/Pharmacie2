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
                    preview.Width = 700;
                    preview.Height = 950;
                    preview.Text = $"Facture — {vente.numeroVente}";
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
            float x = 40f;
            float y = 30f;
            float largeur = 500f;

            // ── Ressources graphiques ─────────────────────────────────────
            using var penLigne = new Pen(Color.FromArgb(34, 85, 34), 1.2f);
            using var penGris = new Pen(Color.FromArgb(200, 200, 200), 0.5f);
            var brushBlanc = Brushes.White;
            var brushNoir = Brushes.Black;
            using var brushVert = new SolidBrush(Color.FromArgb(34, 85, 34));
            using var brushGris = new SolidBrush(Color.FromArgb(100, 100, 100));
            using var brushLightVert = new SolidBrush(Color.FromArgb(232, 245, 233));

            using var fontTitre = new Font("Segoe UI", 18F, FontStyle.Bold);
            using var fontSousTitre = new Font("Segoe UI", 10F, FontStyle.Bold);
            using var fontGras = new Font("Segoe UI", 9F, FontStyle.Bold);
            using var fontNormal = new Font("Segoe UI", 9F);
            using var fontPetit = new Font("Segoe UI", 7.5F);
            using var fontTotal = new Font("Segoe UI", 12F, FontStyle.Bold);

            // ── En-tête Pharmacie ─────────────────────────────────────────
            g.FillRectangle(brushVert, x, y, largeur, 65);
            g.DrawString("🏥  PHARMACIE", fontTitre, brushBlanc, x + 12, y + 8);
            g.DrawString("Votre santé, notre priorité", fontPetit, brushBlanc, x + 12, y + 48);
            y += 75;

            // ── Séparateur titre ──────────────────────────────────────────
            g.DrawString("F A C T U R E", fontSousTitre, brushVert,
                x + largeur / 2 - 55, y);
            y += 22;
            g.DrawLine(penLigne, x, y, x + largeur, y);
            y += 10;

            // ── Bloc info vente / vendeur ─────────────────────────────────
            string nomVendeur = vente.User != null
                ? $"{vente.User.Prenom} {vente.User.Nom}"
                : "—";

            void LigneInfo(string label, string val, float xLeft, float yPos, Font font)
            {
                g.DrawString(label, fontGras, brushGris, xLeft, yPos);
                g.DrawString(val, font, brushNoir, xLeft + 110, yPos);
            }

            LigneInfo("N° Facture :", vente.numeroVente, x, y, fontGras);
            LigneInfo("Date :", vente.DateVente.ToString("dd/MM/yyyy  HH:mm"), x + 270, y, fontNormal);
            y += 18;
            LigneInfo("Vendeur :", nomVendeur, x, y, fontNormal);
            LigneInfo("Paiement :", vente.MoyenPaiement, x + 270, y, fontNormal);
            y += 18;

            // ── Bloc client ───────────────────────────────────────────────
            string nomClient = $"{vente.PrenomClient} {vente.NomClient}".Trim();
            if (string.IsNullOrWhiteSpace(nomClient)) nomClient = "Client anonyme";
            string tel = string.IsNullOrWhiteSpace(vente.TelephoneClient) ? "—" : vente.TelephoneClient;

            LigneInfo("Client :", nomClient, x, y, fontGras);
            LigneInfo("Téléphone :", tel, x + 270, y, fontNormal);
            y += 18;

            if (!string.IsNullOrWhiteSpace(vente.MotifAchat) && vente.MotifAchat != "null")
            {
                LigneInfo("Motif :", vente.MotifAchat, x, y, fontNormal);
                y += 18;
            }

            y += 6;
            g.DrawLine(penLigne, x, y, x + largeur, y);
            y += 10;

            // ── En-tête tableau produits ──────────────────────────────────
            g.FillRectangle(brushLightVert, x, y, largeur, 22);
            g.DrawString("Produit", fontGras, brushVert, x + 5, y + 3);
            g.DrawString("Unité", fontGras, brushVert, x + 210, y + 3);
            g.DrawString("Qté", fontGras, brushVert, x + 285, y + 3);
            g.DrawString("Prix U.", fontGras, brushVert, x + 345, y + 3);
            g.DrawString("Sous-total", fontGras, brushVert, x + 425, y + 3);
            y += 24;

            // ── Lignes produits ───────────────────────────────────────────
            bool altRow = false;
            foreach (var l in lignes)
            {
                if (altRow)
                    g.FillRectangle(
                        new SolidBrush(Color.FromArgb(248, 252, 248)),
                        x, y, largeur, 20);

                string nom = l.Produit?.Nom ?? $"Produit #{l.ProduitId}";
                if (nom.Length > 34) nom = nom.Substring(0, 32) + "…";

                g.DrawString(nom, fontNormal, brushNoir, x + 5, y + 2);
                g.DrawString(l.UniteVendue, fontNormal, brushNoir, x + 210, y + 2);
                g.DrawString(l.Quantite.ToString(), fontNormal, brushNoir, x + 293, y + 2);
                g.DrawString(l.PrixUnitaire.ToString("N0"), fontNormal, brushNoir, x + 345, y + 2);
                g.DrawString(l.SousTotal.ToString("N0"), fontGras, brushNoir, x + 430, y + 2);

                y += 22;
                altRow = !altRow;
            }

            y += 6;
            g.DrawLine(penLigne, x, y, x + largeur, y);
            y += 10;

            // ── Totaux ────────────────────────────────────────────────────
            // Total principal
            g.DrawString("TOTAL", fontTotal, brushNoir, x + 340, y);
            g.DrawString($"{vente.MontantTotal:N0} KMF", fontTotal, brushVert, x + 415, y);
            y += 26;

            // Part mutuelle si applicable
            if (vente.MontantMutuelle > 0)
            {
                string nomMut = vente.Mutuel?.NomEmployeur ?? "Mutuelle";
                decimal partPat = vente.MontantTotal - vente.MontantMutuelle;

                g.DrawString($"Part {nomMut} :", fontNormal, brushGris, x + 290, y);
                g.DrawString($"{vente.MontantMutuelle:N0} KMF", fontGras, brushGris, x + 420, y);
                y += 18;

                g.DrawString("Part patient :", fontNormal, brushNoir, x + 290, y);
                g.DrawString($"{partPat:N0} KMF", fontGras, brushNoir, x + 420, y);
                y += 18;
            }

            // Rendu
            if (vente.MontantRendu > 0)
            {
                g.DrawString("Rendu :", fontNormal, brushGris, x + 290, y);
                g.DrawString($"{vente.MontantRendu:N0} KMF", fontNormal, brushGris, x + 420, y);
                y += 18;
            }

            // Avance crédit versée (si paiement partiel)
            if (vente.Type == "Crédit" && vente.MontantVerse > 0)
            {
                g.DrawString("Avance reçue :", fontNormal, brushGris, x + 290, y);
                g.DrawString($"{vente.MontantVerse:N0} KMF", fontNormal, brushGris, x + 420, y);
                y += 18;
            }

            // Reste à payer (crédit ou impayé)
            if (vente.MontantRestant > 0)
            {
                using var brushRed = new SolidBrush(Color.OrangeRed);
                g.DrawString("Reste à payer :", fontGras, brushRed, x + 290, y);
                g.DrawString($"{vente.MontantRestant:N0} KMF", fontGras, brushRed, x + 420, y);
                y += 18;
            }
            else if (vente.Type == "Crédit")
            {
                // Crédit entièrement soldé
                using var brushGreen = new SolidBrush(Color.FromArgb(27, 94, 32));
                g.DrawString("✅ Soldé", fontGras, brushGreen, x + 370, y);
                y += 18;
            }

            y += 14;
            g.DrawLine(penLigne, x, y, x + largeur, y);
            y += 14;

            // ── Pied de page ──────────────────────────────────────────────
            g.DrawString(
                "Merci pour votre confiance.  Conservez cette facture pour tout remboursement.",
                fontPetit, brushGris,
                x + (largeur / 2) - 175, y);
            y += 14;
            g.DrawString(
                $"Document généré le {DateTime.Now:dd/MM/yyyy à HH:mm}",
                fontPetit, brushGris,
                x + (largeur / 2) - 90, y);
        }

        private void btnAnnuler_Click(object sender, EventArgs e) => this.Close();
    }
}