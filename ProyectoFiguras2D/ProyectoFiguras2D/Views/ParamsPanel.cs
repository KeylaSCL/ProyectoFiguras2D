// ============================================================
//  ParamsPanel.cs  —  Panel lateral de parámetros y resultados
//
//  Permite al usuario ingresar los parámetros de cada figura
//  (lado, radio, altura, etc.) y muestra el Área y Perímetro
//  calculados en tiempo real.
// ============================================================
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectoFiguras2D.Models;

namespace ProyectoFiguras2D.Views
{
    /// <summary>
    /// Panel lateral derecho con:
    ///   • Nombre y descripción de la figura
    ///   • Campos NumericUpDown para cada parámetro
    ///   • Botón "Aplicar"
    ///   • Tarjeta de resultados: Área y Perímetro
    /// </summary>
    public class ParamsPanel : Panel
    {
        // ── Controles internos ───────────────────────────────
        private Label _titleLbl;
        private Label _descLbl;
        private Panel _paramsContainer;
        private Button _applyBtn;
        private Panel _resultsCard;
        private Label _areaLbl;
        private Label _periLbl;
        private Label _formulaLbl;

        // ── Estado ───────────────────────────────────────────
        private Shape _shape;
        private List<NumericUpDown> _inputs = new List<NumericUpDown>();

        // ── Evento: usuario presionó Aplicar ────────────────
        public event EventHandler ParamsChanged;

        // ── Colores ──────────────────────────────────────────
        private static readonly Color C_DARK = Color.FromArgb(45, 50, 80);
        private static readonly Color C_ACCENT = Color.FromArgb(92, 107, 192);
        private static readonly Color C_BG = Color.FromArgb(250, 251, 255);
        private static readonly Color C_CARD = Color.FromArgb(237, 240, 255);
        private static readonly Color C_GREEN = Color.FromArgb(30, 130, 76);
        private static readonly Color C_BLUE = Color.FromArgb(25, 100, 190);

        public ParamsPanel()
        {
            BackColor = C_BG;
            Width = 230;
            Dock = DockStyle.Right;
            Padding = new Padding(10, 8, 10, 8);
            BuildLayout();
        }

        // ════════════════════════════════════════════════════
        //  CONSTRUCCIÓN DEL LAYOUT
        // ════════════════════════════════════════════════════

        private void BuildLayout()
        {
            // ── Título de la figura ──────────────────────────
            _titleLbl = new Label
            {
                Text = "Selecciona una figura",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = C_DARK,
                Dock = DockStyle.Top,
                Height = 28,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Línea separadora
            var line1 = new Panel { Dock = DockStyle.Top, Height = 2, BackColor = C_ACCENT };

            // ── Descripción ──────────────────────────────────
            _descLbl = new Label
            {
                Text = "Use el menú superior\npara seleccionar\nuna figura.",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(100, 100, 130),
                Dock = DockStyle.Top,
                Height = 52,
                Padding = new Padding(0, 6, 0, 6)
            };

            // ── Encabezado "Parámetros" ──────────────────────
            var paramHdr = new Label
            {
                Text = "▸  PARÁMETROS",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = C_ACCENT,
                Dock = DockStyle.Top,
                Height = 22
            };

            // ── Contenedor dinámico de campos ───────────────
            _paramsContainer = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                BackColor = C_BG
            };

            // ── Botón Aplicar ────────────────────────────────
            _applyBtn = new Button
            {
                Text = "Calcular",
                Dock = DockStyle.Top,
                Height = 34,
                BackColor = C_ACCENT,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false
            };
            _applyBtn.FlatAppearance.BorderSize = 0;
            _applyBtn.Click += OnApply;

            var spacer = new Panel { Dock = DockStyle.Top, Height = 8, BackColor = C_BG };

            // ── Tarjeta de resultados ────────────────────────
            _resultsCard = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                BackColor = C_CARD,
                Visible = false,
                Padding = new Padding(10, 8, 10, 8)
            };
            _resultsCard.Paint += (s, e) =>
            {
                // Borde redondeado
                using (Pen pen = new Pen(C_ACCENT, 1.5f))
                    e.Graphics.DrawRectangle(pen, 0, 0,
                        _resultsCard.Width - 1, _resultsCard.Height - 1);
            };

            // Header de la tarjeta
            var resHdr = new Label
            {
                Text = "RESULTADOS",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = C_ACCENT,
                Dock = DockStyle.Top,
                Height = 20
            };

            _areaLbl = new Label
            {
                Text = "Área: —",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = C_GREEN,
                Dock = DockStyle.Top,
                Height = 26
            };

            _periLbl = new Label
            {
                Text = "Perímetro: —",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = C_BLUE,
                Dock = DockStyle.Top,
                Height = 26
            };

