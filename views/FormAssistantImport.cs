using System.Drawing;
using Pharmacie2.Services;

namespace Pharmacie2.views
{
    /// <summary>
    /// Assistant d'import d'une ancienne base, en 4 étapes, annulable à chacune : 1) fichier choisi (avant d'ouvrir l'assistant),
    /// 2) contrôles bloquants, 3) mise à jour de la copie et comparaison avant / après, 4) sauvegarde de la base actuelle puis remplacement.
    /// Le fichier d'origine n'est jamais modifié ; rien n'est remplacé avant la dernière étape.
    /// </summary>
    public class FormAssistantImport : Form
    {
        public enum Etape { Demarrage, Controles, Comparaison, Remplacement, Termine, Bloque }

        private readonly string _source;
        private readonly string? _phrase;
        private ImportBaseService? _service;
        private readonly Label _titre = new() { Name = "lblTitre", AutoSize = true, Tag = "titre", Margin = new Padding(0, 0, 0, 4) };
        private readonly Label _consigne = new() { Name = "lblConsigne", AutoSize = true, MaximumSize = new Size(820, 0), Margin = new Padding(0, 0, 0, 8) };
        private readonly DataGridView _grille = new();
        private readonly Button _btnSuivant = new();
        private readonly Button _btnAnnuler = new();
        private readonly Button _btnRapport = new();
        private readonly Label _avertissement = new() { Name = "lblAvertissement", AutoSize = true, MaximumSize = new Size(820, 0), Visible = false, Margin = new Padding(0, 6, 0, 2), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
        private readonly CheckBox _chkComprends = new() { Name = "chkComprends", Text = "Je comprends", AutoSize = true, Visible = false, Margin = new Padding(0, 2, 0, 2) };
        private string? _sauvegardePrevue;

        public Etape Courante { get; private set; } = Etape.Demarrage;
        public ResultatImport Resultat { get; } = new();

        public FormAssistantImport(string source, string? phrase)
        {
            _source = source;
            _phrase = phrase;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 10F);
            Text = AppInfo.Titre("Importer une ancienne base");
            ClientSize = new Size(880, 520);
            MinimumSize = new Size(700, 420);
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;

            var racine = new TableLayoutPanel { Name = "tlpRoot", Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5, Padding = new Padding(16) };
            racine.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            racine.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            racine.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            racine.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            racine.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            racine.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            racine.Controls.Add(_titre, 0, 0);
            racine.Controls.Add(_consigne, 0, 1);

            _grille.Name = "dgvEtapes";
            _grille.Dock = DockStyle.Fill;
            _grille.AutoGenerateColumns = false;
            _grille.ReadOnly = true;
            _grille.AllowUserToAddRows = false;
            _grille.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grille.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nom", HeaderText = "Contrôle", DataPropertyName = "Nom", FillWeight = 28, MinimumWidth = 160, ReadOnly = true });
            _grille.Columns.Add(new DataGridViewTextBoxColumn { Name = "Resultat", HeaderText = "Résultat", DataPropertyName = "Resultat", FillWeight = 12, MinimumWidth = 90, ReadOnly = true });
            _grille.Columns.Add(new DataGridViewTextBoxColumn { Name = "Detail", HeaderText = "Détail", DataPropertyName = "Detail", FillWeight = 60, MinimumWidth = 220, ReadOnly = true });
            racine.Controls.Add(_grille, 0, 2);

            // Étape 4 : avertissement rouge (base actuelle plus récente) et case « Je comprends » obligatoire avant « Importer »
            var avertissements = new FlowLayoutPanel { Name = "flpAvertissement", Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, FlowDirection = FlowDirection.TopDown, WrapContents = false, Margin = new Padding(0) };
            avertissements.Controls.Add(_avertissement);
            avertissements.Controls.Add(_chkComprends);
            racine.Controls.Add(avertissements, 0, 3);
            _chkComprends.CheckedChanged += (s, e) => { if (Courante == Etape.Remplacement) _btnSuivant.Enabled = !_chkComprends.Visible || _chkComprends.Checked; };

            var bas = new FlowLayoutPanel
            {
                Name = "flpBoutons", Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.RightToLeft, WrapContents = false, Margin = new Padding(0, 8, 0, 0)
            };
            _btnAnnuler.Name = "btnAnnuler"; _btnAnnuler.Text = "Annuler"; _btnAnnuler.AutoSize = true; _btnAnnuler.Margin = new Padding(8, 0, 0, 0);
            _btnAnnuler.Click += (s, e) => { DialogResult = Courante == Etape.Termine ? DialogResult.OK : DialogResult.Cancel; Close(); };
            _btnSuivant.Name = "btnSuivant"; _btnSuivant.Text = "Continuer"; _btnSuivant.Tag = "primaire"; _btnSuivant.AutoSize = true; _btnSuivant.Margin = new Padding(8, 0, 0, 0);
            _btnSuivant.Click += (s, e) => Suivant();
            _btnRapport.Name = "btnRapport"; _btnRapport.Text = "Préparer un rapport pour le développeur"; _btnRapport.AutoSize = true; _btnRapport.Visible = false; _btnRapport.Margin = new Padding(0);
            _btnRapport.Click += (s, e) => PreparerRapport();
            bas.Controls.Add(_btnAnnuler);
            bas.Controls.Add(_btnSuivant);
            bas.Controls.Add(_btnRapport);
            racine.Controls.Add(bas, 0, 4);

            Controls.Add(racine);
            CancelButton = _btnAnnuler;
            Theme.Appliquer(this);
            AfficherEtape("Étape 1 sur 4 : fichier choisi", "Fichier : " + source, new List<Controle>());
            Shown += (s, e) => Demarrer();
            FormClosed += (s, e) => { _service?.Dispose(); _service = null; };
        }

