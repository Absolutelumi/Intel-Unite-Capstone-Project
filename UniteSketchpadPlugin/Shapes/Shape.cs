using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace UniteSketchpadPlugin
{
    internal abstract class Shape
    {
        protected int imgWidth;
        protected int imgHeight;

        protected int x;
        protected int y;

        protected int width;
        protected int height;

        protected Color color;

        internal Shape(Point initial, Point final, Color color, int imgWidth, int imgHeight)
        {
            this.imgWidth = imgWidth;
            this.imgHeight = imgHeight;

            this.x = initial.X;
            this.y = initial.Y;

            this.width = final.X - initial.X;
            this.height = final.Y - initial.Y;

            this.color = color;
        }

        protected abstract void DrawShape(Graphics graphics);

        internal void Reposition(Point initial)
        {
            this.x = initial.X;
            this.y = initial.Y;
        }

        internal void Resize(Point final)
        {
            this.width = final.X - this.x;
            this.height = final.Y - this.y;
        }

        // TODO: Refactor - Should not be calling DrawShape() here
        // Unfortunate, but probably will need to have an angle variable thats simply set here and used later
        internal void Rotate(int angle)
        {
            /*
            using (Matrix matrix = new Matrix())
            {
                // Rotate image by angle
                matrix.RotateAt(angle, new PointF(x + (width / 2), y + (height / 2)));
                graphics.Transform = matrix;

                // Draw shape while image is rotated
                DrawShape();

                // Reset the image rotation
                graphics.ResetTransform();
            }
            */
        }

        internal Image GetInProgressImage()
        {
            Image image = new Bitmap(imgWidth, imgHeight);

            using (var graphics = Graphics.FromImage(image))
            {
                // TODO: Set these to make the line look right
                Pen blackPen = new Pen(Color.Black, 5);
                blackPen.DashPattern = new float[] { 5, 5, 5, 5 };

                // Draw shape first, so line goes over the shape
                DrawShape(graphics);

                // Draw dotted outline around shape
                graphics.DrawLine(blackPen, x, y, x + width, y);
                graphics.DrawLine(blackPen, x, y + height, x + width, y + height);
                graphics.DrawLine(blackPen, x, y, x, y + height);
                graphics.DrawLine(blackPen, x + width, y, x + width, y + height);
            }

            return image;
        }

        internal Image GetImage()
        {
            // Disposal of the bitmap will be the caller's responsibility
            // TODO: Refactor ??
            Image image = new Bitmap(imgWidth, imgHeight);

            using (var graphics = Graphics.FromImage(image))
            {
                DrawShape(graphics);
            }

            return image;
        }

        internal enum Type
        {
            Ellipse,
            Triangle,
            Rectangle
        }

        // TODO: Refactor ?? Not sure how else to do this without being excessively verbose here or elsewhere
        internal static Shape GetInstanceOf(Shape.Type type, Point initial, Point final, Color color, int imgWidth, int imgHeight)
        {
            switch (type)
            {
                case Type.Ellipse:
                    return new Ellipse(initial, final, color, imgWidth, imgHeight);
                case Type.Triangle:
                    return new Triangle(initial, final, color, imgWidth, imgHeight);
                case Type.Rectangle:
                    return new Rectangle(initial, final, color, imgWidth, imgHeight);
                default:
                    return null;
            }
        }
    }
}