"""
Générateur de fichiers *.Designer.cs compatibles avec le concepteur Visual Studio
(aucune lambda, aucune boucle, aucun var dans InitializeComponent).

Usage :
    from gen import *
    ecran = Ecran('UserControl', 'Pharmacie2.views.UserControls', 'Uc_Truc', kids=[...], props={...}, events={...})
    ecran.ecrire('C:/.../Uc_Truc.Designer.cs')
"""
import re

WF = 'System.Windows.Forms.'
DR = 'System.Drawing.'


class R:
    """Expression C# brute."""
    def __init__(self, code):
        self.code = code


def S(txt):
    return R('"' + txt.replace('\\', '\\\\').replace('"', '\\"').replace('\n', '\\n').replace('\r', '') + '"')


def font(pt, style=None):
    st = f'System.Drawing.FontStyle.{style}' if style else 'System.Drawing.FontStyle.Regular'
    return R(f'new System.Drawing.Font("Segoe UI", {pt}F, {st}, System.Drawing.GraphicsUnit.Point, ((byte)(0)))')


def pad(*v):
    if len(v) == 1:
        v = (v[0],) * 4
    return R('new System.Windows.Forms.Padding(' + ', '.join(str(x) for x in v) + ')')


def size(w, h):
    return R(f'new System.Drawing.Size({w}, {h})')


def E(typ, val):
    return R(f'System.Windows.Forms.{typ}.{val}')


def anchor(*parts):
    return R(' | '.join(f'System.Windows.Forms.AnchorStyles.{p}' for p in parts))


# Types de délégués par événement
HANDLERS = {
    'Click': 'EventHandler', 'DoubleClick': 'EventHandler', 'SelectedIndexChanged': 'EventHandler',
    'TextChanged': 'EventHandler', 'CheckedChanged': 'EventHandler', 'ValueChanged': 'EventHandler',
    'SelectionChanged': 'EventHandler', 'Load': 'EventHandler', 'Enter': 'EventHandler', 'Leave': 'EventHandler',
    'Resize': 'EventHandler', 'Shown': 'EventHandler', 'VisibleChanged': 'EventHandler', 'SelectedValueChanged': 'EventHandler',
    'KeyDown': 'KeyEventHandler', 'KeyUp': 'KeyEventHandler', 'KeyPress': 'KeyPressEventHandler',
    'CellContentClick': 'DataGridViewCellEventHandler', 'CellClick': 'DataGridViewCellEventHandler',
    'CellDoubleClick': 'DataGridViewCellEventHandler', 'CellValueChanged': 'DataGridViewCellEventHandler',
    'CellFormatting': 'DataGridViewCellFormattingEventHandler',
    'CellEndEdit': 'DataGridViewCellEventHandler',
    'CellValidating': 'DataGridViewCellValidatingEventHandler', 'DataError': 'DataGridViewDataErrorEventHandler',
    'FormClosing': 'FormClosingEventHandler', 'FormClosed': 'FormClosedEventHandler',
    'LinkClicked': 'LinkLabelLinkClickedEventHandler', 'Paint': 'PaintEventHandler',
}


class Node:
    def __init__(self, typ, name, props=None, events=None, kids=None, rows=None, cols=None, columns=None):
        self.typ = typ
        self.name = name
        self.props = props or {}
        self.events = events or {}
        self.kids = kids or []        # liste de Node ou (Node, (col,row[,cs,rs]))
        self.rows = rows
        self.cols = cols
        self.columns = columns or []  # colonnes de DataGridView : Col(...)


def N(typ, name, kids=None, rows=None, cols=None, columns=None, events=None, **props):
    return Node(typ, name, props, events, kids, rows, cols, columns)


class Col:
    def __init__(self, name, header, prop=None, weight=100, minw=60, kind='TextBox', montant=False, readonly=True, visible=True, field=None):
        self.name, self.header, self.prop = name, header, prop or name
        self.weight, self.minw, self.kind, self.montant, self.readonly, self.visible = weight, minw, kind, montant, readonly, visible
        self.field = field or ('col' + name)


def fmt(v):
    if isinstance(v, R):
        return v.code
    if isinstance(v, bool):
        return 'true' if v else 'false'
    if isinstance(v, int):
        return str(v)
    if isinstance(v, float):
        return f'{v}F'
    if isinstance(v, str):
        return S(v).code
    raise ValueError(v)


