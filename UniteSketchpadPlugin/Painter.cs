using System.Collections.Generic;
using System.Drawing;
using Point = System.Drawing.Point;

namespace UniteSketchpadPlugin
{
    /// <summary>
    /// Allows for the painting of a transparent canvas of a defined width and height.
    /// </summary>
    internal class Painter // TODO: Refactor Painter and Shape such that both are IDisposable ???
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

        // TODO: Make more consistent - currently dotted
        internal Image DrawStroke(Point start, Point finish, int radius, Color color)
        {
            //graphics.FillEllipse(new SolidBrush(color), point.X, point.Y, radius, radius);
            graphics.DrawLine(new Pen(color), start, finish);

            return image;
        }

        // TODO: Resizing needs TOTAL rework, different anchor points will have different functionalities
        internal void ResizeShape(Point final)
        {
            activeShape.Resize(final);
        }

        internal void RepositionShape(Point initial)
        {
            activeShape.Reposition(initial);
        }

        // TODO: Handle resizing, movement
        // TODO ERROR: Is just constantly redrawing the shape on the canvas - To fix, will need 'baseImage' and 'shapeImage' or such
        internal Image DrawShape(Shape.Type shape, Point initial, Point final, Color color, bool finalized = false)
        {
            if (activeShape == null)
            {
                imageCopy = image;
                activeShape = Shape.GetInstanceOf(shape, initial, final, color, image.Width, image.Height);
            }

            // Reset image every time shape is edited
            image = imageCopy;
            graphics = Graphics.FromImage(image);
            
            if (finalized)
            {
                graphics.DrawImage(activeShape.GetImage(), 0, 0, image.Width, image.Height);

                activeShape = null;
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