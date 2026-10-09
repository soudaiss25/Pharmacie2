namespace Pharmacie2.Services
{
    /// <summary>Mesure l'inactivité (clavier, souris). L'horloge est injectable pour les tests.</summary>
    public sealed class SuiviInactivite
    {
        public Func<DateTime> Maintenant { get; set; } = () => DateTime.Now;
        public DateTime DerniereActivite { get; private set; }

        public SuiviInactivite() => DerniereActivite = DateTime.Now;

        public void Activite() => DerniereActivite = Maintenant();

        /// <summary>Vrai si le délai est actif (&gt; 0 minute) et que l'utilisateur n'a rien fait depuis au moins ce délai.</summary>
        public bool DoitVerrouiller(int delaiMinutes)
            => delaiMinutes > 0 && Maintenant() - DerniereActivite >= TimeSpan.FromMinutes(delaiMinutes);
    }

    /// <summary>
    /// Limite les essais de mot de passe de l'écran de verrouillage : à partir de 5 mots de passe faux de suite,
    /// 30 secondes d'attente entre deux essais (journalisé). Un essai réussi remet le compteur à zéro.
    /// </summary>
    public sealed class LimiteEssais
    {
        public const int EssaisAvantDelai = 5;
        public const int SecondesDelai = 30;

        public Func<DateTime> Maintenant { get; set; } = () => DateTime.Now;
        public int Echecs { get; private set; }
        private DateTime? _bloqueJusqua;

        /// <summary>Vrai si un essai est possible ; sinon, renvoie le nombre de secondes à attendre.</summary>
        public bool PeutEssayer(out int secondesRestantes)
        {
            secondesRestantes = 0;
            if (_bloqueJusqua is DateTime fin && Maintenant() < fin)
            {
                secondesRestantes = (int)Math.Ceiling((fin - Maintenant()).TotalSeconds);
                return false;
            }
            return true;
        }

        public void EnregistrerEchec(string login)
        {
            Echecs++;
            if (Echecs >= EssaisAvantDelai)
            {
                _bloqueJusqua = Maintenant().AddSeconds(SecondesDelai);
                Journal.Info($"Écran de verrouillage : {Echecs} mots de passe erronés de suite pour le compte « {login} », {SecondesDelai} secondes d'attente entre les essais.");
            }
        }

        public void EnregistrerReussite()
        {
            Echecs = 0;
            _bloqueJusqua = null;
        }
    }
}
