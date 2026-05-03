using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia necesaria para la herencia

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  11. TRÉBOL — 3 círculos
    //  Parámetro: Radio del círculo (r)
    //  Área ≈ 3·π·r² (tres círculos, sin contar solapamiento exacto)
    //  Perímetro ≈ 3·π·r (tres semicírculos externos)
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa un Trébol compuesto por tres círculos.
    /// Implementa Herencia y validación de datos para asegurar una geometría visible.
    /// </summary>
    public class Trefoil : Shape
    {
        private float _radius;

        // Restricción: El radio de los lóbulos debe ser mayor a 0
        public float Radius
        {
            get => _radius;
            set => _radius = value > 0 ? value : 22;
        }

        /// <summary>
        /// Constructor de la clase Trefoil.
        /// </summary>
        public Trefoil(PointF c, float r, Color color, float scale = 1f)
            : base("Trefoil", color, c, scale) // Envío de datos al constructor base
        {
            Radius = r;
        }

        // --- Polimorfismo: Implementación de cálculos aproximados ---

        /// <summary>Calcula el área aproximada considerando el solapamiento de los círculos.</summary>
        public override double Area() => 3 * Math.PI * Radius * Radius * 0.75;

        /// <summary>Calcula el perímetro aproximado basado en los arcos externos.</summary>
        public override double Perimeter() => 3 * Math.PI * Radius;

        /// <summary>Define los parámetros editables en la interfaz de usuario.</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Radio círculo", "px", (double)Radius) };

        /// <summary>Actualiza el radio desde la vista con validación interna.</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) Radius = (float)v[0];
        }

        /// <summary>
        /// Renderiza el trébol utilizando tres elipses posicionadas simétricamente.
        /// </summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g); // Configuración de suavizado

            // Aplicación de escala y cálculo de desplazamiento entre lóbulos
            float r = Radius * Scale;
            float off = r * 0.55f;

            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                // Definición de las áreas para los tres círculos (superior, izquierdo y derecho)
                var top = new RectangleF(Center.X - r, Center.Y - r - off, r * 2f, r * 2f);
                var left = new RectangleF(Center.X - r - off, Center.Y + r * 0.1f, r * 2f, r * 2f);
                var right = new RectangleF(Center.X - r + off, Center.Y + r * 0.1f, r * 2f, r * 2f);

                // Dibujo de los rellenos
                g.FillEllipse(f, top);
                g.FillEllipse(f, left);
                g.FillEllipse(f, right);

                // Dibujo de los contornos
                g.DrawEllipse(p, top);
                g.DrawEllipse(p, left);
                g.DrawEllipse(p, right);
            }

            // Dibuja la etiqueta informativa ajustada al tamaño total de la figura
            DrawLabel(g, Radius * Scale + off + 5f);
        }
    }
}