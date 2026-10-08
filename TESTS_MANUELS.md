# Check-list de tests manuels (phases 3 à 6)

À faire dans Visual Studio (F5) **sur une copie de la base**, jamais sur la base de la pharmacie.
Copie de travail : fermer l'application, copier `%LOCALAPPDATA%\Pharmacie2Data\PharmacieDB.sqlite` ailleurs,
ou travailler sur une base de test créée par l'application au premier lancement.

**Préparation** : créer 2 produits.
- A « Test Plaquettes » : vendu au détail, 5 plaquettes par boîte, stock 1 boîte + 2 plaquettes (= 7 unités), seuil 1.
- B « Test Boîtes » : vendu à la boîte uniquement, stock 10 boîtes.

Noter à chaque étape le stock affiché dans l'écran **Stock**.

## 1. Vente (phase 3)
- [ ] Produit A : ajouter **2 boîtes** → refus « Stock insuffisant », message indiquant `1 boîte(s) + 2 plaquette(s)`.
- [ ] Produit A : ajouter **1 boîte** → accepté.
- [ ] Même vente, ajouter encore **1 boîte** de A → refusé (quantité cumulée du panier).
- [ ] Même vente, ajouter **2 plaquettes** de A → accepté (1 boîte + 2 plaquettes = 7).
- [ ] Ajouter **1 plaquette** de plus → refusé.
- [ ] Valider (paiement comptant). Stock A = **0**. Une facture s'imprime/prévisualise.
- [ ] Produit B : vendre 3 boîtes → stock B = 7 boîtes.
- [ ] Numéros de vente : deux ventes consécutives ont des numéros qui se suivent (V-xxxxxx).
- [ ] Ouvrir **deux fenêtres de vente** (ou deux postes) sur le même produit A (stock 7) : mettre 1 boîte dans chacune, valider la première puis la seconde → la **seconde est refusée** (« Stock insuffisant … Aucune vente n'a été enregistrée »), stock inchangé par la seconde.

## 2. Annulation de vente
- [ ] Annuler la vente du produit A (étape 1) avec un motif → stock A redevient **7 unités** (`1 boîte(s) + 2 plaquette(s)`).
- [ ] Annuler une seconde fois la même vente → « déjà annulée ».
- [ ] Modifier la fiche de A : passer à **4 plaquettes par boîte** (voir §4), puis annuler une ancienne vente de A faite à 5/boîte → le stock restitué correspond à ce qui avait été vendu (ex. 2 boîtes vendues à 5/boîte = +10, pas +8).

## 3. Modification de vente
- [ ] Vente de 3 boîtes de B (stock 7) : la modifier pour 5 boîtes → stock B = 5 (3 restituées, 5 retirées).
- [ ] La modifier pour 20 boîtes → refus (stock insuffisant), la vente reste à 5 boîtes, stock inchangé.
- [ ] Supprimer une ligne et enregistrer → le stock de ce produit est restitué.
- [ ] Une vente annulée ne peut pas être modifiée.

## 4. Fiche produit (phase 5)
- [ ] Stock de A = 9 unités (1 boîte + 4 plaquettes) : ouvrir la fiche → « Boîtes pleines » = 1, « unités en vrac » = 4.
- [ ] Changer **uniquement le prix** → enregistrer → le stock reste **1 boîte + 4 plaquettes**.
- [ ] Saisir 5 unités en vrac (boîte de 5) → refusé.
- [ ] Passer à 4 plaquettes/boîte → enregistrer : un **avertissement** demande de ressaisir le stock réel ; rien n'est modifié. Ressaisir, enregistrer : une **confirmation** du stock apparaît.
- [ ] Produit B : « unités en vrac » est grisé.

## 5. Affichage du stock (phase 4)
- [ ] A à 9 unités : « 1 boîte(s) + 4 plaquette(s) » dans Stock, Produits, Fournisseurs (produits du fournisseur), Choix produit (vente), Commandes (colonne stock), Statistiques (alertes).
- [ ] Alerte : A (seuil 1 boîte = 5 unités) passe en « Alerte » à 5 unités ou moins, pas avant.

## 6. Réception de commande (phase 6)
- [ ] Commander **10 boîtes** de A (suggestion de quantité cohérente avec le seuil en boîtes). Réceptionner → stock A **+50 unités**.
- [ ] Commander 10 boîtes, **réception partielle** : saisir 4 → stock +20, statut « Reçu partiellement ». Puis « Marquer reçu » → +30, statut « Reçu ». Total 50.
- [ ] Tenter de réceptionner à nouveau une commande « Reçu » → impossible.
- [ ] Les deux écrans (Commandes globales et Commandes d'un fournisseur) donnent le même résultat.
- [ ] **Commande « Reçu partiellement » ancienne** (créée avant ces correctifs) : à l'ouverture, avertissement « Quantité déjà reçue inconnue, vérifiez avant de réceptionner » ; « Marquer reçu » est refusé ; la réception ligne par ligne (écran du fournisseur) fonctionne.

## 7. Sauvegardes et emplacement (phases 1-2)
- [ ] Après lancement, `%LOCALAPPDATA%\Pharmacie2Data\Sauvegardes` contient `PharmacieDB_AAAA-MM-JJ.sqlite`.
- [ ] Bouton « Dossier des sauvegardes » dans le menu : ouvre ce dossier.
- [ ] Lancer l'application depuis un autre dossier : même base, mêmes données.

---

# Stocks « À vérifier » et feuille d'inventaire

## 8. Préparer une base de test avec des produits « À vérifier »

La détection se fait **une seule fois**, pendant la migration `DetectionStocksAVerifier`. Il faut donc créer les
commandes avec l'ancienne version, puis lancer la nouvelle. Travailler sur une base de test (jamais celle de la pharmacie).

**Méthode A (fidèle à la réalité)**
1. Sauvegarder (renommer) votre dossier `%LOCALAPPDATA%\Pharmacie2Data` s'il existe, pour repartir d'une base de développement.
2. Dans Visual Studio : `git stash` si besoin, puis `git checkout 19c3fd0` (phase 5, **avant** la correction de la réception).
3. Lancer (F5). Créer un utilisateur Administrateur, un fournisseur « Four test », puis 5 produits :
   - P1 « Test reçu » : 5 plaquettes/boîte, stock 0 ; P2 « Test reçu boîte » : vendu à la boîte uniquement (1 par boîte) ;
   - P3 « Test partiel » : 5/boîte ; P4 « Test sans commande » : 5/boîte, stock 30 ; P5 « Test en attente » : 5/boîte.
4. Écran Commandes : commander **10 boîtes** de P1 → « Marquer reçu » (le stock de P1 passe à 10 au lieu de 50 : c'est l'ancien défaut). Commander 10 boîtes de P2 → « Marquer reçu ». Commander 6 boîtes de P3 → « Reçu partiellement » (saisir 3). Commander 4 boîtes de P5 → ne pas réceptionner.
5. Fermer l'application, puis `git checkout fiabilisation` et relancer (F5) : la migration s'exécute au démarrage.
6. Résultat attendu : P1 marqué (estimation 40), P3 marqué (sans estimation), P2, P4, P5 **non** marqués.

**Méthode B (rapide)** : sur une base à jour, avec un outil SQLite (par ex. DB Browser), créer les mêmes produits et commandes, puis
`UPDATE produits SET StockAVerifier=1, UnitesManquantesEstimees=40, MotifVerification='10 boîte(s) reçue(s) avant la correction : seulement 10 unité(s) ajoutée(s) au lieu de 50.' WHERE Nom='Test reçu';`
(pour P3 : `UnitesManquantesEstimees=0`). Cette méthode ne teste pas la migration elle-même (déjà couverte par les tests automatiques).

Avant chaque série de tests, faire une copie de la base pour pouvoir recommencer.

## 9. Message à la connexion
- [ ] Se connecter en **Administrateur** : message « 2 produit(s) ont peut-être un stock incorrect… Voulez-vous les vérifier maintenant ? ». Une seule fois (pas de second message en naviguant dans l'application).
- [ ] « **Plus tard** » : le message se ferme, le tableau de bord s'ouvre normalement.
- [ ] Se déconnecter, se reconnecter : le message réapparaît (une fois par session).
- [ ] « **Oui** » : l'écran **Stock** s'ouvre, filtré sur « À vérifier » (P1 et P3 seulement).
- [ ] Même test avec un **Pharmacien**.
- [ ] Un **Caissier** ne voit **aucun** message.
- [ ] Base sans produit marqué : aucun message, aucun changement visible.

## 10. Écran Stock
- [ ] Colonne « Vérification » avec « ⚠ À vérifier » sur P1 et P3, lignes surlignées en jaune.
- [ ] Dans la liste complète, les produits marqués sont **en tête**.
- [ ] Le filtre (liste déroulante du seuil) propose « À vérifier » : seuls P1 et P3 restent.
- [ ] Les autres filtres (« Sous le seuil », « En rupture »…) fonctionnent toujours.

## 11. Fiche produit d'un produit marqué
Ouvrir P1 (« Test reçu », stock 10, estimation 40) :
- [ ] Bandeau orange en haut avec le motif : « 10 boîte(s) reçue(s) avant la correction : seulement 10 unité(s) ajoutée(s) au lieu de 50. »
- [ ] Bouton « Appliquer la correction (+ 40 unités → nouveau stock : 10 boîte(s)) » présent. Cliquer → confirmation → Oui : stock = 50 unités (10 boîtes), la fiche se ferme, le produit n'est plus « À vérifier ». Le journal (`Logs\journal_AAAA-MM.log`) contient la ligne « Vérification de stock… ».
- [ ] Sur une copie fraîche de la base : « Non » à la confirmation → rien ne change, le bandeau reste.
- [ ] Bouton « **Le stock est correct** » (sur une autre copie) : confirmation → le marquage est levé, le stock **n'est pas modifié**.
- [ ] Bouton « **J'ai compté le stock** » : le curseur va sur « Boîtes pleines » ; saisir un nouveau stock, enregistrer → marquage levé, stock = valeur saisie (journalisé).
- [ ] « J'ai compté » puis fermer la fiche **sans enregistrer** → le produit reste « À vérifier ».
- [ ] Enregistrer la fiche en changeant seulement le prix (stock non touché) → le produit **reste** « À vérifier ».

Ouvrir P3 (« Test partiel », Reçu partiellement) :
- [ ] Bandeau orange avec « quantité reçue inconnue… ».
- [ ] **Pas** de bouton « Appliquer la correction » (seulement « Le stock est correct » et « J'ai compté le stock »).

Un produit non marqué (P4) : aucun bandeau.

## 12. Feuille d'inventaire
- [ ] Écran Stock → « 🖨 Feuille d'inventaire » → choisir un emplacement → le fichier `Inventaire_AAAA-MM-JJ.xlsx` se crée et s'ouvre dans Excel sans erreur.
- [ ] Colonnes : Produit, Unité, Stock affiché (ex. « 2 boîte(s) + 3 plaquette(s) »), **Boîtes comptées** et **Unités en vrac comptées** (vides), À vérifier.
- [ ] Les produits « À vérifier » sont **en tête** et surlignés en orange.
- [ ] Annuler la boîte d'enregistrement : rien ne se passe, pas d'erreur.
- [ ] Impression : mise en page paysage, tient en largeur sur une page.

---

# Archivage au lieu de suppression (phase 7)

Sur une copie de la base, avec : un produit déjà **vendu** (A), un produit **jamais vendu** (B), un fournisseur avec commandes (F1),
un fournisseur sans rien (F2), une mutuelle avec ventes (M1), une mutuelle sans rien (M2), un utilisateur avec des ventes (U1, Caissier),
un utilisateur sans historique (U2), et votre compte administrateur.

## 13. Produits (écrans Stock **et** Produits)
- [ ] Le bouton s'appelle maintenant « 📦 Archiver ». À côté : « ♻ Réactiver » et la case « Afficher les archivés ».
- [ ] Archiver A (vendu) : message « Archiver… restera dans l'historique et les statistiques… » → Oui. A disparaît de la liste.
- [ ] Cocher « Afficher les archivés » : A réapparaît, grisé, marqué « 📦 Archivé », en bas de la liste.
- [ ] Vente : « Ajouter un produit » ne propose **pas** A. Commande (Nouvelle commande) : A non proposé non plus.
- [ ] L'historique des ventes qui contiennent A est inchangé (détail de vente, factures, annulation d'une ancienne vente de A fonctionne et restitue le stock).
- [ ] Statistiques : le chiffre d'affaires et le « top produits » incluent toujours A.
- [ ] Archiver B (jamais vendu) : message « n'a aucun historique. Oui = supprimer définitivement / Non = archiver / Annuler » ; tester Annuler (rien), puis Oui (B disparaît pour de bon, même avec « Afficher les archivés »).
- [ ] Sélectionner A archivé (case cochée) → « Réactiver » : A redevient normal et réapparaît dans les choix de vente.
- [ ] « Archiver » sur un produit déjà archivé : message « déjà archivé ».
- [ ] Produit archivé **et** marqué « À vérifier » : il n'est plus compté dans le message de connexion et n'apparaît plus dans le filtre « À vérifier » ; il n'est pas non plus dans la feuille d'inventaire.
- [ ] Statistiques / recommandations : un produit archivé n'apparaît plus dans les alertes de stock, ruptures ou expirations.

## 14. Fournisseurs
- [ ] Archiver F1 (avec commandes) : confirmation d'archivage ; F1 disparaît, ses commandes restent visibles dans l'écran Commandes (filtre par fournisseur inclus).
- [ ] « Afficher les archivés » : F1 grisé « (📦 archivé) ». « Réactiver » le remet.
- [ ] Fiche produit : la liste des fournisseurs ne propose pas F1 archivé. Un produit déjà lié à F1 garde F1 affiché à l'ouverture de sa fiche.
- [ ] Nouvelle commande en tapant le nom de F1 archivé : avertissement « Fournisseur archivé… » et la commande est refusée.
- [ ] F2 (rien) : proposition supprimer / archiver.

## 15. Mutuelles
- [ ] Archiver M1 (avec ventes) : elle disparaît de la liste et du choix « Mutuelle » en vente ; les ventes mutuelle et les paiements mutuelle déjà faits restent visibles.
- [ ] « Afficher les archivés » : « (📦 archivée) » ; « Réactiver » la remet.
- [ ] M2 (rien) : proposition supprimer / archiver.

## 16. Utilisateurs
- [ ] Archiver U1 (Caissier avec ventes) : il disparaît de la liste. Ses ventes et encaissements gardent son nom (écran Caisse, historique).
- [ ] Se déconnecter, essayer de se connecter avec U1 : message « Ce compte a été archivé. Contactez un administrateur. »
- [ ] Réactiver U1 : il peut se reconnecter.
- [ ] Tenter d'archiver **votre propre compte** : refusé (« Vous ne pouvez pas archiver ou supprimer votre propre compte »).
- [ ] S'il n'y a qu'un seul administrateur actif : impossible de l'archiver (« Il doit rester au moins un administrateur actif »).
- [ ] U2 (sans historique) : proposition supprimer / archiver ; « Oui » supprime vraiment.

## 17. Mise à jour d'une base existante
- [ ] Après la mise à jour : **tous** les produits, fournisseurs, mutuelles et utilisateurs existants sont **actifs** (aucun archivé par erreur) et le nombre de ventes, lignes de vente, commandes, paiements est identique à avant (comparer avec la copie de la base d'avant).
- [ ] Une sauvegarde `PharmacieDB_avant-migration_…sqlite` a été créée dans le dossier des sauvegardes.

---

# Modification d'une vente mutuelle

Préparer : deux mutuelles (A « Aaa » 30 %, Z « Zzz » 50 %, A première dans la liste) et une vente **à la mutuelle Z** de 10 boîtes.

## 18. Mutuelle conservée
- [ ] Ouvrir la modification de cette vente : le mode de paiement est « Mutuelle » et **Z est présélectionnée** (pas A, la première de la liste).
- [ ] Changer seulement la quantité (10 → 20) et enregistrer : la vente reste à **Z** ; la part mutuelle = 50 % du nouveau total ; le stock est correct (10 restituées, 20 retirées).
- [ ] Dans l'écran Mutuelles, la dette de Z a augmenté du bon montant ; celle de A n'a **pas** bougé.
- [ ] Choisir volontairement A dans la liste et enregistrer : la vente passe à A (changement voulu).
- [ ] Passer le mode de paiement à « Comptant » : la vente n'a plus de mutuelle ni de part mutuelle.

## 19. Mutuelle archivée
- [ ] Archiver Z (elle a des ventes) puis rouvrir la modification de la vente : la liste contient « Zzz (archivée) », présélectionnée.
- [ ] Une **autre** vente (sans mutuelle) modifiée en « Mutuelle » : Z archivée n'est **pas** proposée.
- [ ] Enregistrer la vente avec Z archivée sans la changer : la vente reste à Z.

## 20. Vente déjà réglée par la mutuelle
- [ ] Dans l'écran Mutuelles, régler les ventes de Z (« Régler »).
- [ ] Tenter de modifier une de ces ventes : message « La mutuelle a déjà réglé cette vente : elle ne peut plus être modifiée. » et l'écran se ferme sans rien changer.
- [ ] L'annulation d'une telle vente (si autorisée par ailleurs) n'est pas concernée par ce changement.
