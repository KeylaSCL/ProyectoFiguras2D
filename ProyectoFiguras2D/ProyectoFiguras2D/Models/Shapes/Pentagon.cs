using System.Drawing;

namespace ProyectoFiguras2D.Models.Shapes
{
    // Pentágono — 5 lados
    public class Pentagon : RegularPolygon
    {
        public Pentagon(PointF c, float r, Color color, float scale = 1f)
            : base(c, r, 5, "Pentagon", color, scale) { }
    }
}