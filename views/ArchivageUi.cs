using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Pharmacie2.Services;

namespace Pharmacie2.views
{
    /// <summary>
    /// Dialogues et contrôles communs à l'archivage (produits, fournisseurs, mutuelles, utilisateurs).
    /// </summary>
    internal static class ArchivageUi
    {
        /// <summary>
        /// Remplace « Supprimer » par « Archiver » et ajoute « Afficher les archivés » + « Réactiver »
        /// à droite des boutons existants. Retourne la case à cocher (l'écran lit .Checked pour filtrer).
        /// </summary>
        public static CheckBox Installer(Button boutonArchiver, TypeElement type,
            Func<(int id, string nom, bool actif)?> selection, Action recharger)
        {
            boutonArchiver.Text = "Archiver";

            var parent = boutonArchiver.Parent;
            bool enFlux = parent is FlowLayoutPanel;

            var btnReactiver = new Button { Text = "Réactiver", Margin = boutonArchiver.Margin };
            Theme.Style(btnReactiver, StyleBouton.Secondaire);

            var chk = new CheckBox
            {
                Text = "Afficher les archivés",
                AutoSize = true,
                Margin = new Padding(12, 8, 0, 0)
            };
            chk.CheckedChanged += (s, e) => recharger();

            if (!enFlux)
            {
                // Parent à positionnement libre (anciens écrans) : à droite des boutons existants
                int droite = parent.Controls.OfType<Button>().Max(b => b.Right);
                btnReactiver.Location = new Point(droite + 10, boutonArchiver.Top);
                chk.Location = new Point(btnReactiver.Right + 12, boutonArchiver.Top + 8);
            }

            btnReactiver.Click += (s, e) =>
            {
                var sel = selection();
                if (sel == null)
                {
                    MessageBox.Show("Sélectionnez un élément archivé à réactiver.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (sel.Value.actif)
                {
                    MessageBox.Show("Cet élément n'est pas archivé.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                try
                {
                    ArchivageService.DefinirActif(type, sel.Value.id, true);
                    recharger();
                }
                catch (Exception ex)
                {
                    Journal.Erreur("Réactivation", ex);
                    MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            parent.Controls.Add(btnReactiver);
            parent.Controls.Add(chk);
            return chk;
        }

        /// <summary>
        /// Clic sur « Archiver ». Sans historique : proposition de suppression définitive ou d'archivage.
        /// Avec historique : archivage seulement. Retourne true si quelque chose a changé.
        /// </summary>
        public static bool Archiver(TypeElement type, (int id, string nom, bool actif)? selection)
        {
            if (selection == null)
            {
                MessageBox.Show("Sélectionnez d'abord un élément.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            var (id, nom, actif) = selection.Value;
            if (!actif)
            {
                MessageBox.Show($"« {nom} » est déjà archivé.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            try
            {
                if (!ArchivageService.AHistorique(type, id))
                {
                    var r = MessageBox.Show(
                        $"« {nom} » n'a aucun historique.\n\n" +
                        "Oui = supprimer définitivement\nNon = archiver seulement\nAnnuler = ne rien faire",
                        "Supprimer ou archiver ?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (r == DialogResult.Cancel) return false;
                    if (r == DialogResult.Yes)
                    {
                        ArchivageService.SupprimerDefinitivement(type, id);
                        return true;
                    }
                }
                else if (MessageBox.Show(
                    $"Archiver « {nom} » ?\n\n" +
                    "Il n'apparaîtra plus dans les choix (ventes, commandes…) mais restera dans l'historique et les statistiques. " +
                    "Vous pourrez le réactiver à tout moment.",
                    "Archiver", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return false;
                }

                ArchivageService.DefinirActif(type, id, false);
                return true;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Action impossible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (Exception ex)
            {
                Journal.Erreur("Archivage", ex);
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
