# Publier une version (mises à jour automatiques Velopack)

Pour le développeur. Les mises à jour de LIGUAPHARME arrivent toutes seules chez le client depuis les **Releases GitHub** du dépôt
(<https://github.com/soudaiss25/Pharmacie2/releases>). Tout est automatique : on pousse un **tag**, le workflow
[`release.yml`](.github/workflows/release.yml) fait le reste. Aucun jeton n'est écrit dans le dépôt (le workflow utilise le `GITHUB_TOKEN` fourni par GitHub).

## Numérotation des versions

- Format **X.Y.Z** (ex. `1.0.0`, `1.1.0`, `1.1.1`), jamais quatre chiffres.
- Z : correction d'un bug sans changement visible ; Y : nouvelle fonction ou migration de la base ; X : refonte.
- **Une version ne se réutilise jamais** et ne diminue jamais : Velopack n'installe qu'une version plus grande que celle installée.
- La version est écrite **une seule fois**, dans `Pharmacie2.csproj` (`<Version>1.0.0</Version>`). Le tag doit lui être égal.
- Un tag `vX.Y.Z-test` publie sur le canal **test** ; un tag `vX.Y.Z` publie sur le canal **stable**.

## Publier une version de test, la tester, puis la publier en stable

1. Changer `<Version>` dans `Pharmacie2.csproj` (ex. `1.1.0`). Commiter, pousser sur la branche, **attendre que le CI « Tests » soit vert**.
2. Créer et pousser le tag **de test** sur ce commit :
   ```bash
   git tag -a v1.1.0-test -m "LIGUAPHARME 1.1.0 (test)"
   git push origin v1.1.0-test
   ```
3. Suivre le workflow : `gh run list --workflow release.yml` puis `gh run watch`. Il (a) vérifie que le tag est égal à `<Version>`,
   (b) lance **tous les tests** (un seul échec : rien n'est publié), (c) publie l'application autonome, (d) récupère la release précédente
   du canal pour fabriquer les paquets delta, (e) fabrique les paquets avec `vpk pack` (identifiant `LIGUAPHARME`, titre « LIGUAPHARME », icône de l'application),
   (f) dépose le tout dans une Release **pré-release** nommée « LIGUAPHARME 1.1.0-test ».
4. **Tester sur votre PC, canal Test** :
   - première fois : installer le fichier `…-test-Setup.exe` de la Release (voir `TESTS_MANUELS.md` §55) ;
   - Administration → *Canal de mise à jour* → **Test** ; *Rechercher une mise à jour maintenant* ;
   - une fois la version installée et vérifiée sur une COPIE de la base, passer à l'étape 5.
5. Publier en **stable**, sur le MÊME commit (donc exactement le code testé) :
   ```bash
   git tag -a v1.1.0 -m "LIGUAPHARME 1.1.0"
   git push origin v1.1.0
   ```
   Le workflow refait les tests et publie la Release stable. Chez le client, la mise à jour est trouvée dans les 4 heures suivant l'ouverture
   de l'application, téléchargée en arrière-plan, puis installée à la prochaine fermeture.
6. Vérifier : la Release `v1.1.0` n'est pas une pré-release et contient `releases.stable.json`.

> Ne publiez jamais en stable une version qui n'a pas d'abord été publiée et essayée en test.

## Le workflow échoue : que faire ?

| Message / étape en échec | Cause | Que faire |
|---|---|---|
| « Tag invalide » | Le tag n'est pas `vX.Y.Z` ou `vX.Y.Z-test` | Supprimer le tag (ci-dessous) et le recréer correctement |
| « Version différente » | `<Version>` du csproj ≠ version du tag | Corriger le csproj, commiter, supprimer le tag, le recréer **sur le nouveau commit** |
| Étape `dotnet test` | Un test échoue | Aucune publication n'a eu lieu. Corriger, commiter, supprimer le tag, le recréer |
| `Fabriquer les paquets` | Problème de `vpk` ou de publication | Lire le journal de l'étape ; vérifier que `.config/dotnet-tools.json` et le package `Velopack` du csproj ont la **même version** (un test le contrôle) |
| `Publier la Release` | Release de même nom déjà existante, droits | Supprimer la Release partielle (ci-dessous) puis relancer |

Supprimer un tag et une Release (publiée ou partielle) :

```bash
gh release delete vX.Y.Z-test --cleanup-tag --yes   # supprime la Release ET le tag distant
git tag -d vX.Y.Z-test                              # supprime le tag local
```

Puis recréer le tag après correction. **Ne jamais supprimer une Release stable que des clients ont déjà téléchargée** : publier plutôt une version plus grande
(voir [RETOUR_ARRIERE.md](RETOUR_ARRIERE.md)).

## Rappels

- Les tests ne tournent que sur GitHub Actions (le PC de développement bloque les binaires compilés localement).
- `vpk` est un outil local du dépôt : `dotnet tool restore` puis `dotnet tool run vpk …`. Sa version doit rester égale à celle du package `Velopack`.
- Les Releases de test sont des pré-releases : le canal stable ne les voit jamais. Pour qu'un PC en canal Test voie une version test, la **dernière**
  Release du dépôt doit être une version test ; si une version stable plus récente existe, republier une version test plus grande.
- La mise à jour ne touche jamais aux données : l'application s'installe dans `%LOCALAPPDATA%\LIGUAPHARME`, la base, les sauvegardes et les journaux
  sont dans `%LOCALAPPDATA%\Pharmacie2Data` (un test vérifie que ces deux dossiers sont distincts).
- Une version qui contient des migrations de base est protégée par la sauvegarde « avant-migration » faite automatiquement au démarrage
  (jamais supprimée) : voir [RETOUR_ARRIERE.md](RETOUR_ARRIERE.md).
