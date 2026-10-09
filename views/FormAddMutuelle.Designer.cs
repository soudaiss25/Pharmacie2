namespace Pharmacie2.views
{
    partial class FormAddMutuelle
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
            this.tlpChamps = new System.Windows.Forms.TableLayoutPanel();
            this.lblNom = new System.Windows.Forms.Label();
            this.txtNomEmployeur = new System.Windows.Forms.TextBox();
            this.lblTaux = new System.Windows.Forms.Label();
            this.numTaux = new System.Windows.Forms.NumericUpDown();
            this.lblExplTaux = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblTelephone = new System.Windows.Forms.Label();
            this.txtTelephone = new System.Windows.Forms.TextBox();
            this.panelEspace = new System.Windows.Forms.Panel();
            this.flpBoutons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnEnregistrer = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numTaux)).BeginInit();
            this.tlpRoot.SuspendLayout();
            this.tlpChamps.SuspendLayout();
            this.panelEspace.SuspendLayout();
            this.flpBoutons.SuspendLayout();
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
            this.tlpRoot.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.tlpChamps, 0, 1);
            this.tlpChamps.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.panelEspace, 0, 2);
            this.panelEspace.TabIndex = 2;
            this.tlpRoot.Controls.Add(this.flpBoutons, 0, 3);
            this.flpBoutons.TabIndex = 3;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Mutuelle";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblTitre.Name = "lblTitre";
            // 
            // tlpChamps
            // 
            this.tlpChamps.ColumnCount = 2;
            this.tlpChamps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpChamps.RowCount = 5;
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpChamps.Controls.Add(this.lblNom, 0, 0);
            this.lblNom.TabIndex = 0;
            this.tlpChamps.Controls.Add(this.txtNomEmployeur, 1, 0);
            this.txtNomEmployeur.TabIndex = 1;
            this.tlpChamps.Controls.Add(this.lblTaux, 0, 1);
            this.lblTaux.TabIndex = 2;
            this.tlpChamps.Controls.Add(this.numTaux, 1, 1);
            this.numTaux.TabIndex = 3;
            this.tlpChamps.Controls.Add(this.lblExplTaux, 1, 2);
            this.lblExplTaux.TabIndex = 4;
            this.tlpChamps.Controls.Add(this.lblEmail, 0, 3);
            this.lblEmail.TabIndex = 5;
            this.tlpChamps.Controls.Add(this.txtEmail, 1, 3);
            this.txtEmail.TabIndex = 6;
            this.tlpChamps.Controls.Add(this.lblTelephone, 0, 4);
            this.lblTelephone.TabIndex = 7;
            this.tlpChamps.Controls.Add(this.txtTelephone, 1, 4);
            this.txtTelephone.TabIndex = 8;
            this.tlpChamps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpChamps.AutoSize = true;
            this.tlpChamps.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpChamps.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.tlpChamps.Name = "tlpChamps";
            // 
            // lblNom
            // 
            this.lblNom.Text = "Nom employeur / mutuelle *";
            this.lblNom.AutoSize = true;
            this.lblNom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNom.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblNom.Name = "lblNom";
            // 
            // txtNomEmployeur
            // 
            this.txtNomEmployeur.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNomEmployeur.MaximumSize = new System.Drawing.Size(380, 0);
            this.txtNomEmployeur.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtNomEmployeur.Name = "txtNomEmployeur";
            // 
            // lblTaux
            // 
            this.lblTaux.Text = "Prise en charge (%) *";
            this.lblTaux.AutoSize = true;
            this.lblTaux.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTaux.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblTaux.Name = "lblTaux";
            // 
            // numTaux
            // 
            this.numTaux.Width = 100;
            this.numTaux.Minimum = new decimal(new int[] { 0, 0, 0, 0});
            this.numTaux.Maximum = new decimal(new int[] { 100, 0, 0, 0});
            this.numTaux.DecimalPlaces = 2;
            this.numTaux.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numTaux.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numTaux.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.numTaux.Name = "numTaux";
            // 
            // lblExplTaux
            // 
            this.lblExplTaux.Text = "Exemple : 80 = la mutuelle paie 80 %, le patient paie 20 %.";
            this.lblExplTaux.AutoSize = true;
            this.lblExplTaux.Tag = "note";
            this.lblExplTaux.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblExplTaux.Name = "lblExplTaux";
            // 
            // lblEmail
            // 
            this.lblEmail.Text = "E-mail de contact";
            this.lblEmail.AutoSize = true;
            this.lblEmail.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEmail.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblEmail.Name = "lblEmail";
            // 
            // txtEmail
            // 
            this.txtEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEmail.MaximumSize = new System.Drawing.Size(380, 0);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtEmail.Name = "txtEmail";
            // 
            // lblTelephone
            // 
            this.lblTelephone.Text = "Téléphone";
            this.lblTelephone.AutoSize = true;
            this.lblTelephone.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTelephone.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblTelephone.Name = "lblTelephone";
            // 
            // txtTelephone
            // 
            this.txtTelephone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTelephone.MaximumSize = new System.Drawing.Size(380, 0);
            this.txtTelephone.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.txtTelephone.Name = "txtTelephone";
            // 
            // panelEspace
            // 
            this.panelEspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEspace.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelEspace.Name = "panelEspace";
            // 
            // flpBoutons
            // 
            this.flpBoutons.Controls.Add(this.btnEnregistrer);
            this.btnEnregistrer.TabIndex = 0;
            this.flpBoutons.Controls.Add(this.btnAnnuler);
            this.btnAnnuler.TabIndex = 1;
            this.flpBoutons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpBoutons.AutoSize = true;
            this.flpBoutons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpBoutons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpBoutons.WrapContents = false;
            this.flpBoutons.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.flpBoutons.Name = "flpBoutons";
            // 
            // btnEnregistrer
            // 
            this.btnEnregistrer.Text = "Enregistrer";
            this.btnEnregistrer.Tag = "primaire";
            this.btnEnregistrer.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnEnregistrer.Name = "btnEnregistrer";
            this.btnEnregistrer.Click += new System.EventHandler(this.btnEnregistrer_Click);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // FormAddMutuelle
            // 
            this.Controls.Add(this.tlpRoot);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(560, 340);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mutuelle";
            this.AcceptButton = this.btnEnregistrer;
            this.CancelButton = this.btnAnnuler;
            this.Name = "FormAddMutuelle";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpBoutons.ResumeLayout(false);
            this.flpBoutons.PerformLayout();
            this.panelEspace.ResumeLayout(false);
            this.tlpChamps.ResumeLayout(false);
            this.tlpChamps.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTaux)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.TableLayoutPanel tlpChamps;
        private System.Windows.Forms.Label lblNom;
        private System.Windows.Forms.TextBox txtNomEmployeur;
        private System.Windows.Forms.Label lblTaux;
        private System.Windows.Forms.NumericUpDown numTaux;
        private System.Windows.Forms.Label lblExplTaux;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblTelephone;
        private System.Windows.Forms.TextBox txtTelephone;
        private System.Windows.Forms.Panel panelEspace;
        private System.Windows.Forms.FlowLayoutPanel flpBoutons;
        private System.Windows.Forms.Button btnEnregistrer;
        private System.Windows.Forms.Button btnAnnuler;
    }
}
