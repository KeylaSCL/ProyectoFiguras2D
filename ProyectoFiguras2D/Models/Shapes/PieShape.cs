using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia necesaria para la herencia

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  16. SECTOR CIRCULAR (PIE)
    //  Parámetros: Radio (r), Ángulo (θ en grados)
    //  Área = (θ/360)·π·r²    Perímetro = 2r + arco = 2r + θ·π·r/180
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa un Sector Circular.
    /// Implementa Herencia y Encapsulamiento con validación de rango angular.
    /// </summary>
    public class PieShape : Shape
    {
        private float _radius;
        private float _sweepAngle;

        // Restricción: El radio debe ser mayor a 0
        public float Radius
        {
            get => _radius;
            set => _radius = value > 0 ? value : 45;
        }

        // Restricción: El ángulo de barrido debe estar entre 1 y 359 grados
        public float SweepAngle
        {
            get => _sweepAngle;
            set => _sweepAngle = Math.Min(359, Math.Max(1, value));
        }

        public float StartAngle { get; set; }

        /// <summary>
        /// Constructor de la clase PieShape.
        /// </summary>
        public PieShape(PointF c, float r, float start, float sweep, Color color, float scale = 1f)
            : base("Pie", color, c, scale)
        {
            Radius = r;
            StartAngle = start;
            SweepAngle = sweep;
        }

        // --- Polimorfismo: Implementación de cálculos geométricos ---

        /// <summary>Calcula el área del sector circular: (θ/360) * π * r²</summary>
        public override double Area() => (SweepAngle / 360.0) * Math.PI * Radius * Radius;

        /// <summary>Calcula el perímetro: 2r + longitud del arco (θ * π * r / 180)</summary>
        public override double Perimeter() => 2 * Radius + (SweepAngle * Math.PI * Radius / 180.0);

        /// <summary>Define los parámetros editables en la interfaz de usuario.</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Radio", "px", (double)Radius), ("Ángulo", "°", (double)SweepAngle) };

        /// <summary>Actualiza el radio y el ángulo desde la vista con validación interna.</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) Radius = (float)v[0];
            if (v.Length > 1) SweepAngle = (float)v[1];
        }

        /// <summary>
        /// Renderiza el sector circular utilizando los métodos nativos de GDI+.
        /// </summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g); // Configuración de suavizado

            float r = Radius * Scale;

            // Definición del rectángulo delimitador centrado
            var rect = new RectangleF(Center.X - r, Center.Y - r, r * 2f, r * 2f);

            // Gestión de recursos mediante bloques 'using'
            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                // Dibuja el sector circular relleno y su contorno
                g.FillPie(f, rect.X, rect.Y, rect.Width, rect.Height, StartAngle, SweepAngle);
                g.DrawPie(p, rect.X, rect.Y, rect.Width, rect.Height, StartAngle, SweepAngle);
            }

            DrawLabel(g, r + 5f); // Etiqueta dinámica
        }
    }
}