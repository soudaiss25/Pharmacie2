namespace Pharmacie2;

/// <summary>Un message que l'application s'apprête à afficher (texte, titre, boutons, icône).</summary>
public sealed record MessageAffiche(string Texte, string Titre, MessageBoxButtons Boutons, MessageBoxIcon Icone);

/// <summary>
/// Point unique d'affichage des messages. En fonctionnement normal, c'est une MessageBox. Les tests installent un intercepteur :
/// aucune fenêtre ne s'ouvre (donc rien ne peut bloquer un test sur GitHub Actions) et les messages d'erreur sont enregistrés.
/// </summary>
public static class Messages
{
    /// <summary>Si renseigné et s'il renvoie un résultat, ce résultat remplace l'affichage d'une fenêtre.</summary>
    public static Func<MessageAffiche, DialogResult?>? Intercepteur { get; set; }

    private static DialogResult AfficherInterne(IWin32Window? proprietaire, string texte, string titre, MessageBoxButtons boutons, MessageBoxIcon icone)
    {
        var intercepte = Intercepteur?.Invoke(new MessageAffiche(texte, titre, boutons, icone));
        if (intercepte.HasValue) return intercepte.Value;
        return proprietaire == null
            ? MessageBox.Show(texte, titre, boutons, icone)
            : MessageBox.Show(proprietaire, texte, titre, boutons, icone);
    }

    public static DialogResult Afficher(string texte)
        => AfficherInterne(null, texte, "", MessageBoxButtons.OK, MessageBoxIcon.None);

    public static DialogResult Afficher(string texte, string titre)
        => AfficherInterne(null, texte, titre, MessageBoxButtons.OK, MessageBoxIcon.None);

    public static DialogResult Afficher(string texte, string titre, MessageBoxButtons boutons)
        => AfficherInterne(null, texte, titre, boutons, MessageBoxIcon.None);

    public static DialogResult Afficher(string texte, string titre, MessageBoxButtons boutons, MessageBoxIcon icone)
        => AfficherInterne(null, texte, titre, boutons, icone);

    public static DialogResult Afficher(IWin32Window proprietaire, string texte)
        => AfficherInterne(proprietaire, texte, "", MessageBoxButtons.OK, MessageBoxIcon.None);

    public static DialogResult Afficher(IWin32Window proprietaire, string texte, string titre)
        => AfficherInterne(proprietaire, texte, titre, MessageBoxButtons.OK, MessageBoxIcon.None);

    public static DialogResult Afficher(IWin32Window proprietaire, string texte, string titre, MessageBoxButtons boutons)
        => AfficherInterne(proprietaire, texte, titre, boutons, MessageBoxIcon.None);

    public static DialogResult Afficher(IWin32Window proprietaire, string texte, string titre, MessageBoxButtons boutons, MessageBoxIcon icone)
        => AfficherInterne(proprietaire, texte, titre, boutons, icone);
}
