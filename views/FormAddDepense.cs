using System;
using System.Drawing;
using System.Windows.Forms;
using Pharmacie2.Models;
using Pharmacie2.Services;
using Pharmacie2.views.Composants;

namespace Pharmacie2.views
{
    public partial class FormAddDepense : Form
    {
        /// <summary>Choix de la liste des commandes : une commande non soldée et ce qu'il reste à payer.</summary>
        private sealed class ChoixCommande
        {
            public DetteCommande Dette { get; }
            public ChoixCommande(DetteCommande d) { Dette = d; }
            public override string ToString()
                => $"N°{Dette.CommandeId} — {Dette.Fournisseur} — {Dette.Date:dd/MM/yyyy} — total {Format.Montant(Dette.Total)} — reste {Format.Montant(Dette.Reste)}";
        }

        public FormAddDepense() : this(null) { }

        /// <summary>Avec <paramref name="commandeId"/> : saisie d'un remboursement de cette commande (catégorie et commande présélectionnées).</summary>
        public FormAddDepense(int? commandeId)
        {
            InitializeComponent();
            Theme.Appliquer(this);
            cbCategorie.SelectedIndex = -1;
            if (commandeId != null)
            {
                cbCategorie.SelectedItem = RemboursementCommandeService.Categorie;
                foreach (var item in cbCommande.Items.Cast<ChoixCommande>())
                    if (item.Dette.CommandeId == commandeId) cbCommande.SelectedItem = item;
            }
        }

        private bool EstRemboursement => cbCategorie.Text == RemboursementCommandeService.Categorie;

        private void cbCommande_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCommande.SelectedItem is not ChoixCommande c) { lblResteCommande.Text = ""; return; }
            lblResteCommande.Text = $"Reste à payer sur cette commande : {Format.Montant(c.Dette.Reste)} (déjà remboursé : {Format.Montant(c.Dette.Rembourse)})";
        }

        private void AfficherChoixCommande()
        {
            bool visible = EstRemboursement;
            lblCommande.Visible = cbCommande.Visible = lblResteCommande.Visible = visible;
            if (!visible) return;
            if (cbCommande.Items.Count == 0)
            {
                using var ctx = new AppDbContext();
                foreach (var d in RemboursementCommandeService.NonSoldees(ctx))
                    cbCommande.Items.Add(new ChoixCommande(d));
            }
            if (cbCommande.Items.Count == 0)
                lblResteCommande.Text = "Aucune commande n'est à rembourser.";
        }

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            // Validations
            if (cbCategorie.SelectedIndex < 0)
            {
                MessageBox.Show("Choisissez une catégorie.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbCategorie.Focus(); return;
            }

            decimal montant = numMontant.Value;
            if (montant <= 0)
            {
                MessageBox.Show("Saisissez un montant supérieur à 0.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numMontant.Focus(); return;
            }

            // Remboursement de commande : la commande est obligatoire et le montant ne dépasse jamais le reste à payer
            int? commandeId = null;
            if (EstRemboursement)
            {
                commandeId = (cbCommande.SelectedItem as ChoixCommande)?.Dette.CommandeId;
                using var ctxControle = new AppDbContext();
                string? refus = RemboursementCommandeService.ControlerRemboursement(ctxControle, commandeId, montant);
                if (refus != null)
                {
                    MessageBox.Show(refus, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    (commandeId == null ? (Control)cbCommande : numMontant).Focus();
                    return;
                }
            }

            try
            {
                using (var ctx = new AppDbContext())
                {
                    ctx.DepensesAnnexes.Add(new DepenseAnnexe
                    {
                        Categorie = cbCategorie.Text,
                        Description = txtDescription.Text.Trim(),
                        Montant = montant,
                        DateDepense = dtpDate.Value.Date,
                        CommandeId = commandeId,   // jamais de session de caisse : les dépenses sont payées par la trésorerie de la pharmacie
                        UserId = SessionUtilisateur.IdCourant
                    });
                    ctx.SaveChanges();
                }

                BandeauNotification.Succes($"Dépense enregistrée : {cbCategorie.Text}, {Format.Montant(montant)}");

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cbCategorie_SelectedIndexChanged(object sender, EventArgs e)
        {
            AfficherChoixCommande();
            // Placeholder selon la catégorie
            txtDescription.PlaceholderText = cbCategorie.Text switch
            {
                "Salaire" => "Ex: Salaire Thomas — Mai 2026",
                "Facture" => "Ex: Facture électricité — Mai 2026",
                "Loyer" => "Ex: Loyer local pharmacie — Mai 2026",
                "Fournitures" => "Ex: Sacs, étiquettes, stylos...",
                RemboursementCommandeService.Categorie => "Ex: Remboursement du grossiste — acompte d'octobre",
                _ => "Description de la dépense..."
            };
        }
    }
}