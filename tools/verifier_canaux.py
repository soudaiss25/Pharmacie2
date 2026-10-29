#!/usr/bin/env python3
"""
Vérifie que les canaux de mise à jour « stable » et « test » ne se masquent pas l'un l'autre.

Reproduit EXACTEMENT ce que fait Velopack 1.2.161 (GithubSource + GitBase.GetReleaseFeed) :
  1. GET /repos/{depot}/releases?per_page=10&page=1          (UNE seule page de 10 releases, sans jeton)
  2. tri par date de publication décroissante, puis, pour le canal stable (prerelease=false), on retire les pré-releases
     (le canal test, prerelease=true, garde tout)
  3. pour chaque release restante : lecture de l'asset « releases.{canal}.json » ; une release qui ne l'a pas est ignorée
  => un canal « voit » ses mises à jour si au moins une release gardée contient son index.

Conséquence à surveiller : les 10 dernières releases, TOUS types confondus, sont les seules examinées. Si 10 pré-releases
« test » ou plus sont publiées après la dernière version stable, le canal stable ne voit plus rien (il est masqué).

Usage :
  python tools/verifier_canaux.py                      # interroge l'API GitHub du dépôt (lecture seule, sans jeton)
  python tools/verifier_canaux.py --depot owner/repo   # autre dépôt
  python tools/verifier_canaux.py --fichier reponse.json   # réponse de l'API enregistrée (hors ligne)
  python tools/verifier_canaux.py --autotest           # scénarios simulés : vérifie la logique et documente la limite des 10 releases
Code de sortie : 0 = canaux sains, 1 = un canal est masqué (ou un scénario inattendu), 2 = erreur d'accès.
"""
import argparse
import json
import sys
import urllib.request
from datetime import datetime, timedelta

DEPOT_PAR_DEFAUT = "soudaiss25/Pharmacie2"
PAR_PAGE = 10  # GithubSource.GetReleases : const int perPage = 10; const int page = 1;


def releases_visibles(releases, canal):
    """Releases que Velopack examinerait pour ce canal, dans l'ordre (la plus récente d'abord)."""
    premiere_page = sorted(releases, key=lambda r: r["published_at"], reverse=True)[:PAR_PAGE]
    # NB : l'API renvoie déjà au plus 10 releases ; le tri/filtre est fait ensuite par le client
    gardees = [r for r in premiere_page if canal == "test" or not r["prerelease"]]
    index = f"releases.{canal}.json"
    return [r for r in gardees if any(a["name"].lower() == index for a in r.get("assets", []))]


def charger_api(depot):
    url = f"https://api.github.com/repos/{depot}/releases?per_page={PAR_PAGE}&page=1"
    requete = urllib.request.Request(url, headers={"Accept": "application/vnd.github.v3+json", "User-Agent": "verifier-canaux"})
    with urllib.request.urlopen(requete, timeout=30) as reponse:
        return json.load(reponse)


def charger_toutes(depot):
    """Toutes les releases (jusqu'à 100) : sert seulement à savoir ce qui EXISTE, pour détecter un canal masqué."""
    url = f"https://api.github.com/repos/{depot}/releases?per_page=100&page=1"
    requete = urllib.request.Request(url, headers={"Accept": "application/vnd.github.v3+json", "User-Agent": "verifier-canaux"})
    with urllib.request.urlopen(requete, timeout=30) as reponse:
        return json.load(reponse)


def existe(releases, canal):
    index = f"releases.{canal}.json"
    return [r for r in releases if (canal == "test" or not r["prerelease"]) and any(a["name"].lower() == index for a in r.get("assets", []))]


