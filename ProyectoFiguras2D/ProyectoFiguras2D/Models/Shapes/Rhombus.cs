using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia necesaria para la herencia

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  18. ROMBO
    //  Parámetros: Diagonal mayor (d1), Diagonal menor (d2)
    //  Área = (d1·d2)/2   Perímetro = 4·√((d1/2)²+(d2/2)²)
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa una figura de Rombo.
    /// Implementa Herencia y Encapsulamiento con validación de parámetros.
    /// </summary>
    public class Rhombus : Shape
    {
        private float _d1;
        private float _d2;

        // Restricción: La diagonal mayor (vertical) debe ser positiva
        public float D1
        {
            get => _d1;
            set => _d1 = value > 0 ? value : 100;
        }

        // Restricción: La diagonal menor (horizontal) debe ser positiva
        public float D2
        {
            get => _d2;
            set => _d2 = value > 0 ? value : 60;
        }

        /// <summary>
        /// Constructor de la clase Rhombus.
        /// </summary>
        public Rhombus(PointF c, float d1, float d2, Color color, float scale = 1f)
            : base("Rhombus", color, c, scale) // Envío de datos al constructor base
        {
            D1 = d1;
            D2 = d2;
        }

        // --- Polimorfismo: Implementación de cálculos geométricos ---

        /// <summary>Calcula el área del rombo: (Diagonal1 * Diagonal2) / 2</summary>
        public override double Area() => (D1 * D2) / 2.0;

        /// <summary>Calcula el perímetro utilizando el Teorema de Pitágoras para sus lados.</summary>
        public override double Perimeter()
        {
            double h = D1 / 2.0, w = D2 / 2.0;
            return 4 * Math.Sqrt(h * h + w * w);
        }

        /// <summary>Define los parámetros editables en la interfaz de usuario.</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Diagonal mayor", "px", (double)D1), ("Diagonal menor", "px", (double)D2) };

        /// <summary>Actualiza las diagonales desde la vista con validación interna.</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) D1 = (float)v[0];
            if (v.Length > 1) D2 = (float)v[1];
        }

        /// <summary>
        /// Renderiza el rombo utilizando un polígono de 4 puntos con GDI+.
        /// </summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g); // Configuración de suavizado (Anti-alias)

            float h = D1 * Scale / 2f;
            float w = D2 * Scale / 2f;

            // Definición de los 4 vértices para formar el rombo centrado respecto al Center
            PointF[] pts = {
                new PointF(Center.X,     Center.Y - h), // Punta superior
                new PointF(Center.X + w, Center.Y),     // Punta derecha
                new PointF(Center.X,     Center.Y + h), // Punta inferior
                new PointF(Center.X - w, Center.Y)      // Punta izquierda
            };

            // Gestión de recursos mediante bloques 'using' para limpieza de memoria
            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                g.FillPolygon(f, pts); // Relleno de la forma
                g.DrawPolygon(p, pts); // Contorno de la forma
            }

            DrawLabel(g, h + 5f); // Etiqueta dinámica debajo de la figura
        }
    }
}