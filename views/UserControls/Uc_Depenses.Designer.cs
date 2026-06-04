using System.Drawing;
using System.Windows.Forms;

namespace Pharmacie2.views.UserControls
{
    partial class Uc_Depenses
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        // ── Contrôles ─────────────────────────────────────────────────────
        private Panel pnlHeader;
        private Label lblTitre;
        private TabControl tabMain;
        private TabPage tabMensuel, tabAnnuel, tabHistorique;

        // Onglet mensuel — filtres
        private Panel pnlFiltreMensuel;
        private Label lblMoisLabel, lblAnneeLabel, lblPeriodeMensuel;
        private ComboBox cbMois, cbAnnee;
        private Button btnActualiser, btnNouvelleDepense;

        // Onglet mensuel — contenu (SplitContainer gauche/droite)
        private SplitContainer splitMensuel;

        // Panel gauche = Revenus + Charges empilés verticalement
        private Panel pnlGauche;

        // Revenus — labels
        private Label lblRevTitre;
        private Label lblEspecesT, lblEspeces;
        private Label lblCBT, lblCB;
        private Label lblAvancesT, lblAvancesCredit;
        private Label lblMutuellePT, lblMutuelleP;
        private Label lblMutuelleET, lblMutuelleE;
        private Label lblSepRevenus;
        private Label lblTotalRevenusT, lblTotalRevenus;

        // Charges — labels
        private Label lblChargeTitre;
        private Label lblCOGST, lblCOGS;
        private Label lblSalairesT, lblSalaires;
        private Label lblLoyerT, lblLoyer;
        private Label lblFacturesT, lblFactures;
        private Label lblFournituresT, lblFournitures;
        private Label lblAutresT, lblAutres;
        private Label lblSepCharges;
        private Label lblTotalChargesT, lblTotalCharges;

        // Panel droit = Résultat net
        private Panel pnlResultat;
        private Label lblBeneficeLabel, lblBeneficeNet, lblMarge;

        // Onglet annuel
        private Panel pnlFiltreAnnuel;
        private Label lblAnneeAnnuelLabel;
        private ComboBox cbAnneeAnnuel;
        private DataGridView dgvAnnuel;
        private DataGridViewTextBoxColumn
            colMois, colCA, colCOGS, colDepenses, colBenefice, colMarge;

        // Onglet historique
        private Panel pnlFiltreHisto;
        private Label lblFiltreHistoCat;
        private ComboBox cbFiltreHistoCat;
        private Button btnSupprimerDep;
        private Label lblTotalHistorique;
        private DataGridView dgvHistorique;
        private DataGridViewTextBoxColumn
            colHistId, colHistDate, colHistCat, colHistDesc, colHistMontant, colHistSaisi;

