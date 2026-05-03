using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia necesaria para heredar de Shape

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  5. ELIPSE
    //  Parámetros: Semieje a (horizontal), Semieje b (vertical)
    //  Área = π·a·b
    //  Perímetro ≈ π[3(a+b) - √((3a+b)(a+3b))]  (Ramanujan)
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa una Elipse.
    /// Implementa Herencia y encapsulamiento con validación de parámetros.
    /// </summary>
    public class Ellipse : Shape
    {
        private float _radiusX;
        private float _radiusY;

        // Restricción: El semieje horizontal debe ser mayor a 0
        public float RadiusX
        {
            get => _radiusX;
            set => _radiusX = value > 0 ? value : 50;
        }

        // Restricción: El semieje vertical debe ser mayor a 0
        public float RadiusY
        {
            get => _radiusY;
            set => _radiusY = value > 0 ? value : 30;
        }

        /// <summary>
        /// Constructor de la clase Ellipse2D.
        /// </summary>
        public Ellipse(PointF c, float rx, float ry, Color color, float scale = 1f)
            : base("Ellipse", color, c, scale)
        {
            RadiusX = rx;
            RadiusY = ry;
        }

        // --- Polimorfismo: Implementación de cálculos geométricos ---

        /// <summary>Calcula el área de la elipse: π * a * b</summary>
        public override double Area() => Math.PI * RadiusX * RadiusY;

        /// <summary>
        /// Calcula el perímetro aproximado utilizando la fórmula de Ramanujan.
        /// </summary>
        public override double Perimeter()
        {
            double a = RadiusX, b = RadiusY;
            return Math.PI * (3 * (a + b) - Math.Sqrt((3 * a + b) * (a + 3 * b)));
        }

        /// <summary>Define los parámetros editables en la interfaz de usuario</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Semieje a", "px", (double)RadiusX), ("Semieje b", "px", (double)RadiusY) };

        /// <summary>Actualiza los parámetros desde la vista con validación interna</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) RadiusX = (float)v[0];
            if (v.Length > 1) RadiusY = (float)v[1];
        }

        /// <summary>
        /// Renderiza la elipse en el lienzo utilizando GDI+.
        /// </summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g);

            // Aplicación del factor de escala a los semiejes
            float rx = RadiusX * Scale;
            float ry = RadiusY * Scale;

            // Definición del rectángulo delimitador centrado
            var rect = new RectangleF(Center.X - rx, Center.Y - ry, rx * 2f, ry * 2f);

            // Gestión de recursos GDI+ con bloques 'using'
            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                g.FillEllipse(f, rect); // Relleno de la elipse
                g.DrawEllipse(p, rect); // Contorno de la elipse
            }

            // Dibuja la etiqueta con el nombre y cálculos (ajustada al semieje vertical)
            DrawLabel(g, ry + 5f);
        }
    }
}