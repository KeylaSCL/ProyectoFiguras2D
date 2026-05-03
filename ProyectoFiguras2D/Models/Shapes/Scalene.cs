using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia necesaria para la herencia

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  9. TRIÁNGULO ESCALENO
    //  Parámetros: Lado a, Lado b, Lado c
    //  Área = Heron: √(s(s-a)(s-b)(s-c))
    //  Perímetro = a+b+c
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa un Triángulo Escaleno.
    /// Implementa Herencia y validación de integridad geométrica.
    /// </summary>
    public class Scalene : Shape
    {
        private float _a;
        private float _b;
        private float _c;

        // Propiedades con validación (Encapsulamiento)
        public float A
        {
            get => _a;
            set => _a = value > 0 ? value : 70;
        }
        public float B
        {
            get => _b;
            set => _b = value > 0 ? value : 55;
        }
        public float C
        {
            get => _c;
            set => _c = value > 0 ? value : 85;
        }

        /// <summary>
        /// Constructor de la clase Scalene.
        /// </summary>
        public Scalene(PointF c, float a, float b, float cc, Color color, float scale = 1f)
            : base("Scalene", color, c, scale)
        {
            A = a;
            B = b;
            C = cc;
            ValidarTriangulo();
        }

        /// <summary>
        /// Verifica que los lados formen un triángulo válido. 
        /// Si no, ajusta el lado C para cumplir la desigualdad triangular.
        /// </summary>
        private void ValidarTriangulo()
        {
            if (A + B <= C || A + C <= B || B + C <= A)
            {
                // Ajuste de emergencia para mantener una forma válida
                C = (A + B) * 0.8f;
            }
        }

        // --- Polimorfismo: Implementación de fórmulas ---

        public override double Perimeter() => A + B + C;

        /// <summary>Calcula el área usando la fórmula de Herón.</summary>
        public override double Area()
        {
            double s = Perimeter() / 2.0;
            double val = s * (s - A) * (s - B) * (s - C);
            return val > 0 ? Math.Sqrt(val) : 0;
        }

        public override (string, string, double)[] GetParams() =>
            new[] {
                ("Lado a", "px", (double)A),
                ("Lado b", "px", (double)B),
                ("Lado c", "px", (double)C)
            };

        public override void SetParams(double[] v)
        {
            if (v.Length > 0) A = (float)v[0];
            if (v.Length > 1) B = (float)v[1];
            if (v.Length > 2) C = (float)v[2];
            ValidarTriangulo(); // Re-validar después de cambios del usuario
        }

        /// <summary>Renderiza el triángulo escaleno calculando vértices por Ley de Cosenos.</summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g);

            float a = A * Scale, b = B * Scale, c = C * Scale;

            // Vértice 1 (izquierda), Vértice 2 (derecha)
            float x1 = Center.X - a / 2f;
            float y1 = Center.Y + b * 0.35f;
            float x2 = Center.X + a / 2f;
            float y2 = y1;

            // Cálculo del tercer vértice usando la Ley de Cosenos para el ángulo A
            double cosA = (a * a + c * c - b * b) / (2.0 * a * c);
            cosA = Math.Max(-1, Math.Min(1, cosA)); // Clamp para evitar errores de precisión

            float x3 = x1 + c * (float)Math.Cos(Math.Acos(cosA));
            float y3 = y1 - c * (float)Math.Sin(Math.Acos(cosA));

            PointF[] pts = { new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3) };

            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                g.FillPolygon(f, pts);
                g.DrawPolygon(p, pts);
            }

            DrawLabel(g, b * Scale * 0.35f + 5f);
        }
    }
}