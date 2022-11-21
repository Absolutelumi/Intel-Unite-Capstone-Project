using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace UnitePluginTest.Views
{
    /// <summary>
    /// Interaction logic for CanvasView.xaml
    /// </summary>
    public partial class CanvasView : UserControl
    {
        private readonly Action<Point> onPress;
        private readonly Action<Point> onPressMove;
        private readonly Action<Point> onPressRelease;

        public CanvasView(Action<Point> onPress, Action<Point> onPressMove, Action<Point> onPressRelease)
        {
            InitializeComponent();

            this.onPress = onPress;
            this.onPressMove = onPressMove;
            this.onPressRelease = onPressRelease;
        }

        public void UpdateCanvas(ImageSource image)
        {
            this.Canvas.Source = image;
        }

        private void Canvas_Press(object sender, EventArgs e)
        {
            // TODO: Get point from canvas ???
            Point point = new Point();

            onPress(point);
        }

        private void Canvas_PressMove(object sender, EventArgs e)
        {
            Point point = new Point();

            onPressMove(point);
        }

        private void Canvas_PressRelease(object sender, EventArgs e)
        {
            Point point = new Point();

            onPressRelease(point);
        }
    }
}
