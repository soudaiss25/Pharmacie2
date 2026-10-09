namespace Pharmacie2.views
{
    partial class PageCaissier
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
            this.tlpContenu = new System.Windows.Forms.TableLayoutPanel();
            this.tlpEntete = new System.Windows.Forms.TableLayoutPanel();
            this.tlpTitres = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitre = new System.Windows.Forms.Label();
            this.lblBienvenue = new System.Windows.Forms.Label();
            this.btnDeconnexion = new System.Windows.Forms.Button();
            this.bandeau = new Pharmacie2.views.Composants.BandeauNotification();
            this.tlpKpi = new System.Windows.Forms.TableLayoutPanel();
            this.carteEncaisse = new Pharmacie2.views.Composants.CarteKpi();
            this.carteVentes = new Pharmacie2.views.Composants.CarteKpi();
            this.carteReste = new Pharmacie2.views.Composants.CarteKpi();
            this.flpVentilation = new System.Windows.Forms.FlowLayoutPanel();
            this.lblEspeces = new System.Windows.Forms.Label();
            this.lblCheque = new System.Windows.Forms.Label();
            this.lblDigital = new System.Windows.Forms.Label();
            this.lblMutuelle = new System.Windows.Forms.Label();
            this.lblCredit = new System.Windows.Forms.Label();
            this.dgvVentes = new System.Windows.Forms.DataGridView();
            this.flpActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNouvelleVente = new System.Windows.Forms.Button();
            this.btnDetail = new System.Windows.Forms.Button();
            this.btnPaiement = new System.Windows.Forms.Button();
            this.btnActualiser = new System.Windows.Forms.Button();
            this.btnCloturerSession = new System.Windows.Forms.Button();
            this.lblSessionInfo = new System.Windows.Forms.Label();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHeure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVerse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRestant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentes)).BeginInit();
            this.tlpContenu.SuspendLayout();
            this.tlpEntete.SuspendLayout();
            this.tlpTitres.SuspendLayout();
            this.tlpKpi.SuspendLayout();
            this.flpVentilation.SuspendLayout();
            this.flpActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpContenu
            // 
            this.tlpContenu.ColumnCount = 1;
            this.tlpContenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpContenu.RowCount = 7;
            this.tlpContenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpContenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpContenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpContenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpContenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpContenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpContenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpContenu.Controls.Add(this.tlpEntete, 0, 0);
            this.tlpEntete.TabIndex = 0;
            this.tlpContenu.Controls.Add(this.bandeau, 0, 1);
            this.bandeau.TabIndex = 1;
            this.tlpContenu.Controls.Add(this.tlpKpi, 0, 2);
            this.tlpKpi.TabIndex = 2;
            this.tlpContenu.Controls.Add(this.flpVentilation, 0, 3);
            this.flpVentilation.TabIndex = 3;
            this.tlpContenu.Controls.Add(this.dgvVentes, 0, 4);
            this.dgvVentes.TabIndex = 4;
            this.tlpContenu.Controls.Add(this.flpActions, 0, 5);
            this.flpActions.TabIndex = 5;
            this.tlpContenu.Controls.Add(this.lblSessionInfo, 0, 6);
            this.lblSessionInfo.TabIndex = 6;
            this.tlpContenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContenu.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.tlpContenu.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpContenu.Name = "tlpContenu";
            // 
            // tlpEntete
            // 
            this.tlpEntete.ColumnCount = 2;
            this.tlpEntete.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpEntete.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.RowCount = 1;
            this.tlpEntete.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEntete.Controls.Add(this.tlpTitres, 0, 0);
            this.tlpTitres.TabIndex = 0;
            this.tlpEntete.Controls.Add(this.btnDeconnexion, 1, 0);
            this.btnDeconnexion.TabIndex = 1;
            this.tlpEntete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEntete.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.tlpEntete.Name = "tlpEntete";
            // 
            // tlpTitres
            // 
            this.tlpTitres.ColumnCount = 1;
            this.tlpTitres.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F));
            this.tlpTitres.RowCount = 2;
            this.tlpTitres.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTitres.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTitres.Controls.Add(this.lblTitre, 0, 0);
            this.lblTitre.TabIndex = 0;
            this.tlpTitres.Controls.Add(this.lblBienvenue, 0, 1);
            this.lblBienvenue.TabIndex = 1;
            this.tlpTitres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTitres.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpTitres.Name = "tlpTitres";
            // 
            // lblTitre
            // 
            this.lblTitre.Text = "Ma caisse";
            this.lblTitre.AutoSize = true;
            this.lblTitre.Tag = "titre";
            this.lblTitre.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.lblTitre.Name = "lblTitre";
            // 
            // lblBienvenue
            // 
            this.lblBienvenue.Text = "";
            this.lblBienvenue.AutoSize = true;
            this.lblBienvenue.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblBienvenue.Name = "lblBienvenue";
            // 
            // btnDeconnexion
            // 
            this.btnDeconnexion.Text = "Déconnexion";
            this.btnDeconnexion.Tag = "danger";
            this.btnDeconnexion.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnDeconnexion.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnDeconnexion.Name = "btnDeconnexion";
            this.btnDeconnexion.Click += new System.EventHandler(this.btnDeconnexion_Click);
            // 
            // bandeau
            // 
            this.bandeau.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bandeau.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.bandeau.Name = "bandeau";
            // 
            // tlpKpi
            // 
            this.tlpKpi.ColumnCount = 3;
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.0F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.0F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.0F));
            this.tlpKpi.RowCount = 1;
            this.tlpKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpKpi.Controls.Add(this.carteEncaisse, 0, 0);
            this.carteEncaisse.TabIndex = 0;
            this.tlpKpi.Controls.Add(this.carteVentes, 1, 0);
            this.carteVentes.TabIndex = 1;
            this.tlpKpi.Controls.Add(this.carteReste, 2, 0);
            this.carteReste.TabIndex = 2;
            this.tlpKpi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpKpi.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.tlpKpi.Name = "tlpKpi";
            // 
            // carteEncaisse
            // 
            this.carteEncaisse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carteEncaisse.Titre = "Encaissé depuis l'ouverture";
            this.carteEncaisse.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.carteEncaisse.Name = "carteEncaisse";
            // 
            // carteVentes
            // 
            this.carteVentes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carteVentes.Titre = "Ventes";
            this.carteVentes.Margin = new System.Windows.Forms.Padding(0, 4, 12, 4);
            this.carteVentes.Name = "carteVentes";
            // 
            // carteReste
            // 
            this.carteReste.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carteReste.Titre = "Reste à encaisser";
            this.carteReste.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.carteReste.Name = "carteReste";
            // 
            // flpVentilation
            // 
            this.flpVentilation.Controls.Add(this.lblEspeces);
            this.lblEspeces.TabIndex = 0;
            this.flpVentilation.Controls.Add(this.lblCheque);
            this.lblCheque.TabIndex = 1;
            this.flpVentilation.Controls.Add(this.lblDigital);
            this.lblDigital.TabIndex = 2;
            this.flpVentilation.Controls.Add(this.lblMutuelle);
            this.lblMutuelle.TabIndex = 3;
            this.flpVentilation.Controls.Add(this.lblCredit);
            this.lblCredit.TabIndex = 4;
            this.flpVentilation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpVentilation.AutoSize = true;
            this.flpVentilation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpVentilation.WrapContents = true;
            this.flpVentilation.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.flpVentilation.Name = "flpVentilation";
            // 
            // lblEspeces
            // 
            this.lblEspeces.Text = "";
            this.lblEspeces.AutoSize = true;
            this.lblEspeces.Tag = "note";
            this.lblEspeces.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.lblEspeces.Name = "lblEspeces";
            // 
            // lblCheque
            // 
            this.lblCheque.Text = "";
            this.lblCheque.AutoSize = true;
            this.lblCheque.Tag = "note";
            this.lblCheque.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.lblCheque.Name = "lblCheque";
            // 
            // lblDigital
            // 
            this.lblDigital.Text = "";
            this.lblDigital.AutoSize = true;
            this.lblDigital.Tag = "note";
            this.lblDigital.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.lblDigital.Name = "lblDigital";
            // 
            // lblMutuelle
            // 
            this.lblMutuelle.Text = "";
            this.lblMutuelle.AutoSize = true;
            this.lblMutuelle.Tag = "note";
            this.lblMutuelle.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.lblMutuelle.Name = "lblMutuelle";
            // 
            // lblCredit
            // 
            this.lblCredit.Text = "";
            this.lblCredit.AutoSize = true;
            this.lblCredit.Tag = "note";
            this.lblCredit.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.lblCredit.Name = "lblCredit";
            // 
            // dgvVentes
            // 
            this.dgvVentes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colNumero,
            this.colClient,
            this.colHeure,
            this.colTotal,
            this.colVerse,
            this.colRestant,
            this.colMode,
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
            this.btnNouvelleVente.TabIndex = 0;
            this.flpActions.Controls.Add(this.btnDetail);
            this.btnDetail.TabIndex = 1;
            this.flpActions.Controls.Add(this.btnPaiement);
            this.btnPaiement.TabIndex = 2;
            this.flpActions.Controls.Add(this.btnActualiser);
            this.btnActualiser.TabIndex = 3;
            this.flpActions.Controls.Add(this.btnCloturerSession);
            this.btnCloturerSession.TabIndex = 4;
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
            // btnPaiement
            // 
            this.btnPaiement.Text = "Encaisser un paiement";
            this.btnPaiement.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnPaiement.Name = "btnPaiement";
            this.btnPaiement.Click += new System.EventHandler(this.btnPaiement_Click);
            // 
            // btnActualiser
            // 
            this.btnActualiser.Text = "Actualiser";
            this.btnActualiser.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnActualiser.Name = "btnActualiser";
            this.btnActualiser.Click += new System.EventHandler(this.btnActualiser_Click);
            // 
            // btnCloturerSession
            // 
            this.btnCloturerSession.Text = "Clôturer la caisse";
            this.btnCloturerSession.Tag = "neutre";
            this.btnCloturerSession.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnCloturerSession.Name = "btnCloturerSession";
            this.btnCloturerSession.Click += new System.EventHandler(this.btnCloturerSession_Click);
            // 
            // lblSessionInfo
            // 
            this.lblSessionInfo.Text = "";
            this.lblSessionInfo.AutoSize = true;
            this.lblSessionInfo.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblSessionInfo.Name = "lblSessionInfo";
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
            // colNumero
            // 
            this.colNumero.HeaderText = "N° de vente";
            this.colNumero.Name = "Numero";
            this.colNumero.DataPropertyName = "Numero";
            this.colNumero.FillWeight = 12F;
            this.colNumero.MinimumWidth = 90;
            this.colNumero.ReadOnly = true;
            // 
            // colClient
            // 
            this.colClient.HeaderText = "Client";
            this.colClient.Name = "Client";
            this.colClient.DataPropertyName = "Client";
            this.colClient.FillWeight = 22F;
            this.colClient.MinimumWidth = 120;
            this.colClient.ReadOnly = true;
            // 
            // colHeure
            // 
            this.colHeure.HeaderText = "Heure";
            this.colHeure.Name = "Heure";
            this.colHeure.DataPropertyName = "Heure";
            this.colHeure.FillWeight = 8F;
            this.colHeure.MinimumWidth = 60;
            this.colHeure.ReadOnly = true;
            // 
            // colTotal
            // 
            this.colTotal.HeaderText = "Total";
            this.colTotal.Name = "Total";
            this.colTotal.DataPropertyName = "Total";
            this.colTotal.FillWeight = 14F;
            this.colTotal.MinimumWidth = 100;
            this.colTotal.ReadOnly = true;
            this.colTotal.Tag = "montant";
            // 
            // colVerse
            // 
            this.colVerse.HeaderText = "Versé";
            this.colVerse.Name = "Verse";
            this.colVerse.DataPropertyName = "Verse";
            this.colVerse.FillWeight = 14F;
            this.colVerse.MinimumWidth = 100;
            this.colVerse.ReadOnly = true;
            this.colVerse.Tag = "montant";
            // 
            // colRestant
            // 
            this.colRestant.HeaderText = "Reste à payer";
            this.colRestant.Name = "Restant";
            this.colRestant.DataPropertyName = "Restant";
            this.colRestant.FillWeight = 14F;
            this.colRestant.MinimumWidth = 100;
            this.colRestant.ReadOnly = true;
            this.colRestant.Tag = "montant";
            // 
            // colMode
            // 
            this.colMode.HeaderText = "Paiement";
            this.colMode.Name = "Mode";
            this.colMode.DataPropertyName = "Mode";
            this.colMode.FillWeight = 18F;
            this.colMode.MinimumWidth = 110;
            this.colMode.ReadOnly = true;
            // 
            // colStatut
            // 
            this.colStatut.HeaderText = "Statut";
            this.colStatut.Name = "Statut";
            this.colStatut.DataPropertyName = "Statut";
            this.colStatut.FillWeight = 10F;
            this.colStatut.MinimumWidth = 70;
            this.colStatut.ReadOnly = true;
            // 
            // PageCaissier
            // 
            this.Controls.Add(this.tlpContenu);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.MinimumSize = new System.Drawing.Size(1024, 640);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.Text = "LIGUAPHARME — Caisse";
            this.Name = "PageCaissier";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PageCaissier_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();
            this.flpActions.ResumeLayout(false);
            this.flpActions.PerformLayout();
            this.flpVentilation.ResumeLayout(false);
            this.flpVentilation.PerformLayout();
            this.tlpKpi.ResumeLayout(false);
            this.tlpKpi.PerformLayout();
            this.tlpTitres.ResumeLayout(false);
            this.tlpTitres.PerformLayout();
            this.tlpEntete.ResumeLayout(false);
            this.tlpEntete.PerformLayout();
            this.tlpContenu.ResumeLayout(false);
            this.tlpContenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentes)).EndInit();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpContenu;
        private System.Windows.Forms.TableLayoutPanel tlpEntete;
        private System.Windows.Forms.TableLayoutPanel tlpTitres;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Label lblBienvenue;
        private System.Windows.Forms.Button btnDeconnexion;
        private Pharmacie2.views.Composants.BandeauNotification bandeau;
        private System.Windows.Forms.TableLayoutPanel tlpKpi;
        private Pharmacie2.views.Composants.CarteKpi carteEncaisse;
        private Pharmacie2.views.Composants.CarteKpi carteVentes;
        private Pharmacie2.views.Composants.CarteKpi carteReste;
        private System.Windows.Forms.FlowLayoutPanel flpVentilation;
        private System.Windows.Forms.Label lblEspeces;
        private System.Windows.Forms.Label lblCheque;
        private System.Windows.Forms.Label lblDigital;
        private System.Windows.Forms.Label lblMutuelle;
        private System.Windows.Forms.Label lblCredit;
        private System.Windows.Forms.DataGridView dgvVentes;
        private System.Windows.Forms.FlowLayoutPanel flpActions;
        private System.Windows.Forms.Button btnNouvelleVente;
        private System.Windows.Forms.Button btnDetail;
        private System.Windows.Forms.Button btnPaiement;
        private System.Windows.Forms.Button btnActualiser;
        private System.Windows.Forms.Button btnCloturerSession;
        private System.Windows.Forms.Label lblSessionInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumero;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHeure;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVerse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRestant;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatut;
    }
}
