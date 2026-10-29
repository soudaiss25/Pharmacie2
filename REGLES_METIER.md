# Règles métier du client (non négociables)

L'utilisateur final est l'oncle du développeur. Ces règles viennent de lui : aucune ne se change sans son accord.
**À relire avant chaque lot de modifications.** Un test (dans `tests/Pharmacie2.Tests/ReglesClient/`, ou un test
existant cité ici) vérifie chaque règle quand c'est automatisable ; sinon la vérification est manuelle
(section de `TESTS_MANUELS.md`).

| # | Règle | Vérification |
|---|-------|--------------|
| 1 | Ouverture de caisse SANS fond de caisse (aucun montant de départ). | Test `Regle01_*` · manuel : §50 |
| 2 | Un caissier ne voit que les ventes de SA session. | Manuel : §36 (page du Caissier) |
| 3 | Facture complète : produits, total, vendeur, client, date, posologie. | Manuel : §1 (facture) |
| 4 | Posologie affichée dès qu'on choisit le médicament. | Manuel : §1 (choix du produit) |
| 5 | Vente à la plaquette possible ; stock toujours en unités de base. | Test `Regle05_*` ; `StockServiceTests`, `VenteTransactionTests` |
| 6 | Crédit et Mutuelle sont des VENTES PARTIELLES : partout, on n'affiche comme encaissé que ce qui a été réellement reçu ; le reste est affiché à part (« reste dû », « part mutuelle en attente »). | Test `Regle06_*` (`Regle06VentesPartiellesTests`) |
| 7 | La part mutuelle est un crédit de l'entreprise jusqu'au clic sur « Régler ». | `VenteMutuelleTests` (`MutuelleReglee_*`) |
| 8 | Le client fixe lui-même le prix de la boîte ET le prix à l'unité ; l'application ne calcule jamais l'un à partir de l'autre. | Tests `Regle08_*` (`Regle08PrixUniteTests`) |
| 9 | Prix de vente > prix d'achat, obligatoire (à la boîte ET à l'unité). | Tests `Regle09_*` (`Regle08PrixUniteTests`) |
| 10 | Clôture de caisse sans mot de passe. | Manuel : §50 |
| 11 | Chèque, Carte bancaire, Mvola et Huri Money sont toujours payés en totalité au moment de la vente. | `FormVenteTests.Modes_regles_en_totalite_enregistrent_toujours_le_montant_verse` ; test `Regle11_*` |
| 12 | Montants en KMF sans centimes. | Test `Regle12_*` ; `FondationsTests` |
| 13 | Verrouillage automatique après 5 minutes d'inactivité (réglage par défaut validé par le client). | Test `Regle13_*` ; `VerrouillageTests` |
| 14 | Le prix de vente n'est jamais recalculé automatiquement ; la marge s'affiche automatiquement (marge = prix de vente − prix d'achat, en KMF et en % du prix d'achat, à l'unité pour un produit au détail). | Tests `Regle14_*` (`Regle14MargeTests`) |
| 15 | La marge d'une vente utilise le prix d'achat du jour de la vente (coût figé sur chaque ligne de vente ; une ligne sans coût connu est exclue de la marge et signalée). | Tests `Regle15_*` (`Regle15CoutAchatTests`) |
| 16 | Le coût des médicaments est compté au moment de la vente. Une dépense « Remboursement commande » est une note reliée à sa commande : elle réduit le reste à payer et n'entre jamais dans le bénéfice. Les dépenses sont payées avec la trésorerie de la pharmacie : elles ne touchent jamais à une caisse. | Tests `RemboursementCommandeTests` |

## Conséquences de calcul (déduites des règles ci-dessus)

- L'argent attendu dans le tiroir = les **espèces réellement reçues** pendant la session (voir R2b dans `FIABILISATION.md`).
- Le mode de chaque paiement est enregistré (Espèces, Chèque, Carte bancaire, Mvola, Huri Money) ; seules les espèces sont dans le tiroir.
- Un montant rendu au client n'est jamais soustrait une seconde fois : le paiement enregistré est déjà net du rendu.
