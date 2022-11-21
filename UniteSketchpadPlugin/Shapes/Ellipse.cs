using System.Drawing;
using Color = System.Drawing.Color;

namespace UniteSketchpadPlugin
{
    internal class Ellipse : Shape
    {
        internal Ellipse(Point initial, Point final, Color color, int imgWidth, int imgHeight)
            : base(initial, final, color, imgWidth, imgHeight) { }

        protected override void DrawShape(Graphics graphics)
        {
            graphics.DrawEllipse(new Pen(color), x, y, width, height);
        }
    }
}
