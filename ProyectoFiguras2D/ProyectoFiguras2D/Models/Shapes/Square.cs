using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia a la clase base Shape

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  1. CUADRADO
    //  Parámetro: Lado (a)
    //  Área = a²          Perímetro = 4a
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa un Cuadrado. 
    /// Implementa Herencia de la clase abstracta Shape.
    /// </summary>
    public class Square : Shape
    {
        // Atributo específico de la figura (Encapsulamiento)
        public float Side { get; set; }

        /// <summary>
        /// Constructor de la clase Square.
        /// </summary>
        /// <param name="c">Centro de la figura (PointF)</param>
        /// <param name="side">Longitud del lado</param>
        /// <param name="color">Color de relleno</param>
        /// <param name="scale">Factor de escala inicial</param>
        public Square(PointF c, float side, Color color, float scale = 1f)
            : base("Square", color, c, scale) // Envía datos al constructor base
        {
            Side = side;
        }

        // --- Implementación de Polimorfismo (Override de métodos abstractos) ---

        /// <summary>Calcula el área del cuadrado (Lado * Lado)</summary>
        public override double Area() => Side * Side;

        /// <summary>Calcula el perímetro del cuadrado (4 * Lado)</summary>
        public override double Perimeter() => 4 * Side;

        /// <summary>Define los parámetros editables para la interfaz de usuario</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Lado", "px", (double)Side) };

        /// <summary>Actualiza el valor del lado desde la vista (MVC)</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) Side = (float)v[0];
        }

        /// <summary>
        /// Realiza el renderizado de la figura en el lienzo.
        /// Uso de GDI+ (Graphics, Pen, Brush).
        /// </summary>
        public override void Draw(Graphics g)
        {
            // Configuración de suavizado (Anti-alias)
            SetupGraphics(g);

            // Aplicación del factor de escala proporcional
            float s = Side * Scale;

            // Definición del rectángulo que encierra al cuadrado centrado
            var rect = new RectangleF(Center.X - s / 2f, Center.Y - s / 2f, s, s);

            // Gestión de recursos GDI+ con el bloque 'using'
            using (SolidBrush f = CreateFillBrush()) // Crea el pincel de relleno
            using (Pen p = CreatePen())             // Crea el lápiz para el contorno
            {
                g.FillRectangle(f, rect);            // Dibuja el fondo de la figura
                g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height); // Dibuja el borde
            }

            // Dibuja la etiqueta con el nombre y cálculos (si están activos)
            DrawLabel(g, s / 2f + 5f);
        }
    }
}