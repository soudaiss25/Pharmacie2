namespace Pharmacie2.views.UserControls
{
    partial class Uc_Utilisateurs
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
            this.tlpEntete = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitre = new System.Windows.Forms.Label();
            this.tlpEnteteActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNouvel = new System.Windows.Forms.Button();
            this.btnModifier = new System.Windows.Forms.Button();
            this.btnSupprimer = new System.Windows.Forms.Button();
            this.tlpFiltres = new System.Windows.Forms.TableLayoutPanel();
            this.lblRecherche = new System.Windows.Forms.Label();
            this.txtRecherche = new System.Windows.Forms.TextBox();
            this.btnEffacerRecherche = new System.Windows.Forms.Button();
            this.tlpCorps = new System.Windows.Forms.TableLayoutPanel();
            this.dgvUtilisateurs = new System.Windows.Forms.DataGridView();
            this.pnlFormulaire = new System.Windows.Forms.TableLayoutPanel();
            this.lblFormTitre = new System.Windows.Forms.Label();
            this.lblNom = new System.Windows.Forms.Label();
            this.txtNom = new System.Windows.Forms.TextBox();
            this.lblPrenom = new System.Windows.Forms.Label();
            this.txtPrenom = new System.Windows.Forms.TextBox();
            this.lblLogin = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.lblMdp = new System.Windows.Forms.Label();
            this.txtMotDePasse = new System.Windows.Forms.TextBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.cbRole = new System.Windows.Forms.ComboBox();
            this.flpBoutonsFormulaire = new System.Windows.Forms.FlowLayoutPanel();
            this.btnEnregistrer = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.lblCompteur = new System.Windows.Forms.Label();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrenom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLogin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUtilisateurs)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.tlpEntete.SuspendLayout();
            this.tlpEnteteActions.SuspendLayout();
            this.tlpFiltres.SuspendLayout();
            this.tlpCorps.SuspendLayout();
            this.pnlFormulaire.SuspendLayout();
            this.flpBoutonsFormulaire.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 4;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.tlpEntete, 0, 0);
            this.tlpEntete.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.tlpFiltres, 0, 1);
            this.tlpFiltres.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.tlpCorps, 0, 2);
            this.tlpCorps.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.lblCompteur, 0, 3);
            this.lblCompteur.TabIndex = 3;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // tlpEntete
            // 
            this.tlpEntete.ColumnCount = 3;
            this.tlpEntete.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpEntete.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.RowCount = 1;
            this.tlpEntete.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpEntete.Controls.Add(this.tlpEnteteActions, 2, 0);
            this.tlpEnteteActions.TabIndex = 1;
            this.tlpEntete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEntete.AutoSize = true;
            this.tlpEntete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpEntete.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.tlpEntete.Name = "tlpEntete";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Utilisateurs";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.lblTitre.Name = "lblTitre";
            // 
            // tlpEnteteActions
            // 
            this.tlpEnteteActions.Controls.Add(this.btnNouvel);
            this.btnNouvel.TabIndex = 0;
            this.tlpEnteteActions.Controls.Add(this.btnModifier);
            this.btnModifier.TabIndex = 1;
            this.tlpEnteteActions.Controls.Add(this.btnSupprimer);
            this.btnSupprimer.TabIndex = 2;
            this.tlpEnteteActions.AutoSize = true;
            this.tlpEnteteActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpEnteteActions.WrapContents = false;
            this.tlpEnteteActions.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tlpEnteteActions.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpEnteteActions.Name = "tlpEnteteActions";
            // 
            // btnNouvel
            // 
            this.btnNouvel.Text = "Nouvel utilisateur";
            this.btnNouvel.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnNouvel.Tag = "primaire";
            this.btnNouvel.Name = "btnNouvel";
            this.btnNouvel.Click += new System.EventHandler(this.btnNouvel_Click);
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
            // tlpFiltres
            // 
            this.tlpFiltres.ColumnCount = 4;
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpFiltres.RowCount = 1;
            this.tlpFiltres.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltres.Controls.Add(this.lblRecherche, 0, 0);
            this.lblRecherche.TabIndex = 0;
            this.tlpFiltres.Controls.Add(this.txtRecherche, 1, 0);
            this.txtRecherche.TabIndex = 1;
            this.tlpFiltres.Controls.Add(this.btnEffacerRecherche, 2, 0);
            this.btnEffacerRecherche.TabIndex = 2;
            this.tlpFiltres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFiltres.AutoSize = true;
            this.tlpFiltres.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpFiltres.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.tlpFiltres.Name = "tlpFiltres";
            // 
            // lblRecherche
            // 
            this.lblRecherche.Text = "Rechercher";
            this.lblRecherche.AutoSize = true;
            this.lblRecherche.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblRecherche.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblRecherche.Name = "lblRecherche";
            // 
            // txtRecherche
            // 
            this.txtRecherche.Width = 260;
            this.txtRecherche.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtRecherche.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.txtRecherche.PlaceholderText = "Nom, prénom, identifiant ou rôle";
            this.txtRecherche.Name = "txtRecherche";
            this.txtRecherche.TextChanged += new System.EventHandler(this.txtRecherche_TextChanged);
            // 
            // btnEffacerRecherche
            // 
            this.btnEffacerRecherche.Text = "Effacer";
            this.btnEffacerRecherche.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnEffacerRecherche.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnEffacerRecherche.Name = "btnEffacerRecherche";
            this.btnEffacerRecherche.Click += new System.EventHandler(this.btnEffacerRecherche_Click);
            // 
            // tlpCorps
            // 
            this.tlpCorps.ColumnCount = 2;
            this.tlpCorps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.0F));
            this.tlpCorps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.0F));
            this.tlpCorps.RowCount = 1;
            this.tlpCorps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpCorps.Controls.Add(this.dgvUtilisateurs, 0, 0);
            this.dgvUtilisateurs.TabIndex = 0;
            this.tlpCorps.Controls.Add(this.pnlFormulaire, 1, 0);
            this.pnlFormulaire.TabIndex = 1;
            this.tlpCorps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCorps.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpCorps.Name = "tlpCorps";
            // 
            // dgvUtilisateurs
            // 
            this.dgvUtilisateurs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colNom,
            this.colPrenom,
            this.colLogin,
            this.colRole,
            this.colStatut});
            this.dgvUtilisateurs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUtilisateurs.AutoGenerateColumns = false;
            this.dgvUtilisateurs.ReadOnly = true;
            this.dgvUtilisateurs.Name = "dgvUtilisateurs";
            this.dgvUtilisateurs.SelectionChanged += new System.EventHandler(this.dgvUtilisateurs_SelectionChanged);
            // 
            // pnlFormulaire
            // 
            this.pnlFormulaire.ColumnCount = 1;
            this.pnlFormulaire.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.pnlFormulaire.RowCount = 12;
            this.pnlFormulaire.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFormulaire.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFormulaire.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFormulaire.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFormulaire.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFormulaire.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFormulaire.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFormulaire.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFormulaire.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFormulaire.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFormulaire.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFormulaire.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.pnlFormulaire.Controls.Add(this.lblFormTitre, 0, 0);
            this.lblFormTitre.TabIndex = 0;
            this.pnlFormulaire.Controls.Add(this.lblNom, 0, 1);
            this.lblNom.TabIndex = 1;
            this.pnlFormulaire.Controls.Add(this.txtNom, 0, 2);
            this.txtNom.TabIndex = 2;
            this.pnlFormulaire.Controls.Add(this.lblPrenom, 0, 3);
            this.lblPrenom.TabIndex = 3;
            this.pnlFormulaire.Controls.Add(this.txtPrenom, 0, 4);
            this.txtPrenom.TabIndex = 4;
            this.pnlFormulaire.Controls.Add(this.lblLogin, 0, 5);
            this.lblLogin.TabIndex = 5;
            this.pnlFormulaire.Controls.Add(this.txtLogin, 0, 6);
            this.txtLogin.TabIndex = 6;
            this.pnlFormulaire.Controls.Add(this.lblMdp, 0, 7);
            this.lblMdp.TabIndex = 7;
            this.pnlFormulaire.Controls.Add(this.txtMotDePasse, 0, 8);
            this.txtMotDePasse.TabIndex = 8;
            this.pnlFormulaire.Controls.Add(this.lblRole, 0, 9);
            this.lblRole.TabIndex = 9;
            this.pnlFormulaire.Controls.Add(this.cbRole, 0, 10);
            this.cbRole.TabIndex = 10;
            this.pnlFormulaire.Controls.Add(this.flpBoutonsFormulaire, 0, 11);
            this.flpBoutonsFormulaire.TabIndex = 11;
            this.pnlFormulaire.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFormulaire.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.pnlFormulaire.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.pnlFormulaire.Tag = "carte";
            this.pnlFormulaire.Name = "pnlFormulaire";
            // 
            // lblFormTitre
            // 
            this.lblFormTitre.Text = "Fiche utilisateur";
            this.lblFormTitre.AutoSize = true;
            this.lblFormTitre.Tag = "section";
            this.lblFormTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblFormTitre.Name = "lblFormTitre";
            // 
            // lblNom
            // 
            this.lblNom.Text = "Nom";
            this.lblNom.AutoSize = true;
            this.lblNom.Margin = new System.Windows.Forms.Padding(0, 6, 0, 2);
            this.lblNom.Name = "lblNom";
            // 
            // txtNom
            // 
            this.txtNom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNom.Enabled = false;
            this.txtNom.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.txtNom.Name = "txtNom";
            // 
            // lblPrenom
            // 
            this.lblPrenom.Text = "Prénom";
            this.lblPrenom.AutoSize = true;
            this.lblPrenom.Margin = new System.Windows.Forms.Padding(0, 6, 0, 2);
            this.lblPrenom.Name = "lblPrenom";
            // 
            // txtPrenom
            // 
            this.txtPrenom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPrenom.Enabled = false;
            this.txtPrenom.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.txtPrenom.Name = "txtPrenom";
            // 
            // lblLogin
            // 
            this.lblLogin.Text = "Identifiant de connexion";
            this.lblLogin.AutoSize = true;
            this.lblLogin.Margin = new System.Windows.Forms.Padding(0, 6, 0, 2);
            this.lblLogin.Name = "lblLogin";
            // 
            // txtLogin
            // 
            this.txtLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogin.Enabled = false;
            this.txtLogin.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.txtLogin.Name = "txtLogin";
            // 
            // lblMdp
            // 
            this.lblMdp.Text = "Mot de passe";
            this.lblMdp.AutoSize = true;
            this.lblMdp.Margin = new System.Windows.Forms.Padding(0, 6, 0, 2);
            this.lblMdp.Name = "lblMdp";
            // 
            // txtMotDePasse
            // 
            this.txtMotDePasse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMotDePasse.Enabled = false;
            this.txtMotDePasse.UseSystemPasswordChar = true;
            this.txtMotDePasse.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.txtMotDePasse.Name = "txtMotDePasse";
            // 
            // lblRole
            // 
            this.lblRole.Text = "Rôle";
            this.lblRole.AutoSize = true;
            this.lblRole.Margin = new System.Windows.Forms.Padding(0, 6, 0, 2);
            this.lblRole.Name = "lblRole";
            // 
            // cbRole
            // 
            this.cbRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRole.Enabled = false;
            this.cbRole.Items.AddRange(new object[] {
            "Administrateur",
            "Pharmacien",
            "Caissier"});
            this.cbRole.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.cbRole.Name = "cbRole";
            // 
            // flpBoutonsFormulaire
            // 
            this.flpBoutonsFormulaire.Controls.Add(this.btnEnregistrer);
            this.btnEnregistrer.TabIndex = 0;
            this.flpBoutonsFormulaire.Controls.Add(this.btnAnnuler);
            this.btnAnnuler.TabIndex = 1;
            this.flpBoutonsFormulaire.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpBoutonsFormulaire.AutoSize = true;
            this.flpBoutonsFormulaire.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpBoutonsFormulaire.WrapContents = true;
            this.flpBoutonsFormulaire.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.flpBoutonsFormulaire.Name = "flpBoutonsFormulaire";
            // 
            // btnEnregistrer
            // 
            this.btnEnregistrer.Text = "Enregistrer";
            this.btnEnregistrer.Tag = "primaire";
            this.btnEnregistrer.Visible = false;
            this.btnEnregistrer.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnEnregistrer.Name = "btnEnregistrer";
            this.btnEnregistrer.Click += new System.EventHandler(this.btnEnregistrer_Click);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.Visible = false;
            this.btnAnnuler.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // lblCompteur
            // 
            this.lblCompteur.Text = "";
            this.lblCompteur.AutoSize = true;
            this.lblCompteur.Tag = "note";
            this.lblCompteur.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblCompteur.Name = "lblCompteur";
            // 
            // colId
            // 
            this.colId.HeaderText = "Id";
            this.colId.Name = "Id";
            this.colId.DataPropertyName = "Id";
            this.colId.FillWeight = 100F;
            this.colId.MinimumWidth = 60;
            this.colId.ReadOnly = true;
            this.colId.Visible = false;
            // 
            // colNom
            // 
            this.colNom.HeaderText = "Nom";
            this.colNom.Name = "Nom";
            this.colNom.DataPropertyName = "Nom";
            this.colNom.FillWeight = 25F;
            this.colNom.MinimumWidth = 100;
            this.colNom.ReadOnly = true;
            // 
            // colPrenom
            // 
            this.colPrenom.HeaderText = "Prénom";
            this.colPrenom.Name = "Prenom";
            this.colPrenom.DataPropertyName = "Prenom";
            this.colPrenom.FillWeight = 25F;
            this.colPrenom.MinimumWidth = 100;
            this.colPrenom.ReadOnly = true;
            // 
            // colLogin
            // 
            this.colLogin.HeaderText = "Identifiant";
            this.colLogin.Name = "Login";
            this.colLogin.DataPropertyName = "Login";
            this.colLogin.FillWeight = 22F;
            this.colLogin.MinimumWidth = 100;
            this.colLogin.ReadOnly = true;
            // 
            // colRole
            // 
            this.colRole.HeaderText = "Rôle";
            this.colRole.Name = "Role";
            this.colRole.DataPropertyName = "Role";
            this.colRole.FillWeight = 18F;
            this.colRole.MinimumWidth = 110;
            this.colRole.ReadOnly = true;
            // 
            // colStatut
            // 
            this.colStatut.HeaderText = "Statut";
            this.colStatut.Name = "Statut";
            this.colStatut.DataPropertyName = "Statut";
            this.colStatut.FillWeight = 10F;
            this.colStatut.MinimumWidth = 80;
            this.colStatut.ReadOnly = true;
            // 
            // Uc_Utilisateurs
            // 
            this.Controls.Add(this.tlpRoot);
            this.tlpRoot.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(1000, 560);
            this.MinimumSize = new System.Drawing.Size(840, 480);
            this.Name = "Uc_Utilisateurs";
            this.ResumeLayout(false);
            this.flpBoutonsFormulaire.ResumeLayout(false);
            this.flpBoutonsFormulaire.PerformLayout();
            this.pnlFormulaire.ResumeLayout(false);
            this.pnlFormulaire.PerformLayout();
            this.tlpCorps.ResumeLayout(false);
            this.tlpCorps.PerformLayout();
            this.tlpFiltres.ResumeLayout(false);
            this.tlpFiltres.PerformLayout();
            this.tlpEnteteActions.ResumeLayout(false);
            this.tlpEnteteActions.PerformLayout();
            this.tlpEntete.ResumeLayout(false);
            this.tlpEntete.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUtilisateurs)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.TableLayoutPanel tlpEntete;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.FlowLayoutPanel tlpEnteteActions;
        private System.Windows.Forms.Button btnNouvel;
        private System.Windows.Forms.Button btnModifier;
        private System.Windows.Forms.Button btnSupprimer;
        private System.Windows.Forms.TableLayoutPanel tlpFiltres;
        private System.Windows.Forms.Label lblRecherche;
        private System.Windows.Forms.TextBox txtRecherche;
        private System.Windows.Forms.Button btnEffacerRecherche;
        private System.Windows.Forms.TableLayoutPanel tlpCorps;
        private System.Windows.Forms.DataGridView dgvUtilisateurs;
        private System.Windows.Forms.TableLayoutPanel pnlFormulaire;
        private System.Windows.Forms.Label lblFormTitre;
        private System.Windows.Forms.Label lblNom;
        private System.Windows.Forms.TextBox txtNom;
        private System.Windows.Forms.Label lblPrenom;
        private System.Windows.Forms.TextBox txtPrenom;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Label lblMdp;
        private System.Windows.Forms.TextBox txtMotDePasse;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.ComboBox cbRole;
        private System.Windows.Forms.FlowLayoutPanel flpBoutonsFormulaire;
        private System.Windows.Forms.Button btnEnregistrer;
        private System.Windows.Forms.Button btnAnnuler;
        private System.Windows.Forms.Label lblCompteur;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrenom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLogin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRole;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatut;
    }
}
