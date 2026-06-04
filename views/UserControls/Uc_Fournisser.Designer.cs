using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views.UserControls
{
    partial class Uc_Fournisser
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle style1 = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            DataGridViewCellStyle style4 = new DataGridViewCellStyle();

            panel1 = new Panel(); label1 = new Label();
            panelSearch = new Panel(); btnClear = new Button();
            txtSearch = new TextBox(); labelSearch = new Label();
            toolTip1 = new ToolTip(components);
            btnAdd = new Button(); btnEdit = new Button(); btnDelete = new Button();
            btnVoirCommandes = new Button(); btnCommanderProduit = new Button();
            splitMain = new SplitContainer();
            dgvFournisseurs = new DataGridView();

            // COLONNES FOURNISSEURS — toutes déclarées ici
            colId = new DataGridViewTextBoxColumn();
            colNom = new DataGridViewTextBoxColumn();
            colContact = new DataGridViewTextBoxColumn();
            colTelephone = new DataGridViewTextBoxColumn();
            colAdresse = new DataGridViewTextBoxColumn();
            btnViewCommandes = new DataGridViewButtonColumn();

            panelButtons = new Panel();
            dgvProduits = new DataGridView();
            colProduitId = new DataGridViewTextBoxColumn();
            colProduitNom = new DataGridViewTextBoxColumn();
            colProduitType = new DataGridViewTextBoxColumn();
            colProduitQte = new DataGridViewTextBoxColumn();
            colProduitSeuil = new DataGridViewTextBoxColumn();
            colProduitPrixAchat = new DataGridViewTextBoxColumn();
            colProduitPrixVente = new DataGridViewTextBoxColumn();
            colProduitEtat = new DataGridViewTextBoxColumn();
            pnlProduitsHeader = new Panel(); lblProduitsTitre = new Label();

            panel1.SuspendLayout();
            panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFournisseurs).BeginInit();
            panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProduits).BeginInit();
            pnlProduitsHeader.SuspendLayout();
            SuspendLayout();

            // panel1 - header vert
            panel1.BackColor = Color.Green;
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1200, 60);
            panel1.TabIndex = 0;

            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(18, 10);
            label1.Name = "label1";
            label1.TabIndex = 0;
            label1.Text = "FOURNISSEURS";

            // panelSearch
            panelSearch.BackColor = Color.WhiteSmoke;
            panelSearch.Controls.Add(btnClear);
            panelSearch.Controls.Add(txtSearch);
            panelSearch.Controls.Add(labelSearch);
            panelSearch.Dock = DockStyle.Top;
            panelSearch.Location = new Point(0, 0);
            panelSearch.Name = "panelSearch";
            panelSearch.Size = new Size(1200, 52);
            panelSearch.TabIndex = 1;

            btnClear.BackColor = Color.FromArgb(240, 240, 240);
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Segoe UI", 9F);
            btnClear.Location = new Point(420, 13);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(90, 28);
            btnClear.TabIndex = 0;
            btnClear.Text = "Effacer";
            btnClear.UseVisualStyleBackColor = false;

            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(130, 13);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(280, 34);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += txtSearch_TextChanged;

            labelSearch.AutoSize = true;
            labelSearch.Font = new Font("Segoe UI", 10F);
            labelSearch.ForeColor = Color.FromArgb(64, 64, 64);
            labelSearch.Location = new Point(14, 16);
            labelSearch.Name = "labelSearch";
            labelSearch.TabIndex = 2;
            labelSearch.Text = "Rechercher :";

            // Boutons CRUD
            btnAdd.BackColor = Color.FromArgb(0, 150, 136);
            btnAdd.FlatAppearance.BorderSize = 0; btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White; btnAdd.Location = new Point(10, 10);
            btnAdd.Name = "btnAdd"; btnAdd.Size = new Size(150, 35); btnAdd.TabIndex = 0;
            btnAdd.Text = "Ajouter"; btnAdd.UseVisualStyleBackColor = false;

            btnEdit.BackColor = Color.FromArgb(33, 150, 243);
            btnEdit.FlatAppearance.BorderSize = 0; btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnEdit.ForeColor = Color.White; btnEdit.Location = new Point(170, 10);
            btnEdit.Name = "btnEdit"; btnEdit.Size = new Size(150, 35); btnEdit.TabIndex = 1;
            btnEdit.Text = "Modifier"; btnEdit.UseVisualStyleBackColor = false;

            btnDelete.BackColor = Color.FromArgb(211, 47, 47);
            btnDelete.FlatAppearance.BorderSize = 0; btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White; btnDelete.Location = new Point(330, 10);
            btnDelete.Name = "btnDelete"; btnDelete.Size = new Size(150, 35); btnDelete.TabIndex = 2;
            btnDelete.Text = "Supprimer"; btnDelete.UseVisualStyleBackColor = false;

            btnVoirCommandes.BackColor = Color.FromArgb(25, 118, 210);
            btnVoirCommandes.Enabled = false; btnVoirCommandes.FlatAppearance.BorderSize = 0;
            btnVoirCommandes.FlatStyle = FlatStyle.Flat;
            btnVoirCommandes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnVoirCommandes.ForeColor = Color.White; btnVoirCommandes.Location = new Point(700, 8);
            btnVoirCommandes.Name = "btnVoirCommandes"; btnVoirCommandes.Size = new Size(190, 32);
            btnVoirCommandes.Text = "Historique commandes"; btnVoirCommandes.UseVisualStyleBackColor = false;

            btnCommanderProduit.BackColor = Color.FromArgb(230, 120, 0);
            btnCommanderProduit.Enabled = false; btnCommanderProduit.FlatAppearance.BorderSize = 0;
            btnCommanderProduit.FlatStyle = FlatStyle.Flat;
            btnCommanderProduit.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCommanderProduit.ForeColor = Color.White; btnCommanderProduit.Location = new Point(900, 8);
            btnCommanderProduit.Name = "btnCommanderProduit"; btnCommanderProduit.Size = new Size(200, 32);
            btnCommanderProduit.Text = "Commander ce produit"; btnCommanderProduit.UseVisualStyleBackColor = false;

            // splitMain
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 0);
            splitMain.Name = "splitMain";
            splitMain.Orientation = Orientation.Horizontal;
            splitMain.Panel1.Controls.Add(dgvFournisseurs);
            splitMain.Panel1.Controls.Add(panelButtons);
            splitMain.Panel1MinSize = 180;
            splitMain.Panel2.Controls.Add(dgvProduits);
            splitMain.Panel2.Controls.Add(pnlProduitsHeader);
            splitMain.Panel2MinSize = 160;
            splitMain.Size = new Size(1200, 600);
            // SplitterDistance defini au Load dans Uc_Fournisser.cs
            splitMain.TabIndex = 0;

            // dgvFournisseurs — TOUTES LES COLONNES DECLAREES
            style1.BackColor = Color.FromArgb(240, 248, 240);
            dgvFournisseurs.AlternatingRowsDefaultCellStyle = style1;
            dgvFournisseurs.AllowUserToAddRows = false;
            dgvFournisseurs.AllowUserToDeleteRows = false;
            dgvFournisseurs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFournisseurs.BackgroundColor = Color.White;
            dgvFournisseurs.BorderStyle = BorderStyle.None;
            style2.BackColor = Color.Green; style2.ForeColor = Color.White;
            style2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            style2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvFournisseurs.ColumnHeadersDefaultCellStyle = style2;
            dgvFournisseurs.ColumnHeadersHeight = 36;
            dgvFournisseurs.EnableHeadersVisualStyles = false;
            dgvFournisseurs.Dock = DockStyle.Fill;
            dgvFournisseurs.Font = new Font("Segoe UI", 9.5F);
            dgvFournisseurs.MultiSelect = false;
            dgvFournisseurs.Name = "dgvFournisseurs";
            dgvFournisseurs.ReadOnly = true;
            dgvFournisseurs.RowHeadersVisible = false;
            dgvFournisseurs.RowTemplate.Height = 34;
            dgvFournisseurs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Colonnes fournisseur
            colId.Name = "colId"; colId.HeaderText = "ID"; colId.Visible = false;
            colNom.Name = "colNom"; colNom.HeaderText = "Nom du fournisseur"; colNom.FillWeight = 120F;
            colContact.Name = "colContact"; colContact.HeaderText = "Contact"; colContact.FillWeight = 100F;
            colTelephone.Name = "colTelephone"; colTelephone.HeaderText = "Telephone"; colTelephone.FillWeight = 80F;
            colAdresse.Name = "colAdresse"; colAdresse.HeaderText = "Adresse"; colAdresse.FillWeight = 120F;
            btnViewCommandes.Name = "btnViewCommandes"; btnViewCommandes.HeaderText = "Historique";
            btnViewCommandes.FillWeight = 40F; btnViewCommandes.ReadOnly = true;
            btnViewCommandes.Text = "Voir"; btnViewCommandes.UseColumnTextForButtonValue = true;

            dgvFournisseurs.Columns.AddRange(new DataGridViewColumn[] {
                colId, colNom, colContact, colTelephone, colAdresse, btnViewCommandes });

            // panelButtons
            panelButtons.BackColor = Color.FromArgb(245, 250, 245);
            panelButtons.Controls.Add(btnAdd);
            panelButtons.Controls.Add(btnEdit);
            panelButtons.Controls.Add(btnDelete);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(1200, 54);
            panelButtons.TabIndex = 3;

            // dgvProduits
            style3.BackColor = Color.FromArgb(245, 250, 245);
            dgvProduits.AlternatingRowsDefaultCellStyle = style3;
            dgvProduits.AllowUserToAddRows = false;
            dgvProduits.AllowUserToDeleteRows = false;
            dgvProduits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProduits.BackgroundColor = Color.White;
            dgvProduits.BorderStyle = BorderStyle.None;
            style4.BackColor = Color.FromArgb(69, 90, 100); style4.ForeColor = Color.White;
            style4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvProduits.ColumnHeadersDefaultCellStyle = style4;
            dgvProduits.ColumnHeadersHeight = 34;
            dgvProduits.EnableHeadersVisualStyles = false;
            dgvProduits.Dock = DockStyle.Fill;
            dgvProduits.Font = new Font("Segoe UI", 9F);
            dgvProduits.MultiSelect = false;
            dgvProduits.Name = "dgvProduits";
            dgvProduits.ReadOnly = true;
            dgvProduits.RowHeadersVisible = false;
            dgvProduits.RowTemplate.Height = 30;
            dgvProduits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            colProduitId.Name = "colProduitId"; colProduitId.HeaderText = "ID"; colProduitId.Visible = false;
            colProduitNom.Name = "colProduitNom"; colProduitNom.HeaderText = "Produit";
            colProduitType.Name = "colProduitType"; colProduitType.HeaderText = "Type"; colProduitType.FillWeight = 60F;
            colProduitQte.Name = "colProduitQte"; colProduitQte.HeaderText = "Stock"; colProduitQte.FillWeight = 40F;
            colProduitSeuil.Name = "colProduitSeuil"; colProduitSeuil.HeaderText = "Seuil"; colProduitSeuil.FillWeight = 40F;
            colProduitPrixAchat.Name = "colProduitPrixAchat"; colProduitPrixAchat.HeaderText = "Prix achat"; colProduitPrixAchat.FillWeight = 60F;
            colProduitPrixVente.Name = "colProduitPrixVente"; colProduitPrixVente.HeaderText = "Prix vente"; colProduitPrixVente.FillWeight = 60F;
            colProduitEtat.Name = "colProduitEtat"; colProduitEtat.HeaderText = "Etat stock"; colProduitEtat.FillWeight = 50F;

            dgvProduits.Columns.AddRange(new DataGridViewColumn[] {
                colProduitId, colProduitNom, colProduitType, colProduitQte,
                colProduitSeuil, colProduitPrixAchat, colProduitPrixVente, colProduitEtat });

            // pnlProduitsHeader
            pnlProduitsHeader.BackColor = Color.FromArgb(230, 245, 230);
            pnlProduitsHeader.Controls.Add(lblProduitsTitre);
            pnlProduitsHeader.Controls.Add(btnVoirCommandes);
            pnlProduitsHeader.Controls.Add(btnCommanderProduit);
            pnlProduitsHeader.Dock = DockStyle.Top;
            pnlProduitsHeader.Name = "pnlProduitsHeader";
            pnlProduitsHeader.Size = new Size(1200, 50);
            pnlProduitsHeader.TabIndex = 1;

            lblProduitsTitre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblProduitsTitre.ForeColor = Color.FromArgb(27, 94, 32);
            lblProduitsTitre.Location = new Point(8, 8);
            lblProduitsTitre.Name = "lblProduitsTitre";
            lblProduitsTitre.Size = new Size(680, 34);
            lblProduitsTitre.TabIndex = 0;
            lblProduitsTitre.Text = "Selectionnez un fournisseur pour voir ses produits";
            lblProduitsTitre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // Uc_Fournisser
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitMain);
            Controls.Add(panelSearch);
            Controls.Add(panel1);
            Name = "Uc_Fournisser";
            Size = new Size(1200, 700);

            panel1.ResumeLayout(false); panel1.PerformLayout();
            panelSearch.ResumeLayout(false); panelSearch.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvFournisseurs).EndInit();
            panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProduits).EndInit();
            pnlProduitsHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        // Declarations
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.DataGridView dgvFournisseurs;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnAdd, btnEdit, btnDelete;
        // Colonnes fournisseurs
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContact;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTelephone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdresse;
        private System.Windows.Forms.DataGridViewButtonColumn btnViewCommandes;
        private System.Windows.Forms.Panel pnlProduitsHeader;
        private System.Windows.Forms.Label lblProduitsTitre;
        private System.Windows.Forms.Button btnVoirCommandes, btnCommanderProduit;
        private System.Windows.Forms.DataGridView dgvProduits;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitNom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitQte;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitSeuil;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitPrixAchat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitPrixVente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduitEtat;
    }
}