# Tests

Les tests sont dans `tests/Pharmacie2.Tests` (xunit) et tournent dans GitHub Actions
(`.github/workflows/tests.yml`, branche `fiabilisation`).

En local : `dotnet test tests/Pharmacie2.Tests/Pharmacie2.Tests.csproj --filter "Category!=Alignement"`

## Variable `PHARMACIE2_DATA_DIR` — usage réservé aux tests

Si elle est définie, `CheminsApp.DossierDonnees` pointe vers ce dossier au lieu de
`%LOCALAPPDATA%\Pharmacie2Data`. Elle sert uniquement à isoler les tests (dossier temporaire du runner).

- **Ne jamais la définir sur le PC de la pharmacie.**
- Les tests refusent de s'exécuter si le dossier n'est pas un dossier temporaire
  ou s'il se termine par `Pharmacie2Data`, et ne suppriment que des fichiers `PharmacieDB.sqlite*` de ce dossier.

## Job « alignement »

Teste la proposition `propositions/AlignementDepensesAnnexes` (non appliquée en production) sur une
base remplie : comptes de lignes identiques avant/après et clé étrangère passée en `SET NULL`.
