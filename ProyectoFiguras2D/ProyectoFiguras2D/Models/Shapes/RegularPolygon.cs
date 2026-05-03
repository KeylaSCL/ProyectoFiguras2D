using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia a la clase base Shape

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  7. POLÍGONO REGULAR (base de Pentágono, Hexágono, Octágono)
    //  Parámetro: Radio circunscrito (R)
    //  Área = (n·R²·sin(2π/n))/2
    //  Perímetro = n · 2R·sin(π/n)
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase base para polígonos regulares. 
    /// Implementa Herencia y Encapsulamiento con restricciones.
    /// </summary>
    public class RegularPolygon : Shape
    {
        private float _radius;

        // Restricción: El radio debe ser mayor a 0 para ser visible
        public float Radius
        {
            get => _radius;
            set => _radius = value > 0 ? value : 40;
        }

        public int Sides { get; set; }
        protected double StartAngle { get; set; }

        public RegularPolygon(PointF c, float r, int sides, string name,
                              Color color, float scale = 1f, double startAngle = -Math.PI / 2)
            : base(name, color, c, scale)
        {
            Radius = r;
            Sides = sides;
            StartAngle = startAngle;
        }

        // Polimorfismo: Implementación de fórmulas
        public override double Area()
            => 0.5 * Sides * Radius * Radius * Math.Sin(2 * Math.PI / Sides);

        public override double Perimeter()
            => Sides * 2 * Radius * Math.Sin(Math.PI / Sides);

        public override (string, string, double)[] GetParams() =>
            new[] { ("Radio", "px", (double)Radius) };

        public override void SetParams(double[] v) { if (v.Length > 0) Radius = (float)v[0]; }

        public override void Draw(Graphics g)
        {
            SetupGraphics(g);
            PointF[] pts = BuildRegularPolygon(Sides, Radius * Scale, StartAngle);
            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                g.FillPolygon(f, pts);
                g.DrawPolygon(p, pts);
            }
            DrawLabel(g, Radius * Scale + 5f);
        }
    }
}