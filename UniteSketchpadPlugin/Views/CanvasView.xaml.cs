using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Point = System.Drawing.Point;
using Image = System.Drawing.Image;
using System.IO;

namespace UniteSketchpadPlugin.Views
{
    public partial class CanvasView : UserControl
    {
        private const string canvasPath = "C:\\Users\\jacks\\Desktop\\Unite Stuff\\UniteSketchpadPlugin\\UniteSketchpadPlugin\\Images\\Temp\\Canvas.png";

        private readonly Func<Point, Image> onPress;
        private readonly Func<Point, Image> onPressMove;
        private readonly Func<Point, Image> onPressRelease;

        private bool isPressed = false;

        public CanvasView(
            Func<Point, Image> onPress, 
            Func<Point, Image> onPressMove, 
            Func<Point, Image> onPressRelease)
        {
            InitializeComponent();

            this.onPress = onPress;
            this.onPressMove = onPressMove;
            this.onPressRelease = onPressRelease;
        }

        private void Canvas_Press(object sender, EventArgs e)
        {
            Point point = GetCurrentPoint();

            isPressed = true;
            UpdateCanvasImage(onPress(point));
        }

        private void Canvas_PressMove(object sender, EventArgs e)
        {
            if (!isPressed) return;

            Point point = GetCurrentPoint();

            UpdateCanvasImage(onPressMove(point));
        }

        private void Canvas_PressRelease(object sender, EventArgs e)
        {
            Point point = GetCurrentPoint();

            isPressed = false;
            UpdateCanvasImage(onPressRelease(point));
        }

        private Point GetCurrentPoint()
        {
            System.Windows.Point winPoint = Mouse.GetPosition(this.Canvas);
            return new Point((int)winPoint.X, (int)winPoint.Y);
        }

        public void UpdateCanvasImage(Image image, bool isBackground = false)
        {
            for (int failCount = 0; ; failCount++)
            {
                string path = canvasPath.Split('.')[0] + failCount + ".png";

                try
                {
                    File.Delete(path);
                    image.Save(path);

                    if (isBackground) this.CanvasBackground.Source = new BitmapImage(new Uri(path));
                    else this.Canvas.Source = new BitmapImage(new Uri(path));

                    break;
                }
                
                catch (Exception) { }
            }
        }
    }
}
