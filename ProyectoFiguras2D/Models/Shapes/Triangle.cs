using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia a la clase base Shape

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  3. TRIÁNGULO EQUILÁTERO (Isósceles en implementación)
    //  Parámetro: Base (b), Altura (h)
    //  Área = (b·h)/2     Perímetro = b + l1 + l2
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa un Triángulo.
    /// Implementa Herencia y validación de datos (Encapsulamiento).
    /// </summary>
    public class Triangle : Shape
    {
        private float _base;
        private float _height;

        // Restricción: La base debe ser mayor a 0
        public float Base
        {
            get => _base;
            set => _base = value > 0 ? value : 70;
        }

        // Restricción: La altura debe ser mayor a 0
        public float Height
        {
            get => _height;
            set => _height = value > 0 ? value : 60;
        }

        /// <summary>
        /// Constructor de la clase Triangle.
        /// </summary>
        public Triangle(PointF c, float b, float h, Color color, float scale = 1f)
            : base("Triangle", color, c, scale)
        {
            Base = b;
            Height = h;
        }

        // --- Polimorfismo: Implementación de cálculos ---

        /// <summary>Calcula el área del triángulo: (Base * Altura) / 2</summary>
        public override double Area() => 0.5 * Base * Height;

        /// <summary>
        /// Calcula el perímetro del triángulo isósceles.
        /// Lado oblicuo calculado con Teorema de Pitágoras: √((b/2)² + h²)
        /// </summary>
        public override double Perimeter() =>
            Base + 2 * Math.Sqrt(Math.Pow(Base / 2.0, 2) + Math.Pow(Height, 2));

        /// <summary>Define los parámetros para la interfaz de usuario</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Base", "px", (double)Base), ("Altura", "px", (double)Height) };

        /// <summary>Actualiza los parámetros desde la vista con validación interna</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) Base = (float)v[0];
            if (v.Length > 1) Height = (float)v[1];
        }

        /// <summary>
        /// Renderiza el triángulo en el lienzo usando GDI+.
        /// </summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g);

            // Aplicación de escala a las dimensiones
            float b = Base * Scale / 2f;
            float h = Height * Scale / 2f;

            // Definición de los tres vértices del triángulo centrados
            PointF[] pts = {
                new PointF(Center.X, Center.Y - h),          // Vértice superior
                new PointF(Center.X + b, Center.Y + h),      // Vértice inferior derecho
                new PointF(Center.X - b, Center.Y + h)       // Vértice inferior izquierdo
            };

            // Gestión de recursos GDI+
            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                g.FillPolygon(f, pts); // Relleno de la forma
                g.DrawPolygon(p, pts); // Contorno de la forma
            }

            // Dibujo de etiqueta dinámica
            DrawLabel(g, h + 5f);
        }
    }
}