using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia necesaria para la herencia

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  13. CRUZ
    //  Parámetros: Tamaño total (s), Grosor brazo (a)
    //  Área = s·a·2 - a²     Perímetro = 4(s-a) + 4a = 4s (aprox)
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa una figura de Cruz.
    /// Implementa Herencia y Encapsulamiento con validación de dimensiones.
    /// </summary>
    public class Cross : Shape
    {
        private float _size;
        private float _arm;

        // Restricción: El tamaño total debe ser mayor a 0
        public float Size
        {
            get => _size;
            set => _size = value > 0 ? value : 65;
        }

        // Restricción: El grosor del brazo debe ser positivo y menor al tamaño total
        public float Arm
        {
            get => _arm;
            set => _arm = (value > 0 && value < Size) ? value : Size * 0.33f;
        }

        /// <summary>
        /// Constructor de la clase Cross.
        /// </summary>
        public Cross(PointF c, float size, float arm, Color color, float scale = 1f)
            : base("Cross", color, c, scale) // Envío de datos al constructor base
        {
            Size = size;
            Arm = arm;
        }

        // --- Polimorfismo: Implementación de cálculos geométricos ---

        /// <summary>Calcula el área de la cruz (Suma de los dos rectángulos menos el cuadrado central).</summary>
        public override double Area() => 2.0 * Size * Arm - Arm * Arm;

        /// <summary>Calcula el perímetro sumando todos los segmentos externos.</summary>
        public override double Perimeter() => 4.0 * (Size - Arm) + 4.0 * Arm;

        /// <summary>Define los parámetros editables en la interfaz de usuario.</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Tamaño total", "px", (double)Size), ("Grosor brazo", "px", (double)Arm) };

        /// <summary>Actualiza los parámetros desde la vista con validación interna.</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) Size = (float)v[0];
            if (v.Length > 1) Arm = (float)v[1];
        }

        /// <summary>
        /// Renderiza la cruz utilizando un polígono de 12 puntos con GDI+.
        /// </summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g); // Configuración de suavizado

            float s = Size * Scale / 2f;
            float a = Arm * Scale / 2f;

            // Definición de los 12 vértices para formar la cruz centrada respecto al Center
            PointF[] pts = {
                new PointF(Center.X - a, Center.Y - s),
                new PointF(Center.X + a, Center.Y - s),
                new PointF(Center.X + a, Center.Y - a),
                new PointF(Center.X + s, Center.Y - a),
                new PointF(Center.X + s, Center.Y + a),
                new PointF(Center.X + a, Center.Y + a),
                new PointF(Center.X + a, Center.Y + s),
                new PointF(Center.X - a, Center.Y + s),
                new PointF(Center.X - a, Center.Y + a),
                new PointF(Center.X - s, Center.Y + a),
                new PointF(Center.X - s, Center.Y - a),
                new PointF(Center.X - a, Center.Y - a)
            };

            // Gestión de recursos mediante bloques 'using'
            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                g.FillPolygon(f, pts); // Relleno de la forma
                g.DrawPolygon(p, pts); // Contorno de la forma
            }

            DrawLabel(g, s + 5f); // Etiqueta dinámica
        }
    }
}