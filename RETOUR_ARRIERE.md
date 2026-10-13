# Revenir à la version précédente

Pour le développeur. **Velopack ne revient jamais en arrière tout seul** : il n'installe qu'une version PLUS GRANDE que celle qui est installée.
Donc, pour annuler une mauvaise version, on republie l'ancien code sous un numéro plus grand.

## 1. Republier le code de la version précédente sous un numéro plus grand

Exemple : la 1.2.0 pose problème, la bonne version précédente est la 1.1.0. On publie la **1.2.1** qui contient le code de la 1.1.0.

```bash
git checkout -b retour-1.2.1 v1.1.0          # le code de la version précédente
# dans Pharmacie2.csproj : <Version>1.2.1</Version>   (plus grand que 1.2.0)
git commit -am "Retour au code de la 1.1.0 sous le numéro 1.2.1"
git push origin retour-1.2.1
# attendre le CI vert, puis, comme d'habitude :
git tag -a v1.2.1-test -m "retour" && git push origin v1.2.1-test     # essai en canal test
git tag -a v1.2.1 -m "retour"      && git push origin v1.2.1          # puis stable
```

Chez le client, la 1.2.1 est trouvée comme n'importe quelle mise à jour. Si la mauvaise version 1.2.0 n'a pas encore été téléchargée par le client,
publier directement la 1.2.1 suffit : le client ne verra que la dernière.

## 2. La base de données : faut-il la restaurer ?

À chaque démarrage qui contient des migrations en attente, le logiciel fait d'abord une copie **« avant-migration »** de la base
(`%LOCALAPPDATA%\Pharmacie2Data\Sauvegardes\PharmacieDB_avant-migration_<date>.sqlite`, jamais supprimée ; un test vérifie qu'elle est bien faite et qu'elle date d'avant la migration).

| La version qu'on abandonne contient… | Retour arrière |
|---|---|
| **Aucune migration** | Rien à faire : republier l'ancien code (étape 1). La base n'a pas changé. |
| Seulement des migrations qui **ajoutent** une colonne facultative ou une valeur par défaut (ex. `AjoutFicheAVerifier`, `AjoutPrixVenteUnite`, `AjoutAttenduCloture`, `AjoutModePaiementPaiement`) | L'ancien code redémarre sans restauration, mais **ignore** les nouvelles colonnes (ex. il recalculerait un prix à l'unité). À éviter si ces données comptent : préférer corriger en avant (publier une 1.2.1 corrigée). |
| Une migration qui **supprime, renomme ou change le type** d'une colonne ou d'une table, qui **reconstruit une table** (ex. `AjoutArchivage`), ou qui **convertit des données** (ex. `RepriseVentesPayeesEnTotalite` ajoute des paiements) | **Retour arrière impossible sans restauration** : l'ancien code ne comprend plus la base. Restaurer la sauvegarde « avant-migration » (étape 3). |

Règle simple : **si le doute existe, restaurer** la sauvegarde « avant-migration ».

## 3. Restaurer la sauvegarde « avant-migration »

1. Republier d'abord l'ancien code (étape 1) et le laisser s'installer chez le client.
2. Sur le PC concerné : **Administration → « Restaurer une sauvegarde… »**, choisir le fichier
   `PharmacieDB_avant-migration_<date>.sqlite` (dossier des sauvegardes) puis confirmer.
3. Le logiciel met la base actuelle de côté (`…avant-restauration…`), la remplace par la sauvegarde, puis redémarre.
4. **Attention : tout ce qui a été saisi depuis la migration est absent de cette sauvegarde** (ventes, stock…). Si le client a travaillé avec la nouvelle version,
   noter ce qui manque : la base mise de côté (« avant-restauration ») permet de le retrouver.

## 4. Si le client ne peut plus lancer le logiciel

- Il clique sur « Préparer un rapport pour le développeur » (message d'erreur ou Administration) et vous envoie le ZIP.
- L'ancienne version (raccourci « ANCIENNE version — ne plus utiliser ») reste utilisable en secours pendant deux semaines :
  voir [INSTALLATION_CLIENT.md](INSTALLATION_CLIENT.md). Elle ne connaît pas la base de la nouvelle version : ne l'utiliser qu'en dernier recours.
