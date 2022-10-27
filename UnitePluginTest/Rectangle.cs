using System.Drawing;

namespace UniteSketchpadPlugin
{
    internal class Rectangle : Shape
    {
        internal Rectangle(int x, int y, int width, int height, Color color, int imgWidth, int imgHeight) : base(x, y, width, height, color, imgWidth, imgHeight) { }

        protected override void DrawShape()
        {
            graphics.DrawRectangle(new Pen(color), x, y, width, height);
        }
    }
}
