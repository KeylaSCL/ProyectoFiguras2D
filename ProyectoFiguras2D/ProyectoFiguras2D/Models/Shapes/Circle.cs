using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia a la clase base Shape

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  2. CÍRCULO
    //  Parámetro: Radio (r)
    //  Área = π·r²        Perímetro = 2π·r
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa un Círculo.
    /// Implementa Herencia de la clase abstracta Shape.
    /// </summary>
    public class Circle : Shape
    {
        // Atributo específico encapsulado para el radio
        public float Radius { get; set; }

        /// <summary>
        /// Constructor de la clase Circle.
        /// </summary>
        /// <param name="c">Punto central (PointF)</param>
        /// <param name="radius">Radio inicial</param>
        /// <param name="color">Color de relleno</param>
        /// <param name="scale">Escala proporcional</param>
        public Circle(PointF c, float radius, Color color, float scale = 1f)
            : base("Circle", color, c, scale) // Envío de datos al constructor de la clase padre
        {
            Radius = radius;
        }

        // --- Implementación de Polimorfismo mediante Override ---

        /// <summary>Calcula el área del círculo (π * r²)</summary>
        public override double Area() => Math.PI * Radius * Radius;

        /// <summary>Calcula el perímetro del círculo (2 * π * r)</summary>
        public override double Perimeter() => 2 * Math.PI * Radius;

        /// <summary>Define los parámetros geométricos para la interfaz de usuario</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Radio", "px", (double)Radius) };

        /// <summary>Actualiza el radio desde el panel de parámetros (MVC)</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) Radius = (float)v[0];
        }

        /// <summary>
        /// Renderiza el círculo utilizando GDI+.
        /// Uso adecuado de Pen, SolidBrush y RectangleF.
        /// </summary>
        public override void Draw(Graphics g)
        {
            // Configuración del suavizado de bordes
            SetupGraphics(g);

            // Aplicación de la escala proporcional
            float r = Radius * Scale;

            // Definición del área de dibujo centrada
            var rect = new RectangleF(Center.X - r, Center.Y - r, r * 2f, r * 2f);

            // Gestión eficiente de recursos GDI+
            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                g.FillEllipse(f, rect); // Dibuja el relleno circular
                g.DrawEllipse(p, rect); // Dibuja el contorno circular
            }

            // Dibuja la etiqueta dinámica (Nombre, Área, Perímetro)
            DrawLabel(g, r + 5f);
        }
    }
}