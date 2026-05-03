using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia necesaria para la herencia

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  8. PARALELOGRAMO
    //  Parámetros: Base (b), Altura (h), Lado oblicuo (l)
    //  Área = b·h         Perímetro = 2(b+l)
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa un Paralelogramo.
    /// Implementa Herencia y Encapsulamiento con validación de datos.
    /// </summary>
    public class Parallelogram : Shape
    {
        private float _w;
        private float _h;
        private float _side;

        // Restricción: La base (W) debe ser mayor a 0
        public float W
        {
            get => _w;
            set => _w = value > 0 ? value : 80;
        }

        // Restricción: La altura (H) debe ser mayor a 0
        public float H
        {
            get => _h;
            set => _h = value > 0 ? value : 45;
        }

        // Restricción: El lado oblicuo debe ser coherente con la altura
        public float Side
        {
            get => _side;
            set => _side = value > H ? value : H + 5;
        }

        /// <summary>
        /// Desplazamiento visual calculado para el renderizado GDI+.
        /// </summary>
        public float Offset { get; set; }

        /// <summary>
        /// Constructor de la clase Parallelogram.
        /// </summary>
        public Parallelogram(PointF c, float w, float h, float offset, Color color, float scale = 1f)
            : base("Parallelogram", color, c, scale)
        {
            W = w;
            H = h;
            Offset = offset;
            // Cálculo inicial del lado oblicuo mediante Teorema de Pitágoras
            Side = (float)Math.Sqrt(offset * offset + h * h);
        }

        // --- Polimorfismo: Implementación de fórmulas ---

        /// <summary>Calcula el área del paralelogramo (Base * Altura)</summary>
        public override double Area() => W * H;

        /// <summary>Calcula el perímetro (2 * (Base + Lado Oblicuo))</summary>
        public override double Perimeter() => 2 * (W + Side);

        /// <summary>Define los parámetros editables en la interfaz</summary>
        public override (string, string, double)[] GetParams() =>
            new[] {
                ("Base", "px", (double)W),
                ("Altura", "px", (double)H),
                ("Lado oblicuo", "px", (double)Side)
            };

        /// <summary>Actualiza los valores desde la UI y recalcula el Offset visual</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) W = (float)v[0];
            if (v.Length > 1) H = (float)v[1];
            if (v.Length > 2) Side = (float)v[2];

            // Recalcular el desplazamiento visual basado en el nuevo lado y altura
            Offset = (float)Math.Sqrt(Math.Max(0, Side * Side - H * H));
        }

        /// <summary>
        /// Renderiza el paralelogramo utilizando un array de puntos (PointF).
        /// </summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g);

            // Aplicación de escala y preparación de dimensiones
            float w = W * Scale / 2f;
            float h = H * Scale / 2f;
            float o = Offset * Scale;

            // Definición de los 4 vértices para formar el polígono
            PointF[] pts = {
                new PointF(Center.X - w + o, Center.Y - h),
                new PointF(Center.X + w + o, Center.Y - h),
                new PointF(Center.X + w - o, Center.Y + h),
                new PointF(Center.X - w - o, Center.Y + h)
            };

            // Dibujo de la forma con Pen y Brush
            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                g.FillPolygon(f, pts);
                g.DrawPolygon(p, pts);
            }

            // Etiqueta informativa dinámica debajo de la figura
            DrawLabel(g, h + 5f);
        }
    }
}