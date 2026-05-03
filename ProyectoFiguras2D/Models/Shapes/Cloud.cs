using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia necesaria para la herencia

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  20. NUBE
    //  Parámetro: Tamaño (s)
    //  Área ≈ π·(s/2)² · 3   (tres círculos aprox)
    //  Perímetro ≈ 5·π·(s/3)
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa una figura de Nube.
    /// Implementa Herencia y Encapsulamiento con validación de dimensiones.
    /// </summary>
    public class Cloud : Shape
    {
        private float _size;

        // Restricción: El tamaño debe ser mayor a 0 para que la nube sea visible
        public float Size
        {
            get => _size;
            set => _size = value > 0 ? value : 70;
        }

        /// <summary>
        /// Constructor de la clase Cloud.
        /// </summary>
        public Cloud(PointF c, float size, Color color, float scale = 1f)
            : base("Cloud", color, c, scale) // Envío de datos al constructor base
        {
            Size = size;
        }

        // --- Polimorfismo: Implementación de cálculos geométricos aproximados ---

        /// <summary>Calcula el área aproximada basada en la suma de los círculos componentes.</summary>
        public override double Area() => 3 * Math.PI * Math.Pow(Size / 3.0, 2);

        /// <summary>Calcula el perímetro aproximado de los bordes externos.</summary>
        public override double Perimeter() => 5 * Math.PI * (Size / 3.0);

        /// <summary>Define los parámetros editables en la interfaz de usuario.</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Tamaño", "px", (double)Size) };

        /// <summary>Actualiza el tamaño desde la vista con validación interna.</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) Size = (float)v[0];
        }

        /// <summary>
        /// Renderiza la nube utilizando múltiples elipses superpuestas para crear el efecto orgánico.
        /// </summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g); // Configuración de suavizado (Anti-alias)

            float s = Size * Scale;

            // Definición de las 5 partes (elipses) que conforman la nube de forma asimétrica
            RectangleF[] parts = {
                new RectangleF(Center.X - s * 0.5f,  Center.Y - s * 0.10f, s * 1.0f,  s * 0.65f), // Base
                new RectangleF(Center.X - s * 0.55f, Center.Y - s * 0.35f, s * 0.50f, s * 0.50f), // Lóbulo izquierdo
                new RectangleF(Center.X - s * 0.20f, Center.Y - s * 0.55f, s * 0.60f, s * 0.55f), // Lóbulo central superior
                new RectangleF(Center.X + s * 0.05f, Center.Y - s * 0.38f, s * 0.45f, s * 0.48f), // Lóbulo derecho superior
                new RectangleF(Center.X + s * 0.20f, Center.Y - s * 0.18f, s * 0.35f, s * 0.40f)  // Lóbulo derecho bajo
            };

            // Gestión de recursos mediante bloques 'using' para optimizar memoria GDI+
            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                // Primero rellenamos todas las partes para que parezca una sola masa
                foreach (var r in parts) g.FillEllipse(f, r);

                // Luego dibujamos los contornos para dar definición
                foreach (var r in parts) g.DrawEllipse(p, r);
            }

            DrawLabel(g, s * 0.55f + 5f); // Etiqueta dinámica
        }
    }
}