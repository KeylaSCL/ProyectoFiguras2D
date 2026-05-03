using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using ProyectoFiguras2D.Models; // Referencia necesaria para la herencia

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  12. CRECIENTE (luna)
    //  Parámetros: Radio exterior (R), Radio interior (r)
    //  Área ≈ π(R² - r²)/2   (media diferencia de círculos)
    //  Perímetro ≈ π(R + r)
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa una figura de Luna Creciente.
    /// Implementa Herencia y Encapsulamiento con validación de radios.
    /// </summary>
    public class Crescent : Shape
    {
        private float _outerR;
        private float _innerR;

        // Restricción: El radio exterior debe ser mayor a 0
        public float OuterR
        {
            get => _outerR;
            set => _outerR = value > 0 ? value : 38;
        }

        // Restricción: El radio interior debe ser positivo y menor al exterior para formar la curva
        public float InnerR
        {
            get => _innerR;
            set => _innerR = (value > 0 && value < OuterR) ? value : OuterR * 0.78f;
        }

        /// <summary>
        /// Constructor de la clase Crescent.
        /// </summary>
        public Crescent(PointF c, float outer, Color color, float scale = 1f)
            : base("Crescent", color, c, scale) // Envío de datos al constructor base
        {
            OuterR = outer;
            InnerR = outer * 0.78f;
        }

        // --- Polimorfismo: Implementación de cálculos geométricos ---

        /// <summary>Calcula el área aproximada de la media diferencia de círculos.</summary>
        public override double Area() => Math.PI * (OuterR * OuterR - InnerR * InnerR) / 2.0;

        /// <summary>Calcula el perímetro aproximado basado en la suma de las circunferencias.</summary>
        public override double Perimeter() => Math.PI * (OuterR + InnerR);

        /// <summary>Define los parámetros editables en la interfaz de usuario.</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Radio ext.", "px", (double)OuterR), ("Radio int.", "px", (double)InnerR) };

        /// <summary>Actualiza los radios desde la vista con validación interna.</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) OuterR = (float)v[0];
            if (v.Length > 1) InnerR = (float)v[1];
        }

        /// <summary>
        /// Renderiza la luna utilizando operaciones de región (Exclusión de elipses) con GDI+.
        /// </summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g); // Configuración de suavizado

            float r = OuterR * Scale;
            float ri = InnerR * Scale;

            // Uso de GraphicsPath y Region para crear la forma de medialuna
            using (GraphicsPath outerP = new GraphicsPath())
            using (GraphicsPath innerP = new GraphicsPath())
            {
                // Elipse exterior base
                outerP.AddEllipse(Center.X - r, Center.Y - r, r * 2f, r * 2f);

                // Elipse interior desplazada para "recortar" la luna
                innerP.AddEllipse(Center.X - ri * 0.25f, Center.Y - ri * 1.09f, ri * 1.6f, ri * 1.6f);

                using (Region reg = new Region(outerP))
                {
                    reg.Exclude(innerP); // Operación booleana de resta de áreas
                    using (SolidBrush f = CreateFillBrush()) g.FillRegion(f, reg);
                }

                // Dibujo de los contornos para definición visual
                using (Pen p = CreatePen())
                {
                    g.DrawPath(p, outerP);
                    g.DrawPath(p, innerP);
                }
            }

            DrawLabel(g, r + 5f); // Etiqueta dinámica
        }
    }
}