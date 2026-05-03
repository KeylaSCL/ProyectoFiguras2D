// ============================================================
//  MainForm.cs  —  VISTA PRINCIPAL (V en MVC)
//
//  Esta clase se encarga exclusivamente de la interfaz gráfica.
//  Delega toda la lógica de creación y gestión de figuras al 
//  FigureController.
// ============================================================
using ProyectoFiguras2D.Controllers;
using ProyectoFiguras2D.Models;
using ProyectoFiguras2D.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ProyectoFiguras2D.Views
{
    public class MainForm : Form
    {
        // ── Componentes de la Vista ─────────────────────────────
        private DrawingCanvas canvas;
        private ParamsPanel paramsPanel;
        private MenuStrip mainMenu;

        // ── Referencia al Controlador ───────────────────────────
        private FigureController _controller;

        // ── Datos estáticos para la interfaz ────────────────────
        private static readonly Color[] Palette =
        {
            Color.FromArgb( 76, 175,  80), Color.FromArgb(244,  67,  54),
            Color.FromArgb(255, 152,   0), Color.FromArgb(255, 215,   0),
            Color.FromArgb(103,  58, 183), Color.FromArgb( 63, 116, 181),
            Color.FromArgb(233,  30,  99), Color.FromArgb(  0, 188, 212),
            Color.FromArgb( 33, 150, 243), Color.FromArgb(156,  39, 176),
            Color.FromArgb(255, 193,   7), Color.FromArgb(  0, 150, 136),
            Color.FromArgb(205,  92,  92), Color.FromArgb( 96, 125, 139),
            Color.FromArgb(100, 181, 246), Color.FromArgb(174, 213, 129),
            Color.FromArgb(255, 138, 101), Color.FromArgb(206, 147, 216),
            Color.FromArgb( 77, 182, 172), Color.FromArgb(189, 189, 189)
        };

        private static readonly string[] Descriptions =
        {
            "Cuadrado: 4 lados iguales. Ingresa el Lado.",
            "Círculo: curva cerrada equidistante del centro. Ingresa el Radio.",
            "Triángulo isósceles. Ingresa Base y Altura.",
            "Rectángulo: 4 ángulos rectos. Ingresa Ancho y Alto.",
            "Elipse: dos ejes de simetría. Ingresa semiejes a y b.",
            "Estrella de 5 puntas. Ingresa Radio exterior e interior.",
            "Pentágono regular: 5 lados iguales. Ingresa el Radio.",
            "Paralelogramo: lados paralelos. Ingresa Base, Altura y Lado oblicuo.",
            "Triángulo escaleno: 3 lados distintos. Ingresa los 3 Lados.",
            "Trapecio: un par de lados paralelos. Ingresa Base mayor, Base menor y Altura.",
            "Trébol: 3 círculos. Ingresa el Radio de cada círculo.",
            "Hexágono regular: 6 lados iguales. Ingresa el Radio.",
            "Creciente (luna): diferencia de círculos. Ingresa Radio ext. e int.",
            "Cruz: polígono de 12 vértices. Ingresa Tamaño total y Grosor de brazo.",
            "Semicírculo: mitad de un círculo. Ingresa el Radio.",
            "Cometa (Kite): 2 diagonales. Ingresa Diagonal horizontal y vertical.",
            "Octágono regular: 8 lados iguales. Ingresa el Radio.",
            "Sector circular: porción de círculo. Ingresa Radio y Ángulo (°).",
            "Corazón: curvas de Bézier. Ingresa el Tamaño.",
            "Rombo: diagonales perpendiculares. Ingresa Diagonal mayor y menor."
        };

        // ════════════════════════════════════════════════════
        //  CONSTRUCTOR
        // ════════════════════════════════════════════════════

        public MainForm()
        {
            // Configuración estética del formulario
            Text = "Figuras Geométricas";
            Size = new Size(1150, 730);
            MinimumSize = new Size(860, 560);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(240, 242, 248);
            Font = new Font("Segoe UI", 9f);

            // Inicialización de la interfaz
            BuildMenu();
            BuildParamsPanel();
            BuildCanvas();

            // Inyección de dependencias: El controlador toma el control de los componentes
            _controller = new FigureController(canvas, paramsPanel, Palette);

        }

        // ════════════════════════════════════════════════════
        //  CONSTRUCCIÓN DE LA UI (VIEW)
        // ════════════════════════════════════════════════════

        private void BuildMenu()
        {
            mainMenu = new MenuStrip
            {
                BackColor = Color.FromArgb(45, 50, 80),
                Renderer = new ToolStripProfessionalRenderer(new DarkTheme())
            };

            var mnuFig = new ToolStripMenuItem("▦  Figuras") { ForeColor = Color.White };

            // CATEGORÍA 1: Polígonos Regulares
            var mnuPoligonos = new ToolStripMenuItem("⬠  Polígonos Regulares");
            AgregarItemA(mnuPoligonos, "Cuadrado", 0);
            AgregarItemA(mnuPoligonos, "Pentágono", 6);
            AgregarItemA(mnuPoligonos, "Hexágono", 11);
            AgregarItemA(mnuPoligonos, "Octágono", 16);
            mnuFig.DropDownItems.Add(mnuPoligonos);

            // CATEGORÍA 2: Curvas y Círculos
            var mnuCurvas = new ToolStripMenuItem("◯  Curvas y Círculos");
            AgregarItemA(mnuCurvas, "Círculo", 1);
            AgregarItemA(mnuCurvas, "Elipse", 4);
            AgregarItemA(mnuCurvas, "Semicírculo", 14);
            AgregarItemA(mnuCurvas, "Luna (Creciente)", 12);
            AgregarItemA(mnuCurvas, "Sector Circular (Pie)", 17);
            mnuFig.DropDownItems.Add(mnuCurvas);

            // CATEGORÍA 3: Triángulos y Cuadriláteros
            var mnuTriangulos = new ToolStripMenuItem("⊿  Triángulos y Cuadriláteros");
            AgregarItemA(mnuTriangulos, "Triángulo Isósceles", 2);
            AgregarItemA(mnuTriangulos, "Triángulo Escaleno", 8);
            AgregarItemA(mnuTriangulos, "Rectángulo", 3);
            AgregarItemA(mnuTriangulos, "Paralelogramo", 7);
            AgregarItemA(mnuTriangulos, "Trapecio", 9);
            AgregarItemA(mnuTriangulos, "Rombo", 19);
            AgregarItemA(mnuTriangulos, "Cometa (Kite)", 15);
            mnuFig.DropDownItems.Add(mnuTriangulos);

            // CATEGORÍA 4: Figuras Especiales
            var mnuEspeciales = new ToolStripMenuItem("★  Figuras Especiales");
            AgregarItemA(mnuEspeciales, "Estrella", 5);
            AgregarItemA(mnuEspeciales, "Corazón", 18);
            AgregarItemA(mnuEspeciales, "Cruz", 13);
            AgregarItemA(mnuEspeciales, "Trébol", 10);
            mnuFig.DropDownItems.Add(mnuEspeciales);

            mainMenu.Items.Add(mnuFig);
            Controls.Add(mainMenu);
            MainMenuStrip = mainMenu;
        }

        private void AgregarItemA(ToolStripMenuItem menuPadre, string texto, int indice)
        {
            var it = new ToolStripMenuItem($"{indice + 1:D2}. {texto}");
            // Delegamos la acción de clic al controlador
            it.Click += (s, e) => _controller.SeleccionarFigura(indice, Descriptions[indice]);
            menuPadre.DropDownItems.Add(it);
        }

        private void BuildParamsPanel()
        {
            paramsPanel = new ParamsPanel();
            paramsPanel.Visible = false;
            // El panel de parámetros le avisa a la vista que debe redibujarse cuando cambian los datos
            paramsPanel.ParamsChanged += (s, e) => canvas.Invalidate();
            Controls.Add(paramsPanel);
        }

        private void BuildCanvas()
        {
            canvas = new DrawingCanvas
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 249, 252)
            };

            // Si el lienzo cambia de tamaño, el controlador debe refrescar la figura actual
            canvas.Resize += (s, e) =>
            {
                if (canvas.CurrentShape != null)
                {
                    // Forzamos un redibujado de la galería o figura según corresponda
                    _controller.RefrescarLienzo();
                }
            };
            Controls.Add(canvas);
        }
    }

    // ════════════════════════════════════════════════════════
    //  DrawingCanvas — Componente de dibujo (Soporte de la Vista)
    // ════════════════════════════════════════════════════════
    public class DrawingCanvas : Panel
    {
        private List<Shape> _shapes = new List<Shape>();

        public Shape CurrentShape => (_shapes.Count == 1) ? _shapes[0] : null;

        public DrawingCanvas()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void LoadAll(List<Func<PointF, float, Shape>> factories, float scale)
        {
            _shapes.Clear();
            const int COLS = 5, ROWS = 4;
            float cw = ClientSize.Width / (float)COLS;
            float ch = ClientSize.Height / (float)ROWS;

            for (int i = 0; i < factories.Count; i++)
            {
                float cx = cw * (i % COLS) + cw / 2f;
                float cy = ch * (i / COLS) + ch / 2f - 8f;
                _shapes.Add(factories[i](new PointF(cx, cy), scale * 0.80f));
            }
            Invalidate();
        }

        public void LoadSingle(Shape shape)
        {
            _shapes.Clear();
            _shapes.Add(shape);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Dibujar fondo de cuadrícula
            g.Clear(BackColor);
            using (Pen dot = new Pen(Color.FromArgb(20, 80, 100, 180), 1f))
            {
                dot.DashStyle = DashStyle.Dot;
                for (int x = 0; x <= ClientSize.Width; x += 40) g.DrawLine(dot, x, 0, x, ClientSize.Height);
                for (int y = 0; y <= ClientSize.Height; y += 40) g.DrawLine(dot, 0, y, ClientSize.Width, y);
            }

            // Dibujar figuras
            foreach (Shape sh in _shapes) sh.Draw(g);
        }
    }

    // ════════════════════════════════════════════════════════
    //  DarkTheme — Personalización visual del menú
    // ════════════════════════════════════════════════════════
    internal class DarkTheme : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(70, 76, 120);
        public override Color MenuBorder => Color.FromArgb(40, 44, 70);
        public override Color MenuStripGradientBegin => Color.FromArgb(45, 50, 80);
        public override Color MenuStripGradientEnd => Color.FromArgb(45, 50, 80);
        public override Color ToolStripDropDownBackground => Color.FromArgb(55, 60, 95);
        public override Color ImageMarginGradientBegin => Color.FromArgb(55, 60, 95);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(55, 60, 95);
        public override Color ImageMarginGradientEnd => Color.FromArgb(55, 60, 95);
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(70, 76, 120);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(70, 76, 120);
    }
}