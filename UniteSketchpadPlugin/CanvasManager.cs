using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitePluginTest.Views;
using UniteSketchpadPlugin;
using System.Drawing;

namespace UnitePluginTest
{
    /// <summary>
    /// Enables user interfacing with the Painter class.
    /// </summary>
    internal class CanvasManager
    {
        private readonly Painter painter;

        // Current Mode
        private Mode mode = Mode.Pen;

        // Settings
        private int radius = 50; // TODO: Rework into Pen instead ???
        private Color color = Color.Red;
        private Shape.Type shape = Shape.Type.Rectangle;

        // Points
        private Point initial;
        private Point current;

        internal CanvasManager(int imgWidth, int imgHeight)
        {
            painter = new Painter(imgWidth,imgHeight);
        }

        internal Image SetMode(Mode mode)
        {
            Mode previousMode = this.mode;
            this.mode = mode;

            // Finalize shape on mode change
            if (previousMode == Mode.ShapeEdit)
            {
                return painter.DrawShape(this.shape, this.initial, this.current, this.color, true);
            }

            return painter.GetCanvas();
        }

        internal void SetShape(Shape.Type shape)
        {
            this.shape = shape;
        }

        internal void SetColor(Color color)
        {
            this.color = color;
        }

        internal Image OnPress(Point point)
        {
            initial = point;
            current = point;

            if (this.mode == Mode.Pen)
            {
                return painter.DrawStroke(this.current, this.radius, this.color);
            }

            else if (this.mode == Mode.Shape)
            {
                return painter.DrawShape(this.shape, this.initial, this.current, this.color);
            }

            else if (this.mode == Mode.Fill)
            {
                return painter.Fill(this.current, this.color);
            }

            return painter.GetCanvas();
        }

        internal Image OnPressMove(Point point)
        {
            current = point;

            if (this.mode == Mode.Pen)
            {
                return painter.DrawStroke(this.current, this.radius, this.color);
            }

            else if (this.mode == Mode.Shape)
            {
                return painter.DrawShape(this.shape, this.initial, this.current, this.color);
            }

            else if (this.mode == Mode.ShapeEdit)
            {
                // If inside shape
                painter.RepositionShape(current);

                // If on border of shape
                // TODO

                return painter.DrawShape(this.shape, this.initial, this.current, this.color);
            }

            return painter.GetCanvas();
        }

        internal Image OnRelease(Point point)
        {
            current = point;

            if (this.mode == Mode.Shape)
            {
                this.mode = Mode.ShapeEdit;
                return painter.DrawShape(this.shape, this.initial, this.current, this.color);
            }

            else
            {
                return painter.GetCanvas();
            }
        }

        internal enum Mode
        {
            Pen,
            Shape,
            ShapeEdit,
            Fill
        }
    }
}
