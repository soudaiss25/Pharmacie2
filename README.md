# Pharmacie2 (LIGUAPHARME)

Logiciel de bureau de gestion de pharmacie : stock, ventes, caisse, commandes fournisseurs et statistiques, pour une petite pharmacie qui travaille en local, sans connexion internet.

![Capture](docs/screenshot.png)
<!-- TODO : ajouter une capture d'écran (écran d'accueil « Ma journée » avec des données fictives) dans docs/screenshot.png -->

## Contexte

Logiciel en production, utilisé chaque jour par une pharmacie à Mohéli (Comores). Les montants sont en KMF, sans centimes. Les règles métier viennent de l'utilisateur final et sont listées dans [REGLES_METIER.md](REGLES_METIER.md) ; chacune est couverte par un test automatisé ou par un scénario de test manuel.

## Fonctionnalités principales

- **Stock** : produits avec vente à la boîte ou à l'unité, alertes de péremption, détection des stocks à vérifier, archivage plutôt que suppression.
- **Ventes** : paiement en espèces, chèque, carte, Mvola, Huri Money ; ventes à crédit et ventes avec mutuelle (part mutuelle suivie jusqu'à son règlement) ; prestations sans stock.
- **Caisse** : ouverture sans fond de caisse, clôture avec motif obligatoire au-delà de 100 KMF d'écart, une seule caisse ouverte par utilisateur.
- **Commandes fournisseurs** : réception totale ou partielle, remboursement de commande.
- **Statistiques** : tableau de bord « Ma journée », marges, exports PDF et Excel.
- **Utilisateurs et rôles** : Administrateur, Pharmacien, Caissier ; mots de passe hachés (BCrypt), clé de secours, verrouillage automatique après inactivité.
- **Sauvegardes** : sauvegarde automatique et copie externe chiffrée (AES-256-GCM) avec restauration.

## Stack technique

- C# .NET 8, Windows Forms
- SQLite via Entity Framework Core 8 (migrations)
- BCrypt.Net, ClosedXML (Excel), PdfSharp-MigraDoc (PDF)
- Velopack (installation et mises à jour automatiques)
- xUnit, GitHub Actions

## Points techniques intéressants

- **Base de données** : 30 migrations EF Core, dont plusieurs testées sur une base déjà remplie (`MigrationTests`, `ArchivageMigrationTests`, `RepriseVentesMigrationTests`…).
- **Sécurité** : mots de passe et clé de secours hachés avec BCrypt, blocage temporaire après 5 erreurs de clé de secours, verrouillage automatique, copie externe chiffrée AES-256-GCM (clé dérivée d'une phrase secrète par PBKDF2, protégée par DPAPI).
- **Respect de la vie privée dans le support** : le rapport pour le développeur (ZIP de journaux) est testé pour ne contenir aucune donnée métier (`RapportSansDonneesPersonnellesTests`).
- **Tests** : plus de 50 fichiers de tests (métier, migrations, écrans pilotés par code). `LayoutTests` vérifie la mise en page de tous les écrans sur plusieurs résolutions et niveaux de zoom.
- **CI/CD** : [`tests.yml`](.github/workflows/tests.yml) lance les tests sur chaque push ; [`release.yml`](.github/workflows/release.yml) publie une version Velopack à partir d'un tag (`vX.Y.Z-test` pour le canal de test, `vX.Y.Z` pour le canal stable), après contrôle que le tag correspond à la version du projet. Voir [PUBLIER.md](PUBLIER.md).

## Installation et lancement

Pour l'utilisateur final : installer le fichier `Setup.exe` d'une [release](https://github.com/soudaiss25/Pharmacie2/releases) (voir [INSTALLATION_CLIENT.md](INSTALLATION_CLIENT.md)).

Pour le développement (Windows, SDK .NET 8) :

```bash
git clone https://github.com/soudaiss25/Pharmacie2.git
cd Pharmacie2
git checkout fiabilisation
dotnet run --project Pharmacie2.csproj
```

Au premier lancement, l'application demande de créer le compte administrateur. La base SQLite est créée dans `%LOCALAPPDATA%\Pharmacie2Data`.

## Tests

```bash
dotnet test tests/Pharmacie2.Tests/Pharmacie2.Tests.csproj
```

La variable `PHARMACIE2_DATA_DIR` isole les données des tests (voir [TESTS.md](TESTS.md)). Ne jamais la définir sur le PC de la pharmacie.

![Tests](https://github.com/soudaiss25/Pharmacie2/actions/workflows/tests.yml/badge.svg?branch=fiabilisation)

## Ce que j'ai appris

- Fiabiliser un logiciel existant sans casser les données : migrations testées sur base remplie, archivage au lieu de suppression, retour arrière documenté ([RETOUR_ARRIERE.md](RETOUR_ARRIERE.md)).
- Traduire des règles métier orales en règles vérifiables, chacune reliée à un test.
- Faire tenir une interface Windows Forms sur des écrans et des niveaux de zoom très différents.

## Statut

En production. Développement actif sur la branche `fiabilisation`.
