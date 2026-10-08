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
