using System.Drawing;

namespace UniteSketchpadPlugin
{
    /// <summary>
    /// Allows for the painting of a transparent image of a defined width and height.
    /// </summary>
    internal class Painter
    {
        private readonly Image image;
        private readonly Graphics graphics;

        internal Painter(int imgWidth, int imgHeight)
        {
            image = new Bitmap(imgWidth, imgHeight);
            graphics = Graphics.FromImage(image);
        }

        // TODO: Stroke type, transparency (baked into color???)
        internal Image DrawStroke(int x, int y, int radius, Color color)
        {
            graphics.DrawEllipse(new Pen(color), x, y, radius, radius);

            return image;
        }

        // TODO: Handle resizing, movement
        internal Image DrawShape(int x, int y, int width, int height, Shape shape, Color color)
        {
            if (shape == Shape.Rectangle)
            {
                graphics.DrawRectangle(new Pen(color), x, y, width, height);
            }

            else if (shape == Shape.Triangle)
            {
                Point[] points = new Point[3];

                points[0] = new Point(x + width / 2, y);
                points[1] = new Point(x, y + height);
                points[2] = new Point(x + width, y + height);

                graphics.DrawPolygon(new Pen(color), points);
            }

            else if (shape == Shape.Ellipse)
            {
                graphics.DrawEllipse(new Pen(color), x, y, width, height);
            }

            return image;
        }

        // TODO: Add functionality - queue system
        internal Image Fill(int x, int y, Color color)
        {
            return image;
        }

        internal enum Shape
        {
            Rectangle,
            Triangle,
            Ellipse
        }
    }
}