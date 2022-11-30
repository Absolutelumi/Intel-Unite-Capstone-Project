using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace UniteSketchpadPlugin.Views
{
    public partial class CanvasControlsView : UserControl
    {
        private CanvasManager.Settings settings;

        private readonly Action<CanvasManager.Settings> onSettingsUpdate;

        public CanvasControlsView(CanvasManager.Settings settings, Action<CanvasManager.Settings> onSettingsUpdate)
        {
            InitializeComponent();

            this.settings = settings;
            this.onSettingsUpdate = onSettingsUpdate;
        }

        private void OnRedClick(object sender, RoutedEventArgs e) 
        {
            settings.color = Color.Red;

            onSettingsUpdate(settings);
        }

        private void OnGreenClick(object sender, RoutedEventArgs e)
        {
            settings.color = Color.Green;

            onSettingsUpdate(settings);
        }

        private void OnBlueClick(object sender, RoutedEventArgs e)
        {
            settings.color = Color.Blue;

            onSettingsUpdate(settings);
        }

        private void OnBlackClick(object sender, RoutedEventArgs e)
        {
            settings.color = Color.Black;

            onSettingsUpdate(settings);
        }

        private void OnPenClick(object sender, RoutedEventArgs e)
        {
            settings.mode = CanvasManager.Mode.Pen;

            onSettingsUpdate(settings);
        }

        private void OnFillClick(object sender, RoutedEventArgs e)
        {
            settings.mode = CanvasManager.Mode.Fill;

            onSettingsUpdate(settings);
        }

        private void OnEllipseClick(object sender, RoutedEventArgs e)
        {
            settings.shape = Shape.Type.Ellipse;
            settings.mode = CanvasManager.Mode.Shape;

            onSettingsUpdate(settings);
        }

        private void OnTriangleClick(object sender, RoutedEventArgs e)
        {
            settings.shape = Shape.Type.Triangle;
            settings.mode = CanvasManager.Mode.Shape;

            onSettingsUpdate(settings);
        }

        private void OnRectangleClick(object sender, RoutedEventArgs e)
        {
            settings.shape = Shape.Type.Rectangle;
            settings.mode = CanvasManager.Mode.Shape;

            onSettingsUpdate(settings);
        }
    }
}
