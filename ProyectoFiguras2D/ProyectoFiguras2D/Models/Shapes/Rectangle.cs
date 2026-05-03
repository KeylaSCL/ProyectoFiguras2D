using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia necesaria para heredar de Shape

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  4. RECTÁNGULO
    //  Parámetros: Ancho (a), Alto (b)
    //  Área = a·b         Perímetro = 2(a+b)
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa un Rectángulo.
    /// Implementa Herencia y validación de datos para robustez del software.
    /// </summary>
    public class Rectangle : Shape
    {
        private float _w;
        private float _h;

        // Restricción: El ancho debe ser mayor a 0
        public float W
        {
            get => _w;
            set => _w = value > 0 ? value : 80;
        }

        // Restricción: El alto debe ser mayor a 0
        public float H
        {
            get => _h;
            set => _h = value > 0 ? value : 50;
        }

        /// <summary>
        /// Constructor de la clase Rect2D.
        /// </summary>
        public Rectangle(PointF c, float w, float h, Color color, float scale = 1f)
            : base("Rectangle", color, c, scale)
        {
            W = w;
            H = h;
        }

        // --- Polimorfismo: Implementación de cálculos geométricos ---

        /// <summary>Calcula el área del rectángulo: Ancho * Alto</summary>
        public override double Area() => W * H;

        /// <summary>Calcula el perímetro del rectángulo: 2 * (Ancho + Alto)</summary>
        public override double Perimeter() => 2 * (W + H);

        /// <summary>Define los parámetros editables en la interfaz de usuario</summary>
        public override (string, string, double)[] GetParams() =>
            new[] { ("Ancho", "px", (double)W), ("Alto", "px", (double)H) };

        /// <summary>Actualiza los parámetros desde la vista con validación interna</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) W = (float)v[0];
            if (v.Length > 1) H = (float)v[1];
        }

        /// <summary>
        /// Renderiza el rectángulo en el lienzo utilizando GDI+.
        /// Uso de Pen, SolidBrush y RectangleF centrado.
        /// </summary>
        public override void Draw(Graphics g)
        {
            SetupGraphics(g);

            // Aplicación del factor de escala a las dimensiones
            float w = W * Scale;
            float h = H * Scale;

            // Definición del área de dibujo centrada respecto al punto Center
            var rect = new RectangleF(Center.X - w / 2f, Center.Y - h / 2f, w, h);

            // Gestión de recursos mediante bloques 'using' para limpieza de memoria
            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                g.FillRectangle(f, rect); // Relleno de la figura
                g.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height); // Contorno
            }

            // Dibuja la etiqueta con el nombre y cálculos debajo de la figura
            DrawLabel(g, h / 2f + 5f);
        }
    }
}