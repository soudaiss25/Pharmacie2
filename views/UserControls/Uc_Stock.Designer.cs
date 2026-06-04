using System;
using System.Windows.Forms;

namespace Pharmacie2.views.UserControls
{
    public partial class Uc_Stock : UserControl
    {
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle headerStyle =
                new System.Windows.Forms.DataGridViewCellStyle();

            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.txtSearchProduit = new System.Windows.Forms.TextBox();
            this.cbSeuil = new System.Windows.Forms.ComboBox();
            this.cbDisponibilite = new System.Windows.Forms.ComboBox();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnAjouter = new System.Windows.Forms.Button();
            this.btnModifier = new System.Windows.Forms.Button();
            this.btnSupprimer = new System.Windows.Forms.Button();
            this.btnCommander = new System.Windows.Forms.Button();

            this.panelHeader.SuspendLayout();
            this.panelFilters.SuspendLayout();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
            this.SuspendLayout();

            // ── panelHeader ───────────────────────────────────────────────
            this.panelHeader.BackColor = System.Drawing.Color.ForestGreen;
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(900, 60);
            this.panelHeader.TabIndex = 5;

            // ── lblTitle ──────────────────────────────────────────────────
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F,
                System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "📦  Gestion du Stock";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.TabIndex = 0;

            // ── panelFilters ──────────────────────────────────────────────
            this.panelFilters.Controls.Add(this.txtSearchProduit);
            this.panelFilters.Controls.Add(this.cbSeuil);
            this.panelFilters.Controls.Add(this.cbDisponibilite);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Padding = new System.Windows.Forms.Padding(10);
            this.panelFilters.Size = new System.Drawing.Size(900, 54);
            this.panelFilters.TabIndex = 4;
            this.panelFilters.BackColor = System.Drawing.Color.FromArgb(245, 250, 245);

            // ── txtSearchProduit ──────────────────────────────────────────
            this.txtSearchProduit.Location = new System.Drawing.Point(10, 14);
            this.txtSearchProduit.Name = "txtSearchProduit";
            this.txtSearchProduit.Size = new System.Drawing.Size(220, 26);
            this.txtSearchProduit.TabIndex = 0;
            this.txtSearchProduit.PlaceholderText = "🔍 Rechercher un produit…";
            this.txtSearchProduit.Font = new System.Drawing.Font("Segoe UI", 9F);

            // ── cbSeuil ───────────────────────────────────────────────────
            this.cbSeuil.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeuil.Items.AddRange(new object[] { "Tous", "Sous le seuil", "Normal" });
            this.cbSeuil.Location = new System.Drawing.Point(244, 14);
            this.cbSeuil.Name = "cbSeuil";
            this.cbSeuil.Size = new System.Drawing.Size(160, 28);
            this.cbSeuil.TabIndex = 1;
            this.cbSeuil.Font = new System.Drawing.Font("Segoe UI", 9F);

            // ── cbDisponibilite ───────────────────────────────────────────
            this.cbDisponibilite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDisponibilite.Items.AddRange(new object[] { "Tous", "En rupture", "Disponible" });
            this.cbDisponibilite.Location = new System.Drawing.Point(420, 14);
            this.cbDisponibilite.Name = "cbDisponibilite";
            this.cbDisponibilite.Size = new System.Drawing.Size(160, 28);
            this.cbDisponibilite.TabIndex = 2;
            this.cbDisponibilite.Font = new System.Drawing.Font("Segoe UI", 9F);

            // ── dgvStock ──────────────────────────────────────────────────
            headerStyle.BackColor = System.Drawing.Color.FromArgb(46, 125, 50);
            headerStyle.ForeColor = System.Drawing.Color.White;
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 9F,
                System.Drawing.FontStyle.Bold);
            headerStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            headerStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;

            this.dgvStock.ColumnHeadersDefaultCellStyle = headerStyle;
            this.dgvStock.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStock.EnableHeadersVisualStyles = false;
            this.dgvStock.BackgroundColor = System.Drawing.Color.White;
            this.dgvStock.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.ReadOnly = true;
            this.dgvStock.RowHeadersWidth = 30;
            this.dgvStock.RowTemplate.Height = 30;
            this.dgvStock.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStock.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.dgvStock.TabIndex = 0;
            this.dgvStock.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvStock.AlternatingRowsDefaultCellStyle.BackColor =
                System.Drawing.Color.FromArgb(240, 248, 240);

