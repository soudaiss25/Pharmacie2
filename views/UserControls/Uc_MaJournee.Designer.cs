namespace Pharmacie2.views.UserControls
{
    partial class Uc_MaJournee
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
            this.lblBonjour = new System.Windows.Forms.Label();
            this.tlpKpi = new System.Windows.Forms.TableLayoutPanel();
            this.carteEncaisse = new Pharmacie2.views.Composants.CarteKpi();
            this.carteVentes = new Pharmacie2.views.Composants.CarteKpi();
            this.carteARecuperer = new Pharmacie2.views.Composants.CarteKpi();
            this.carteBenefice = new Pharmacie2.views.Composants.CarteKpi();
            this.tlpBas = new System.Windows.Forms.TableLayoutPanel();
            this.lblAFaire = new System.Windows.Forms.Label();
            this.lblGraphique = new System.Windows.Forms.Label();
            this.listeActions = new Pharmacie2.views.Composants.ListeActions();
            this.graphique = new Pharmacie2.views.Composants.GraphiqueBarres();
            this.tlpRoot.SuspendLayout();
            this.tlpKpi.SuspendLayout();
            this.tlpBas.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.RowCount = 3;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpRoot.Controls.Add(this.lblBonjour, 0, 0);
            this.lblBonjour.TabIndex = 0;
            this.tlpRoot.Controls.Add(this.tlpKpi, 0, 1);
            this.tlpKpi.TabIndex = 1;
            this.tlpRoot.Controls.Add(this.tlpBas, 0, 2);
            this.tlpBas.TabIndex = 2;
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpRoot.Name = "tlpRoot";
            // 
            // lblBonjour
            // 
            this.lblBonjour.Text = "Bonjour";
            this.lblBonjour.AutoSize = true;
            this.lblBonjour.Tag = "titre";
            this.lblBonjour.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblBonjour.Name = "lblBonjour";
            // 
            // tlpKpi
            // 
            this.tlpKpi.ColumnCount = 4;
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0F));
            this.tlpKpi.RowCount = 1;
            this.tlpKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpKpi.Controls.Add(this.carteEncaisse, 0, 0);
            this.carteEncaisse.TabIndex = 0;
            this.tlpKpi.Controls.Add(this.carteVentes, 1, 0);
            this.carteVentes.TabIndex = 1;
            this.tlpKpi.Controls.Add(this.carteARecuperer, 2, 0);
            this.carteARecuperer.TabIndex = 2;
            this.tlpKpi.Controls.Add(this.carteBenefice, 3, 0);
            this.carteBenefice.TabIndex = 3;
            this.tlpKpi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpKpi.AutoSize = true;
            this.tlpKpi.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpKpi.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.tlpKpi.Name = "tlpKpi";
            // 
            // carteEncaisse
            // 
            this.carteEncaisse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carteEncaisse.Titre = "Encaissé aujourd'hui";
            this.carteEncaisse.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.carteEncaisse.Name = "carteEncaisse";
            // 
            // carteVentes
            // 
            this.carteVentes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carteVentes.Titre = "Ventes aujourd'hui";
            this.carteVentes.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.carteVentes.Name = "carteVentes";
            // 
            // carteARecuperer
            // 
            this.carteARecuperer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carteARecuperer.Titre = "Argent à récupérer";
            this.carteARecuperer.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.carteARecuperer.Name = "carteARecuperer";
            // 
            // carteBenefice
            // 
            this.carteBenefice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carteBenefice.Titre = "Bénéfice du mois";
            this.carteBenefice.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.carteBenefice.Name = "carteBenefice";
            // 
            // tlpBas
            // 
            this.tlpBas.ColumnCount = 2;
            this.tlpBas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.0F));
            this.tlpBas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.0F));
            this.tlpBas.RowCount = 2;
            this.tlpBas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpBas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpBas.Controls.Add(this.lblAFaire, 0, 0);
            this.lblAFaire.TabIndex = 0;
            this.tlpBas.Controls.Add(this.lblGraphique, 1, 0);
            this.lblGraphique.TabIndex = 1;
            this.tlpBas.Controls.Add(this.listeActions, 0, 1);
            this.listeActions.TabIndex = 2;
            this.tlpBas.Controls.Add(this.graphique, 1, 1);
            this.graphique.TabIndex = 3;
            this.tlpBas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBas.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpBas.Name = "tlpBas";
            // 
            // lblAFaire
            // 
            this.lblAFaire.Text = "À faire maintenant";
            this.lblAFaire.AutoSize = true;
            this.lblAFaire.Tag = "section";
            this.lblAFaire.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblAFaire.Name = "lblAFaire";
            // 
            // lblGraphique
            // 
            this.lblGraphique.Text = "Argent reçu ces 7 derniers jours";
            this.lblGraphique.AutoSize = true;
            this.lblGraphique.Tag = "section";
            this.lblGraphique.Margin = new System.Windows.Forms.Padding(12, 0, 0, 6);
            this.lblGraphique.Name = "lblGraphique";
            // 
            // listeActions
            // 
            this.listeActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listeActions.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.listeActions.Name = "listeActions";
            // 
            // graphique
            // 
            this.graphique.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.graphique.Height = 220;
            this.graphique.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.graphique.Name = "graphique";
            // 
            // Uc_MaJournee
            // 
            this.Controls.Add(this.tlpRoot);
            this.tlpRoot.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(1000, 520);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "Uc_MaJournee";
            this.ResumeLayout(false);
            this.tlpBas.ResumeLayout(false);
            this.tlpBas.PerformLayout();
            this.tlpKpi.ResumeLayout(false);
            this.tlpKpi.PerformLayout();
            this.tlpRoot.ResumeLayout(false);
            this.tlpRoot.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblBonjour;
        private System.Windows.Forms.TableLayoutPanel tlpKpi;
        private Pharmacie2.views.Composants.CarteKpi carteEncaisse;
        private Pharmacie2.views.Composants.CarteKpi carteVentes;
        private Pharmacie2.views.Composants.CarteKpi carteARecuperer;
        private Pharmacie2.views.Composants.CarteKpi carteBenefice;
        private System.Windows.Forms.TableLayoutPanel tlpBas;
        private System.Windows.Forms.Label lblAFaire;
        private System.Windows.Forms.Label lblGraphique;
        private Pharmacie2.views.Composants.ListeActions listeActions;
        private Pharmacie2.views.Composants.GraphiqueBarres graphique;
    }
}
