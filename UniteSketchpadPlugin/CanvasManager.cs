using System.Drawing;
using System.Windows.Media.Media3D;

namespace UniteSketchpadPlugin
{
    public class CanvasManager
    {
        private readonly Painter painter;

        private Settings settings;

        private Point initial;
        private Point current;

        // Shape Edit Settings
        private bool repositioning = false;
        private bool resizing = false;

        internal CanvasManager(int imgWidth, int imgHeight)
        {
            painter = new Painter(imgWidth,imgHeight);

            settings = new Settings
            {
                mode = Mode.Pen,
                color = Color.Black,
                shape = Shape.Type.Rectangle,
                radius = 5
            };
        }

        internal Image GetCanvas() => painter.GetCanvas();

        internal Settings GetSettings() => settings;

        internal Image SetSettings(Settings settings)
        {
            // If the mode was changed *from* shape edit, the dotted line needs to be removed so an image has to return here
            if (this.settings.mode != settings.mode &&
                this.settings.mode == Mode.ShapeEdit)
            {
                this.settings = settings;
                return painter.DrawShapeFinal();
            }

            this.settings = settings;
            return GetCanvas();
        }

        internal Image OnPress(Point point)
        {
            Point previousInitial = initial;
            Point previousCurrent = current;

            initial = point;
            current = point;

            if (settings.mode == Mode.Pen)
            {
                return painter.DrawStroke(this.initial, this.current, settings.radius, settings.color);
            }

            else if (settings.mode == Mode.Shape)
            {
                return painter.DrawShape(settings.shape, this.initial, this.current, settings.color);
            }

            else if (settings.mode == Mode.ShapeEdit)
            {
                // If inside shape set anchor point and set repositioning mode on
                if (painter.activeShapeContains(initial)) repositioning = true;

                // If on border of shape

                // If point outside of shape, user is trying to make another
                else
                {
                    settings.mode = Mode.Shape;
                    repositioning = false;

                    painter.DrawShapeFinal();
                    return painter.DrawShape(settings.shape, initial, current, settings.color);
                }
            }

            else if (settings.mode == Mode.Fill)
            {
                return painter.Fill(this.current, settings.color);
            }

            return painter.GetCanvas();
        }

        internal Image OnPressMove(Point point)
        {
            Point prevCurrent = current;
            current = point;

            if (settings.mode == Mode.Pen)
            {
                return painter.DrawStroke(prevCurrent, current, settings.radius, settings.color);
            }

            else if (settings.mode == Mode.Shape)
            {
                return painter.DrawShape(settings.shape, this.initial, this.current, settings.color);
            }

            else if (settings.mode == Mode.ShapeEdit)
            {
                if (repositioning)
                {
                    return painter.RepositionShape(current.X - prevCurrent.X, current.Y - prevCurrent.Y);
                }
            }

            return painter.GetCanvas();
        }

        internal Image OnRelease(Point point)
        {
            current = point;

            if (settings.mode == Mode.Shape)
            {
                settings.mode = Mode.ShapeEdit;
                return painter.DrawShape(settings.shape, this.initial, this.current, settings.color, inShapeEdit: true);
            }

            return painter.GetCanvas();
        }

        internal enum Mode
        {
            Pen,
            Shape,
            ShapeEdit,
            Fill
        }

        public struct Settings
        {
            internal Mode mode;
            internal Color color;
            internal Shape.Type shape;
            internal int radius;
        }
    }
}
