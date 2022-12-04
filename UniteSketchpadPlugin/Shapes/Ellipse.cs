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
            graphics.DrawEllipse(new Pen(new SolidBrush(color), 5), x, y, width, height);
        }
    }
}
