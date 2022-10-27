using System.Drawing;

namespace UniteSketchpadPlugin
{
    internal abstract class Shape
    {
        protected readonly Image image;
        protected readonly Graphics graphics;

        protected int x;
        protected int y;

        protected int width;
        protected int height;

        protected Color color;

        internal Shape(int x, int y, int width, int height, Color color, int imgWidth, int imgHeight)
        {
            image = new Bitmap(imgWidth, imgHeight);
            graphics = Graphics.FromImage(image);

            this.x = x;
            this.y = y;

            this.width = width;
            this.height = height;

            this.color = color;
        }

        protected abstract void DrawShape();

        internal void Reposition(int x, int y)
        {
            this.x = x;
            this.y = y;

            DrawShape();
        }

        internal void Resize(int width, int height)
        {
            this.width = width;
            this.height = height;

            DrawShape();
        }

        internal Image GetInProgressImage()
        {
            // Make dotted outline on image

            return image;
        }

        internal Image GetImage()
        {
            return image;
        }
    }
}
