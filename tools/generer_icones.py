"""
Génère les icônes monochromes (blanc sur transparent, 48 x 48) du menu dans resources/icones.
Usage : python tools/generer_icones.py   (nécessite Pillow). Hors application : les PNG générés sont inclus en ressources.
"""
import os
from PIL import Image, ImageDraw

SORTIE = os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', 'resources', 'icones')
os.makedirs(SORTIE, exist_ok=True)

T = 48          # taille finale
K = 8           # sur-échantillonnage pour des bords lisses
S = T * K
W = (255, 255, 255, 255)
E = 4.0 * K     # épaisseur du trait


def nouvelle():
    im = Image.new('RGBA', (S, S), (0, 0, 0, 0))
    return im, ImageDraw.Draw(im)


def p(*c):
    return [v * K for v in c]


def enregistrer(im, nom):
    im.resize((T, T), Image.LANCZOS).save(os.path.join(SORTIE, nom + '.png'))


def ligne(d, *c, e=E):
    d.line(p(*c), fill=W, width=int(e), joint='curve')
    for i in range(0, len(c), 2):
        x, y = c[i] * K, c[i + 1] * K
        d.ellipse([x - e / 2, y - e / 2, x + e / 2, y + e / 2], fill=W)


def rect(d, x0, y0, x1, y1, e=E, plein=False):
    if plein:
        d.rectangle(p(x0, y0, x1, y1), fill=W)
    else:
        d.rectangle(p(x0, y0, x1, y1), outline=W, width=int(e))


def cercle(d, cx, cy, r, e=E, plein=False):
    d.ellipse(p(cx - r, cy - r, cx + r, cy + r), fill=W if plein else None, outline=W, width=int(e))


# accueil / ma journée : maison
im, d = nouvelle()
ligne(d, 6, 24, 24, 8, 42, 24)
ligne(d, 11, 22, 11, 41, 37, 41, 37, 22)
rect(d, 20, 29, 28, 41, plein=True)
enregistrer(im, 'accueil')

# vendre : panier
im, d = nouvelle()
ligne(d, 4, 8, 11, 8, 15, 30, 37, 30, 41, 14, 12, 14)
cercle(d, 19, 38, 3.2, plein=True)
cercle(d, 33, 38, 3.2, plein=True)
enregistrer(im, 'vendre')

# caisse : billet
im, d = nouvelle()
rect(d, 5, 12, 43, 36)
cercle(d, 24, 24, 5.5)
cercle(d, 11, 24, 1.6, plein=True)
cercle(d, 37, 24, 1.6, plein=True)
enregistrer(im, 'caisse')

# stock : boîte
im, d = nouvelle()
ligne(d, 24, 5, 42, 14, 42, 34, 24, 43, 6, 34, 6, 14, 24, 5)
ligne(d, 6, 14, 24, 23, 42, 14)
ligne(d, 24, 23, 24, 43)
enregistrer(im, 'stock')

# crédits clients : personne
im, d = nouvelle()
cercle(d, 24, 15, 7)
ligne(d, 9, 42, 9, 36, 14, 29, 34, 29, 39, 36, 39, 42)
enregistrer(im, 'credits')

# mutuelles : bouclier
im, d = nouvelle()
ligne(d, 24, 5, 40, 11, 40, 24, 36, 34, 24, 43, 12, 34, 8, 24, 8, 11, 24, 5)
ligne(d, 16, 24, 22, 30, 32, 18)
enregistrer(im, 'mutuelles')

# commandes : camion
im, d = nouvelle()
rect(d, 4, 13, 28, 33)
ligne(d, 28, 20, 38, 20, 44, 27, 44, 33, 28, 33)
cercle(d, 13, 36, 4.2, plein=True)
cercle(d, 35, 36, 4.2, plein=True)
enregistrer(im, 'commandes')

# fournisseurs : immeuble
im, d = nouvelle()
rect(d, 9, 6, 31, 42)
ligne(d, 31, 18, 40, 18, 40, 42)
for x in (14, 22):
    for y in (12, 21, 30):
        rect(d, x, y, x + 4, y + 4, plein=True)
ligne(d, 4, 42, 44, 42)
enregistrer(im, 'fournisseurs')

# catalogue : livre
im, d = nouvelle()
ligne(d, 24, 12, 24, 40)
ligne(d, 24, 12, 8, 9, 8, 37, 24, 40)
ligne(d, 24, 12, 40, 9, 40, 37, 24, 40)
enregistrer(im, 'catalogue')

# dépenses : flèche vers le bas dans un cercle
im, d = nouvelle()
cercle(d, 24, 24, 18)
ligne(d, 24, 14, 24, 33)
ligne(d, 16, 26, 24, 34, 32, 26)
enregistrer(im, 'depenses')

# statistiques : histogramme
im, d = nouvelle()
ligne(d, 6, 6, 6, 42, 42, 42)
rect(d, 13, 28, 19, 40, plein=True)
rect(d, 23, 18, 29, 40, plein=True)
rect(d, 33, 10, 39, 40, plein=True)
enregistrer(im, 'statistiques')

# administration : engrenage
import math
im, d = nouvelle()
cx = cy = 24
pts = []
for i in range(16):
    a0 = 2 * math.pi * i / 16
    r = 19 if i % 2 == 0 else 14.5
    pts.append((cx + r * math.cos(a0), cy + r * math.sin(a0)))
d.polygon([(x * K, y * K) for x, y in pts], outline=W, fill=None)
d.line([(x * K, y * K) for x, y in pts + [pts[0]]], fill=W, width=int(E * 0.8), joint='curve')
cercle(d, 24, 24, 6)
enregistrer(im, 'administration')

# déconnexion : porte et flèche
im, d = nouvelle()
ligne(d, 20, 6, 8, 6, 8, 42, 20, 42)
ligne(d, 18, 24, 42, 24)
ligne(d, 33, 15, 42, 24, 33, 33)
enregistrer(im, 'deconnexion')

# verrouiller : cadenas
im, d = nouvelle()
rect(d, 11, 21, 37, 42)
ligne(d, 16, 21, 16, 14, 24, 6, 32, 14, 32, 21)
cercle(d, 24, 31, 2.6, plein=True)
enregistrer(im, 'verrouiller')

# menu : trois barres
im, d = nouvelle()
ligne(d, 8, 12, 40, 12)
ligne(d, 8, 24, 40, 24)
ligne(d, 8, 36, 40, 36)
enregistrer(im, 'menu')
print('icônes générées dans', os.path.abspath(SORTIE))