            _formulaLbl = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 8f, FontStyle.Italic),
                ForeColor = Color.FromArgb(120, 120, 150),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft
            };

            _resultsCard.Controls.Add(_formulaLbl);
            _resultsCard.Controls.Add(_periLbl);
            _resultsCard.Controls.Add(_areaLbl);
            _resultsCard.Controls.Add(resHdr);

            // ── Ensamblar (orden inverso por Dock=Top) ───────
            Controls.Add(_formulaLbl);        // placeholder inferior
            Controls.Add(_resultsCard);
            Controls.Add(spacer);
            Controls.Add(_applyBtn);
            Controls.Add(_paramsContainer);
            Controls.Add(paramHdr);
            Controls.Add(_descLbl);
            Controls.Add(line1);
            Controls.Add(_titleLbl);
        }

        // ════════════════════════════════════════════════════
        //  CARGA DE FIGURA
        // ════════════════════════════════════════════════════

        /// <summary>
        /// Carga una figura en el panel: genera los campos de entrada
        /// dinámicamente según GetParams() y muestra los resultados.
        /// </summary>
        public void LoadShape(Shape shape, string description)
        {
            _shape = shape;

            _titleLbl.Text = shape.Name;
            _descLbl.Text = description;

            BuildInputFields();
            UpdateResults();

            _applyBtn.Visible = true;
            _resultsCard.Visible = true;
        }

        /// <summary>Genera dinámicamente los NumericUpDown para cada parámetro.</summary>
        private void BuildInputFields()
        {
            _paramsContainer.Controls.Clear();
            _inputs.Clear();

            var pars = _shape.GetParams();

            // Construir en orden inverso (Dock=Top los apila)
            for (int i = pars.Length - 1; i >= 0; i--)
            {
                var par = pars[i];
                string labelText = par.Item1; // Representa el nombre del parámetro
                string unitText = par.Item2;  // Representa la unidad (px, °, etc.)
                double val = par.Item3;       // Representa el valor numérico
                int idx = i; // captura para evento

                // Contenedor de fila
                var row = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 54,
                    BackColor = C_BG,
                    Padding = new Padding(0, 4, 0, 0)
                };

                // Etiqueta
                var lbl = new Label
                {
                    Text = $"{labelText} ({unitText})",
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = C_DARK,
                    Location = new Point(0, 2),
                    Size = new Size(210, 18)
                };

                // NumericUpDown
                var num = new NumericUpDown
                {
                    Minimum = 1,
                    Maximum = 5000,
                    Value = (decimal)Math.Max(1, Math.Round(val, 1)),
                    DecimalPlaces = 1,
                    Increment = 5,
                    Location = new Point(0, 22),
                    Size = new Size(210, 26),
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    BackColor = Color.White,
                    ForeColor = C_DARK,

                    InterceptArrowKeys = false,
                    UpDownAlign = LeftRightAlignment.Right
                };

                num.Controls[0].Visible = false;

                // Actualización en tiempo real al cambiar valor
                num.ValueChanged += (s, e) =>
                {
                    if (_shape != null) ApplyAndRefresh();
                };

                row.Controls.Add(num);
                row.Controls.Add(lbl);
                _paramsContainer.Controls.Add(row);
                _inputs.Insert(0, num); // mantener orden
            }
        }

        // ════════════════════════════════════════════════════
        //  EVENTOS
        // ════════════════════════════════════════════════════

        private void OnApply(object sender, EventArgs e)
        {
            ApplyAndRefresh();
        }

        private void ApplyAndRefresh()
        {
            if (_shape == null) return;

            // Leer valores y actualizar la figura
            double[] values = new double[_inputs.Count];
            for (int i = 0; i < _inputs.Count; i++)
                values[i] = (double)_inputs[i].Value;

            _shape.SetParams(values);
            UpdateResults();

            // Notificar al canvas para redibujar
            ParamsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Actualiza las etiquetas de Área y Perímetro.</summary>
        private void UpdateResults()
        {
            if (_shape == null) return;
            _areaLbl.Text = $"Área:  {_shape.Area():F2} px²";
            _periLbl.Text = $"Perím: {_shape.Perimeter():F2} px";
            _formulaLbl.Text = GetFormula(_shape.Name);
        }

        // Retorna la fórmula para mostrar en la tarjeta
        private string GetFormula(string name)
        {
            switch (name)
            {
                case "Square": return "A = a²\nP = 4·a";
                case "Circle": return "A = π·r²\nP = 2·π·r";
                case "Triangle": return "A = (b·h)/2\nP = b + 2·lado";
                case "Rectangle": return "A = a·b\nP = 2(a+b)";
                case "Ellipse": return "A = π·a·b\nP ≈ Ramanujan";
                case "Star": return "A = n·Ri·Re·sin(2π/n)\nP = 2n·lado";
                case "Pentagon":
                case "Hexagon":
                case "Octagon": return "A = n·R²·sin(2π/n)/2\nP = n·2R·sin(π/n)";
                case "Parallelogram": return "A = b·h\nP = 2(b+l)";
                case "Scalene": return "A = Heron\nP = a+b+c";
                case "Trapeze": return "A = (B+b)·h/2\nP = B+b+2·leg";
                case "Trefoil": return "A ≈ 3·π·r²·0.75\nP ≈ 3·π·r";
                case "Crescent": return "A ≈ π(R²-r²)/2\nP ≈ π(R+r)";
                case "Cross": return "A = 2·s·a - a²\nP = 4·(s-a)+4a";
                case "Semicircle": return "A = π·r²/2\nP = π·r + 2r";
                case "Kite": return "A = d1·d2/2\nP = 2√(...)+2√(...)";
                case "Pie": return "A = (θ/360)·π·r²\nP = 2r + arc";
                case "Heart": return "A ≈ π·r² + r²/2\nP ≈ 4·π·r·0.6";
                case "Rhombus": return "A = d1·d2/2\nP = 4·√((d1/2)²+(d2/2)²)";
                default: return "Fórmulas geométricas";
            }
        }

        /// <summary>Retorna la figura actualmente cargada.</summary>
        public Shape CurrentShape => _shape;
    }
}
