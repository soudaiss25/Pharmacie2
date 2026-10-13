using System.Drawing;
using Pharmacie2.Services;

namespace Pharmacie2.views
{
    /// <summary>
    /// L'application a déjà été installée (traces trouvées) mais aucune base non vide n'est retrouvée : on ne propose PAS de créer une
    /// nouvelle base vide. L'utilisateur indique le fichier (copié, jamais déplacé) ou prépare un rapport pour le développeur.
    /// </summary>
    public class FormBaseIntrouvable : Form
    {
        public string? FichierChoisi { get; private set; }

        public FormBaseIntrouvable()
        {
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 10F);
            Text = AppInfo.Titre("Base de données introuvable");
            ClientSize = new Size(620, 330);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;

            var racine = new TableLayoutPanel { Name = "tlpRoot", Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 6, Padding = new Padding(20) };
            racine.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < 4; i++) racine.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            racine.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            racine.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            racine.Controls.Add(new Label { Name = "lblTitre", Text = "Base de données introuvable", AutoSize = true, Tag = "titre", Margin = new Padding(0, 0, 0, 8) }, 0, 0);
            racine.Controls.Add(new Label
            {
                Name = "lblConsigne", AutoSize = true, MaximumSize = new Size(560, 0), Margin = new Padding(0, 0, 0, 12),
                Text = "Ce logiciel a déjà été utilisé sur cet ordinateur, mais le fichier des ventes et du stock (PharmacieDB.sqlite) n'a pas été retrouvé.\n\n" +
                       "Pour ne rien perdre, aucune nouvelle base vide n'est créée."
            }, 0, 1);

            var choisir = new Button { Name = "btnChoisir", Text = "Choisir le fichier…", Tag = "primaire", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 0, 0, 8) };
            choisir.Click += (s, e) => ChoisirFichier();
            racine.Controls.Add(choisir, 0, 2);

            var rapport = new Button { Name = "btnRapport", Text = "Préparer un rapport pour le développeur", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 0, 0, 8) };
            rapport.Click += (s, e) => PreparerRapport();
            racine.Controls.Add(rapport, 0, 3);

            racine.Controls.Add(new Label
            {
                Name = "lblNote", AutoSize = true, MaximumSize = new Size(560, 0), Tag = "note", Margin = new Padding(0, 4, 0, 0),
                Text = "Le fichier choisi est copié : l'original n'est jamais déplacé ni supprimé."
            }, 0, 4);

            var bas = new FlowLayoutPanel { Name = "flpBoutons", Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
            var quitter = new Button { Name = "btnQuitter", Text = "Quitter", AutoSize = true, Margin = new Padding(0) };
            quitter.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            bas.Controls.Add(quitter);
            racine.Controls.Add(bas, 0, 5);

            Controls.Add(racine);
            CancelButton = quitter;
            Theme.Appliquer(this);
        }

        private void ChoisirFichier()
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Choisir le fichier PharmacieDB.sqlite",
                Filter = "Base de données (*.sqlite)|*.sqlite|Tous les fichiers (*.*)|*.*",
                CheckFileExists = true
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            var info = new FileInfo(dlg.FileName);
            if (info.Length == 0 || MigrationEmplacementBase.CompterVentes(info.FullName) < 0)
            {
                MessageBox.Show("Ce fichier n'est pas une base de données de la pharmacie (vide ou illisible). Choisissez un autre fichier.",
                    "Fichier non valide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FichierChoisi = info.FullName;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void PreparerRapport()
        {
            try
            {
                string zip = RapportDeveloppeurService.Preparer(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), this);
                MessageBox.Show("Le rapport est prêt sur le Bureau :\n" + Path.GetFileName(zip) + "\n\nEnvoyez-le au développeur. Il ne contient aucune donnée de la pharmacie.",
                    "Rapport prêt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Journal.Erreur("Rapport (base introuvable)", ex);
                MessageBox.Show("Le rapport n'a pas pu être préparé : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
