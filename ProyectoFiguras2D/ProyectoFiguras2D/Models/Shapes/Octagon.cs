using System;
using System.Drawing;

namespace ProyectoFiguras2D.Models.Shapes
{
    // Octágono — 8 lados
    public class Octagon : RegularPolygon
    {
        public Octagon(PointF c, float r, Color color, float scale = 1f)
            : base(c, r, 8, "Octagon", color, scale, -Math.PI / 8) { }
    }
}