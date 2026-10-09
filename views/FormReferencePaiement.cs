using Pharmacie2.Services;

namespace Pharmacie2.views
{
    /// <summary>Saisie facultative de la référence d'un règlement de mutuelle (virement, chèque…).</summary>
    public partial class FormReferencePaiement : Form
    {
        public FormReferencePaiement()
        {
            InitializeComponent();
            Theme.Appliquer(this);
        }

        public string Reference => txtReference.Text.Trim();
    }
}
