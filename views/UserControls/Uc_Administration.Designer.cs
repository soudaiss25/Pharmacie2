namespace Pharmacie2.views.UserControls
{
    partial class Uc_Administration
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
            this.tlpActions = new System.Windows.Forms.TableLayoutPanel();
            this.lblSectionComptes = new System.Windows.Forms.Label();
            this.btnUtilisateurs = new System.Windows.Forms.Button();
            this.lblDescUtilisateurs = new System.Windows.Forms.Label();
            this.btnSauvegarde = new System.Windows.Forms.Button();
            this.lblDescSauvegarde = new System.Windows.Forms.Label();
            this.btnDossier = new System.Windows.Forms.Button();
            this.lblDescDossier = new System.Windows.Forms.Label();
            this.lblSectionExterne = new System.Windows.Forms.Label();
            this.lblEtatExterne = new System.Windows.Forms.Label();
            this.btnDossierExterne = new System.Windows.Forms.Button();
            this.lblDescDossierExterne = new System.Windows.Forms.Label();
            this.btnCopierMaintenant = new System.Windows.Forms.Button();
            this.lblDescCopier = new System.Windows.Forms.Label();
            this.btnRestaurer = new System.Windows.Forms.Button();
            this.lblDescRestaurer = new System.Windows.Forms.Label();
            this.lblSectionAssistance = new System.Windows.Forms.Label();
            this.btnRapport = new System.Windows.Forms.Button();
            this.lblDescRapport = new System.Windows.Forms.Label();
            this.tlpRoot.SuspendLayout();
            this.tlpActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 2;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.tlpActions, 0, 1);
            this.tlpActions.TabIndex = 1;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpRoot.AutoSize = true;
            this.tlpRoot.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Administration";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblTitre.Name = "lblTitre";
            // 
            // tlpActions
            // 
            this.tlpActions.ColumnCount = 2;
            this.tlpActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpActions.RowCount = 11;
            this.tlpActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.Controls.Add(this.lblSectionComptes, 0, 0);
            this.tlpActions.SetColumnSpan(this.lblSectionComptes, 2);
            this.lblSectionComptes.TabIndex = 0;
            this.tlpActions.Controls.Add(this.btnUtilisateurs, 0, 1);
            this.btnUtilisateurs.TabIndex = 1;
            this.tlpActions.Controls.Add(this.lblDescUtilisateurs, 1, 1);
            this.lblDescUtilisateurs.TabIndex = 2;
            this.tlpActions.Controls.Add(this.btnSauvegarde, 0, 2);
            this.btnSauvegarde.TabIndex = 3;
            this.tlpActions.Controls.Add(this.lblDescSauvegarde, 1, 2);
            this.lblDescSauvegarde.TabIndex = 4;
            this.tlpActions.Controls.Add(this.btnDossier, 0, 3);
            this.btnDossier.TabIndex = 5;
            this.tlpActions.Controls.Add(this.lblDescDossier, 1, 3);
            this.lblDescDossier.TabIndex = 6;
            this.tlpActions.Controls.Add(this.lblSectionExterne, 0, 4);
            this.tlpActions.SetColumnSpan(this.lblSectionExterne, 2);
            this.lblSectionExterne.TabIndex = 7;
            this.tlpActions.Controls.Add(this.lblEtatExterne, 0, 5);
            this.tlpActions.SetColumnSpan(this.lblEtatExterne, 2);
            this.lblEtatExterne.TabIndex = 8;
            this.tlpActions.Controls.Add(this.btnDossierExterne, 0, 6);
            this.btnDossierExterne.TabIndex = 9;
            this.tlpActions.Controls.Add(this.lblDescDossierExterne, 1, 6);
            this.lblDescDossierExterne.TabIndex = 10;
            this.tlpActions.Controls.Add(this.btnCopierMaintenant, 0, 7);
            this.btnCopierMaintenant.TabIndex = 11;
            this.tlpActions.Controls.Add(this.lblDescCopier, 1, 7);
            this.lblDescCopier.TabIndex = 12;
            this.tlpActions.Controls.Add(this.btnRestaurer, 0, 8);
            this.btnRestaurer.TabIndex = 13;
            this.tlpActions.Controls.Add(this.lblDescRestaurer, 1, 8);
            this.lblDescRestaurer.TabIndex = 14;
            this.tlpActions.Controls.Add(this.lblSectionAssistance, 0, 9);
            this.tlpActions.SetColumnSpan(this.lblSectionAssistance, 2);
            this.lblSectionAssistance.TabIndex = 15;
            this.tlpActions.Controls.Add(this.btnRapport, 0, 10);
            this.btnRapport.TabIndex = 16;
            this.tlpActions.Controls.Add(this.lblDescRapport, 1, 10);
            this.lblDescRapport.TabIndex = 17;
            this.tlpActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpActions.AutoSize = true;
            this.tlpActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpActions.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpActions.Name = "tlpActions";
            // 
            // lblSectionComptes
            // 
            this.lblSectionComptes.Text = "Comptes et données";
            this.lblSectionComptes.AutoSize = true;
            this.lblSectionComptes.Tag = "section";
            this.lblSectionComptes.Margin = new System.Windows.Forms.Padding(0, 14, 0, 4);
            this.lblSectionComptes.Name = "lblSectionComptes";
            // 
            // btnUtilisateurs
            // 
            this.btnUtilisateurs.Text = "Gérer les utilisateurs";
            this.btnUtilisateurs.Width = 290;
            this.btnUtilisateurs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnUtilisateurs.Margin = new System.Windows.Forms.Padding(0, 6, 16, 6);
            this.btnUtilisateurs.Tag = "primaire";
            this.btnUtilisateurs.Name = "btnUtilisateurs";
            this.btnUtilisateurs.Click += new System.EventHandler(this.btnUtilisateurs_Click);
            // 
            // lblDescUtilisateurs
            // 
            this.lblDescUtilisateurs.Text = "Créer, modifier ou archiver les comptes (administrateur, pharmacien, caissier).";
            this.lblDescUtilisateurs.AutoSize = true;
            this.lblDescUtilisateurs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDescUtilisateurs.MaximumSize = new System.Drawing.Size(520, 0);
            this.lblDescUtilisateurs.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.lblDescUtilisateurs.Name = "lblDescUtilisateurs";
            // 
            // btnSauvegarde
            // 
            this.btnSauvegarde.Text = "Exporter les données (CSV)";
            this.btnSauvegarde.Width = 290;
            this.btnSauvegarde.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSauvegarde.Margin = new System.Windows.Forms.Padding(0, 6, 16, 6);
            this.btnSauvegarde.Name = "btnSauvegarde";
            this.btnSauvegarde.Click += new System.EventHandler(this.btnSauvegarde_Click);
            // 
            // lblDescSauvegarde
            // 
            this.lblDescSauvegarde.Text = "Exporte toutes les données en fichiers dans le dossier de votre choix.";
            this.lblDescSauvegarde.AutoSize = true;
            this.lblDescSauvegarde.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDescSauvegarde.MaximumSize = new System.Drawing.Size(520, 0);
            this.lblDescSauvegarde.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.lblDescSauvegarde.Name = "lblDescSauvegarde";
            // 
            // btnDossier
            // 
            this.btnDossier.Text = "Ouvrir le dossier des sauvegardes";
            this.btnDossier.Width = 290;
            this.btnDossier.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnDossier.Margin = new System.Windows.Forms.Padding(0, 6, 16, 6);
            this.btnDossier.Name = "btnDossier";
            this.btnDossier.Click += new System.EventHandler(this.btnDossier_Click);
            // 
            // lblDescDossier
            // 
            this.lblDescDossier.Text = "Affiche les copies automatiques de la base de données faites par le logiciel.";
            this.lblDescDossier.AutoSize = true;
            this.lblDescDossier.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDescDossier.MaximumSize = new System.Drawing.Size(520, 0);
            this.lblDescDossier.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.lblDescDossier.Name = "lblDescDossier";
            // 
            // lblSectionExterne
            // 
            this.lblSectionExterne.Text = "Sauvegarde hors du PC";
            this.lblSectionExterne.AutoSize = true;
            this.lblSectionExterne.Tag = "section";
            this.lblSectionExterne.Margin = new System.Windows.Forms.Padding(0, 14, 0, 4);
            this.lblSectionExterne.Name = "lblSectionExterne";
            // 
            // lblEtatExterne
            // 
            this.lblEtatExterne.Text = "";
            this.lblEtatExterne.AutoSize = true;
            this.lblEtatExterne.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEtatExterne.MaximumSize = new System.Drawing.Size(820, 0);
            this.lblEtatExterne.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblEtatExterne.Name = "lblEtatExterne";
            // 
            // btnDossierExterne
            // 
            this.btnDossierExterne.Text = "Choisir le dossier externe…";
            this.btnDossierExterne.Width = 290;
            this.btnDossierExterne.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnDossierExterne.Margin = new System.Windows.Forms.Padding(0, 6, 16, 6);
            this.btnDossierExterne.Name = "btnDossierExterne";
            this.btnDossierExterne.Click += new System.EventHandler(this.btnDossierExterne_Click);
            // 
            // lblDescDossierExterne
            // 
            this.lblDescDossierExterne.Text = "Dossier « Mon Drive » de Google Drive pour ordinateur, ou clé USB. Les copies y sont chiffrées avec votre phrase secrète.";
            this.lblDescDossierExterne.AutoSize = true;
            this.lblDescDossierExterne.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDescDossierExterne.MaximumSize = new System.Drawing.Size(520, 0);
            this.lblDescDossierExterne.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.lblDescDossierExterne.Name = "lblDescDossierExterne";
            // 
            // btnCopierMaintenant
            // 
            this.btnCopierMaintenant.Text = "Copier maintenant";
            this.btnCopierMaintenant.Width = 290;
            this.btnCopierMaintenant.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCopierMaintenant.Margin = new System.Windows.Forms.Padding(0, 6, 16, 6);
            this.btnCopierMaintenant.Name = "btnCopierMaintenant";
            this.btnCopierMaintenant.Click += new System.EventHandler(this.btnCopierMaintenant_Click);
            // 
            // lblDescCopier
            // 
            this.lblDescCopier.Text = "Envoie tout de suite une copie chiffrée de la base (sinon : une fois par jour, au démarrage).";
            this.lblDescCopier.AutoSize = true;
            this.lblDescCopier.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDescCopier.MaximumSize = new System.Drawing.Size(520, 0);
            this.lblDescCopier.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.lblDescCopier.Name = "lblDescCopier";
            // 
            // btnRestaurer
            // 
            this.btnRestaurer.Text = "Restaurer une sauvegarde…";
            this.btnRestaurer.Width = 290;
            this.btnRestaurer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnRestaurer.Margin = new System.Windows.Forms.Padding(0, 6, 16, 6);
            this.btnRestaurer.Name = "btnRestaurer";
            this.btnRestaurer.Click += new System.EventHandler(this.btnRestaurer_Click);
            // 
            // lblDescRestaurer
            // 
            this.lblDescRestaurer.Text = "Remplace les données par celles d'une sauvegarde (locale ou externe). La base actuelle est d'abord mise de côté.";
            this.lblDescRestaurer.AutoSize = true;
            this.lblDescRestaurer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDescRestaurer.MaximumSize = new System.Drawing.Size(520, 0);
            this.lblDescRestaurer.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.lblDescRestaurer.Name = "lblDescRestaurer";
            // 
            // lblSectionAssistance
            // 
            this.lblSectionAssistance.Text = "Assistance";
            this.lblSectionAssistance.AutoSize = true;
            this.lblSectionAssistance.Tag = "section";
            this.lblSectionAssistance.Margin = new System.Windows.Forms.Padding(0, 14, 0, 4);
            this.lblSectionAssistance.Name = "lblSectionAssistance";
            // 
            // btnRapport
            // 
            this.btnRapport.Text = "Préparer un rapport pour le développeur";
            this.btnRapport.Width = 290;
            this.btnRapport.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnRapport.Margin = new System.Windows.Forms.Padding(0, 6, 16, 6);
            this.btnRapport.Name = "btnRapport";
            this.btnRapport.Click += new System.EventHandler(this.btnRapport_Click);
            // 
            // lblDescRapport
            // 
            this.lblDescRapport.Text = "Crée un fichier ZIP sur le Bureau (journaux filtrés et informations techniques, sans aucune donnée de la pharmacie) à envoyer par WhatsApp ou e-mail.";
            this.lblDescRapport.AutoSize = true;
            this.lblDescRapport.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDescRapport.MaximumSize = new System.Drawing.Size(520, 0);
            this.lblDescRapport.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.lblDescRapport.Name = "lblDescRapport";
            // 
            // Uc_Administration
            // 
            this.Controls.Add(this.tlpRoot);
            this.tlpRoot.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(900, 500);
            this.MinimumSize = new System.Drawing.Size(840, 480);
            this.AutoScroll = true;
            this.Name = "Uc_Administration";
            this.ResumeLayout(false);
            this.tlpActions.ResumeLayout(false);
            this.tlpActions.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.TableLayoutPanel tlpActions;
        private System.Windows.Forms.Label lblSectionComptes;
        private System.Windows.Forms.Button btnUtilisateurs;
        private System.Windows.Forms.Label lblDescUtilisateurs;
        private System.Windows.Forms.Button btnSauvegarde;
        private System.Windows.Forms.Label lblDescSauvegarde;
        private System.Windows.Forms.Button btnDossier;
        private System.Windows.Forms.Label lblDescDossier;
        private System.Windows.Forms.Label lblSectionExterne;
        private System.Windows.Forms.Label lblEtatExterne;
        private System.Windows.Forms.Button btnDossierExterne;
        private System.Windows.Forms.Label lblDescDossierExterne;
        private System.Windows.Forms.Button btnCopierMaintenant;
        private System.Windows.Forms.Label lblDescCopier;
        private System.Windows.Forms.Button btnRestaurer;
        private System.Windows.Forms.Label lblDescRestaurer;
        private System.Windows.Forms.Label lblSectionAssistance;
        private System.Windows.Forms.Button btnRapport;
        private System.Windows.Forms.Label lblDescRapport;
    }
}
