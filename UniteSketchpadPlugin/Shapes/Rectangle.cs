using System.Drawing;
using Color = System.Drawing.Color;

namespace UniteSketchpadPlugin
{
    internal class Rectangle : Shape
    {
        internal Rectangle(Point initial, Point final, Color color, int imgWidth, int imgHeight) 
            : base(initial, final, color, imgWidth, imgHeight) { }

        protected override void DrawShape(Graphics graphics)
        {
            graphics.DrawRectangle(new Pen(color), x, y, width, height);
        }
    }
}