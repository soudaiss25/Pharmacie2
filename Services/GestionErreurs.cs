namespace Pharmacie2.Services
{
    /// <summary>
    /// Gestion globale des erreurs imprévues : journal détaillé, message simple en français (sans pile d'appels).
    /// </summary>
    public static class GestionErreurs
    {
        public static void Installer()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) => Signaler(e.Exception, fatale: false);
            AppDomain.CurrentDomain.UnhandledException += (s, e) => Signaler(e.ExceptionObject as Exception, fatale: true);
        }

        public static void Signaler(Exception? ex, bool fatale)
        {
            Journal.Erreur(fatale ? "Erreur fatale non gérée" : "Erreur non gérée", ex);
            try
            {
                var reponse = MessageBox.Show(MessageUtilisateur(fatale) + "\n\nVoulez-vous préparer un rapport à envoyer au développeur ? (aucune donnée de la pharmacie n'y figure)",
                    "Erreur", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (reponse == DialogResult.Yes)
                {
                    RapportDeveloppeurService.PreparerSurLeBureauEtMontrer();
                    MessageBox.Show(RapportDeveloppeurService.MessageApresRapport, "Rapport pour le développeur", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex2)
            {
                Journal.Erreur("Affichage du message d'erreur impossible", ex2);
            }
            if (fatale)
                Environment.Exit(1);
        }

        /// <summary>Message affiché à l'utilisateur : aucune information technique.</summary>
        public static string MessageUtilisateur(bool fatale)
            => "Une erreur inattendue s'est produite. Vos données déjà enregistrées ne sont pas perdues.\n\n" +
               (fatale
                   ? "L'application va se fermer. Relancez-la ; si l'erreur se reproduit, contactez le développeur."
                   : "Recommencez l'opération. Si l'erreur se reproduit, contactez le développeur.") +
               "\n\nL'erreur a été notée dans le journal.";
    }
}