            // ── panelButtons ──────────────────────────────────────────────
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Height = 54;
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(245, 250, 245);
            this.panelButtons.Controls.Add(this.btnAjouter);
            this.panelButtons.Controls.Add(this.btnModifier);
            this.panelButtons.Controls.Add(this.btnSupprimer);
            this.panelButtons.Controls.Add(this.btnCommander);
            this.panelButtons.TabIndex = 6;

            // ── btnAjouter ────────────────────────────────────────────────
            this.btnAjouter.BackColor = System.Drawing.Color.FromArgb(46, 125, 50);
            this.btnAjouter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAjouter.FlatAppearance.BorderSize = 0;
            this.btnAjouter.ForeColor = System.Drawing.Color.White;
            this.btnAjouter.Location = new System.Drawing.Point(10, 10);
            this.btnAjouter.Name = "btnAjouter";
            this.btnAjouter.Size = new System.Drawing.Size(130, 35);
            this.btnAjouter.TabIndex = 1;
            this.btnAjouter.Text = "➕ Ajouter";
            this.btnAjouter.Font = new System.Drawing.Font("Segoe UI", 9F,
                System.Drawing.FontStyle.Bold);
            this.btnAjouter.UseVisualStyleBackColor = false;

            // ── btnModifier ───────────────────────────────────────────────
            this.btnModifier.BackColor = System.Drawing.Color.FromArgb(25, 118, 210);
            this.btnModifier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModifier.FlatAppearance.BorderSize = 0;
            this.btnModifier.ForeColor = System.Drawing.Color.White;
            this.btnModifier.Location = new System.Drawing.Point(150, 10);
            this.btnModifier.Name = "btnModifier";
            this.btnModifier.Size = new System.Drawing.Size(130, 35);
            this.btnModifier.TabIndex = 2;
            this.btnModifier.Text = "✏️ Modifier";
            this.btnModifier.Font = new System.Drawing.Font("Segoe UI", 9F,
                System.Drawing.FontStyle.Bold);
            this.btnModifier.UseVisualStyleBackColor = false;

            // ── btnSupprimer ──────────────────────────────────────────────
            this.btnSupprimer.BackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.btnSupprimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSupprimer.FlatAppearance.BorderSize = 0;
            this.btnSupprimer.ForeColor = System.Drawing.Color.White;
            this.btnSupprimer.Location = new System.Drawing.Point(290, 10);
            this.btnSupprimer.Name = "btnSupprimer";
            this.btnSupprimer.Size = new System.Drawing.Size(130, 35);
            this.btnSupprimer.TabIndex = 3;
            this.btnSupprimer.Text = "🗑️ Supprimer";
            this.btnSupprimer.Font = new System.Drawing.Font("Segoe UI", 9F,
                System.Drawing.FontStyle.Bold);
            this.btnSupprimer.UseVisualStyleBackColor = false;

            // ── btnCommander ──────────────────────────────────────────────
            this.btnCommander.BackColor = System.Drawing.Color.FromArgb(230, 120, 0);
            this.btnCommander.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCommander.FlatAppearance.BorderSize = 0;
            this.btnCommander.ForeColor = System.Drawing.Color.White;
            this.btnCommander.Location = new System.Drawing.Point(430, 10);
            this.btnCommander.Name = "btnCommander";
            this.btnCommander.Size = new System.Drawing.Size(200, 35);
            this.btnCommander.TabIndex = 4;
            this.btnCommander.Text = "🛒 Commander au fournisseur";
            this.btnCommander.Font = new System.Drawing.Font("Segoe UI", 9F,
                System.Drawing.FontStyle.Bold);
            this.btnCommander.UseVisualStyleBackColor = false;

            // ── UserControl ───────────────────────────────────────────────
            this.Controls.Add(this.dgvStock);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelFilters);
            this.Controls.Add(this.panelHeader);
            this.Name = "Uc_Stock";
            this.Size = new System.Drawing.Size(900, 560);

            this.panelHeader.ResumeLayout(false);
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
            this.ResumeLayout(false);
        }

        // ── Déclaration des champs ────────────────────────────────────────
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.TextBox txtSearchProduit;
        private System.Windows.Forms.ComboBox cbSeuil;
        private System.Windows.Forms.ComboBox cbDisponibilite;
        private System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnAjouter;
        private System.Windows.Forms.Button btnModifier;
        private System.Windows.Forms.Button btnSupprimer;
        private System.Windows.Forms.Button btnCommander;
    }
}