using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using ProyectoFiguras2D.Models; // Referencia necesaria para la herencia

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  14. SEMICÍRCULO
    //  Parámetro: Radio (r)
    //  Área = π·r²/2      Perímetro = π·r + 2r
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa una figura de Semicírculo.
    /// Implementa Herencia y Encapsulamiento con validación de parámetros.
    /// </summary>
    public class Semicircle : Shape
    {
        private float _radius;

        // Restricción: El radio debe ser mayor a 0 para que la figura sea visible
        public float Radius
        {
            get => _radius;
            set => _radius = value > 0 ? value : 40;
        }

        /// <summary>
        /// Constructor de la clase Semicircle.
        /// </summary>
        public Semicircle(PointF c, float r, Color color, float scale = 1f)
            : base("Semicircle", color, c, scale) // Envío de datos al constructor base
        {
            Radius = r;
        }

        // --- Polimorfismo: Implementación de cálculos geométricos ---

        /// <summary>Calcula el área del semicírculo (mitad de un círculo completo).</summary>
        public override double Area() => Math.PI * Radius * Radius / 2.0;

        /// <summary>Calcula el perímetro sumando el arco y el diámetro base.</summary>
        public override double Perimeter() => Math.PI * Radius + 2 * Radius;

        /// <summary>Define los parámetros editables en la interfaz de usuario.</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Radio", "px", (double)Radius) };

        /// <summary>Actualiza el radio desde la vista con validación interna.</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) Radius = (float)v[0];
        }

        /// <summary>
        /// Renderiza el semicírculo utilizando GraphicsPath para cerrar la figura.
        /// </summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g); // Configuración de suavizado (Anti-alias)

            float r = Radius * Scale;

            // Definición del rectángulo delimitador centrado
            var rect = new RectangleF(Center.X - r, Center.Y - r, r * 2f, r * 2f);

            // Uso de GraphicsPath para crear una forma cerrada (arco + línea base)
            using (GraphicsPath path = new GraphicsPath())
            {
                // Agrega un arco de 180 grados empezando desde la izquierda
                path.AddArc(rect, 180f, 180f);
                path.CloseFigure(); // Cierra el camino con una línea recta (diámetro)

                using (SolidBrush f = CreateFillBrush())
                using (Pen p = CreatePen())
                {
                    g.FillPath(f, path); // Relleno de la forma cerrada
                    g.DrawPath(p, path); // Contorno de la forma cerrada
                }
            }

            DrawLabel(g, r + 5f); // Etiqueta dinámica
        }
    }
}