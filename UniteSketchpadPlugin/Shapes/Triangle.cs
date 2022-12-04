using System.Drawing;
using Color = System.Drawing.Color;

namespace UniteSketchpadPlugin
{
    internal class Triangle : Shape
    {
        internal Triangle(Point initial, Point final, Color color, int imgWidth, int imgHeight)
            : base(initial, final, color, imgWidth, imgHeight) { }

        protected override void DrawShape(Graphics graphics)
        {
            Point[] points = new Point[3];

            points[0] = new Point(x + width / 2, y);
            points[1] = new Point(x, y + height);
            points[2] = new Point(x + width, y + height);

            graphics.DrawPolygon(new Pen(new SolidBrush(color), 5), points);
        }
    }
}