def diagnostiquer(toutes):
    """Retourne (liste de problèmes, lignes de rapport)."""
    problemes, lignes = [], []
    triees = sorted(toutes, key=lambda r: r["published_at"], reverse=True)
    for canal in ("stable", "test"):
        reelles = existe(triees, canal)
        vues = releases_visibles(triees, canal)
        nom = lambda r: r["tag_name"]
        lignes.append(f"Canal {canal} : {len(reelles)} release(s) publiée(s), {len(vues)} visible(s) par Velopack"
                      + (f" (la plus récente : {nom(vues[0])})" if vues else ""))
        if reelles and not vues:
            problemes.append(f"Le canal {canal} est MASQUÉ : {len(reelles)} release(s) existent mais aucune n'est dans les {PAR_PAGE} dernières releases examinées.")
        elif reelles and vues and nom(vues[0]) != nom(reelles[0]):
            problemes.append(f"Le canal {canal} ne voit pas sa release la plus récente ({nom(reelles[0])}).")
    # marge avant masquage du canal stable
    consecutives = 0
    for r in triees:
        if r["prerelease"]:
            consecutives += 1
        else:
            break
    stables = [r for r in triees if not r["prerelease"]]
    if stables:
        lignes.append(f"Pré-releases publiées depuis la dernière stable ({stables[0]['tag_name']}) : {consecutives} ; "
                      f"le canal stable est masqué à partir de {PAR_PAGE} → il en reste {max(0, PAR_PAGE - 1 - consecutives)} avant le risque.")
    return problemes, lignes


# ───────── scénarios simulés ─────────

def _fabrique(sequence):
    """sequence : liste de 'stable' / 'test' du plus ancien au plus récent."""
    debut = datetime(2026, 1, 1)
    sortie = []
    for i, genre in enumerate(sequence):
        canal = "stable" if genre == "stable" else "test"
        sortie.append({
            "tag_name": f"v{i}-{genre}",
            "published_at": (debut + timedelta(days=i)).isoformat(),
            "prerelease": genre == "test",
            "assets": [{"name": f"releases.{canal}.json"}, {"name": f"LIGUAPHARME-{canal}-Setup.exe"}],
        })
    return sortie


def autotest():
    cas = [
        ("une version test publiée APRÈS une stable ne masque pas la stable (canal stable)", ["stable", "test"], "stable", True),
        ("plusieurs versions test après une stable ne masquent pas la stable (9 tests)", ["stable"] + ["test"] * 9, "stable", True),
        ("LIMITE : 10 versions test après une stable masquent la stable", ["stable"] + ["test"] * 10, "stable", False),
        ("une version stable publiée APRÈS un test ne masque pas le test (canal test)", ["test", "stable"], "test", True),
        ("le canal test voit toujours sa dernière release au milieu de stables", ["stable", "test", "stable", "stable"], "test", True),
        ("canal stable : un test plus récent n'est jamais proposé", ["stable", "test"], "stable", True),
    ]
    echecs = 0
    for libelle, sequence, canal, attendu in cas:
        releases = _fabrique(sequence)
        vues = releases_visibles(releases, canal)
        existantes = existe(releases, canal)
        ok = bool(vues) and vues[0]["tag_name"] == existantes[0]["tag_name"]
        statut = "OK " if ok == attendu else "ÉCHEC"
        if ok != attendu:
            echecs += 1
        print(f"[{statut}] {libelle} → canal {canal} {'voit' if ok else 'ne voit PAS'} sa dernière release")
    # le canal stable ne propose jamais une pré-release
    mixte = _fabrique(["stable", "test"])
    assert all(not r["prerelease"] for r in releases_visibles(mixte, "stable")), "le canal stable ne doit jamais proposer une pré-release"
    return echecs


def main():
    if hasattr(sys.stdout, "reconfigure"):
        sys.stdout.reconfigure(encoding="utf-8")   # console Windows (cp1252) : les accents et les flèches s'affichent
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--depot", default=DEPOT_PAR_DEFAUT)
    ap.add_argument("--fichier", help="réponse JSON de l'API GitHub enregistrée (hors ligne)")
    ap.add_argument("--autotest", action="store_true", help="scénarios simulés, sans réseau")
    args = ap.parse_args()

    if args.autotest:
        sys.exit(1 if autotest() else 0)

    try:
        toutes = json.load(open(args.fichier, encoding="utf-8")) if args.fichier else charger_toutes(args.depot)
    except Exception as ex:  # réseau, dépôt privé, limite d'appels…
        print(f"Impossible de lire les releases : {ex}")
        sys.exit(2)

    problemes, lignes = diagnostiquer(toutes)
    print(f"Dépôt {args.depot} : {len(toutes)} release(s)")
    for l in lignes:
        print("  " + l)
    for p in problemes:
        print("  PROBLÈME : " + p)
    print("Canaux sains." if not problemes else "Canal masqué : voir ci-dessus.")
    sys.exit(1 if problemes else 0)


if __name__ == "__main__":
    main()
