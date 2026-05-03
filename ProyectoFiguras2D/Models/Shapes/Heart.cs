using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using ProyectoFiguras2D.Models; // Referencia necesaria para la herencia

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  17. CORAZÓN
    //  Parámetro: Tamaño (s)
    //  Área ≈ π·(s/2)² + s² / 2  (círculo + triángulo aprox)
    //  Perímetro ≈ 4·π·(s/2)     (dos semicírculos)
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa una figura de Corazón.
    /// Implementa Herencia y Encapsulamiento con validación de parámetros.
    /// </summary>
    public class Heart : Shape
    {
        private float _size;

        // Restricción: El tamaño debe ser mayor a 0 para que la figura sea visible
        public float Size
        {
            get => _size;
            set => _size = value > 0 ? value : 50;
        }

        /// <summary>
        /// Constructor de la clase Heart.
        /// </summary>
        public Heart(PointF c, float size, Color color, float scale = 1f)
            : base("Heart", color, c, scale) // Envío de datos al constructor base
        {
            Size = size;
        }

        // --- Polimorfismo: Implementación de cálculos geométricos aproximados ---

        /// <summary>Calcula el área aproximada sumando el área del círculo superior y el triángulo inferior.</summary>
        public override double Area() => Math.PI * Size * Size * 0.36 + Size * Size * 0.5;

        /// <summary>Calcula el perímetro aproximado basado en la suma de los semicírculos.</summary>
        public override double Perimeter() => 4 * Math.PI * Size * 0.6;

        /// <summary>Define los parámetros editables en la interfaz de usuario.</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Tamaño", "px", (double)Size) };

        /// <summary>Actualiza el tamaño desde la vista con validación interna.</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) Size = (float)v[0];
        }

        /// <summary>
        /// Renderiza el corazón utilizando curvas de Bézier para suavizar la forma.
        /// </summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g); // Configuración de suavizado (Anti-alias)

            float s = Size * Scale;
            float x = Center.X, y = Center.Y;

            // Uso de GraphicsPath para construir la silueta mediante curvas de Bézier
            using (GraphicsPath path = new GraphicsPath())
            {
                // Mitad izquierda
                path.AddBezier(x, y - s * 0.2f, x, y - s * 0.6f, x - s * 0.6f, y - s * 0.6f, x - s * 0.6f, y - s * 0.1f);
                path.AddBezier(x - s * 0.6f, y - s * 0.1f, x - s * 0.6f, y + s * 0.4f, x, y + s * 0.5f, x, y + s * 0.6f);

                // Mitad derecha (simetría)
                path.AddBezier(x, y + s * 0.6f, x, y + s * 0.5f, x + s * 0.6f, y + s * 0.4f, x + s * 0.6f, y - s * 0.1f);
                path.AddBezier(x + s * 0.6f, y - s * 0.1f, x + s * 0.6f, y - s * 0.6f, x, y - s * 0.6f, x, y - s * 0.2f);

                path.CloseFigure(); // Cierra el camino para permitir el relleno

                using (SolidBrush f = CreateFillBrush())
                using (Pen p = CreatePen())
                {
                    g.FillPath(f, path); // Relleno de la forma compleja
                    g.DrawPath(p, path); // Contorno de la forma compleja
                }
            }

            DrawLabel(g, s * 0.6f + 5f); // Etiqueta dinámica
        }
    }
}