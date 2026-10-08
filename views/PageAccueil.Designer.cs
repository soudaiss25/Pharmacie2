using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views
{
    partial class PageAccueil
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Button btnProduits;
        private System.Windows.Forms.Button btnFournisseurs;
        private System.Windows.Forms.Button btnCommandes;       // ← NOUVEAU
        private System.Windows.Forms.Button btnMutuelles;
        private System.Windows.Forms.Button btnStatistiques;
        private System.Windows.Forms.Button btnVentes;
        private System.Windows.Forms.Button btnDeconnexion;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Button btn_stock;
        private System.Windows.Forms.Button BtnGestionUtilisateur;
        private System.Windows.Forms.Button BtnCaisse;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelMenu = new Panel();
            BtnCaisse = new Button();
            BtnGestionUtilisateur = new Button();
            btn_stock = new Button();
            btnCommandes = new Button();
            lblTitre = new Label();
            btnProduits = new Button();
            btnFournisseurs = new Button();
            btnMutuelles = new Button();
            btnVentes = new Button();
            btnStatistiques = new Button();
            btnDeconnexion = new Button();
            panelContent = new Panel();

            panelMenu.SuspendLayout();
            SuspendLayout();

            // ── Panel menu ─────────────────────────────────────────────────
            panelMenu.BackColor = Color.Green;
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(220, 720);
            panelMenu.TabIndex = 0;

            // Helper bouton menu
            void BtnMenu(Button b, string txt, int y, EventHandler handler)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.Font = new Font("Segoe UI", 10F);
                b.ForeColor = Color.White;
                b.Location = new Point(10, y);
                b.Size = new Size(200, 40);
                b.Text = txt;
                b.UseVisualStyleBackColor = false;
                b.BackColor = Color.Transparent;
                b.Click += handler;
                panelMenu.Controls.Add(b);
            }

            // Titre
            lblTitre.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitre.ForeColor = Color.White;
            lblTitre.Location = new Point(0, 15);
            lblTitre.Size = new Size(220, 45);
            lblTitre.Text = "📋 Tableau de bord";
            lblTitre.TextAlign = ContentAlignment.MiddleCenter;
            panelMenu.Controls.Add(lblTitre);

            // Boutons dans l'ordre vertical
            int y = 68, gap = 44;
            BtnMenu(btnProduits, "💊 Enregistrer Produits", y, btnProduits_Click); y += gap;
            BtnMenu(btnFournisseurs, "🏭 Fournisseurs", y, btnFournisseurs_Click); y += gap;
            BtnMenu(btnCommandes, "📦 Commandes", y, btnCommandes_Click); y += gap; // ← NOUVEAU
            BtnMenu(btn_stock, "📊 Stock", y, btn_stock_Click); y += gap;
            BtnMenu(btnMutuelles, "🏥 Mutuelles", y, btnMutuelles_Click); y += gap;
            BtnMenu(btnVentes, "🛒 Enregistrer Ventes", y, btnVentes_Click); y += gap;
            BtnMenu(BtnGestionUtilisateur, "👤 Gestion Utilisateur", y, BtnGestionUtilisateur_Click); y += gap;
            BtnMenu(BtnCaisse, "💰 Visualisation caisse", y, BtnCaisse_Click_1); y += gap;
            BtnMenu(btnStatistiques, "📈 Statistiques", y, btnStatistiques_Click); y += gap;
            var btnDep = new Button();
            BtnMenu(btnDep, "💸 Dépenses annexes", y, btnDepenses_Click); y += gap;

            // Déconnexion (tout en bas)
            var btnSauvegarde2 = new Button();
            BtnMenu(btnSauvegarde2, "💾 Sauvegarder données", y, btnSauvegarde_Click); y += gap;
            var btnDossierSauvegardes = new Button();
            BtnMenu(btnDossierSauvegardes, "📂 Dossier des sauvegardes", y, btnOuvrirDossierSauvegardes_Click); y += gap;
            BtnMenu(btnDeconnexion, "🚪 Déconnexion", y + 10, btnDeconnexion_Click);

            // Agrandir le panel pour contenir tous les boutons
            panelMenu.Size = new Size(220, 720);

            // ── Panel contenu ──────────────────────────────────────────────
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(220, 0);
            panelContent.Name = "panelContent";
            panelContent.TabIndex = 1;

            // ── Form ───────────────────────────────────────────────────────
            ClientSize = new Size(1280, 720);
            Controls.Add(panelContent);
            Controls.Add(panelMenu);
            Name = "PageAccueil";
            Text = "Tableau de bord - Gestion Pharmacie";

            panelMenu.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}