        private void InitializeComponent()
        {
            pnlHeader = new Panel(); lblTitre = new Label();
            tabMain = new TabControl();
            tabMensuel = new TabPage(); tabAnnuel = new TabPage(); tabHistorique = new TabPage();
            pnlFiltreMensuel = new Panel();
            lblMoisLabel = new Label(); cbMois = new ComboBox();
            lblAnneeLabel = new Label(); cbAnnee = new ComboBox();
            lblPeriodeMensuel = new Label();
            btnActualiser = new Button(); btnNouvelleDepense = new Button();
            splitMensuel = new SplitContainer();
            pnlGauche = new Panel();

            lblRevTitre = new Label();
            lblEspecesT = new Label(); lblEspeces = new Label();
            lblCBT = new Label(); lblCB = new Label();
            lblAvancesT = new Label(); lblAvancesCredit = new Label();
            lblMutuellePT = new Label(); lblMutuelleP = new Label();
            lblMutuelleET = new Label(); lblMutuelleE = new Label();
            lblSepRevenus = new Label();
            lblTotalRevenusT = new Label(); lblTotalRevenus = new Label();
            lblChargeTitre = new Label();
            lblCOGST = new Label(); lblCOGS = new Label();
            lblSalairesT = new Label(); lblSalaires = new Label();
            lblLoyerT = new Label(); lblLoyer = new Label();
            lblFacturesT = new Label(); lblFactures = new Label();
            lblFournituresT = new Label(); lblFournitures = new Label();
            lblAutresT = new Label(); lblAutres = new Label();
            lblSepCharges = new Label();
            lblTotalChargesT = new Label(); lblTotalCharges = new Label();
            pnlResultat = new Panel();
            lblBeneficeLabel = new Label(); lblBeneficeNet = new Label(); lblMarge = new Label();

            pnlFiltreAnnuel = new Panel();
            lblAnneeAnnuelLabel = new Label(); cbAnneeAnnuel = new ComboBox();
            dgvAnnuel = new DataGridView();
            colMois = new DataGridViewTextBoxColumn();
            colCA = new DataGridViewTextBoxColumn();
            colCOGS = new DataGridViewTextBoxColumn();
            colDepenses = new DataGridViewTextBoxColumn();
            colBenefice = new DataGridViewTextBoxColumn();
            colMarge = new DataGridViewTextBoxColumn();

            pnlFiltreHisto = new Panel();
            lblFiltreHistoCat = new Label(); cbFiltreHistoCat = new ComboBox();
            btnSupprimerDep = new Button(); lblTotalHistorique = new Label();
            dgvHistorique = new DataGridView();
            colHistId = new DataGridViewTextBoxColumn();
            colHistDate = new DataGridViewTextBoxColumn();
            colHistCat = new DataGridViewTextBoxColumn();
            colHistDesc = new DataGridViewTextBoxColumn();
            colHistMontant = new DataGridViewTextBoxColumn();
            colHistSaisi = new DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)splitMensuel).BeginInit();
            splitMensuel.Panel1.SuspendLayout();
            splitMensuel.Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAnnuel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvHistorique).BeginInit();
            tabMain.SuspendLayout();
            SuspendLayout();

            // ── HEADER ────────────────────────────────────────────────────
            lblTitre.Text = "  Dépenses & Résultat";
            lblTitre.Dock = DockStyle.Fill;
            lblTitre.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitre.ForeColor = Color.White;
            lblTitre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblTitre.Padding = new Padding(14, 0, 0, 0);
            pnlHeader.BackColor = Color.FromArgb(183, 28, 28);
            pnlHeader.Controls.Add(lblTitre);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 55;

            // ── TABCONTROL ────────────────────────────────────────────────
            tabMain.Dock = DockStyle.Fill;
            tabMain.Font = new Font("Segoe UI", 10F);
            tabMensuel.Text = "  Résultat mensuel";
            tabAnnuel.Text = "  Tableau annuel";
            tabHistorique.Text = "  Historique dépenses";
            tabMain.TabPages.AddRange(new TabPage[] { tabMensuel, tabAnnuel, tabHistorique });

            // ══════════════════════════════════════════════════════════════
            // ONGLET 1 — RÉSULTAT MENSUEL
            // ══════════════════════════════════════════════════════════════

            // Barre filtres (Dock Top)
            pnlFiltreMensuel.Dock = DockStyle.Top;
            pnlFiltreMensuel.Height = 50;
            pnlFiltreMensuel.BackColor = Color.FromArgb(250, 250, 250);

            lblMoisLabel.Text = "Mois :";
            lblMoisLabel.Location = new Point(10, 15);
            lblMoisLabel.Size = new Size(44, 22);
            lblMoisLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            cbMois.Location = new Point(57, 12);
            cbMois.Size = new Size(140, 28);
            cbMois.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMois.Font = new Font("Segoe UI", 9.5F);

            lblAnneeLabel.Text = "Année :";
            lblAnneeLabel.Location = new Point(210, 15);
            lblAnneeLabel.Size = new Size(52, 22);
            lblAnneeLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            cbAnnee.Location = new Point(266, 12);
            cbAnnee.Size = new Size(90, 28);
            cbAnnee.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAnnee.Font = new Font("Segoe UI", 9.5F);

            btnActualiser.Text = "Actualiser";
            btnActualiser.Location = new Point(370, 11);
            btnActualiser.Size = new Size(110, 30);
            btnActualiser.BackColor = Color.FromArgb(69, 90, 100);
            btnActualiser.ForeColor = Color.White;
            btnActualiser.FlatStyle = FlatStyle.Flat;
            btnActualiser.FlatAppearance.BorderSize = 0;
            btnActualiser.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            btnNouvelleDepense.Text = "+ Saisir une dépense";
            btnNouvelleDepense.Location = new Point(492, 9);
            btnNouvelleDepense.Size = new Size(190, 34);
            btnNouvelleDepense.BackColor = Color.FromArgb(183, 28, 28);
            btnNouvelleDepense.ForeColor = Color.White;
            btnNouvelleDepense.FlatStyle = FlatStyle.Flat;
            btnNouvelleDepense.FlatAppearance.BorderSize = 0;
            btnNouvelleDepense.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            lblPeriodeMensuel.Text = "";
            lblPeriodeMensuel.Location = new Point(696, 15);
            lblPeriodeMensuel.Size = new Size(500, 22);
            lblPeriodeMensuel.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblPeriodeMensuel.ForeColor = Color.DimGray;

            pnlFiltreMensuel.Controls.AddRange(new Control[] {
                lblMoisLabel, cbMois, lblAnneeLabel, cbAnnee,
                btnActualiser, btnNouvelleDepense, lblPeriodeMensuel });

            // SplitContainer : gauche = P&L, droite = résultat net
            splitMensuel.Dock = DockStyle.Fill;
            splitMensuel.Orientation = Orientation.Vertical;
            splitMensuel.Panel1MinSize = 400;
            splitMensuel.Panel2MinSize = 220;
            // SplitterDistance défini au Load dans Uc_Depenses.cs

            // ── PANEL GAUCHE — lignes P&L avec Dock empilées ──────────────
            pnlGauche.Dock = DockStyle.Fill;
            pnlGauche.BackColor = Color.White;
            pnlGauche.AutoScroll = true;
            pnlGauche.Padding = new Padding(20, 12, 20, 12);

            // Helper : crée une ligne titre+valeur en FlowLayout
            int rowH = 32;
            int yPos = 12;

            Panel MakeLigne(Label lt, string titre, Label lv, bool isTitre = false, bool isTotal = false)
            {
                var pnl = new Panel { Height = rowH, Dock = DockStyle.Top };

                lt.Text = titre;
                lt.Dock = DockStyle.Fill;
                lt.Font = isTotal
                    ? new Font("Segoe UI", 10F, FontStyle.Bold)
                    : isTitre
                        ? new Font("Segoe UI", 10F, FontStyle.Bold | FontStyle.Underline)
                        : new Font("Segoe UI", 9.5F);
                lt.ForeColor = isTotal
                    ? Color.FromArgb(50, 50, 50)
                    : isTitre
                        ? Color.FromArgb(30, 30, 30)
                        : Color.FromArgb(70, 70, 70);
                lt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                lt.Padding = new Padding(isTitre ? 0 : 16, 0, 0, 0);

                lv.Text = "—";
                lv.Dock = DockStyle.Right;
                lv.Width = 180;
                lv.Font = isTotal
                    ? new Font("Segoe UI", 10F, FontStyle.Bold)
                    : new Font("Segoe UI", 9.5F, FontStyle.Bold);
                lv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                lv.ForeColor = Color.FromArgb(50, 50, 50);

                pnl.Controls.Add(lt);
                pnl.Controls.Add(lv);
                return pnl;
            }

            Panel MakeSep(Color c) => new Panel
            {
                Height = 2,
                Dock = DockStyle.Top,
                BackColor = c
            };

            Panel MakeEspace(int h = 8) => new Panel { Height = h, Dock = DockStyle.Top };

            // ── Titre Revenus ─────────────────────────────────────────────
            lblRevTitre.Text = "📈  REVENUS ENCAISSÉS";
            lblRevTitre.Dock = DockStyle.Top;
            lblRevTitre.Height = 36;
            lblRevTitre.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblRevTitre.ForeColor = Color.White;
            lblRevTitre.BackColor = Color.FromArgb(27, 94, 32);
            lblRevTitre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblRevTitre.Padding = new Padding(14, 0, 0, 0);

            var pnlEspeces = MakeLigne(lblEspecesT, "Ventes comptant (espèces)", lblEspeces);
            var pnlCB = MakeLigne(lblCBT, "CB / Chèque / Mobile", lblCB);
            var pnlAvances = MakeLigne(lblAvancesT, "Avances crédit reçues", lblAvancesCredit);
            var pnlMutP = MakeLigne(lblMutuellePT, "Mutuelle — part patient", lblMutuelleP);
            var pnlMutE = MakeLigne(lblMutuelleET, "Mutuelle — part entreprise", lblMutuelleE);
            var pnlTotalR = MakeLigne(lblTotalRevenusT, "TOTAL REVENUS", lblTotalRevenus, false, true);

            // ── Titre Charges ─────────────────────────────────────────────
            lblChargeTitre.Text = "📉  CHARGES";
            lblChargeTitre.Dock = DockStyle.Top;
            lblChargeTitre.Height = 36;
            lblChargeTitre.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblChargeTitre.ForeColor = Color.White;
            lblChargeTitre.BackColor = Color.FromArgb(183, 28, 28);
            lblChargeTitre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblChargeTitre.Padding = new Padding(14, 0, 0, 0);

            var pnlCOGS = MakeLigne(lblCOGST, "Coût d'achat produits vendus", lblCOGS);
            var pnlSalaires = MakeLigne(lblSalairesT, "Salaires", lblSalaires);
            var pnlLoyer = MakeLigne(lblLoyerT, "Loyer", lblLoyer);
            var pnlFactures = MakeLigne(lblFacturesT, "Factures (eau, électricité...)", lblFactures);
            var pnlFournitures = MakeLigne(lblFournituresT, "Fournitures & matériel", lblFournitures);
            var pnlAutres = MakeLigne(lblAutresT, "Autres dépenses", lblAutres);
            var pnlTotalC = MakeLigne(lblTotalChargesT, "TOTAL CHARGES", lblTotalCharges, false, true);

            // Ordre Dock.Top : le DERNIER ajouté apparaît en HAUT
            // Donc on ajoute dans l'ordre inverse d'affichage
            pnlGauche.Controls.Add(pnlTotalC);
            pnlGauche.Controls.Add(MakeSep(Color.FromArgb(183, 28, 28)));
            pnlGauche.Controls.Add(pnlAutres);
            pnlGauche.Controls.Add(pnlFournitures);
            pnlGauche.Controls.Add(pnlFactures);
            pnlGauche.Controls.Add(pnlLoyer);
            pnlGauche.Controls.Add(pnlSalaires);
            pnlGauche.Controls.Add(pnlCOGS);
            pnlGauche.Controls.Add(MakeEspace(6));
            pnlGauche.Controls.Add(lblChargeTitre);
            pnlGauche.Controls.Add(MakeEspace(10));
            pnlGauche.Controls.Add(pnlTotalR);
            pnlGauche.Controls.Add(MakeSep(Color.FromArgb(27, 94, 32)));
            pnlGauche.Controls.Add(pnlMutE);
            pnlGauche.Controls.Add(pnlMutP);
            pnlGauche.Controls.Add(pnlAvances);
            pnlGauche.Controls.Add(pnlCB);
            pnlGauche.Controls.Add(pnlEspeces);
            pnlGauche.Controls.Add(MakeEspace(6));
            pnlGauche.Controls.Add(lblRevTitre);

            splitMensuel.Panel1.Controls.Add(pnlGauche);

            // ── PANEL DROIT — Résultat net ────────────────────────────────
            pnlResultat.Dock = DockStyle.Fill;
            pnlResultat.BackColor = Color.FromArgb(245, 245, 245);
            pnlResultat.Padding = new Padding(24, 30, 24, 20);

            var barreResultat = new Panel
            {
                Dock = DockStyle.Top,
                Height = 5,
                BackColor = Color.FromArgb(183, 28, 28)
            };

            lblBeneficeLabel.Text = "BÉNÉFICE NET DU MOIS";
            lblBeneficeLabel.Dock = DockStyle.Top;
            lblBeneficeLabel.Height = 36;
            lblBeneficeLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblBeneficeLabel.ForeColor = Color.FromArgb(60, 60, 60);
            lblBeneficeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            lblBeneficeNet.Text = "—";
            lblBeneficeNet.Dock = DockStyle.Top;
            lblBeneficeNet.Height = 70;
            lblBeneficeNet.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            lblBeneficeNet.ForeColor = Color.FromArgb(27, 94, 32);
            lblBeneficeNet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            lblMarge.Text = "";
            lblMarge.Dock = DockStyle.Top;
            lblMarge.Height = 30;
            lblMarge.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMarge.ForeColor = Color.DimGray;
            lblMarge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Ordre inverse pour Dock.Top
            splitMensuel.Panel2.Controls.Add(lblMarge);
            splitMensuel.Panel2.Controls.Add(lblBeneficeNet);
            splitMensuel.Panel2.Controls.Add(lblBeneficeLabel);
            splitMensuel.Panel2.Controls.Add(barreResultat);

            tabMensuel.Controls.Add(splitMensuel);
            tabMensuel.Controls.Add(pnlFiltreMensuel);

            // ══════════════════════════════════════════════════════════════
            // ONGLET 2 — TABLEAU ANNUEL
            // ══════════════════════════════════════════════════════════════

            pnlFiltreAnnuel.Dock = DockStyle.Top; pnlFiltreAnnuel.Height = 50;
            pnlFiltreAnnuel.BackColor = Color.FromArgb(250, 250, 250);
            lblAnneeAnnuelLabel.Text = "Année :"; lblAnneeAnnuelLabel.Location = new Point(10, 15); lblAnneeAnnuelLabel.Size = new Size(52, 22); lblAnneeAnnuelLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            cbAnneeAnnuel.Location = new Point(65, 12); cbAnneeAnnuel.Size = new Size(90, 28); cbAnneeAnnuel.DropDownStyle = ComboBoxStyle.DropDownList; cbAnneeAnnuel.Font = new Font("Segoe UI", 9.5F);
            pnlFiltreAnnuel.Controls.Add(lblAnneeAnnuelLabel);
            pnlFiltreAnnuel.Controls.Add(cbAnneeAnnuel);

            colMois.Name = "colMois"; colMois.HeaderText = "Mois"; colMois.FillWeight = 50F;
            colCA.Name = "colCA"; colCA.HeaderText = "CA (KMF)"; colCA.FillWeight = 90F;
            colCOGS.Name = "colCOGS"; colCOGS.HeaderText = "Achats (KMF)"; colCOGS.FillWeight = 90F;
            colDepenses.Name = "colDepenses"; colDepenses.HeaderText = "Dépenses (KMF)"; colDepenses.FillWeight = 90F;
            colBenefice.Name = "colBenefice"; colBenefice.HeaderText = "Bénéfice net"; colBenefice.FillWeight = 90F;
            colMarge.Name = "colMarge"; colMarge.HeaderText = "Marge %"; colMarge.FillWeight = 40F;

            dgvAnnuel.Columns.AddRange(new DataGridViewColumn[] {
                colMois, colCA, colCOGS, colDepenses, colBenefice, colMarge });
            dgvAnnuel.Dock = DockStyle.Fill;
            dgvAnnuel.ReadOnly = true;
            dgvAnnuel.AllowUserToAddRows = false;
            dgvAnnuel.RowHeadersVisible = false;
            dgvAnnuel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAnnuel.BackgroundColor = Color.White;
            dgvAnnuel.BorderStyle = BorderStyle.None;
            dgvAnnuel.Font = new Font("Segoe UI", 10F);
            dgvAnnuel.RowTemplate.Height = 38;
            dgvAnnuel.ColumnHeadersHeight = 40;
            dgvAnnuel.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(183, 28, 28);
            dgvAnnuel.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAnnuel.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvAnnuel.EnableHeadersVisualStyles = false;
            dgvAnnuel.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 248);

            tabAnnuel.Controls.Add(dgvAnnuel);
            tabAnnuel.Controls.Add(pnlFiltreAnnuel);

            // ══════════════════════════════════════════════════════════════
            // ONGLET 3 — HISTORIQUE
            // ══════════════════════════════════════════════════════════════

            pnlFiltreHisto.Dock = DockStyle.Top; pnlFiltreHisto.Height = 50;
            pnlFiltreHisto.BackColor = Color.FromArgb(250, 250, 250);
            lblFiltreHistoCat.Text = "Catégorie :"; lblFiltreHistoCat.Location = new Point(10, 15); lblFiltreHistoCat.Size = new Size(80, 22); lblFiltreHistoCat.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            cbFiltreHistoCat.Location = new Point(93, 12); cbFiltreHistoCat.Size = new Size(150, 28); cbFiltreHistoCat.DropDownStyle = ComboBoxStyle.DropDownList; cbFiltreHistoCat.Font = new Font("Segoe UI", 9.5F);
            btnSupprimerDep.Text = "Supprimer"; btnSupprimerDep.Location = new Point(256, 12); btnSupprimerDep.Size = new Size(110, 28);
            btnSupprimerDep.BackColor = Color.FromArgb(211, 47, 47); btnSupprimerDep.ForeColor = Color.White;
            btnSupprimerDep.FlatStyle = FlatStyle.Flat; btnSupprimerDep.FlatAppearance.BorderSize = 0;
            btnSupprimerDep.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalHistorique.Text = ""; lblTotalHistorique.Location = new Point(380, 15); lblTotalHistorique.Size = new Size(500, 22);
            lblTotalHistorique.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblTotalHistorique.ForeColor = Color.FromArgb(183, 28, 28);
            pnlFiltreHisto.Controls.AddRange(new Control[] {
                lblFiltreHistoCat, cbFiltreHistoCat, btnSupprimerDep, lblTotalHistorique });

            colHistId.Name = "colHistId"; colHistId.Visible = false;
            colHistDate.Name = "colHistDate"; colHistDate.HeaderText = "Date"; colHistDate.FillWeight = 55F;
            colHistCat.Name = "colHistCat"; colHistCat.HeaderText = "Catégorie"; colHistCat.FillWeight = 70F;
            colHistDesc.Name = "colHistDesc"; colHistDesc.HeaderText = "Description"; colHistDesc.FillWeight = 180F;
            colHistMontant.Name = "colHistMontant"; colHistMontant.HeaderText = "Montant"; colHistMontant.FillWeight = 60F;
            colHistSaisi.Name = "colHistSaisi"; colHistSaisi.HeaderText = "Saisi par"; colHistSaisi.FillWeight = 70F;

            dgvHistorique.Columns.AddRange(new DataGridViewColumn[] {
                colHistId, colHistDate, colHistCat, colHistDesc, colHistMontant, colHistSaisi });
            dgvHistorique.Dock = DockStyle.Fill;
            dgvHistorique.ReadOnly = true;
            dgvHistorique.AllowUserToAddRows = false;
            dgvHistorique.RowHeadersVisible = false;
            dgvHistorique.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistorique.BackgroundColor = Color.White;
            dgvHistorique.BorderStyle = BorderStyle.None;
            dgvHistorique.Font = new Font("Segoe UI", 9.5F);
            dgvHistorique.RowTemplate.Height = 34;
            dgvHistorique.ColumnHeadersHeight = 36;
            dgvHistorique.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(183, 28, 28);
            dgvHistorique.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHistorique.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvHistorique.EnableHeadersVisualStyles = false;
            dgvHistorique.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 245, 245);

            tabHistorique.Controls.Add(dgvHistorique);
            tabHistorique.Controls.Add(pnlFiltreHisto);

            // ── ASSEMBLAGE FINAL ───────────────────────────────────────────
            Controls.Add(tabMain);
            Controls.Add(pnlHeader);

            BackColor = Color.White;
            Size = new Size(1200, 750);
            Name = "Uc_Depenses";

            splitMensuel.Panel1.ResumeLayout(false);
            splitMensuel.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMensuel).EndInit();
            tabMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAnnuel).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvHistorique).EndInit();
            ResumeLayout(false);
        }
    }
}