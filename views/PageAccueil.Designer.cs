namespace Pharmacie2.views
{
    partial class PageAccueil
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
            this.tlpPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.tlpMenu = new System.Windows.Forms.TableLayoutPanel();
            this.btnMenu = new Pharmacie2.views.Composants.BoutonMenu();
            this.lblNomPharmacie = new System.Windows.Forms.Label();
            this.flpMenu = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSectionQuotidien = new System.Windows.Forms.Label();
            this.btnMaJournee = new Pharmacie2.views.Composants.BoutonMenu();
            this.btnVentes = new Pharmacie2.views.Composants.BoutonMenu();
            this.BtnCaisse = new Pharmacie2.views.Composants.BoutonMenu();
            this.btn_stock = new Pharmacie2.views.Composants.BoutonMenu();
            this.btnCredits = new Pharmacie2.views.Composants.BoutonMenu();
            this.btnMutuelles = new Pharmacie2.views.Composants.BoutonMenu();
            this.lblSectionGestion = new System.Windows.Forms.Label();
            this.btnCommandes = new Pharmacie2.views.Composants.BoutonMenu();
            this.btnFournisseurs = new Pharmacie2.views.Composants.BoutonMenu();
            this.btnProduits = new Pharmacie2.views.Composants.BoutonMenu();
            this.btnDepenses = new Pharmacie2.views.Composants.BoutonMenu();
            this.btnStatistiques = new Pharmacie2.views.Composants.BoutonMenu();
            this.traitAdmin = new System.Windows.Forms.Panel();
            this.btnAdministration = new Pharmacie2.views.Composants.BoutonMenu();
            this.tlpBas = new System.Windows.Forms.TableLayoutPanel();
            this.traitMenu = new System.Windows.Forms.Panel();
            this.lblUtilisateur = new System.Windows.Forms.Label();
            this.btnDeconnexion = new Pharmacie2.views.Composants.BoutonMenu();
            this.tlpContenu = new System.Windows.Forms.TableLayoutPanel();
            this.bandeau = new Pharmacie2.views.Composants.BandeauNotification();
            this.panelContent = new System.Windows.Forms.Panel();
            this.tlpPrincipal.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.tlpMenu.SuspendLayout();
            this.flpMenu.SuspendLayout();
            this.traitAdmin.SuspendLayout();
            this.tlpBas.SuspendLayout();
            this.traitMenu.SuspendLayout();
            this.tlpContenu.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpPrincipal
            // 
            this.tlpPrincipal.ColumnCount = 2;
            this.tlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpPrincipal.RowCount = 1;
            this.tlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpPrincipal.Controls.Add(this.panelMenu, 0, 0);
            this.panelMenu.TabIndex = 0;
            this.tlpPrincipal.Controls.Add(this.tlpContenu, 1, 0);
            this.tlpContenu.TabIndex = 1;
            this.tlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPrincipal.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpPrincipal.Name = "tlpPrincipal";
            // 
            // panelMenu
            // 
            this.panelMenu.Controls.Add(this.tlpMenu);
            this.tlpMenu.TabIndex = 0;
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(27, 94, 32);
            this.panelMenu.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelMenu.Name = "panelMenu";
            // 
            // tlpMenu
            // 
            this.tlpMenu.ColumnCount = 1;
            this.tlpMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpMenu.RowCount = 4;
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpMenu.Controls.Add(this.btnMenu, 0, 0);
            this.btnMenu.TabIndex = 0;
            this.tlpMenu.Controls.Add(this.lblNomPharmacie, 0, 1);
            this.lblNomPharmacie.TabIndex = 1;
            this.tlpMenu.Controls.Add(this.flpMenu, 0, 2);
            this.flpMenu.TabIndex = 2;
            this.tlpMenu.Controls.Add(this.tlpBas, 0, 3);
            this.tlpBas.TabIndex = 3;
            this.tlpMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMenu.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpMenu.Name = "tlpMenu";
            // 
            // btnMenu
            // 
            this.btnMenu.Text = "Menu";
            this.btnMenu.Icone = "menu";
            this.btnMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMenu.Visible = false;
            this.btnMenu.Margin = new System.Windows.Forms.Padding(0, 8, 0, 4);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // lblNomPharmacie
            // 
            this.lblNomPharmacie.Text = "LIGUAPHARME";
            this.lblNomPharmacie.AutoSize = true;
            this.lblNomPharmacie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNomPharmacie.Padding = new System.Windows.Forms.Padding(12, 14, 8, 8);
            this.lblNomPharmacie.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNomPharmacie.ForeColor = System.Drawing.Color.White;
            this.lblNomPharmacie.Tag = "menuinfo";
            this.lblNomPharmacie.Name = "lblNomPharmacie";
            // 
            // flpMenu
            // 
            this.flpMenu.Controls.Add(this.lblSectionQuotidien);
            this.lblSectionQuotidien.TabIndex = 0;
            this.flpMenu.Controls.Add(this.btnMaJournee);
            this.btnMaJournee.TabIndex = 1;
            this.flpMenu.Controls.Add(this.btnVentes);
            this.btnVentes.TabIndex = 2;
            this.flpMenu.Controls.Add(this.BtnCaisse);
            this.BtnCaisse.TabIndex = 3;
            this.flpMenu.Controls.Add(this.btn_stock);
            this.btn_stock.TabIndex = 4;
            this.flpMenu.Controls.Add(this.btnCredits);
            this.btnCredits.TabIndex = 5;
            this.flpMenu.Controls.Add(this.btnMutuelles);
            this.btnMutuelles.TabIndex = 6;
            this.flpMenu.Controls.Add(this.lblSectionGestion);
            this.lblSectionGestion.TabIndex = 7;
            this.flpMenu.Controls.Add(this.btnCommandes);
            this.btnCommandes.TabIndex = 8;
            this.flpMenu.Controls.Add(this.btnFournisseurs);
            this.btnFournisseurs.TabIndex = 9;
            this.flpMenu.Controls.Add(this.btnProduits);
            this.btnProduits.TabIndex = 10;
            this.flpMenu.Controls.Add(this.btnDepenses);
            this.btnDepenses.TabIndex = 11;
            this.flpMenu.Controls.Add(this.btnStatistiques);
            this.btnStatistiques.TabIndex = 12;
            this.flpMenu.Controls.Add(this.traitAdmin);
            this.traitAdmin.TabIndex = 13;
            this.flpMenu.Controls.Add(this.btnAdministration);
            this.btnAdministration.TabIndex = 14;
            this.flpMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMenu.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMenu.WrapContents = false;
            this.flpMenu.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.flpMenu.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.flpMenu.Name = "flpMenu";
            // 
            // lblSectionQuotidien
            // 
            this.lblSectionQuotidien.Text = "AU QUOTIDIEN";
            this.lblSectionQuotidien.AutoSize = true;
            this.lblSectionQuotidien.Tag = "menusection";
            this.lblSectionQuotidien.Margin = new System.Windows.Forms.Padding(12, 8, 0, 2);
            this.lblSectionQuotidien.ForeColor = System.Drawing.Color.FromArgb(165, 214, 167);
            this.lblSectionQuotidien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSectionQuotidien.Name = "lblSectionQuotidien";
            // 
            // btnMaJournee
            // 
            this.btnMaJournee.Text = "Ma journée";
            this.btnMaJournee.Icone = "accueil";
            this.btnMaJournee.Size = new System.Drawing.Size(206, 32);
            this.btnMaJournee.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnMaJournee.Name = "btnMaJournee";
            this.btnMaJournee.Click += new System.EventHandler(this.btnMaJournee_Click);
            // 
            // btnVentes
            // 
            this.btnVentes.Text = "Vendre";
            this.btnVentes.Icone = "vendre";
            this.btnVentes.Size = new System.Drawing.Size(206, 32);
            this.btnVentes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnVentes.Name = "btnVentes";
            this.btnVentes.Click += new System.EventHandler(this.btnVentes_Click);
            // 
            // BtnCaisse
            // 
            this.BtnCaisse.Text = "Caisse";
            this.BtnCaisse.Icone = "caisse";
            this.BtnCaisse.Size = new System.Drawing.Size(206, 32);
            this.BtnCaisse.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.BtnCaisse.Name = "BtnCaisse";
            this.BtnCaisse.Click += new System.EventHandler(this.BtnCaisse_Click_1);
            // 
            // btn_stock
            // 
            this.btn_stock.Text = "Stock";
            this.btn_stock.Icone = "stock";
            this.btn_stock.Size = new System.Drawing.Size(206, 32);
            this.btn_stock.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btn_stock.Name = "btn_stock";
            this.btn_stock.Click += new System.EventHandler(this.btn_stock_Click);
            // 
            // btnCredits
            // 
            this.btnCredits.Text = "Crédits clients";
            this.btnCredits.Icone = "credits";
            this.btnCredits.Size = new System.Drawing.Size(206, 32);
            this.btnCredits.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnCredits.Name = "btnCredits";
            this.btnCredits.Click += new System.EventHandler(this.btnCredits_Click);
            // 
            // btnMutuelles
            // 
            this.btnMutuelles.Text = "Mutuelles";
            this.btnMutuelles.Icone = "mutuelles";
            this.btnMutuelles.Size = new System.Drawing.Size(206, 32);
            this.btnMutuelles.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnMutuelles.Name = "btnMutuelles";
            this.btnMutuelles.Click += new System.EventHandler(this.btnMutuelles_Click);
            // 
            // lblSectionGestion
            // 
            this.lblSectionGestion.Text = "GESTION";
            this.lblSectionGestion.AutoSize = true;
            this.lblSectionGestion.Tag = "menusection";
            this.lblSectionGestion.Margin = new System.Windows.Forms.Padding(12, 8, 0, 2);
            this.lblSectionGestion.ForeColor = System.Drawing.Color.FromArgb(165, 214, 167);
            this.lblSectionGestion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSectionGestion.Name = "lblSectionGestion";
            // 
            // btnCommandes
            // 
            this.btnCommandes.Text = "Commandes";
            this.btnCommandes.Icone = "commandes";
            this.btnCommandes.Size = new System.Drawing.Size(206, 32);
            this.btnCommandes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnCommandes.Name = "btnCommandes";
            this.btnCommandes.Click += new System.EventHandler(this.btnCommandes_Click);
            // 
            // btnFournisseurs
            // 
            this.btnFournisseurs.Text = "Fournisseurs";
            this.btnFournisseurs.Icone = "fournisseurs";
            this.btnFournisseurs.Size = new System.Drawing.Size(206, 32);
            this.btnFournisseurs.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnFournisseurs.Name = "btnFournisseurs";
            this.btnFournisseurs.Click += new System.EventHandler(this.btnFournisseurs_Click);
            // 
            // btnProduits
            // 
            this.btnProduits.Text = "Catalogue produits";
            this.btnProduits.Icone = "catalogue";
            this.btnProduits.Size = new System.Drawing.Size(206, 32);
            this.btnProduits.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnProduits.Name = "btnProduits";
            this.btnProduits.Click += new System.EventHandler(this.btnProduits_Click);
            // 
            // btnDepenses
            // 
            this.btnDepenses.Text = "Dépenses";
            this.btnDepenses.Icone = "depenses";
            this.btnDepenses.Size = new System.Drawing.Size(206, 32);
            this.btnDepenses.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnDepenses.Name = "btnDepenses";
            this.btnDepenses.Click += new System.EventHandler(this.btnDepenses_Click);
            // 
            // btnStatistiques
            // 
            this.btnStatistiques.Text = "Statistiques";
            this.btnStatistiques.Icone = "statistiques";
            this.btnStatistiques.Size = new System.Drawing.Size(206, 32);
            this.btnStatistiques.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnStatistiques.Name = "btnStatistiques";
            this.btnStatistiques.Click += new System.EventHandler(this.btnStatistiques_Click);
            // 
            // traitAdmin
            // 
            this.traitAdmin.Width = 182;
            this.traitAdmin.Height = 1;
            this.traitAdmin.BackColor = System.Drawing.Color.FromArgb(76, 140, 80);
            this.traitAdmin.Margin = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.traitAdmin.Name = "traitAdmin";
            // 
            // btnAdministration
            // 
            this.btnAdministration.Text = "Administration";
            this.btnAdministration.Icone = "administration";
            this.btnAdministration.Size = new System.Drawing.Size(206, 32);
            this.btnAdministration.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnAdministration.Name = "btnAdministration";
            this.btnAdministration.Click += new System.EventHandler(this.btnAdministration_Click);
            // 
            // tlpBas
            // 
            this.tlpBas.ColumnCount = 1;
            this.tlpBas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpBas.RowCount = 3;
            this.tlpBas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpBas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpBas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpBas.Controls.Add(this.traitMenu, 0, 0);
            this.traitMenu.TabIndex = 0;
            this.tlpBas.Controls.Add(this.lblUtilisateur, 0, 1);
            this.lblUtilisateur.TabIndex = 1;
            this.tlpBas.Controls.Add(this.btnDeconnexion, 0, 2);
            this.btnDeconnexion.TabIndex = 2;
            this.tlpBas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBas.AutoSize = true;
            this.tlpBas.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpBas.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.tlpBas.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpBas.Name = "tlpBas";
            // 
            // traitMenu
            // 
            this.traitMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.traitMenu.Height = 1;
            this.traitMenu.BackColor = System.Drawing.Color.FromArgb(76, 140, 80);
            this.traitMenu.Margin = new System.Windows.Forms.Padding(12, 0, 12, 8);
            this.traitMenu.Name = "traitMenu";
            // 
            // lblUtilisateur
            // 
            this.lblUtilisateur.Text = "Utilisateur";
            this.lblUtilisateur.AutoSize = true;
            this.lblUtilisateur.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUtilisateur.Tag = "menuinfo";
            this.lblUtilisateur.ForeColor = System.Drawing.Color.White;
            this.lblUtilisateur.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUtilisateur.Margin = new System.Windows.Forms.Padding(12, 0, 8, 4);
            this.lblUtilisateur.Name = "lblUtilisateur";
            // 
            // btnDeconnexion
            // 
            this.btnDeconnexion.Text = "Se déconnecter";
            this.btnDeconnexion.Icone = "deconnexion";
            this.btnDeconnexion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeconnexion.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnDeconnexion.Name = "btnDeconnexion";
            this.btnDeconnexion.Click += new System.EventHandler(this.btnDeconnexion_Click);
            // 
            // tlpContenu
            // 
            this.tlpContenu.ColumnCount = 1;
            this.tlpContenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpContenu.RowCount = 2;
            this.tlpContenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpContenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpContenu.Controls.Add(this.bandeau, 0, 0);
            this.bandeau.TabIndex = 0;
            this.tlpContenu.Controls.Add(this.panelContent, 0, 1);
            this.panelContent.TabIndex = 1;
            this.tlpContenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContenu.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpContenu.Name = "tlpContenu";
            // 
            // bandeau
            // 
            this.bandeau.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bandeau.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.bandeau.Name = "bandeau";
            // 
            // panelContent
            // 
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.AutoScroll = true;
            this.panelContent.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelContent.Name = "panelContent";
            // 
            // PageAccueil
            // 
            this.Controls.Add(this.tlpPrincipal);
            this.tlpPrincipal.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(1280, 640);
            this.MinimumSize = new System.Drawing.Size(1024, 600);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.Text = "LIGUAPHARME — Gestion";
            this.Name = "PageAccueil";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PageAccueil_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.tlpContenu.ResumeLayout(false);
            this.tlpContenu.PerformLayout();
            this.traitMenu.ResumeLayout(false);
            this.tlpBas.ResumeLayout(false);
            this.tlpBas.PerformLayout();
            this.traitAdmin.ResumeLayout(false);
            this.flpMenu.ResumeLayout(false);
            this.flpMenu.PerformLayout();
            this.tlpMenu.ResumeLayout(false);
            this.tlpMenu.PerformLayout();
            this.panelMenu.ResumeLayout(false);
            this.tlpPrincipal.ResumeLayout(false);
            this.tlpPrincipal.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpPrincipal;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.TableLayoutPanel tlpMenu;
        private Pharmacie2.views.Composants.BoutonMenu btnMenu;
        private System.Windows.Forms.Label lblNomPharmacie;
        private System.Windows.Forms.FlowLayoutPanel flpMenu;
        private System.Windows.Forms.Label lblSectionQuotidien;
        private Pharmacie2.views.Composants.BoutonMenu btnMaJournee;
        private Pharmacie2.views.Composants.BoutonMenu btnVentes;
        private Pharmacie2.views.Composants.BoutonMenu BtnCaisse;
        private Pharmacie2.views.Composants.BoutonMenu btn_stock;
        private Pharmacie2.views.Composants.BoutonMenu btnCredits;
        private Pharmacie2.views.Composants.BoutonMenu btnMutuelles;
        private System.Windows.Forms.Label lblSectionGestion;
        private Pharmacie2.views.Composants.BoutonMenu btnCommandes;
        private Pharmacie2.views.Composants.BoutonMenu btnFournisseurs;
        private Pharmacie2.views.Composants.BoutonMenu btnProduits;
        private Pharmacie2.views.Composants.BoutonMenu btnDepenses;
        private Pharmacie2.views.Composants.BoutonMenu btnStatistiques;
        private System.Windows.Forms.Panel traitAdmin;
        private Pharmacie2.views.Composants.BoutonMenu btnAdministration;
        private System.Windows.Forms.TableLayoutPanel tlpBas;
        private System.Windows.Forms.Panel traitMenu;
        private System.Windows.Forms.Label lblUtilisateur;
        private Pharmacie2.views.Composants.BoutonMenu btnDeconnexion;
        private System.Windows.Forms.TableLayoutPanel tlpContenu;
        private Pharmacie2.views.Composants.BandeauNotification bandeau;
        private System.Windows.Forms.Panel panelContent;
    }
}
