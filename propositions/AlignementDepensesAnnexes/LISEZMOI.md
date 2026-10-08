# Proposition : AlignementDepensesAnnexes (NON appliquée)

Écart constaté : `AppDbContext.OnModelCreating` déclare `DepenseAnnexe → User` en `OnDelete(SetNull)`,
mais la migration `AjoutDepensesAnnexes` (et donc la base réelle) crée la clé étrangère sans action
(`NO ACTION`). Conséquence : supprimer un utilisateur ayant saisi des dépenses échoue (violation de clé étrangère).

Cette proposition aligne la base sur le modèle (reconstruction SQLite de la table `DepensesAnnexes`).
Elle n'est PAS dans `Migrations/` : elle ne s'exécute donc pas. Le job CI « alignement » la copie
dans `Migrations/` et vérifie que les comptes de lignes sont identiques avant/après.
Pour l'adopter après accord : copier les 3 fichiers `.txt` dans `Migrations/` en retirant `.txt`.
