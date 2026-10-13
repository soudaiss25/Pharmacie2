using System.Drawing;
using Pharmacie2.Services;

namespace Pharmacie2.views
{
    /// <summary>
    /// Plusieurs anciennes bases non vides ont été trouvées : l'utilisateur choisit celle à reprendre (la plus récente est présélectionnée).
    /// Rien n'est supprimé : la base choisie est copiée.
    /// </summary>
    public class FormChoixBase : Form
    {
        private readonly List<BaseTrouvee> _bases;
        private readonly DataGridView _grille = new();
        private readonly Button _btnUtiliser = new();
        private readonly Button _btnAnnuler = new();

        public BaseTrouvee? BaseChoisie { get; private set; }

        public FormChoixBase(IReadOnlyList<BaseTrouvee> bases)
        {
            _bases = bases.OrderByDescending(b => b.DateModif).ToList();
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 10F);
            Text = AppInfo.Titre("Choisir la base de données");
            ClientSize = new Size(820, 380);
            MinimumSize = new Size(640, 300);
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;

            var racine = new TableLayoutPanel { Name = "tlpRoot", Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(16) };
            racine.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            racine.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            racine.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            racine.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            racine.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            racine.Controls.Add(new Label { Name = "lblTitre", Text = "Plusieurs bases de données trouvées", AutoSize = true, Tag = "titre", Margin = new Padding(0, 0, 0, 6) }, 0, 0);
            racine.Controls.Add(new Label
            {
                Name = "lblConsigne", AutoSize = true, MaximumSize = new Size(760, 0), Tag = "note", Margin = new Padding(0, 0, 0, 8),
                Text = "Choisissez celle de la pharmacie : la plus récente est déjà sélectionnée. Le fichier choisi est copié, l'original n'est jamais déplacé ni supprimé."
            }, 0, 1);

            _grille.Name = "dgvBases";
            _grille.Dock = DockStyle.Fill;
            _grille.AutoGenerateColumns = false;
            _grille.ReadOnly = true;
            _grille.MultiSelect = false;
            _grille.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grille.Columns.Add(new DataGridViewTextBoxColumn { Name = "Chemin", HeaderText = "Emplacement", DataPropertyName = "Chemin", FillWeight = 56, MinimumWidth = 200, ReadOnly = true });
            _grille.Columns.Add(new DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Modifiée le", DataPropertyName = "Date", FillWeight = 18, MinimumWidth = 120, ReadOnly = true });
            _grille.Columns.Add(new DataGridViewTextBoxColumn { Name = "Taille", HeaderText = "Taille", DataPropertyName = "Taille", FillWeight = 12, MinimumWidth = 80, ReadOnly = true });
            _grille.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ventes", HeaderText = "Ventes", DataPropertyName = "Ventes", FillWeight = 10, MinimumWidth = 70, ReadOnly = true });
            _grille.DataSource = _bases.Select(b => new
            {
                b.Chemin,
                Date = b.DateModif.ToString("dd/MM/yyyy HH:mm"),
                Taille = Format.Octets(b.Taille),
                Ventes = b.NbVentes < 0 ? "illisible" : b.NbVentes.ToString()
            }).ToList();
            racine.Controls.Add(_grille, 0, 2);

            var bas = new FlowLayoutPanel
            {
                Name = "flpBoutons", Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.RightToLeft, WrapContents = false, Margin = new Padding(0, 8, 0, 0)
            };
            _btnAnnuler.Name = "btnAnnuler"; _btnAnnuler.Text = "Quitter"; _btnAnnuler.AutoSize = true; _btnAnnuler.Margin = new Padding(8, 0, 0, 0);
            _btnAnnuler.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            _btnUtiliser.Name = "btnUtiliser"; _btnUtiliser.Text = "Utiliser cette base"; _btnUtiliser.Tag = "primaire"; _btnUtiliser.AutoSize = true; _btnUtiliser.Margin = new Padding(0);
            _btnUtiliser.Click += (s, e) => Valider();
            bas.Controls.Add(_btnAnnuler);
            bas.Controls.Add(_btnUtiliser);
            racine.Controls.Add(bas, 0, 3);

            Controls.Add(racine);
            AcceptButton = _btnUtiliser;
            Theme.Appliquer(this);
            Shown += (s, e) => { if (_grille.Rows.Count > 0) { _grille.ClearSelection(); _grille.Rows[0].Selected = true; _grille.CurrentCell = _grille.Rows[0].Cells[0]; } };
        }

        /// <summary>Base actuellement sélectionnée dans la liste (la plus récente au départ).</summary>
        public BaseTrouvee? Selection
        {
            get
            {
                int i = _grille.SelectedRows.Count > 0 ? _grille.SelectedRows[0].Index : (_grille.Rows.Count > 0 ? 0 : -1);
                return i >= 0 && i < _bases.Count ? _bases[i] : null;
            }
        }

        private void Valider()
        {
            BaseChoisie = Selection;
            if (BaseChoisie == null) return;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
