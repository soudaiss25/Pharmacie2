# Fiabilisation de LIGUAPHARME (Pharmacie2)

Logiciel de gestion d'une petite pharmacie à Mohéli (KMF, travail en local, WinForms .NET 8, SQLite).
**L'utilisateur final est l'oncle du développeur** (le client), pas « une pharmacienne » : ses règles métier sont dans
[REGLES_METIER.md](REGLES_METIER.md) et sont non négociables.

Objectif : aucune perte de données, calculs de stock et d'argent justes, interface claire en français.
Règles de travail : pas de déploiement, pas de merge sur `master`, `Pharmacie2.sln` jamais commité, migrations seulement via EF et
testées sur base remplie, archiver au lieu de supprimer, un commit par étape, tests uniquement sur GitHub Actions.

## État d'avancement

| Lot | Contenu | Statut |
|-----|---------|--------|
| Phases 3 à 11 | Stock et réceptions, archivage, mots de passe, ventes, erreurs et journal, statistiques, 16 premières migrations | Fait |
| Généralisation de l'interface | Tous les écrans en `AutoScaleMode.Dpi`, mode compact (< 1200 unités), tableaux uniformes, mise en page testée aux 5 configurations | **Fait** |
| 7a | Sauvegarde chiffrée hors du PC (dossier synchronisé / clé USB) | **Fait** |
| 7b | Rapport pour le développeur (ZIP sans donnée personnelle) | **Fait** |
| 7c | Verrouillage automatique (5 minutes par défaut) | **Fait** |
| R1 à R8 | Règles du client et anciennes erreurs de caisse (voir ci-dessous) | Fait, CI vert |
| Phase 12 | Déploiement, retrait de `PharmacieSetup.vdproj` | Hors périmètre : rien sans accord |

## R1 à R8 : règles du client et anciennes erreurs de caisse

| Point | Sujet | Statut | Migration | Tests |
|-------|-------|--------|-----------|-------|
| R0 | `REGLES_METIER.md`, un test par règle (`ReglesClient/`) | Fait | — | `ReglesClientTests`, `Regle06…`, `Regle08…` |
| R1 | Plus de fond de caisse (champ supprimé, `FondOuverture` = 0, colonne masquée, export sans fond) | Fait | — (colonne gardée en base pour l'historique) | `Regle01…` |
| R2a | Mode de chaque paiement (`Paiement.Mode` : Espèces, Chèque, Carte bancaire, Mvola, Huri Money) | Fait | `AjoutModePaiementPaiement` | `ModePaiementMigrationTests`, `FormVenteTests` |
| R2b | Montant attendu = espèces réellement reçues (`CaisseService`, un seul calcul partout) | Fait | — | `CaisseServiceTests` |
| R2c | Montant attendu figé à la clôture ; anciennes sessions « (recalculé) » | Fait | `AjoutAttenduCloture` | `ClotureTests` |
| R3 | Ventes partielles : crédit = versements reçus + reste dû à part ; mutuelle = part patient reçue + part des entreprises en attente | Fait | — | `Regle06VentesPartiellesTests` |
| R4 | Modification de vente : complément encaissé ou remboursement négatif `R-…`, même transaction | Fait | — | `ModificationVentePaiementsTests` |
| R5 | Reprise des anciennes ventes payées en totalité sans paiement complet | Fait | `RepriseVentesPayeesEnTotalite` | `RepriseVentesMigrationTests` |
| R6 | Prix à l'unité fixé par le client (jamais calculé) | Fait | `AjoutPrixVenteUnite` | `Regle08PrixUniteTests` |
| R7 | Ancienne base du déploiement d'avril (recherche élargie, choix, écran « Base de données introuvable ») | Fait | — | `MigrationEmplacementBaseTests` |
| R8 | Documentation (ce fichier, `TESTS_MANUELS.md` §50 et §51) | Fait | — | — |

### Décisions de conception

- **Espèces attendues** = somme des `Paiement.Montant` en Espèces, du caissier de la session, datés pendant la session, ventes non annulées ;
  remboursements négatifs inclus ; le rendu de monnaie n'est jamais soustrait (le paiement est déjà net) ; les autres modes sont « Hors tiroir ».
- Le règlement d'une part de mutuelle par l'entreprise (écran Mutuelles, `MutuelPaiement`) n'est pas un paiement de caisse : il n'entre pas dans le tiroir.
- Remboursement lors d'une modification : même mode que le dernier paiement reçu de la vente ; complément : mode choisi, Espèces par défaut.
- Crédit : aucun paiement automatique à la modification, sauf remboursement si le versé dépasse le nouveau total.
- Reprise des prix à l'unité : `ROUND(PrixVente / NbUniteParBoite, 0)` pour que les ventes continuent comme avant ; la liste (produit, prix de la boîte, prix repris) est écrite dans le journal au premier démarrage pour que le client la vérifie.
- Une base est « non vide » si elle contient au moins une vente (ou est illisible) ; une base sans vente reste une trace d'installation.
