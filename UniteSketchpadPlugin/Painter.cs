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
        private readonly Image image;
        private readonly Graphics graphics;

        private Shape activeShape;

        internal Painter(int imgWidth, int imgHeight)
        {
            image = new Bitmap(imgWidth, imgHeight);
            graphics = Graphics.FromImage(image);
        }

        internal Image GetCanvas()
        {
            return image;
        }

        // TODO: Stroke type, transparency (baked into color???)
        internal Image DrawStroke(Point point, int radius, Color color)
        {
            graphics.DrawEllipse(new Pen(color), point.X, point.Y, radius, radius);

            return image;
        }

        // TODO: Refactor ???
        // Resizing needs TOTAL rework, different anchor points will have different functionalities
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
                activeShape = Shape.GetInstanceOf(shape, initial, final, color, image.Width, image.Height);
            }
            
            if (finalized)
            {
                graphics.DrawImage(activeShape.GetImage(), 0, 0, image.Width, image.Height);

                activeShape = null;
            }

            else graphics.DrawImage(activeShape.GetInProgressImage(), 0, 0, image.Width, image.Height);

            return image;
        }

        /// <summary>
        /// Sets the color of all adjacent pixels of the same color of targetPixel to replacementColor.
        /// </summary>
        /// <param name="targetPixel">The pixel to be targeted.</param>
        /// <param name="replacementColor">The color of the fill.</param>
        /// <returns>The canvas image after the fill operation.</returns>
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

            return image;
        }
    }
}