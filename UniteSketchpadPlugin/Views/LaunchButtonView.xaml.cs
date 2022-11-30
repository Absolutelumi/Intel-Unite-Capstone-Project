using System;
using System.Windows;
using System.Windows.Controls;

namespace UniteSketchpadPlugin.Views
{
    /// <summary>
    /// Interaction logic for LaunchButtonView.xaml
    /// </summary>
    public partial class LaunchButtonView : UserControl
    {
        private readonly Action onLaunch;

        public LaunchButtonView(Action onLaunch)
        {
            InitializeComponent();

            this.onLaunch = onLaunch;
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            onLaunch();
        }
    }
}
