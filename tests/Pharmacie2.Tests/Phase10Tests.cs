using Pharmacie2.Models;
using Pharmacie2.Services;
using Xunit;

public class Phase10Tests
{
    [Fact]
    public void Message_d_erreur_simple_en_francais_sans_pile_d_appels()
    {
        foreach (bool fatale in new[] { false, true })
        {
            string m = GestionErreurs.MessageUtilisateur(fatale);
            Assert.Contains("erreur inattendue", m);
            Assert.Contains("journal", m);
            Assert.DoesNotContain("Exception", m);
            Assert.DoesNotContain(" at ", m);
            Assert.DoesNotContain("System.", m);
        }
        Assert.Contains("va se fermer", GestionErreurs.MessageUtilisateur(true));
        Assert.DoesNotContain("va se fermer", GestionErreurs.MessageUtilisateur(false));
    }

    [Fact]
    public void Roles_gardent_les_valeurs_enregistrees_en_base()
    {
        Assert.Equal("Administrateur", Roles.Administrateur);
        Assert.Equal("Pharmacien", Roles.Pharmacien);
        Assert.Equal("Caissier", Roles.Caissier);
        Assert.True(Roles.EstGestionnaire(Roles.Pharmacien));
        Assert.False(Roles.EstGestionnaire(Roles.Caissier));
        Assert.False(Roles.EstGestionnaire(null));
    }

    [Fact]
    public void Deconnexion_vide_l_utilisateur_et_la_session_de_caisse()
    {
        SessionUtilisateur.Courant = new User { Id = 7, Nom = "A", Prenom = "B" };
        SessionUtilisateur.SessionCaisseEnCours = new SessionCaisse { Id = 3 };

        SessionUtilisateur.Deconnecter();

        Assert.Null(SessionUtilisateur.Courant);
        Assert.Null(SessionUtilisateur.SessionCaisseEnCours);
        Assert.Equal(0, SessionUtilisateur.IdCourant);
    }
}
