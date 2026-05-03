//  Shape.cs  —  Clase base abstracta para todas las figuras
//
//    • Area()      — cálculo del área (abstracto)
//    • Perimeter() — cálculo del perímetro (abstracto)
//    • GetParams() — parámetros ingresables por el usuario
//    • SetParams() — aplica los valores ingresados
// ============================================================
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFiguras2D.Models
{
    /// <summary>
    /// Clase abstracta base de todas las figuras 2D.
    /// Aplica POO: encapsulación, herencia y polimorfismo.
    /// </summary>
    public abstract class Shape
    {
        // ── Propiedades de presentación ─────────────────────
        public Color FillColor { get; set; }
        public Color BorderColor { get; set; }
        public float BorderWidth { get; set; }
        public string Name { get; set; }
        public float Scale { get; set; }
        public PointF Center { get; set; }

        /// <summary>
        /// Si true, la etiqueta muestra Área y Perímetro.
        /// Se activa en vista individual.
        /// </summary>
        public bool ShowCalcs { get; set; } = false;

        // ── Constructor base ────────────────────────────────
        protected Shape(string name, Color fillColor, PointF center, float scale = 1f)
        {
            Name = name;
            FillColor = fillColor;
            BorderColor = Color.FromArgb(60, 0, 0, 0);
            BorderWidth = 1.5f;
            Center = center;
            Scale = scale;
        }

        // ════════════════════════════════════════════════════
        //  MÉTODOS ABSTRACTOS — contrato POO
        // ════════════════════════════════════════════════════

        /// <summary>Dibuja la figura (polimorfismo).</summary>
        public abstract void Draw(Graphics g);

        /// <summary>Retorna el área en unidades².</summary>
        public abstract double Area();

        /// <summary>Retorna el perímetro en unidades.</summary>
        public abstract double Perimeter();

        /// <summary>
        /// Describe los parámetros geométricos que el usuario puede ingresar.
        /// Retorna un array de (Etiqueta, Unidad, ValorActual).
        /// Ej: [("Radio", "px", 50), ("Altura", "px", 80)]
        /// </summary>
        public abstract (string Label, string Unit, double Value)[] GetParams();

        /// <summary>
        /// Actualiza los parámetros de la figura con los valores
        /// ingresados por el usuario (mismo orden que GetParams).
        /// </summary>
        public abstract void SetParams(double[] values);

        // ════════════════════════════════════════════════════
        //  HELPERS PROTEGIDOS
        // ════════════════════════════════════════════════════

        protected void SetupGraphics(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        /// <summary>Crea un Pen con el estilo del contorno.</summary>
        protected Pen CreatePen()
        {
            return new Pen(BorderColor, BorderWidth)
            {
                LineJoin = LineJoin.Round,
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
        }

        /// <summary>Crea un SolidBrush con el color de relleno.</summary>
        protected SolidBrush CreateFillBrush()
        {
            return new SolidBrush(FillColor);
        }

        /// <summary>
        /// Dibuja etiqueta con nombre debajo de la figura.
        /// Si ShowCalcs == true, añade Área y Perímetro.
        /// </summary>
        protected void DrawLabel(Graphics g, float yOffset)
        {
            float ls = Math.Max(Scale, 0.65f);

            using (Font nameFnt = new Font("Segoe UI", 7.5f * ls, FontStyle.Bold))
            using (Font calcFnt = new Font("Segoe UI", 7.0f * ls, FontStyle.Regular))
            using (Brush tb = new SolidBrush(Color.FromArgb(50, 50, 70)))
            using (Brush cb = new SolidBrush(Color.FromArgb(80, 40, 160)))
            {
                string txt = Name.ToUpper();
                SizeF nsz = g.MeasureString(txt, nameFnt);
                float x = Center.X - nsz.Width / 2f;
                float y = Center.Y + yOffset;

                // Calcular altura total del bloque etiqueta
                float bgH = nsz.Height + 4;
                if (ShowCalcs) bgH += (calcFnt.Height + 2) * 2;

                // Fondo redondeado
                using (Brush bg = new SolidBrush(Color.FromArgb(170, 255, 255, 255)))
                    FillRounded(g, bg, x - 4, y - 2, nsz.Width + 8, bgH, 4f);

                // Nombre
                g.DrawString(txt, nameFnt, tb, x, y);

                if (ShowCalcs)
                {
                    string aStr = $"Área = {Area():F2}";
                    string pStr = $"Perím. = {Perimeter():F2}";
                    SizeF asz = g.MeasureString(aStr, calcFnt);
                    SizeF psz = g.MeasureString(pStr, calcFnt);
                    float ay = y + nsz.Height + 2;
                    g.DrawString(aStr, calcFnt, cb, Center.X - asz.Width / 2f, ay);
                    g.DrawString(pStr, calcFnt, cb, Center.X - psz.Width / 2f, ay + calcFnt.Height + 2);
                }
            }
        }

        /// <summary>Construye los vértices de un polígono regular.</summary>
        protected PointF[] BuildRegularPolygon(int sides, float radius,
                                               double startAngle = -Math.PI / 2)
        {
            PointF[] pts = new PointF[sides];
            double step = 2.0 * Math.PI / sides;
            for (int i = 0; i < sides; i++)
            {
                double a = i * step + startAngle;
                pts[i] = new PointF(
                    Center.X + radius * (float)Math.Cos(a),
                    Center.Y + radius * (float)Math.Sin(a));
            }
            return pts;
        }

        // Rectángulo redondeado reutilizable
        private void FillRounded(Graphics g, Brush br, float x, float y,
                                 float w, float h, float r)
        {
            r = Math.Min(r, Math.Min(w / 2f, h / 2f));
            using (GraphicsPath p = new GraphicsPath())
            {
                p.AddArc(x, y, r * 2, r * 2, 180, 90);
                p.AddArc(x + w - r * 2, y, r * 2, r * 2, 270, 90);
                p.AddArc(x + w - r * 2, y + h - r * 2, r * 2, r * 2, 0, 90);
                p.AddArc(x, y + h - r * 2, r * 2, r * 2, 90, 90);
                p.CloseFigure();
                g.FillPath(br, p);
            }
        }
    }
}
