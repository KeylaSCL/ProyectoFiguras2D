using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia necesaria para la herencia

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  15. COMETA (KITE)
    //  Parámetros: Diagonal horizontal (d1), Diagonal vertical (d2)
    //  Área = (d1·d2)/2   Perímetro = 2·√((d1/2)²+(d2·k)²)+2·√((d1/2)²+(d2·(1-k))²)
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa una figura de Cometa (Kite).
    /// Implementa Herencia y Encapsulamiento con validación de parámetros.
    /// </summary>
    public class Kite : Shape
    {
        private float _d1;
        private float _d2;

        // Restricción: La diagonal horizontal debe ser mayor a 0
        public float D1
        {
            get => _d1;
            set => _d1 = value > 0 ? value : 60;
        }

        // Restricción: La diagonal vertical debe ser mayor a 0
        public float D2
        {
            get => _d2;
            set => _d2 = value > 0 ? value : 90;
        }

        /// <summary>
        /// Constructor de la clase Kite.
        /// </summary>
        public Kite(PointF c, float d1, float d2, Color color, float scale = 1f)
            : base("Kite", color, c, scale)
        {
            D1 = d1;
            D2 = d2;
        }

        // --- Polimorfismo: Implementación de cálculos geométricos ---

        /// <summary>Calcula el área del cometa: (D1 * D2) / 2</summary>
        public override double Area() => (D1 * D2) / 2.0;

        /// <summary>
        /// Calcula el perímetro considerando la asimetría de las diagonales.
        /// Tope: 38% de D2; Cola: 62% de D2.
        /// </summary>
        public override double Perimeter()
        {
            double topH = D2 * 0.38, botH = D2 * 0.62;
            double half = D1 / 2.0;
            return 2 * Math.Sqrt(half * half + topH * topH)
                 + 2 * Math.Sqrt(half * half + botH * botH);
        }

        /// <summary>Define los parámetros editables en la interfaz de usuario.</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Diagonal horiz.", "px", (double)D1), ("Diagonal vert.", "px", (double)D2) };

        /// <summary>Actualiza las diagonales desde la vista con validación interna.</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) D1 = (float)v[0];
            if (v.Length > 1) D2 = (float)v[1];
        }

        /// <summary>
        /// Renderiza el cometa utilizando un polígono de 4 puntos con GDI+.
        /// </summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g); // Configuración de suavizado

            float w = D1 * Scale / 2f;
            float ht = D2 * Scale;
            float top = ht * 0.38f;

            // Definición de los 4 vértices para formar el cometa centrado
            PointF[] pts = {
                new PointF(Center.X, Center.Y - top),          // Punta superior
                new PointF(Center.X + w, Center.Y),            // Esquina derecha
                new PointF(Center.X, Center.Y + ht - top),     // Punta inferior (cola)
                new PointF(Center.X - w, Center.Y)             // Esquina izquierda
            };

            // Gestión de recursos mediante bloques 'using'
            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                g.FillPolygon(f, pts);
                g.DrawPolygon(p, pts);
            }

            DrawLabel(g, ht - top + 5f); // Etiqueta dinámica debajo de la cola
        }
    }
}