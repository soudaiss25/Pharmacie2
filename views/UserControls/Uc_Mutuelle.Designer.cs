namespace Pharmacie2.views.UserControls
{
    partial class Uc_Mutuelle
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
            this.lblTitre = new System.Windows.Forms.Label();
            this.flpActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNouvelleMutuelle = new System.Windows.Forms.Button();
            this.btnModifier = new System.Windows.Forms.Button();
            this.btnSupprimer = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnActualiser = new System.Windows.Forms.Button();
            this.dgvMutuelles = new System.Windows.Forms.DataGridView();
            this.lblRecapImpaye = new System.Windows.Forms.Label();
            this.flpReglement = new System.Windows.Forms.FlowLayoutPanel();
            this.btnReglertout = new System.Windows.Forms.Button();
            this.btnReglerSelection = new System.Windows.Forms.Button();
            this.dgvVentesImpayees = new System.Windows.Forms.DataGridView();
            this.colIdMutuel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMutuelle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaux = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTelephone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNbImpayees = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalImpaye = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCoche = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colVenteId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumeroVente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateVente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatricule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMontantTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMontantMutuelle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendeur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMutuelles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentesImpayees)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.flpActions.SuspendLayout();
            this.flpReglement.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 6;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.flpActions, 0, 1);
            this.flpActions.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.dgvMutuelles, 0, 2);
            this.dgvMutuelles.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.lblRecapImpaye, 0, 3);
            this.lblRecapImpaye.TabIndex = 3;
            this.tlpRoot.Controls.Add(this.flpReglement, 0, 4);
            this.flpReglement.TabIndex = 4;
            this.tlpRoot.Controls.Add(this.dgvVentesImpayees, 0, 5);
            this.dgvVentesImpayees.TabIndex = 5;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Mutuelles";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTitre.Name = "lblTitre";
            // 
            // flpActions
            // 
            this.flpActions.Controls.Add(this.btnNouvelleMutuelle);
            this.btnNouvelleMutuelle.TabIndex = 0;
            this.flpActions.Controls.Add(this.btnModifier);
            this.btnModifier.TabIndex = 1;
            this.flpActions.Controls.Add(this.btnSupprimer);
            this.btnSupprimer.TabIndex = 2;
            this.flpActions.Controls.Add(this.btnExportExcel);
            this.btnExportExcel.TabIndex = 3;
            this.flpActions.Controls.Add(this.btnActualiser);
            this.btnActualiser.TabIndex = 4;
            this.flpActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpActions.AutoSize = true;
            this.flpActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpActions.WrapContents = true;
            this.flpActions.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.flpActions.Name = "flpActions";
            // 
            // btnNouvelleMutuelle
            // 
            this.btnNouvelleMutuelle.Text = "Nouvelle mutuelle";
            this.btnNouvelleMutuelle.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnNouvelleMutuelle.Tag = "primaire";
            this.btnNouvelleMutuelle.Name = "btnNouvelleMutuelle";
            this.btnNouvelleMutuelle.Click += new System.EventHandler(this.btnNouvelleMutuelle_Click);
            // 
            // btnModifier
            // 
            this.btnModifier.Text = "Modifier";
            this.btnModifier.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnModifier.Name = "btnModifier";
            this.btnModifier.Click += new System.EventHandler(this.btnModifier_Click);
            // 
            // btnSupprimer
            // 
            this.btnSupprimer.Text = "Archiver";
            this.btnSupprimer.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnSupprimer.Name = "btnSupprimer";
            this.btnSupprimer.Click += new System.EventHandler(this.btnSupprimer_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Text = "Exporter en Excel";
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnActualiser
            // 
            this.btnActualiser.Text = "Actualiser";
            this.btnActualiser.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnActualiser.Name = "btnActualiser";
            this.btnActualiser.Click += new System.EventHandler(this.btnActualiser_Click);
            // 
            // dgvMutuelles
            // 
            this.dgvMutuelles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIdMutuel,
            this.colMutuelle,
            this.colTaux,
            this.colTelephone,
            this.colEmail,
            this.colNbImpayees,
            this.colTotalImpaye});
            this.dgvMutuelles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMutuelles.AutoGenerateColumns = false;
            this.dgvMutuelles.ReadOnly = true;
            this.dgvMutuelles.Name = "dgvMutuelles";
            this.dgvMutuelles.SelectionChanged += new System.EventHandler(this.DgvMutuelles_SelectionChanged);
            // 
            // lblRecapImpaye
            // 
            this.lblRecapImpaye.Text = "Sélectionnez une mutuelle pour voir ses ventes à régler.";
            this.lblRecapImpaye.AutoSize = true;
            this.lblRecapImpaye.Tag = "section";
            this.lblRecapImpaye.Margin = new System.Windows.Forms.Padding(0, 8, 0, 4);
            this.lblRecapImpaye.Name = "lblRecapImpaye";
            // 
            // flpReglement
            // 
            this.flpReglement.Controls.Add(this.btnReglertout);
            this.btnReglertout.TabIndex = 0;
            this.flpReglement.Controls.Add(this.btnReglerSelection);
            this.btnReglerSelection.TabIndex = 1;
            this.flpReglement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpReglement.AutoSize = true;
            this.flpReglement.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpReglement.WrapContents = true;
            this.flpReglement.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.flpReglement.Name = "flpReglement";
            // 
            // btnReglertout
            // 
            this.btnReglertout.Text = "Régler tout";
            this.btnReglertout.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnReglertout.Tag = "primaire";
            this.btnReglertout.Enabled = false;
            this.btnReglertout.Name = "btnReglertout";
            this.btnReglertout.Click += new System.EventHandler(this.btnReglerTout_Click);
            // 
            // btnReglerSelection
            // 
            this.btnReglerSelection.Text = "Régler la sélection";
            this.btnReglerSelection.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnReglerSelection.Enabled = false;
            this.btnReglerSelection.Name = "btnReglerSelection";
            this.btnReglerSelection.Click += new System.EventHandler(this.btnReglerSelection_Click);
            // 
            // dgvVentesImpayees
            // 
            this.dgvVentesImpayees.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCoche,
            this.colVenteId,
            this.colNumeroVente,
            this.colDateVente,
            this.colClient,
            this.colMatricule,
            this.colMontantTotal,
            this.colMontantMutuelle,
            this.colVendeur});
            this.dgvVentesImpayees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVentesImpayees.AutoGenerateColumns = false;
            this.dgvVentesImpayees.AllowUserToAddRows = false;
            this.dgvVentesImpayees.Name = "dgvVentesImpayees";
            // 
            // colIdMutuel
            // 
            this.colIdMutuel.HeaderText = "Id";
            this.colIdMutuel.Name = "IdMutuel";
            this.colIdMutuel.DataPropertyName = "IdMutuel";
            this.colIdMutuel.FillWeight = 100F;
            this.colIdMutuel.MinimumWidth = 60;
            this.colIdMutuel.ReadOnly = true;
            this.colIdMutuel.Visible = false;
            // 
            // colMutuelle
            // 
            this.colMutuelle.HeaderText = "Mutuelle / Employeur";
            this.colMutuelle.Name = "Mutuelle";
            this.colMutuelle.DataPropertyName = "Mutuelle";
            this.colMutuelle.FillWeight = 30F;
            this.colMutuelle.MinimumWidth = 160;
            this.colMutuelle.ReadOnly = true;
            // 
            // colTaux
            // 
            this.colTaux.HeaderText = "Taux (%)";
            this.colTaux.Name = "Taux";
            this.colTaux.DataPropertyName = "Taux";
            this.colTaux.FillWeight = 9F;
            this.colTaux.MinimumWidth = 70;
            this.colTaux.ReadOnly = true;
            // 
            // colTelephone
            // 
            this.colTelephone.HeaderText = "Téléphone";
            this.colTelephone.Name = "Telephone";
            this.colTelephone.DataPropertyName = "Telephone";
            this.colTelephone.FillWeight = 16F;
            this.colTelephone.MinimumWidth = 110;
            this.colTelephone.ReadOnly = true;
            // 
            // colEmail
            // 
            this.colEmail.HeaderText = "E-mail";
            this.colEmail.Name = "Email";
            this.colEmail.DataPropertyName = "Email";
            this.colEmail.FillWeight = 19F;
            this.colEmail.MinimumWidth = 130;
            this.colEmail.ReadOnly = true;
            // 
            // colNbImpayees
            // 
            this.colNbImpayees.HeaderText = "Ventes impayées";
            this.colNbImpayees.Name = "NbImpayees";
            this.colNbImpayees.DataPropertyName = "NbImpayees";
            this.colNbImpayees.FillWeight = 12F;
            this.colNbImpayees.MinimumWidth = 100;
            this.colNbImpayees.ReadOnly = true;
            // 
            // colTotalImpaye
            // 
            this.colTotalImpaye.HeaderText = "Total dû";
            this.colTotalImpaye.Name = "TotalImpaye";
            this.colTotalImpaye.DataPropertyName = "TotalImpaye";
            this.colTotalImpaye.FillWeight = 14F;
            this.colTotalImpaye.MinimumWidth = 100;
            this.colTotalImpaye.ReadOnly = true;
            this.colTotalImpaye.Tag = "montant";
            // 
            // colCoche
            // 
            this.colCoche.HeaderText = "Régler";
            this.colCoche.Name = "colCoche";
            this.colCoche.DataPropertyName = "colCoche";
            this.colCoche.FillWeight = 7F;
            this.colCoche.MinimumWidth = 60;
            this.colCoche.ReadOnly = false;
            // 
            // colVenteId
            // 
            this.colVenteId.HeaderText = "Id";
            this.colVenteId.Name = "colVenteId";
            this.colVenteId.DataPropertyName = "colVenteId";
            this.colVenteId.FillWeight = 100F;
            this.colVenteId.MinimumWidth = 60;
            this.colVenteId.ReadOnly = true;
            this.colVenteId.Visible = false;
            // 
            // colNumeroVente
            // 
            this.colNumeroVente.HeaderText = "N° de vente";
            this.colNumeroVente.Name = "colNumeroVente";
            this.colNumeroVente.DataPropertyName = "colNumeroVente";
            this.colNumeroVente.FillWeight = 13F;
            this.colNumeroVente.MinimumWidth = 100;
            this.colNumeroVente.ReadOnly = true;
            // 
            // colDateVente
            // 
            this.colDateVente.HeaderText = "Date";
            this.colDateVente.Name = "colDateVente";
            this.colDateVente.DataPropertyName = "colDateVente";
            this.colDateVente.FillWeight = 14F;
            this.colDateVente.MinimumWidth = 110;
            this.colDateVente.ReadOnly = true;
            // 
            // colClient
            // 
            this.colClient.HeaderText = "Client";
            this.colClient.Name = "colClient";
            this.colClient.DataPropertyName = "colClient";
            this.colClient.FillWeight = 20F;
            this.colClient.MinimumWidth = 120;
            this.colClient.ReadOnly = true;
            // 
            // colMatricule
            // 
            this.colMatricule.HeaderText = "Matricule";
            this.colMatricule.Name = "colMatricule";
            this.colMatricule.DataPropertyName = "colMatricule";
            this.colMatricule.FillWeight = 11F;
            this.colMatricule.MinimumWidth = 90;
            this.colMatricule.ReadOnly = true;
            // 
            // colMontantTotal
            // 
            this.colMontantTotal.HeaderText = "Total de la vente";
            this.colMontantTotal.Name = "colMontantTotal";
            this.colMontantTotal.DataPropertyName = "colMontantTotal";
            this.colMontantTotal.FillWeight = 12F;
            this.colMontantTotal.MinimumWidth = 100;
            this.colMontantTotal.ReadOnly = true;
            this.colMontantTotal.Tag = "montant";
            // 
            // colMontantMutuelle
            // 
            this.colMontantMutuelle.HeaderText = "Part de la mutuelle (due)";
            this.colMontantMutuelle.Name = "colMontantMutuelle";
            this.colMontantMutuelle.DataPropertyName = "colMontantMutuelle";
            this.colMontantMutuelle.FillWeight = 14F;
            this.colMontantMutuelle.MinimumWidth = 110;
            this.colMontantMutuelle.ReadOnly = true;
            this.colMontantMutuelle.Tag = "montant";
            // 
            // colVendeur
            // 
            this.colVendeur.HeaderText = "Vendeur";
            this.colVendeur.Name = "colVendeur";
            this.colVendeur.DataPropertyName = "colVendeur";
            this.colVendeur.FillWeight = 13F;
            this.colVendeur.MinimumWidth = 100;
            this.colVendeur.ReadOnly = true;
            // 
            // Uc_Mutuelle
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(1146, 700);
            this.Name = "Uc_Mutuelle";
            this.ResumeLayout(false);
            this.flpReglement.ResumeLayout(false);
            this.flpReglement.PerformLayout();
            this.flpActions.ResumeLayout(false);
            this.flpActions.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentesImpayees)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMutuelles)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.FlowLayoutPanel flpActions;
        private System.Windows.Forms.Button btnNouvelleMutuelle;
        private System.Windows.Forms.Button btnModifier;
        private System.Windows.Forms.Button btnSupprimer;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnActualiser;
        private System.Windows.Forms.DataGridView dgvMutuelles;
        private System.Windows.Forms.Label lblRecapImpaye;
        private System.Windows.Forms.FlowLayoutPanel flpReglement;
        private System.Windows.Forms.Button btnReglertout;
        private System.Windows.Forms.Button btnReglerSelection;
        private System.Windows.Forms.DataGridView dgvVentesImpayees;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdMutuel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMutuelle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaux;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTelephone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNbImpayees;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalImpaye;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCoche;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVenteId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumeroVente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDateVente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatricule;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMontantTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMontantMutuelle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendeur;
    }
}