        // ───────────── étapes ─────────────

        /// <summary>Étape 2 : copie de travail et contrôles bloquants.</summary>
        public void Demarrer()
        {
            if (Courante != Etape.Demarrage) return;
            try
            {
                _service = new ImportBaseService(_source, _phrase);
            }
            catch (PhraseSecreteIncorrecteException)
            {
                Bloquer("Étape 2 sur 4 : contrôles", "La phrase secrète est incorrecte (ou le fichier est abîmé).", new Controle("Ouverture du fichier", false, "Phrase secrète incorrecte."));
                return;
            }
            catch (Exception ex)
            {
                Journal.Erreur("Import : ouverture du fichier", ex);
                Bloquer("Étape 2 sur 4 : contrôles", "Le fichier n'a pas pu être ouvert.", new Controle("Ouverture du fichier", false, ex.Message));
                return;
            }

            var controles = _service.Controler();
            Resultat.Controles.AddRange(controles);
            if (controles.Any(c => c.Bloquant && !c.Ok))
            {
                Bloquer("Étape 2 sur 4 : contrôles", "Un contrôle bloquant a échoué : l'import est refusé. Rien n'a été modifié.", controles.ToArray());
                return;
            }
            Courante = Etape.Controles;
            AfficherEtape("Étape 2 sur 4 : contrôles", "Tous les contrôles sont réussis. Le fichier d'origine n'a pas été modifié : le travail se fait sur une copie. Vous pouvez encore annuler.", controles);
        }

        /// <summary>Bouton « Continuer » : étape suivante.</summary>
        public void Suivant()
        {
            switch (Courante)
            {
                case Etape.Controles: MettreAJourEtComparer(); break;
                case Etape.Comparaison: PreparerRemplacement(); break;
                case Etape.Remplacement: if (_btnSuivant.Enabled) Remplacer(); break;   // grisé tant que « Je comprends » n'est pas coché
                case Etape.Termine: DialogResult = DialogResult.OK; Close(); break;
            }
        }

