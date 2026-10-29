Générateur de fichiers .Designer.cs (Python, hors application) : décrire l'écran avec Ecran/N/Col puis appeler ecrire(chemin).

## verifier_canaux.py : les canaux stable et test se masquent-ils ?

Reproduit ce que fait Velopack 1.2.161 (`GithubSource` : une seule page de 10 releases, puis retrait des pré-releases pour le canal stable)
et dit si le canal stable ou le canal test est masqué. `python tools/verifier_canaux.py` interroge l'API GitHub du dépôt (lecture seule, sans jeton) ;
`--autotest` rejoue des scénarios simulés sans réseau (code de sortie 0 = logique conforme). Limite connue : au-delà de 9 pré-releases
« test » publiées après la dernière version stable, le canal stable n'est plus visible (voir PUBLIER.md).
