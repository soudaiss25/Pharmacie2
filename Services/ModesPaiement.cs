namespace Pharmacie2.Services
{
    /// <summary>
    /// Moyens de paiement (valeurs enregistrées dans ventes.MoyenPaiement et ventes.Type).
    /// Mvola est le service de Telma Comores (l'ancien libellé « Mvolo » était une faute).
    /// </summary>
    public static class ModesPaiement
    {
        public const string Comptant = "Comptant";
        public const string Credit = "Crédit";
        public const string Mutuelle = "Mutuelle";
        public const string Cheque = "Chèque";
        public const string CarteBancaire = "Carte bancaire";
        public const string Mvola = "Mvola";
        public const string HuriMoney = "Huri Money";

        public static readonly string[] Tous = { Comptant, Credit, Mutuelle, Cheque, CarteBancaire, Mvola, HuriMoney };

        /// <summary>Paiements électroniques réglés en totalité à la vente.</summary>
        public static readonly string[] Mobiles = { Mvola, HuriMoney };

        /// <summary>Modes réglés en totalité au moment de la vente (montant versé = total).</summary>
        public static readonly string[] ReglesEnTotalite = { Cheque, CarteBancaire, Mvola, HuriMoney };

        public static bool EstMobile(string? mode) => mode == Mvola || mode == HuriMoney;
    }
}
