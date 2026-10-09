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
            this.btnUtilisateurs = new System.Windows.Forms.Button();
            this.lblDescUtilisateurs = new System.Windows.Forms.Label();
            this.btnSauvegarde = new System.Windows.Forms.Button();
            this.lblDescSauvegarde = new System.Windows.Forms.Label();
            this.btnDossier = new System.Windows.Forms.Button();
            this.lblDescDossier = new System.Windows.Forms.Label();
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
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.tlpActions, 0, 1);
            this.tlpActions.TabIndex = 1;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Administration";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.lblTitre.Name = "lblTitre";
            // 
            // tlpActions
            // 
            this.tlpActions.ColumnCount = 2;
            this.tlpActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpActions.RowCount = 3;
            this.tlpActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpActions.Controls.Add(this.btnUtilisateurs, 0, 0);
            this.btnUtilisateurs.TabIndex = 0;
            this.tlpActions.Controls.Add(this.lblDescUtilisateurs, 1, 0);
            this.lblDescUtilisateurs.TabIndex = 1;
            this.tlpActions.Controls.Add(this.btnSauvegarde, 0, 1);
            this.btnSauvegarde.TabIndex = 2;
            this.tlpActions.Controls.Add(this.lblDescSauvegarde, 1, 1);
            this.lblDescSauvegarde.TabIndex = 3;
            this.tlpActions.Controls.Add(this.btnDossier, 0, 2);
            this.btnDossier.TabIndex = 4;
            this.tlpActions.Controls.Add(this.lblDescDossier, 1, 2);
            this.lblDescDossier.TabIndex = 5;
            this.tlpActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpActions.AutoSize = true;
            this.tlpActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpActions.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpActions.Name = "tlpActions";
            // 
            // btnUtilisateurs
            // 
            this.btnUtilisateurs.Text = "Gérer les utilisateurs";
            this.btnUtilisateurs.Width = 260;
            this.btnUtilisateurs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnUtilisateurs.Margin = new System.Windows.Forms.Padding(0, 10, 16, 10);
            this.btnUtilisateurs.Tag = "primaire";
            this.btnUtilisateurs.Name = "btnUtilisateurs";
            this.btnUtilisateurs.Click += new System.EventHandler(this.btnUtilisateurs_Click);
            // 
            // lblDescUtilisateurs
            // 
            this.lblDescUtilisateurs.Text = "Créer, modifier ou archiver les comptes (administrateur, pharmacien, caissier).";
            this.lblDescUtilisateurs.AutoSize = true;
            this.lblDescUtilisateurs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDescUtilisateurs.MaximumSize = new System.Drawing.Size(560, 0);
            this.lblDescUtilisateurs.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.lblDescUtilisateurs.Name = "lblDescUtilisateurs";
            // 
            // btnSauvegarde
            // 
            this.btnSauvegarde.Text = "Sauvegarder les données";
            this.btnSauvegarde.Width = 260;
            this.btnSauvegarde.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSauvegarde.Margin = new System.Windows.Forms.Padding(0, 10, 16, 10);
            this.btnSauvegarde.Name = "btnSauvegarde";
            this.btnSauvegarde.Click += new System.EventHandler(this.btnSauvegarde_Click);
            // 
            // lblDescSauvegarde
            // 
            this.lblDescSauvegarde.Text = "Exporte toutes les données en fichiers dans le dossier de votre choix.";
            this.lblDescSauvegarde.AutoSize = true;
            this.lblDescSauvegarde.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDescSauvegarde.MaximumSize = new System.Drawing.Size(560, 0);
            this.lblDescSauvegarde.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.lblDescSauvegarde.Name = "lblDescSauvegarde";
            // 
            // btnDossier
            // 
            this.btnDossier.Text = "Ouvrir le dossier des sauvegardes";
            this.btnDossier.Width = 260;
            this.btnDossier.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnDossier.Margin = new System.Windows.Forms.Padding(0, 10, 16, 10);
            this.btnDossier.Name = "btnDossier";
            this.btnDossier.Click += new System.EventHandler(this.btnDossier_Click);
            // 
            // lblDescDossier
            // 
            this.lblDescDossier.Text = "Affiche les copies automatiques de la base de données faites par le logiciel.";
            this.lblDescDossier.AutoSize = true;
            this.lblDescDossier.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDescDossier.MaximumSize = new System.Drawing.Size(560, 0);
            this.lblDescDossier.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.lblDescDossier.Name = "lblDescDossier";
            // 
            // Uc_Administration
            // 
            this.Controls.Add(this.tlpRoot);
            this.tlpRoot.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(900, 400);
            this.MinimumSize = new System.Drawing.Size(700, 320);
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
        private System.Windows.Forms.Button btnUtilisateurs;
        private System.Windows.Forms.Label lblDescUtilisateurs;
        private System.Windows.Forms.Button btnSauvegarde;
        private System.Windows.Forms.Label lblDescSauvegarde;
        private System.Windows.Forms.Button btnDossier;
        private System.Windows.Forms.Label lblDescDossier;
    }
}
