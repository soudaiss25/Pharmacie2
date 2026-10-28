using System.Drawing;
using Pharmacie2.Services;

namespace Pharmacie2.views
{
    /// <summary>
    /// À la clôture, quand le montant compté diffère du montant attendu de plus de 100 KMF : le motif est obligatoire (liste fermée),
    /// le commentaire est facultatif. Aucun mot de passe : la clôture reste libre (règle du client).
    /// </summary>
    public class FormEcartCloture : Form
    {
        private readonly ComboBox _motif = new();
        private readonly TextBox _commentaire = new();
        private readonly Button _ok = new();

        public string Motif => _motif.SelectedItem?.ToString() ?? "";
        public string Commentaire => _commentaire.Text.Trim();

        public FormEcartCloture(decimal ecart)
        {
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 10F);
            Text = AppInfo.Titre("Écart de caisse");
            ClientSize = new Size(520, 300);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            var racine = new TableLayoutPanel { Name = "tlpRoot", Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 7, Padding = new Padding(16) };
            racine.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < 5; i++) racine.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            racine.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            racine.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            racine.Controls.Add(new Label { Name = "lblTitre", Text = "Écart de caisse", AutoSize = true, Tag = "titre", Margin = new Padding(0, 0, 0, 6) }, 0, 0);
            racine.Controls.Add(new Label
            {
                Name = "lblEcart", AutoSize = true, MaximumSize = new Size(480, 0), Margin = new Padding(0, 0, 0, 10), Tag = ecart < 0 ? "urgent" : "attention",
                Text = ecart < 0
                    ? $"Il manque {Format.Montant(-ecart)} par rapport au montant attendu."
                    : $"Il y a {Format.Montant(ecart)} de trop par rapport au montant attendu."
            }, 0, 1);
            racine.Controls.Add(new Label { Name = "lblMotif", Text = "Motif de l'écart (obligatoire)", AutoSize = true, Margin = new Padding(0, 0, 0, 2) }, 0, 2);
            _motif.Name = "cbMotif"; _motif.DropDownStyle = ComboBoxStyle.DropDownList; _motif.Dock = DockStyle.Top; _motif.Margin = new Padding(0, 0, 0, 8);
            _motif.Items.AddRange(CaisseService.MotifsEcart);
            _motif.SelectedIndexChanged += (s, e) => _ok.Enabled = _motif.SelectedIndex >= 0;
            racine.Controls.Add(_motif, 0, 3);
            racine.Controls.Add(new Label { Name = "lblCommentaire", Text = "Commentaire (facultatif)", AutoSize = true, Margin = new Padding(0, 0, 0, 2) }, 0, 4);
            _commentaire.Name = "txtCommentaire"; _commentaire.Dock = DockStyle.Top; _commentaire.Margin = new Padding(0, 0, 0, 8);
            racine.Controls.Add(_commentaire, 0, 5);

            var bas = new FlowLayoutPanel { Name = "flpBoutons", Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
            var annuler = new Button { Name = "btnAnnuler", Text = "Annuler la clôture", AutoSize = true, Margin = new Padding(8, 0, 0, 0) };
            annuler.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            _ok.Name = "btnOk"; _ok.Text = "Enregistrer et clôturer"; _ok.Tag = "danger"; _ok.AutoSize = true; _ok.Enabled = false; _ok.Margin = new Padding(0);
            _ok.Click += (s, e) => { if (_motif.SelectedIndex >= 0) { DialogResult = DialogResult.OK; Close(); } };
            bas.Controls.Add(annuler);
            bas.Controls.Add(_ok);
            racine.Controls.Add(bas, 0, 6);

            Controls.Add(racine);
            AcceptButton = _ok;
            CancelButton = annuler;
            Theme.Appliquer(this);
        }
    }
}
