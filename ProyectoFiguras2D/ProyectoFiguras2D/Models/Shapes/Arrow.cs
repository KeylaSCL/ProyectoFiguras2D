using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia necesaria para la herencia

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  19. FLECHA
    //  Parámetros: Ancho (w), Alto (h)
    //  Área ≈ w·h·0.65    Perímetro = suma de lados
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa una figura de Flecha.
    /// Implementa Herencia y Encapsulamiento con validación de parámetros.
    /// </summary>
    public class Arrow : Shape
    {
        private float _w;
        private float _h;

        // Restricción: El ancho debe ser mayor a 0
        public float W
        {
            get => _w;
            set => _w = value > 0 ? value : 80;
        }

        // Restricción: El alto debe ser mayor a 0
        public float H
        {
            get => _h;
            set => _h = value > 0 ? value : 50;
        }

        /// <summary>
        /// Constructor de la clase Arrow.
        /// </summary>
        public Arrow(PointF c, float w, float h, Color color, float scale = 1f)
            : base("Arrow", color, c, scale) // Envío de datos al constructor base
        {
            W = w;
            H = h;
        }

        // --- Polimorfismo: Implementación de cálculos geométricos ---

        /// <summary>Calcula el área aproximada de la flecha.</summary>
        public override double Area() => W * H * 0.65;

        /// <summary>Calcula el perímetro aproximado de la flecha.</summary>
        public override double Perimeter() => 2 * (W + H) * 0.85;

        /// <summary>Define los parámetros editables en la interfaz de usuario.</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Ancho", "px", (double)W), ("Alto", "px", (double)H) };

        /// <summary>Actualiza el ancho y alto desde la vista con validación interna.</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) W = (float)v[0];
            if (v.Length > 1) H = (float)v[1];
        }

        /// <summary>
        /// Renderiza la flecha utilizando un polígono de 7 puntos con GDI+.
        /// </summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g); // Configuración de suavizado (Anti-alias)

            float w = W * Scale / 2f;
            float ht = H * Scale / 2f;
            float ty = ht * 0.35f; // Proporción del cuello de la flecha

            // Definición de los 7 vértices para formar la flecha centrada respecto al Center
            PointF[] pts = {
                new PointF(Center.X - w, Center.Y - ty), // Esquina superior trasera
                new PointF(Center.X,     Center.Y - ty), // Cuello superior
                new PointF(Center.X,     Center.Y - ht), // Punta superior
                new PointF(Center.X + w, Center.Y),      // Punta extrema
                new PointF(Center.X,     Center.Y + ht), // Punta inferior
                new PointF(Center.X,     Center.Y + ty), // Cuello inferior
                new PointF(Center.X - w, Center.Y + ty)  // Esquina inferior trasera
            };

            // Gestión de recursos mediante bloques 'using'
            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                g.FillPolygon(f, pts); // Relleno de la forma
                g.DrawPolygon(p, pts); // Contorno de la forma
            }

            DrawLabel(g, ht + 5f); // Etiqueta dinámica debajo de la figura
        }
    }
}