        /// <summary>Étape 3 : mise à jour de la copie, administrateur actif, comparaison avant / après (blocage si une ligne disparaît).</summary>
        private void MettreAJourEtComparer()
        {
            var lignes = new List<Controle>();
            var migration = _service!.Migrer();
            lignes.Add(migration);
            Resultat.Controles.Add(migration);
            if (!migration.Ok) { Bloquer("Étape 3 sur 4 : mise à jour et comparaison", "La copie n'a pas pu être mise à jour : import refusé. Rien n'a été modifié.", lignes.ToArray()); return; }

            var admin = _service.ControlerAdministrateur();
            lignes.Add(admin);
            Resultat.Controles.Add(admin);
            if (!admin.Ok) { Bloquer("Étape 3 sur 4 : mise à jour et comparaison", "Import refusé. Rien n'a été modifié.", lignes.ToArray()); return; }

            Resultat.Comparaison = _service.Comparer();
            foreach (var l in Resultat.Comparaison)
                lignes.Add(new Controle("Lignes : " + l.Table, !l.Perdue, l.Apres < 0 ? $"{l.Avant} avant, table absente après" : $"{l.Avant} avant → {l.Apres} après"));
            var perdues = Resultat.Comparaison.Where(l => l.Perdue).ToList();
            if (perdues.Count > 0)
            {
                Bloquer("Étape 3 sur 4 : mise à jour et comparaison", "Des lignes disparaîtraient : import refusé. Rien n'a été modifié.", lignes.ToArray());
                return;
            }
            Courante = Etape.Comparaison;
            AfficherEtape("Étape 3 sur 4 : mise à jour et comparaison", "La copie a été mise à jour et aucune ligne n'a disparu. Vous pouvez encore annuler.", lignes);
        }

        private void PreparerRemplacement()
        {
            Courante = Etape.Remplacement;
            bool active = File.Exists(CheminsApp.CheminBase);
            var lignes = ResumeRemplacement();
            AfficherEtape("Étape 4 sur 4 : remplacement",
                active
                    ? "Les données actuelles de la pharmacie vont être REMPLACÉES par celles de l'ancienne base. La base actuelle est d'abord sauvegardée (« avant-import ») dans le dossier des sauvegardes. Cliquez sur « Importer » pour confirmer, ou sur « Annuler »."
                    : "L'ancienne base va devenir la base de la pharmacie sur cet ordinateur. Cliquez sur « Importer » pour confirmer, ou sur « Annuler ».",
                lignes);
            _btnSuivant.Text = "Importer";
            _btnSuivant.Enabled = !_chkComprends.Visible;   // base actuelle plus récente : « Importer » reste grisé tant que « Je comprends » n'est pas coché
        }

        /// <summary>Résumé de l'étape 4 : base importée, base actuelle remplacée, contrôles, diagnostic prévu, emplacement de la sauvegarde.</summary>
        private List<Controle> ResumeRemplacement()
        {
            var importee = _service!.ResumeImporte();
            var actuelle = ResumeBase.Lire(CheminsApp.CheminBase);
            _sauvegardePrevue = actuelle.Existe ? ImportBaseService.CheminSauvegardeAvantImport() : null;
            var lignes = new List<Controle>
            {
                new("Base importée", true, $"fichier {importee.Fichier} ; dernière vente : {importee.DerniereVenteTexte} ; {Format.Compte(importee.Ventes, "vente")}, " +
                                         $"{Format.Compte(importee.Produits, "produit")}, {Format.Compte(importee.Utilisateurs, "utilisateur")} ; chiffre d'affaires total : {Format.Montant(importee.ChiffreAffaires)}", false),
                new("Base actuelle (sera remplacée)", true,
                    !actuelle.Existe ? "aucune base sur cet ordinateur : vide"
                    : !actuelle.Lisible ? "base illisible (sauvegardée avant d'être remplacée)"
                    : actuelle.Vide ? "vide (aucune vente)"
                    : $"{Format.Compte(actuelle.Ventes, "vente")} ; dernière vente : {actuelle.DerniereVenteTexte}", false),
            };

            var controles = Resultat.Controles;
            int reussis = controles.Count(c => c.Ok);
            lignes.Add(new Controle("Contrôles des étapes précédentes", reussis == controles.Count,
                $"{reussis} sur {controles.Count} réussis (intégrité, tables, migrations, administrateur)", false));
            var comparaison = Resultat.Comparaison;
            lignes.Add(new Controle("Comparaison avant / après", comparaison.Count > 0 && !comparaison.Any(l => l.Perdue),
                comparaison.Count == 0 ? "non effectuée" : $"{comparaison.Count} tables comparées, aucune ligne perdue", false));

            try
            {
                var (corrections, averifier) = _service.ApercuDiagnostic();
                lignes.Add(new Controle("Diagnostic de la base", true,
                    $"{Format.Compte(corrections, "correction automatique")} prévue{(corrections > 1 ? "s" : "")} après l'import ; {averifier} élément{(averifier > 1 ? "s" : "")} « À vérifier »", false));
            }
            catch (Exception ex)
            {
                Journal.Erreur("Import : aperçu du diagnostic", ex);
                lignes.Add(new Controle("Diagnostic de la base", false, "aperçu impossible : " + ex.Message, false));
            }

            lignes.Add(new Controle("Sauvegarde « avant-import »", true,
                _sauvegardePrevue != null ? _sauvegardePrevue : "aucune (il n'y a pas de base actuelle à sauvegarder)", false));

            // Base actuelle plus récente que la base importée : avertissement rouge, case « Je comprends » obligatoire
            bool plusRecente = actuelle.DerniereVente.HasValue && (!importee.DerniereVente.HasValue || actuelle.DerniereVente > importee.DerniereVente);
            _avertissement.Visible = _chkComprends.Visible = plusRecente;
            _chkComprends.Checked = false;
            _avertissement.Text = plusRecente ? "Attention : la base actuelle contient des ventes plus récentes que la base importée. Elles seront remplacées." : "";
            _avertissement.ForeColor = Theme.UrgentTexte;
            return lignes;
        }

