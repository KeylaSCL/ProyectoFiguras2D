using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia necesaria para la herencia

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  6. ESTRELLA (N puntas)
    //  Parámetro: Radio exterior (R), Radio interior (r), Puntas (n)
    //  Área = n * r * R * sin(2π/n)
    //  Perímetro = 2 * n * lado_punta
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa una Estrella de N puntas.
    /// Implementa Herencia y Polimorfismo con validación de parámetros dinámicos.
    /// </summary>
    public class Star : Shape
    {
        private float _outerR;
        private float _innerR;
        private int _points;

        // Restricción: El radio exterior debe ser positivo
        public float OuterR
        {
            get => _outerR;
            set => _outerR = value > 0 ? value : 45;
        }

        // Restricción: El radio interior debe ser positivo y menor al exterior
        public float InnerR
        {
            get => _innerR;
            set => _innerR = (value > 0 && value < OuterR) ? value : OuterR * 0.4f;
        }

        // Restricción: Una estrella debe tener al menos 3 puntas
        public int Points
        {
            get => _points;
            set => _points = value >= 3 ? value : 5;
        }

        /// <summary>
        /// Constructor de la clase Star.
        /// </summary>
        public Star(PointF c, float outer, float inner, int pts, Color color, float scale = 1f)
            : base("Star", color, c, scale)
        {
            OuterR = outer;
            InnerR = inner;
            Points = pts;
        }

        // --- Polimorfismo: Implementación de cálculos geométricos ---

        /// <summary>Calcula el área para una estrella de N puntas.</summary>
        public override double Area()
        {
            double step = Math.PI / Points;
            return Points * InnerR * OuterR * Math.Sin(2 * step);
        }

        /// <summary>Calcula el perímetro sumando los lados de cada punta (Ley del Coseno).</summary>
        public override double Perimeter()
        {
            double step = Math.PI / Points;
            double side = Math.Sqrt(OuterR * OuterR + InnerR * InnerR
                          - 2 * OuterR * InnerR * Math.Cos(2 * step));
            return 2 * Points * side;
        }

        /// <summary>Define los parámetros editables, incluyendo ahora la cantidad de puntas.</summary>
        public override (string, string, double)[] GetParams() =>
            new[] {
                ("Radio ext.", "px", (double)OuterR),
                ("Radio int.", "px", (double)InnerR),
                ("Puntas", "n", (double)Points)
            };

        /// <summary>Actualiza los parámetros desde la vista permitiendo cambiar la forma de la estrella.</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) OuterR = (float)v[0];
            if (v.Length > 1) InnerR = (float)v[1];
            if (v.Length > 2) Points = (int)v[2]; // Captura la nueva cantidad de puntas
        }

        /// <summary>Calcula los vértices de la estrella dinámicamente.</summary>
        private PointF[] BuildPoints()
        {
            PointF[] pts = new PointF[Points * 2];
            double step = Math.PI / Points;
            double off = -Math.PI / 2.0; // Orientación hacia arriba
            for (int i = 0; i < Points * 2; i++)
            {
                float r = (i % 2 == 0) ? OuterR * Scale : InnerR * Scale;
                double a = i * step + off;
                pts[i] = new PointF(Center.X + r * (float)Math.Cos(a),
                                      Center.Y + r * (float)Math.Sin(a));
            }
            return pts;
        }

        /// <summary>Renderiza la estrella en el lienzo utilizando GDI+.</summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g); // Configuración de suavizado
            PointF[] pts = BuildPoints();

            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                g.FillPolygon(f, pts); // Relleno de la estrella
                g.DrawPolygon(p, pts); // Contorno de la estrella
            }

            DrawLabel(g, OuterR * Scale + 5f); // Etiqueta dinámica
        }
    }
}