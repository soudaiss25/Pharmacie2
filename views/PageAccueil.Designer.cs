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
            this.lblSectionAdministration = new System.Windows.Forms.Label();
            this.BtnGestionUtilisateur = new Pharmacie2.views.Composants.BoutonMenu();
            this.btnSauvegarde = new Pharmacie2.views.Composants.BoutonMenu();
            this.btnDossierSauvegardes = new Pharmacie2.views.Composants.BoutonMenu();
            this.tlpBas = new System.Windows.Forms.TableLayoutPanel();
            this.lblUtilisateur = new System.Windows.Forms.Label();
            this.btnDeconnexion = new System.Windows.Forms.Button();
            this.tlpContenu = new System.Windows.Forms.TableLayoutPanel();
            this.bandeau = new Pharmacie2.views.Composants.BandeauNotification();
            this.panelContent = new System.Windows.Forms.Panel();
            this.tlpPrincipal.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.tlpMenu.SuspendLayout();
            this.flpMenu.SuspendLayout();
            this.tlpBas.SuspendLayout();
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
            this.tlpPrincipal.Controls.Add(this.tlpContenu, 1, 0);
            this.tlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPrincipal.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpPrincipal.Name = "tlpPrincipal";
            // 
            // panelMenu
            // 
            this.panelMenu.Controls.Add(this.tlpMenu);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(27, 94, 32);
            this.panelMenu.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelMenu.Name = "panelMenu";
            // 
            // tlpMenu
            // 
            this.tlpMenu.ColumnCount = 1;
            this.tlpMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpMenu.RowCount = 3;
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpMenu.Controls.Add(this.lblNomPharmacie, 0, 0);
            this.tlpMenu.Controls.Add(this.flpMenu, 0, 1);
            this.tlpMenu.Controls.Add(this.tlpBas, 0, 2);
            this.tlpMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMenu.Name = "tlpMenu";
            // 
            // lblNomPharmacie
            // 
            this.lblNomPharmacie.Text = "LIGUAPHARME";
            this.lblNomPharmacie.AutoSize = true;
            this.lblNomPharmacie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNomPharmacie.Padding = new System.Windows.Forms.Padding(16, 16, 8, 8);
            this.lblNomPharmacie.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNomPharmacie.ForeColor = System.Drawing.Color.White;
            this.lblNomPharmacie.Tag = "menuinfo";
            this.lblNomPharmacie.Name = "lblNomPharmacie";
            // 
            // flpMenu
            // 
            this.flpMenu.Controls.Add(this.lblSectionQuotidien);
            this.flpMenu.Controls.Add(this.btnMaJournee);
            this.flpMenu.Controls.Add(this.btnVentes);
            this.flpMenu.Controls.Add(this.BtnCaisse);
            this.flpMenu.Controls.Add(this.btn_stock);
            this.flpMenu.Controls.Add(this.btnCredits);
            this.flpMenu.Controls.Add(this.btnMutuelles);
            this.flpMenu.Controls.Add(this.lblSectionGestion);
            this.flpMenu.Controls.Add(this.btnCommandes);
            this.flpMenu.Controls.Add(this.btnFournisseurs);
            this.flpMenu.Controls.Add(this.btnProduits);
            this.flpMenu.Controls.Add(this.btnDepenses);
            this.flpMenu.Controls.Add(this.btnStatistiques);
            this.flpMenu.Controls.Add(this.lblSectionAdministration);
            this.flpMenu.Controls.Add(this.BtnGestionUtilisateur);
            this.flpMenu.Controls.Add(this.btnSauvegarde);
            this.flpMenu.Controls.Add(this.btnDossierSauvegardes);
            this.flpMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMenu.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMenu.WrapContents = false;
            this.flpMenu.AutoScroll = true;
            this.flpMenu.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.flpMenu.Name = "flpMenu";
            // 
            // lblSectionQuotidien
            // 
            this.lblSectionQuotidien.Text = "AU QUOTIDIEN";
            this.lblSectionQuotidien.AutoSize = true;
            this.lblSectionQuotidien.Tag = "menusection";
            this.lblSectionQuotidien.Margin = new System.Windows.Forms.Padding(8, 14, 0, 4);
            this.lblSectionQuotidien.ForeColor = System.Drawing.Color.FromArgb(165, 214, 167);
            this.lblSectionQuotidien.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSectionQuotidien.Name = "lblSectionQuotidien";
            // 
            // btnMaJournee
            // 
            this.btnMaJournee.Text = "Ma journée";
            this.btnMaJournee.Size = new System.Drawing.Size(206, 40);
            this.btnMaJournee.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnMaJournee.Name = "btnMaJournee";
            this.btnMaJournee.Click += new System.EventHandler(this.btnMaJournee_Click);
            // 
            // btnVentes
            // 
            this.btnVentes.Text = "Vendre";
            this.btnVentes.Size = new System.Drawing.Size(206, 40);
            this.btnVentes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnVentes.Name = "btnVentes";
            this.btnVentes.Click += new System.EventHandler(this.btnVentes_Click);
            // 
            // BtnCaisse
            // 
            this.BtnCaisse.Text = "Caisse";
            this.BtnCaisse.Size = new System.Drawing.Size(206, 40);
            this.BtnCaisse.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.BtnCaisse.Name = "BtnCaisse";
            this.BtnCaisse.Click += new System.EventHandler(this.BtnCaisse_Click_1);
            // 
            // btn_stock
            // 
            this.btn_stock.Text = "Stock";
            this.btn_stock.Size = new System.Drawing.Size(206, 40);
            this.btn_stock.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btn_stock.Name = "btn_stock";
            this.btn_stock.Click += new System.EventHandler(this.btn_stock_Click);
            // 
            // btnCredits
            // 
            this.btnCredits.Text = "Crédits clients";
            this.btnCredits.Size = new System.Drawing.Size(206, 40);
            this.btnCredits.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnCredits.Name = "btnCredits";
            this.btnCredits.Click += new System.EventHandler(this.btnCredits_Click);
            // 
            // btnMutuelles
            // 
            this.btnMutuelles.Text = "Mutuelles";
            this.btnMutuelles.Size = new System.Drawing.Size(206, 40);
            this.btnMutuelles.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnMutuelles.Name = "btnMutuelles";
            this.btnMutuelles.Click += new System.EventHandler(this.btnMutuelles_Click);
            // 
            // lblSectionGestion
            // 
            this.lblSectionGestion.Text = "GESTION";
            this.lblSectionGestion.AutoSize = true;
            this.lblSectionGestion.Tag = "menusection";
            this.lblSectionGestion.Margin = new System.Windows.Forms.Padding(8, 14, 0, 4);
            this.lblSectionGestion.ForeColor = System.Drawing.Color.FromArgb(165, 214, 167);
            this.lblSectionGestion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSectionGestion.Name = "lblSectionGestion";
            // 
            // btnCommandes
            // 
            this.btnCommandes.Text = "Commandes";
            this.btnCommandes.Size = new System.Drawing.Size(206, 40);
            this.btnCommandes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnCommandes.Name = "btnCommandes";
            this.btnCommandes.Click += new System.EventHandler(this.btnCommandes_Click);
            // 
            // btnFournisseurs
            // 
            this.btnFournisseurs.Text = "Fournisseurs";
            this.btnFournisseurs.Size = new System.Drawing.Size(206, 40);
            this.btnFournisseurs.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnFournisseurs.Name = "btnFournisseurs";
            this.btnFournisseurs.Click += new System.EventHandler(this.btnFournisseurs_Click);
            // 
            // btnProduits
            // 
            this.btnProduits.Text = "Catalogue produits";
            this.btnProduits.Size = new System.Drawing.Size(206, 40);
            this.btnProduits.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnProduits.Name = "btnProduits";
            this.btnProduits.Click += new System.EventHandler(this.btnProduits_Click);
            // 
            // btnDepenses
            // 
            this.btnDepenses.Text = "Dépenses";
            this.btnDepenses.Size = new System.Drawing.Size(206, 40);
            this.btnDepenses.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnDepenses.Name = "btnDepenses";
            this.btnDepenses.Click += new System.EventHandler(this.btnDepenses_Click);
            // 
            // btnStatistiques
            // 
            this.btnStatistiques.Text = "Statistiques";
            this.btnStatistiques.Size = new System.Drawing.Size(206, 40);
            this.btnStatistiques.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnStatistiques.Name = "btnStatistiques";
            this.btnStatistiques.Click += new System.EventHandler(this.btnStatistiques_Click);
            // 
            // lblSectionAdministration
            // 
            this.lblSectionAdministration.Text = "ADMINISTRATION";
            this.lblSectionAdministration.AutoSize = true;
            this.lblSectionAdministration.Tag = "menusection";
            this.lblSectionAdministration.Margin = new System.Windows.Forms.Padding(8, 14, 0, 4);
            this.lblSectionAdministration.ForeColor = System.Drawing.Color.FromArgb(165, 214, 167);
            this.lblSectionAdministration.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSectionAdministration.Name = "lblSectionAdministration";
            // 
            // BtnGestionUtilisateur
            // 
            this.BtnGestionUtilisateur.Text = "Utilisateurs";
            this.BtnGestionUtilisateur.Size = new System.Drawing.Size(206, 40);
            this.BtnGestionUtilisateur.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.BtnGestionUtilisateur.Name = "BtnGestionUtilisateur";
            this.BtnGestionUtilisateur.Click += new System.EventHandler(this.BtnGestionUtilisateur_Click);
            // 
            // btnSauvegarde
            // 
            this.btnSauvegarde.Text = "Sauvegarder les données";
            this.btnSauvegarde.Size = new System.Drawing.Size(206, 40);
            this.btnSauvegarde.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnSauvegarde.Name = "btnSauvegarde";
            this.btnSauvegarde.Click += new System.EventHandler(this.btnSauvegarde_Click);
            // 
            // btnDossierSauvegardes
            // 
            this.btnDossierSauvegardes.Text = "Dossier des sauvegardes";
            this.btnDossierSauvegardes.Size = new System.Drawing.Size(206, 40);
            this.btnDossierSauvegardes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.btnDossierSauvegardes.Name = "btnDossierSauvegardes";
            this.btnDossierSauvegardes.Click += new System.EventHandler(this.btnOuvrirDossierSauvegardes_Click);
            // 
            // tlpBas
            // 
            this.tlpBas.ColumnCount = 1;
            this.tlpBas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpBas.RowCount = 2;
            this.tlpBas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpBas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpBas.Controls.Add(this.lblUtilisateur, 0, 0);
            this.tlpBas.Controls.Add(this.btnDeconnexion, 0, 1);
            this.tlpBas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBas.Padding = new System.Windows.Forms.Padding(12, 8, 12, 12);
            this.tlpBas.Name = "tlpBas";
            // 
            // lblUtilisateur
            // 
            this.lblUtilisateur.Text = "Utilisateur";
            this.lblUtilisateur.AutoSize = true;
            this.lblUtilisateur.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUtilisateur.Tag = "menuinfo";
            this.lblUtilisateur.ForeColor = System.Drawing.Color.White;
            this.lblUtilisateur.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUtilisateur.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblUtilisateur.Name = "lblUtilisateur";
            // 
            // btnDeconnexion
            // 
            this.btnDeconnexion.Text = "Déconnexion";
            this.btnDeconnexion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeconnexion.Tag = "danger";
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
            this.tlpContenu.Controls.Add(this.panelContent, 0, 1);
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
            this.panelContent.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.panelContent.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.panelContent.Name = "panelContent";
            // 
            // PageAccueil
            // 
            this.Controls.Add(this.tlpPrincipal);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.MinimumSize = new System.Drawing.Size(1024, 640);
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
            this.tlpBas.ResumeLayout(false);
            this.tlpBas.PerformLayout();
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
        private System.Windows.Forms.Label lblSectionAdministration;
        private Pharmacie2.views.Composants.BoutonMenu BtnGestionUtilisateur;
        private Pharmacie2.views.Composants.BoutonMenu btnSauvegarde;
        private Pharmacie2.views.Composants.BoutonMenu btnDossierSauvegardes;
        private System.Windows.Forms.TableLayoutPanel tlpBas;
        private System.Windows.Forms.Label lblUtilisateur;
        private System.Windows.Forms.Button btnDeconnexion;
        private System.Windows.Forms.TableLayoutPanel tlpContenu;
        private Pharmacie2.views.Composants.BandeauNotification bandeau;
        private System.Windows.Forms.Panel panelContent;
    }
}
