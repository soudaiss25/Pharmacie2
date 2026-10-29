using System.Drawing;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views
{
    /// <summary>
    /// Rapport du diagnostic de la base (règles D1 à D23), en trois groupes : corrigés automatiquement, à vérifier, pour information.
    /// Affiché juste après un import (corrections appliquées) et depuis Administration (lecture seule : aucune correction automatique).
    /// </summary>
    public class FormDiagnostic : Form
    {
        private readonly RapportDiagnostic _rapport;
        private readonly TabControl _onglets = new();
        private readonly Dictionary<GroupeConstat, DataGridView> _grilles = new();
        private readonly Label _resume = new();
        private readonly Button _btnSessions = new();
        private readonly Button _btnExcel = new();

        public FormDiagnostic(RapportDiagnostic rapport, bool apresImport)
        {
            _rapport = rapport;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 10F);
            Text = AppInfo.Titre(apresImport ? "Diagnostic de l'ancienne base" : "Diagnostic de la base");
            ClientSize = new Size(980, 560);
            MinimumSize = new Size(700, 420);
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = true;
            MinimizeBox = false;

            var racine = new TableLayoutPanel { Name = "tlpRoot", Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(16) };
            racine.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            racine.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            racine.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            racine.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            racine.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            racine.Controls.Add(new Label
            {
                Name = "lblTitre", AutoSize = true, Tag = "titre", Margin = new Padding(0, 0, 0, 4),
                Text = apresImport ? "Diagnostic de l'ancienne base" : "Diagnostic de la base (lecture seule)"
            }, 0, 0);

            _resume.Name = "lblResume";
            _resume.AutoSize = true;
            _resume.MaximumSize = new Size(900, 0);
            _resume.Margin = new Padding(0, 0, 0, 8);
            racine.Controls.Add(_resume, 0, 1);

            _onglets.Name = "tabGroupes";
            _onglets.Dock = DockStyle.Fill;
            foreach (var (groupe, nom) in new[]
            {
                (GroupeConstat.Corrige, "corrigé"), (GroupeConstat.AVerifier, "à vérifier"), (GroupeConstat.Information, "information")
            })
            {
                var page = new TabPage { Name = "tab" + groupe };
                var grille = new DataGridView
                {
                    Name = "dgv" + groupe, Dock = DockStyle.Fill, AutoGenerateColumns = false, ReadOnly = true,
                    AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect
                };
                grille.Columns.Add(new DataGridViewTextBoxColumn { Name = "Code", HeaderText = "Règle", DataPropertyName = "Code", FillWeight = 8, MinimumWidth = 60, ReadOnly = true });
                grille.Columns.Add(new DataGridViewTextBoxColumn { Name = "Objet", HeaderText = "Vente / produit", DataPropertyName = "Objet", FillWeight = 27, MinimumWidth = 140, ReadOnly = true });
                grille.Columns.Add(new DataGridViewTextBoxColumn { Name = "Detail", HeaderText = "Détail", DataPropertyName = "Detail", FillWeight = 65, MinimumWidth = 220, ReadOnly = true });
                page.Controls.Add(grille);
                _onglets.TabPages.Add(page);
                _grilles[groupe] = grille;
            }
            racine.Controls.Add(_onglets, 0, 2);

            var bas = new FlowLayoutPanel
            {
                Name = "flpBoutons", Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.RightToLeft, WrapContents = false, Margin = new Padding(0, 8, 0, 0)
            };
            var fermer = new Button { Name = "btnFermer", Text = "Fermer", AutoSize = true, Margin = new Padding(8, 0, 0, 0) };
            fermer.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };
            _btnExcel.Name = "btnExcel"; _btnExcel.Text = "Exporter le rapport (Excel)"; _btnExcel.AutoSize = true; _btnExcel.Margin = new Padding(8, 0, 0, 0);
            _btnExcel.Click += (s, e) => ExporterVersFichier();
            _btnSessions.Name = "btnSessions"; _btnSessions.Text = "Clôturer ces sessions"; _btnSessions.Tag = "primaire"; _btnSessions.AutoSize = true; _btnSessions.Margin = new Padding(0);
            _btnSessions.Click += (s, e) => CloturerSessions();
            bas.Controls.Add(fermer);
            bas.Controls.Add(_btnExcel);
            bas.Controls.Add(_btnSessions);
            racine.Controls.Add(bas, 0, 3);

            Controls.Add(racine);
            CancelButton = fermer;
            Theme.Appliquer(this);
            Actualiser();
        }

        public DataGridView Grille(GroupeConstat g) => _grilles[g];

        /// <summary>Le bouton « Clôturer ces sessions » est proposé tant que des sessions sont restées ouvertes (même état que le bouton affiché).</summary>
        public bool BoutonSessionsVisible => _rapport.SessionsOuvertes.Count > 0;

        private void Actualiser()
        {
            foreach (var (groupe, grille) in _grilles)
                grille.DataSource = _rapport.De(groupe).Select(c => new { c.Code, c.Objet, c.Detail }).ToList();

            int c = _rapport.Nombre(GroupeConstat.Corrige), v = _rapport.Nombre(GroupeConstat.AVerifier), i = _rapport.Nombre(GroupeConstat.Information);
            _onglets.TabPages["tab" + GroupeConstat.Corrige]!.Text = $"Corrigés automatiquement ({c})";
            _onglets.TabPages["tab" + GroupeConstat.AVerifier]!.Text = $"À vérifier ({v})";
            _onglets.TabPages["tab" + GroupeConstat.Information]!.Text = $"Pour information ({i})";

            _resume.Text = _rapport.CorrectionsAppliquees
                ? $"{Format.Compte(c, "correction")} appliquée(s) automatiquement (certaines, sans risque) ; {v} cas à vérifier avec le client ; {i} pour information. Rien d'autre n'a été modifié."
                : $"Lecture seule : aucune correction n'a été appliquée. {v} cas à vérifier, {i} pour information. Une anomalie trouvée maintenant signale un nouveau problème : elle ne doit pas être corrigée en silence.";
            _resume.ForeColor = v > 0 ? Theme.AttentionTexte : Theme.SuccesTexte;
            _btnSessions.Visible = _rapport.SessionsOuvertes.Count > 0;
        }

        private void CloturerSessions()
        {
            if (Messages.Afficher($"Clôturer {Format.Compte(_rapport.SessionsOuvertes.Count, "session")} restée(s) ouverte(s) ?\n\n" +
                                "Chaque session est clôturée à la fin de sa journée, avec le commentaire « " + DiagnosticBaseService.CommentaireCloture + " » et le montant attendu calculé.",
                                "Clôturer ces sessions", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                var ids = _rapport.SessionsOuvertes.ToList();
                using (var ctx = new AppDbContext())
                    DiagnosticBaseService.CloturerSessions(ctx, ids);
                _rapport.Constats.RemoveAll(c => c.Code == "D22");
                foreach (var id in ids)
                    _rapport.Constats.Add(new Constat("D22", GroupeConstat.Corrige, $"Session n°{id}", "clôturée lors de l'import (attendu figé)"));
                _rapport.SessionsOuvertes.Clear();
                DiagnosticBaseService.Enregistrer(_rapport);
                Actualiser();
                BandeauNotification.Succes("Sessions clôturées");
            }
            catch (Exception ex)
            {
                Journal.Erreur("Clôture des sessions oubliées", ex);
                Messages.Afficher("Les sessions n'ont pas pu être clôturées : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Écrit le fichier Excel (un onglet par groupe, aucun nom de client).</summary>
        public void Exporter(string chemin) => DiagnosticBaseService.ExporterExcel(_rapport, chemin);

        private void ExporterVersFichier()
        {
            using var dlg = new SaveFileDialog
            {
                Title = "Exporter le rapport", Filter = "Classeur Excel (*.xlsx)|*.xlsx",
                FileName = $"Diagnostic_base_{DateTime.Now:yyyy-MM-dd}.xlsx",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;
            try
            {
                Exporter(dlg.FileName);
                Messages.Afficher("Le rapport est enregistré :\n" + dlg.FileName, "Exporter le rapport", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Export du diagnostic", ex);
                Messages.Afficher("Le rapport n'a pas pu être exporté : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