ENUMS = {
    'Dock': 'DockStyle', 'FlowDirection': 'FlowDirection', 'BorderStyle': 'BorderStyle',
    'AutoSizeMode': 'AutoSizeMode', 'FlatStyle': 'FlatStyle', 'DropDownStyle': 'ComboBoxStyle',
    'SizeMode': 'PictureBoxSizeMode', 'ScrollBars': 'ScrollBars', 'SelectionMode': 'DataGridViewSelectionMode',
    'AutoSizeColumnsMode': 'DataGridViewAutoSizeColumnsMode', 'HorizontalScrollbar': None,
    'FormBorderStyle': 'FormBorderStyle', 'StartPosition': 'FormStartPosition', 'WindowState': 'FormWindowState',
    'AutoScaleMode': 'AutoScaleMode', 'ReadOnly': None, 'SelectionBackColor': None,
}
ALIGN_PROPS = {'TextAlign': 'ContentAlignment', 'ImageAlign': 'ContentAlignment', 'CheckAlign': 'ContentAlignment'}


def prop_value(node, key, v):
    if isinstance(v, R):
        return v.code
    if key == 'Dock':
        return f'{WF}DockStyle.{v}'
    if key == 'Anchor':
        return ' | '.join(f'{WF}AnchorStyles.{p.strip()}' for p in v.split(','))
    if key == 'TextAlign':
        if node.typ in ('TextBox', 'NumericUpDown'):
            return f'{WF}HorizontalAlignment.{v}'
        return f'System.Drawing.ContentAlignment.{v}'
    if key in ENUMS and ENUMS[key] and isinstance(v, str):
        return f'{WF}{ENUMS[key]}.{v}'
    return fmt(v)


def tn(typ):
    """Nom complet du type : les types contenant un point sont déjà qualifiés (composants personnalisés)."""
    return typ if '.' in typ else WF + typ