        public Label Avertissement => _avertissement;
        public CheckBox CaseComprends => _chkComprends;
        public Button BoutonImporter => _btnSuivant;

        private void Remplacer()
        {
            try
            {
                Resultat.SauvegardeAvantImport = _service!.Appliquer(_sauvegardePrevue);
                Resultat.OriginalIntact = _service.OriginalIntact();
                Resultat.Issue = IssueImport.Importee;
                Courante = Etape.Termine;
                var l = new List<Controle> { new("Base remplacée", true, "L'ancienne base est maintenant la base de la pharmacie.") };
                if (Resultat.SauvegardeAvantImport != null) l.Add(new Controle("Sauvegarde avant-import", true, Path.GetFileName(Resultat.SauvegardeAvantImport)));
                l.Add(new Controle("Fichier d'origine", Resultat.OriginalIntact, Resultat.OriginalIntact ? "Intact (empreinte SHA-256 identique)." : "ATTENTION : le fichier d'origine a changé."));
                AfficherEtape("Import terminé", "L'import est terminé.", l);
                _btnSuivant.Text = "Terminer";
                _btnAnnuler.Visible = false;
            }
            catch (Exception ex)
            {
                Journal.Erreur("Import : remplacement de la base", ex);
                Bloquer("Étape 4 sur 4 : remplacement", "Le remplacement a échoué : " + ex.Message + " Vos données actuelles n'ont pas été modifiées.", new Controle("Remplacement", false, ex.Message));
            }
        }

        private void Bloquer(string titre, string consigne, params Controle[] lignes)
        {
            Courante = Etape.Bloque;
            Resultat.Issue = IssueImport.Bloquee;
            Resultat.Message = consigne;
            AfficherEtape(titre, consigne, lignes);
            _btnSuivant.Visible = false;
            _btnRapport.Visible = true;
            _btnAnnuler.Text = "Fermer";
        }

        private void AfficherEtape(string titre, string consigne, IEnumerable<Controle> lignes)
        {
            _titre.Text = titre;
            _consigne.Text = consigne;
            _grille.DataSource = lignes.Select(c => new { c.Nom, Resultat = c.Ok ? "✓ OK" : "✗ Échec", c.Detail }).ToList();
        }

        public DataGridView Grille => _grille;

        /// <summary>La copie de travail est supprimée dès que la fenêtre est libérée (fermée ou non).</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing) { _service?.Dispose(); _service = null; }
            base.Dispose(disposing);
        }

        private void PreparerRapport()
        {
            try
            {
                string zip = RapportDeveloppeurService.Preparer(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), this);
                Messages.Afficher("Le rapport est prêt sur le Bureau :\n" + Path.GetFileName(zip) + "\n\nEnvoyez-le au développeur. " + RapportDeveloppeurService.MentionConfidentialite,
                    "Rapport prêt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Rapport (assistant d'import)", ex);
                Messages.Afficher("Le rapport n'a pas pu être préparé : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
