using System;
using System.Drawing;
using ProyectoFiguras2D.Models; // Referencia necesaria para la herencia

namespace ProyectoFiguras2D.Models.Shapes
{
    // ══════════════════════════════════════════════════════
    //  10. TRAPECIO
    //  Parámetros: Base mayor (B), Base menor (b), Altura (h)
    //  Área = ((B+b)·h)/2
    //  Perímetro = B + b + 2·lado_oblicuo
    // ══════════════════════════════════════════════════════

    /// <summary>
    /// Clase que representa un Trapecio isósceles.
    /// Implementa Herencia y validación de datos para evitar dimensiones inválidas.
    /// </summary>
    public class Trapeze : Shape
    {
        private float _topW;
        private float _botW;
        private float _h;

        // Restricción: La base menor debe ser positiva
        public float TopW
        {
            get => _topW;
            set => _topW = value > 0 ? value : 50;
        }

        // Restricción: La base mayor debe ser positiva
        public float BotW
        {
            get => _botW;
            set => _botW = value > 0 ? value : 85;
        }

        // Restricción: La altura debe ser positiva
        public float H
        {
            get => _h;
            set => _h = value > 0 ? value : 40;
        }

        /// <summary>
        /// Constructor de la clase Trapeze.
        /// </summary>
        public Trapeze(PointF c, float topW, float botW, float h, Color color, float scale = 1f)
            : base("Trapeze", color, c, scale)
        {
            TopW = topW;
            BotW = botW;
            H = h;
        }

        // --- Polimorfismo: Implementación de cálculos ---

        /// <summary>Calcula el área del trapecio: ((BaseMayor + BaseMenor) * Altura) / 2</summary>
        public override double Area() => ((TopW + BotW) * H) / 2.0;

        /// <summary>
        /// Calcula el perímetro sumando las bases y los dos lados oblicuos (Pitágoras).
        /// </summary>
        public override double Perimeter()
        {
            // Lado oblicuo con Pitágoras: offset horizontal = (BotW-TopW)/2
            double lateralH = H;
            double lateralX = Math.Abs(BotW - TopW) / 2.0;
            double leg = Math.Sqrt(lateralX * lateralX + lateralH * lateralH);
            return TopW + BotW + 2 * leg;
        }

        /// <summary>Define los parámetros editables en la interfaz de usuario</summary>
        public override (string, string, double)[] GetParams() =>
            new[] {
                ("Base mayor", "px", (double)BotW),
                ("Base menor", "px", (double)TopW),
                ("Altura", "px", (double)H)
            };

        /// <summary>Actualiza los parámetros desde la vista con validación interna</summary>
        public override void SetParams(double[] v)
        {
            if (v.Length > 0) BotW = (float)v[0];
            if (v.Length > 1) TopW = (float)v[1];
            if (v.Length > 2) H = (float)v[2];
        }

        /// <summary>
        /// Renderiza el trapecio en el lienzo utilizando GDI+.
        /// </summary>
        public override void Draw(Graphics g)
        {
            // Configuración de suavizado (Anti-alias)
            SetupGraphics(g);

            // Aplicación de escala proporcional a las dimensiones
            float tw = TopW * Scale / 2f;
            float bw = BotW * Scale / 2f;
            float h = H * Scale / 2f;

            // Definición de los 4 vértices para formar el polígono centrado
            PointF[] pts = {
                new PointF(Center.X - tw, Center.Y - h), // Esquina superior izquierda
                new PointF(Center.X + tw, Center.Y - h), // Esquina superior derecha
                new PointF(Center.X + bw, Center.Y + h), // Esquina inferior derecha
                new PointF(Center.X - bw, Center.Y + h)  // Esquina inferior izquierda
            };

            // Gestión de pinceles y lápices con bloques using
            using (SolidBrush f = CreateFillBrush())
            using (Pen p = CreatePen())
            {
                g.FillPolygon(f, pts); // Relleno de la forma
                g.DrawPolygon(p, pts); // Contorno de la forma
            }

            // Dibuja la etiqueta con nombre y cálculos debajo de la figura
            DrawLabel(g, h + 5f);
        }
    }
}