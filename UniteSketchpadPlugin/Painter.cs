using System.Collections.Generic;
using System.Drawing;
using Point = System.Drawing.Point;

namespace UniteSketchpadPlugin
{
    internal class Painter
    {
        private Image image;
        private Image imageCopy;
        private Graphics graphics;

        private Shape activeShape;

        internal Painter(int imgWidth, int imgHeight)
        {
            image = new Bitmap(imgWidth, imgHeight);
            graphics = Graphics.FromImage(image);
        }

        internal Image GetCanvas() => image;

        internal Image DrawStroke(Point start, Point finish, int radius, Color color)
        {
            Pen strokePen = new Pen(new SolidBrush(color), radius);
            strokePen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            graphics.DrawLine(strokePen, start, finish);

            return image;
        }

        // TODO: Resizing needs TOTAL rework, different anchor points will have different functionalities
        internal void ResizeShape(Point final)
        {
            activeShape.Resize(final);
        }

        internal Image RepositionShape(int dx, int dy)
        {
            // Reset image every time shape is edited
            image = (Image)imageCopy.Clone();
            graphics = Graphics.FromImage(image);

            activeShape.Reposition(dx, dy);

            graphics.DrawImage(activeShape.GetInProgressImage(), 0, 0, image.Width, image.Height);

            return image;
        }

        internal Image DrawShape()
        {
            if (activeShape == null) return GetCanvas();

            // Reset image every time shape is edited
            image = (Image)imageCopy.Clone();
            graphics = Graphics.FromImage(image);

            graphics.DrawImage(activeShape.GetImage(), 0, 0, image.Width, image.Height);
            return image;
        }

        internal Image DrawShapeFinal()
        {
            if (activeShape == null) return GetCanvas();

            // Reset image every time shape is edited
            image = (Image)imageCopy.Clone();
            graphics = Graphics.FromImage(image);

            graphics.DrawImage(activeShape.GetImage(), 0, 0, image.Width, image.Height);
            activeShape = null;

            return image;
        }

        internal bool activeShapeContains(Point point) => activeShape.Contains(point);

        internal Image DrawShape(Shape.Type shape, Point initial, Point final, Color color, bool inShapeEdit = false, bool finalized = false)
        {
            if (activeShape == null)
            {
                imageCopy = (Image)image.Clone();
                activeShape = Shape.GetInstanceOf(shape, initial, final, color, image.Width, image.Height);
            }
            else
            {
                // Reset image every time shape is edited
                image = (Image)imageCopy.Clone();
                graphics = Graphics.FromImage(image);

                activeShape.Resize(final);
            }
            
            if (!inShapeEdit)
            {
                graphics.DrawImage(activeShape.GetImage(), 0, 0, image.Width, image.Height);

                if (finalized) activeShape = null;
            }

            else graphics.DrawImage(activeShape.GetInProgressImage(), 0, 0, image.Width, image.Height);

            return image;
        }

        // Sets the color of all adjacent pixels of the same color of targetPixel to replacementColor.
        internal Image Fill(Point targetPixel, Color replacementColor)
        {
            Bitmap bitmap = (Bitmap)image;
            Color targetColor = bitmap.GetPixel(targetPixel.X, targetPixel.Y);

            Stack<Point> pixels = new Stack<Point>();
            pixels.Push(targetPixel);

            while (pixels.Count > 0)
            {
                Point pixel = pixels.Pop();

                if (pixel.X < bitmap.Width && pixel.X > 0 &&
                    pixel.Y < bitmap.Height && pixel.Y > 0)
                {
                    if (bitmap.GetPixel(pixel.X, pixel.Y) == targetColor)
                    {
                        bitmap.SetPixel(pixel.X, pixel.Y, replacementColor);

                        pixels.Push(new Point(pixel.X - 1, pixel.Y));
                        pixels.Push(new Point(pixel.X + 1, pixel.Y));
                        pixels.Push(new Point(pixel.X, pixel.Y - 1));
                        pixels.Push(new Point(pixel.X, pixel.Y + 1));
                    }
                }
            }

            image = bitmap;
            graphics = Graphics.FromImage(image);

            return image;
        }
    }
}