namespace Pharmacie2.views.UserControls
{
    partial class Uc_Vente
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpRoot = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.flpFiltres = new System.Windows.Forms.FlowLayoutPanel();
            this.lblRecherche = new System.Windows.Forms.Label();
            this.txtSearchVente = new System.Windows.Forms.TextBox();
            this.chkACaisser = new System.Windows.Forms.CheckBox();
            this.dgvVentes = new System.Windows.Forms.DataGridView();
            this.flpActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNouvelleVente = new System.Windows.Forms.Button();
            this.btnDetail = new System.Windows.Forms.Button();
            this.btnEnregistrerPaiement = new System.Windows.Forms.Button();
            this.btnModifierVente = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.flpPied = new System.Windows.Forms.FlowLayoutPanel();
            this.lblNombreVentes = new System.Windows.Forms.Label();
            this.lblTotalVentes = new System.Windows.Forms.Label();
            this.lblVentesCredit = new System.Windows.Forms.Label();
            this.colIdVente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumeroVente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMontantTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMontantVerse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMontantRestant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMoyenPaiement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendeur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentes)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.flpFiltres.SuspendLayout();
            this.flpActions.SuspendLayout();
            this.flpPied.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 5;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.lblTitle, 0, 0);
            this.tlpRoot.Controls.Add(this.flpFiltres, 0, 1);
            this.tlpRoot.Controls.Add(this.dgvVentes, 0, 2);
            this.tlpRoot.Controls.Add(this.flpActions, 0, 3);
            this.tlpRoot.Controls.Add(this.flpPied, 0, 4);
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitle
            // 
            this.lblTitle.Text = "Ventes";
            this.lblTitle.AutoSize = true;
            this.lblTitle.Tag = "titre";
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTitle.Name = "lblTitle";
            // 
            // flpFiltres
            // 
            this.flpFiltres.Controls.Add(this.lblRecherche);
            this.flpFiltres.Controls.Add(this.txtSearchVente);
            this.flpFiltres.Controls.Add(this.chkACaisser);
            this.flpFiltres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpFiltres.AutoSize = true;
            this.flpFiltres.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpFiltres.WrapContents = true;
            this.flpFiltres.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.flpFiltres.Name = "flpFiltres";
            // 
            // lblRecherche
            // 
            this.lblRecherche.Text = "Rechercher";
            this.lblRecherche.AutoSize = true;
            this.lblRecherche.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblRecherche.Name = "lblRecherche";
            // 
            // txtSearchVente
            // 
            this.txtSearchVente.Width = 300;
            this.txtSearchVente.MaximumSize = new System.Drawing.Size(400, 0);
            this.txtSearchVente.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.txtSearchVente.PlaceholderText = "Nom, prénom, téléphone ou n° de vente";
            this.txtSearchVente.Name = "txtSearchVente";
            this.txtSearchVente.TextChanged += new System.EventHandler(this.txtSearchVente_TextChanged);
            // 
            // chkACaisser
            // 
            this.chkACaisser.Text = "Seulement les ventes à encaisser";
            this.chkACaisser.AutoSize = true;
            this.chkACaisser.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.chkACaisser.Name = "chkACaisser";
            this.chkACaisser.CheckedChanged += new System.EventHandler(this.chkACaisser_CheckedChanged);
            // 
            // dgvVentes
            // 
            this.dgvVentes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIdVente,
            this.colNumeroVente,
            this.colClient,
            this.colTel,
            this.colDate,
            this.colMontantTotal,
            this.colMontantVerse,
            this.colMontantRestant,
            this.colMoyenPaiement,
            this.colVendeur,
            this.colStatut});
            this.dgvVentes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVentes.AutoGenerateColumns = false;
            this.dgvVentes.ReadOnly = true;
            this.dgvVentes.Name = "dgvVentes";
            this.dgvVentes.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvVentes_CellFormatting);
            // 
            // flpActions
            // 
            this.flpActions.Controls.Add(this.btnNouvelleVente);
            this.flpActions.Controls.Add(this.btnDetail);
            this.flpActions.Controls.Add(this.btnEnregistrerPaiement);
            this.flpActions.Controls.Add(this.btnModifierVente);
            this.flpActions.Controls.Add(this.btnAnnuler);
            this.flpActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpActions.AutoSize = true;
            this.flpActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpActions.WrapContents = true;
            this.flpActions.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.flpActions.Name = "flpActions";
            // 
            // btnNouvelleVente
            // 
            this.btnNouvelleVente.Text = "Nouvelle vente (F2)";
            this.btnNouvelleVente.Tag = "primaire";
            this.btnNouvelleVente.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnNouvelleVente.Name = "btnNouvelleVente";
            this.btnNouvelleVente.Click += new System.EventHandler(this.btnNouvelleVente_Click);
            // 
            // btnDetail
            // 
            this.btnDetail.Text = "Voir le détail";
            this.btnDetail.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnDetail.Name = "btnDetail";
            this.btnDetail.Click += new System.EventHandler(this.btnDetail_Click);
            // 
            // btnEnregistrerPaiement
            // 
            this.btnEnregistrerPaiement.Text = "Encaisser un paiement";
            this.btnEnregistrerPaiement.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnEnregistrerPaiement.Name = "btnEnregistrerPaiement";
            this.btnEnregistrerPaiement.Click += new System.EventHandler(this.btnEnregistrerPaiement_Click);
            // 
            // btnModifierVente
            // 
            this.btnModifierVente.Text = "Modifier la vente";
            this.btnModifierVente.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnModifierVente.Name = "btnModifierVente";
            this.btnModifierVente.Click += new System.EventHandler(this.btnModifierVente_Click);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Text = "Annuler la vente";
            this.btnAnnuler.Tag = "danger";
            this.btnAnnuler.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // flpPied
            // 
            this.flpPied.Controls.Add(this.lblNombreVentes);
            this.flpPied.Controls.Add(this.lblTotalVentes);
            this.flpPied.Controls.Add(this.lblVentesCredit);
            this.flpPied.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPied.AutoSize = true;
            this.flpPied.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpPied.WrapContents = true;
            this.flpPied.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.flpPied.Name = "flpPied";
            // 
            // lblNombreVentes
            // 
            this.lblNombreVentes.Text = "";
            this.lblNombreVentes.AutoSize = true;
            this.lblNombreVentes.Margin = new System.Windows.Forms.Padding(0, 0, 24, 0);
            this.lblNombreVentes.Name = "lblNombreVentes";
            // 
            // lblTotalVentes
            // 
            this.lblTotalVentes.Text = "";
            this.lblTotalVentes.AutoSize = true;
            this.lblTotalVentes.Margin = new System.Windows.Forms.Padding(0, 0, 24, 0);
            this.lblTotalVentes.Name = "lblTotalVentes";
            // 
            // lblVentesCredit
            // 
            this.lblVentesCredit.Text = "";
            this.lblVentesCredit.AutoSize = true;
            this.lblVentesCredit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVentesCredit.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.lblVentesCredit.Name = "lblVentesCredit";
            // 
            // colIdVente
            // 
            this.colIdVente.HeaderText = "Id";
            this.colIdVente.Name = "IdVente";
            this.colIdVente.DataPropertyName = "IdVente";
            this.colIdVente.FillWeight = 100F;
            this.colIdVente.MinimumWidth = 60;
            this.colIdVente.ReadOnly = true;
            this.colIdVente.Visible = false;
            // 
            // colNumeroVente
            // 
            this.colNumeroVente.HeaderText = "N° de vente";
            this.colNumeroVente.Name = "Numero";
            this.colNumeroVente.DataPropertyName = "Numero";
            this.colNumeroVente.FillWeight = 10F;
            this.colNumeroVente.MinimumWidth = 90;
            this.colNumeroVente.ReadOnly = true;
            // 
            // colClient
            // 
            this.colClient.HeaderText = "Client";
            this.colClient.Name = "Client";
            this.colClient.DataPropertyName = "Client";
            this.colClient.FillWeight = 17F;
            this.colClient.MinimumWidth = 110;
            this.colClient.ReadOnly = true;
            // 
            // colTel
            // 
            this.colTel.HeaderText = "Téléphone";
            this.colTel.Name = "Tel";
            this.colTel.DataPropertyName = "Tel";
            this.colTel.FillWeight = 11F;
            this.colTel.MinimumWidth = 90;
            this.colTel.ReadOnly = true;
            // 
            // colDate
            // 
            this.colDate.HeaderText = "Date";
            this.colDate.Name = "Date";
            this.colDate.DataPropertyName = "Date";
            this.colDate.FillWeight = 12F;
            this.colDate.MinimumWidth = 110;
            this.colDate.ReadOnly = true;
            // 
            // colMontantTotal
            // 
            this.colMontantTotal.HeaderText = "Total";
            this.colMontantTotal.Name = "Total";
            this.colMontantTotal.DataPropertyName = "Total";
            this.colMontantTotal.FillWeight = 10F;
            this.colMontantTotal.MinimumWidth = 90;
            this.colMontantTotal.ReadOnly = true;
            this.colMontantTotal.Tag = "montant";
            // 
            // colMontantVerse
            // 
            this.colMontantVerse.HeaderText = "Versé";
            this.colMontantVerse.Name = "Verse";
            this.colMontantVerse.DataPropertyName = "Verse";
            this.colMontantVerse.FillWeight = 10F;
            this.colMontantVerse.MinimumWidth = 90;
            this.colMontantVerse.ReadOnly = true;
            this.colMontantVerse.Tag = "montant";
            // 
            // colMontantRestant
            // 
            this.colMontantRestant.HeaderText = "Reste à payer";
            this.colMontantRestant.Name = "Restant";
            this.colMontantRestant.DataPropertyName = "Restant";
            this.colMontantRestant.FillWeight = 10F;
            this.colMontantRestant.MinimumWidth = 90;
            this.colMontantRestant.ReadOnly = true;
            this.colMontantRestant.Tag = "montant";
            // 
            // colMoyenPaiement
            // 
            this.colMoyenPaiement.HeaderText = "Paiement";
            this.colMoyenPaiement.Name = "Mode";
            this.colMoyenPaiement.DataPropertyName = "Mode";
            this.colMoyenPaiement.FillWeight = 10F;
            this.colMoyenPaiement.MinimumWidth = 90;
            this.colMoyenPaiement.ReadOnly = true;
            // 
            // colVendeur
            // 
            this.colVendeur.HeaderText = "Vendeur";
            this.colVendeur.Name = "Vendeur";
            this.colVendeur.DataPropertyName = "Vendeur";
            this.colVendeur.FillWeight = 12F;
            this.colVendeur.MinimumWidth = 90;
            this.colVendeur.ReadOnly = true;
            // 
            // colStatut
            // 
            this.colStatut.HeaderText = "Statut";
            this.colStatut.Name = "Statut";
            this.colStatut.DataPropertyName = "Statut";
            this.colStatut.FillWeight = 8F;
            this.colStatut.MinimumWidth = 70;
            this.colStatut.ReadOnly = true;
            // 
            // Uc_Vente
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(1146, 700);
            this.Name = "Uc_Vente";
            this.ResumeLayout(false);
            this.flpPied.ResumeLayout(false);
            this.flpPied.PerformLayout();
            this.flpActions.ResumeLayout(false);
            this.flpActions.PerformLayout();
            this.flpFiltres.ResumeLayout(false);
            this.flpFiltres.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentes)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel flpFiltres;
        private System.Windows.Forms.Label lblRecherche;
        private System.Windows.Forms.TextBox txtSearchVente;
        private System.Windows.Forms.CheckBox chkACaisser;
        private System.Windows.Forms.DataGridView dgvVentes;
        private System.Windows.Forms.FlowLayoutPanel flpActions;
        private System.Windows.Forms.Button btnNouvelleVente;
        private System.Windows.Forms.Button btnDetail;
        private System.Windows.Forms.Button btnEnregistrerPaiement;
        private System.Windows.Forms.Button btnModifierVente;
        private System.Windows.Forms.Button btnAnnuler;
        private System.Windows.Forms.FlowLayoutPanel flpPied;
        private System.Windows.Forms.Label lblNombreVentes;
        private System.Windows.Forms.Label lblTotalVentes;
        private System.Windows.Forms.Label lblVentesCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdVente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumeroVente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMontantTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMontantVerse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMontantRestant;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMoyenPaiement;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendeur;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatut;
    }
}
