using System.Drawing;

namespace ProyectoFiguras2D.Models.Shapes
{
    // Hexágono — 6 lados
    public class Hexagon : RegularPolygon
    {
        public Hexagon(PointF c, float r, Color color, float scale = 1f)
            : base(c, r, 6, "Hexagon", color, scale) { }
    }
}