class Ecran:
    def __init__(self, kind, ns, name, kids, props=None, events=None, rows=None, cols=None, usings=None):
        self.kind, self.ns, self.name = kind, ns, name
        self.root = Node(kind, 'this', props or {}, events or {}, kids, rows, cols)

    # ── collecte ──
    def _walk(self, node, out):
        for k in node.kids:
            child = k[0] if isinstance(k, tuple) else k
            out.append(child)
            self._walk(child, out)

    def generer(self):
        controls = []
        self._walk(self.root, controls)
        L = []
        w = L.append

        w('namespace ' + self.ns)
        w('{')
        w(f'    partial class {self.name}')
        w('    {')
        w('        /// <summary>')
        w('        /// Variable nécessaire au concepteur.')
        w('        /// </summary>')
        w('        private System.ComponentModel.IContainer components = null;')
        w('')
        w('        /// <summary>')
        w('        /// Nettoyage des ressources utilisées.')
        w('        /// </summary>')
        w('        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>')
        w('        protected override void Dispose(bool disposing)')
        w('        {')
        w('            if (disposing && (components != null))')
        w('            {')
        w('                components.Dispose();')
        w('            }')
        w('            base.Dispose(disposing);')
        w('        }')
        w('')
        w('        #region Code généré par le Concepteur Windows Form')
        w('')
        w('        /// <summary>')
        w('        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas')
        w('        /// le contenu de cette méthode avec l\'éditeur de code.')
        w('        /// </summary>')
        w('        private void InitializeComponent()')
        w('        {')

        # colonnes de grilles
        cols = []
        for c in controls:
            for col in c.columns:
                cols.append((c, col))

        for c in controls:
            w(f'            this.{c.name} = new {tn(c.typ)}();')
        for c, col in cols:
            w(f'            this.{col.field} = new {WF}DataGridView{col.kind}Column();')

        # BeginInit
        inits = [c for c in controls if c.typ in ('NumericUpDown', 'DataGridView', 'PictureBox')]
        for c in inits:
            w(f'            (({"System.ComponentModel.ISupportInitialize"})(this.{c.name})).BeginInit();')
        # SuspendLayout
        containers = [c for c in controls if c.typ in ('TableLayoutPanel', 'FlowLayoutPanel', 'Panel', 'GroupBox', 'SplitContainer', 'TabControl', 'TabPage')]
        for c in containers:
            w(f'            this.{c.name}.SuspendLayout();')
        w('            this.SuspendLayout();')

        # propriétés
        for c in controls:
            self._emit(L, c)
        for c, col in cols:
            w(f'            // ')
            w(f'            // {col.field}')
            w(f'            // ')
            w(f'            this.{col.field}.HeaderText = {S(col.header).code};')
            w(f'            this.{col.field}.Name = "{col.name}";')
            if col.kind != 'Button':
                w(f'            this.{col.field}.DataPropertyName = "{col.prop}";')
            w(f'            this.{col.field}.FillWeight = {col.weight}F;')
            w(f'            this.{col.field}.MinimumWidth = {col.minw};')
            if col.kind != 'Button':
                w(f'            this.{col.field}.ReadOnly = {"true" if col.readonly else "false"};')
            if not col.visible:
                w(f'            this.{col.field}.Visible = false;')
            if col.montant:
                w(f'            this.{col.field}.Tag = "montant";')

        # racine
        w('            // ')
        w(f'            // {self.name}')
        w('            // ')
        self._emit_props(L, self.root, 'this', root=True)

        w('            this.ResumeLayout(false);')
        if self.kind == 'Form':
            w('            this.PerformLayout();')
        for c in reversed(containers):
            w(f'            this.{c.name}.ResumeLayout(false);')
            if c.typ in ('TableLayoutPanel', 'FlowLayoutPanel'):
                w(f'            this.{c.name}.PerformLayout();')
        for c in reversed(inits):
            w(f'            (({"System.ComponentModel.ISupportInitialize"})(this.{c.name})).EndInit();')
        w('')
        w('        }')
        w('')
        w('        #endregion')
        w('')
        for c in controls:
            w(f'        private {tn(c.typ)} {c.name};')
        for c, col in cols:
            w(f'        private {WF}DataGridView{col.kind}Column {col.field};')
        w('    }')
        w('}')
        return '\r\n'.join(L) + '\r\n'

    def _emit(self, L, c):
        w = L.append
        w('            // ')
        w(f'            // {c.name}')
        w('            // ')
        self._emit_props(L, c, 'this.' + c.name)

    def _emit_props(self, L, c, ref, root=False):
        w = L.append
        # enfants
        if c.typ == 'TableLayoutPanel' or (root and c.rows):
            self._emit_table(L, c, ref)
        for ordre, k in enumerate(c.kids):
            child, cell = (k if isinstance(k, tuple) else (k, None))
            if c.typ == 'TableLayoutPanel' or (root and c.rows):
                if cell is None:
                    raise ValueError(f'{child.name} : cellule manquante dans {c.name}')
                col_, row_ = cell[0], cell[1]
                w(f'            {ref}.Controls.Add(this.{child.name}, {col_}, {row_});')
                if len(cell) > 2:
                    if cell[2] > 1:
                        w(f'            {ref}.SetColumnSpan(this.{child.name}, {cell[2]});')
                    if len(cell) > 3 and cell[3] > 1:
                        w(f'            {ref}.SetRowSpan(this.{child.name}, {cell[3]});')
            else:
                w(f'            {ref}.Controls.Add(this.{child.name});')
            if 'TabIndex' not in child.props:
                w(f'            this.{child.name}.TabIndex = {ordre};')   # ordre de tabulation = ordre de déclaration
        if c.columns:
            w(f'            {ref}.Columns.AddRange(new {WF}DataGridViewColumn[] {{')
            for i, col in enumerate(c.columns):
                w(f'            this.{col.field}' + (',' if i < len(c.columns) - 1 else '});'))
        # propriétés simples
        for key, v in c.props.items():
            if key == 'Items':
                w(f'            {ref}.Items.AddRange(new object[] {{')
                for i, x in enumerate(v):
                    w('            ' + fmt(x) + (',' if i < len(v) - 1 else '});'))
                continue
            if root and key in ('AutoScaleDimensions', 'AutoScaleMode'):
                pass
            w(f'            {ref}.{key} = {prop_value(c, key, v)};')
        w(f'            {ref}.Name = "{self.name if root else c.name}";')
        for ev, handler in c.events.items():
            h = HANDLERS[ev]
            hn = 'System.EventHandler' if h == 'EventHandler' else WF + h
            w(f'            {ref}.{ev} += new {hn}(this.{handler});')

    def _emit_table(self, L, c, ref):
        w = L.append
        cols = c.cols or ['P100']
        rows = c.rows or ['P100']
        w(f'            {ref}.ColumnCount = {len(cols)};')
        for s in cols:
            w(f'            {ref}.ColumnStyles.Add({self._style("Column", s)});')
        w(f'            {ref}.RowCount = {len(rows)};')
        for s in rows:
            w(f'            {ref}.RowStyles.Add({self._style("Row", s)});')

    @staticmethod
    def _style(kind, s):
        t = f'{WF}{kind}Style'
        if s == 'A':
            return f'new {t}({WF}SizeType.AutoSize)'
        if s[0] == 'P':
            return f'new {t}({WF}SizeType.Percent, {float(s[1:])}F)'
        if s[0] == 'F':
            return f'new {t}({WF}SizeType.Absolute, {int(s[1:])}F)'
        raise ValueError(s)

    def ecrire(self, chemin):
        txt = self.generer()
        with open(chemin, 'w', encoding='utf-8-sig', newline='') as f:
            f.write(txt)
        return chemin


# ── Propriétés communes des formulaires / contrôles utilisateur (règle B0.1) ──
def base_ecran(form=True, **extra):
    p = {
        'AutoScaleDimensions': R('new System.Drawing.SizeF(7F, 15F)'),
        'AutoScaleMode': E('AutoScaleMode', 'Font'),
        'Font': font(10),
    }
    p.update(extra)
    return p
