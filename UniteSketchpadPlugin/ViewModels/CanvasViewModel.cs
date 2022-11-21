using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Intel.Unite.Common.Display;
using UnitePlugin.HubViewModel;
using UnitePluginTest.Utility;
using UnitePluginTest.EventArg;

namespace UnitePluginTest.ViewModels
{
    [Serializable]
    internal class CanvasViewModel : HubViewModel
    {
        private WriteableBitmap screenShot;

        internal WriteableBitmap ScreenShot
        {
            get => screenShot;
            set
            {
                if (screenShot == value) return;
                screenShot = value;
                NotifyPropertyChanged();
            }
        }

        internal GetScreenShot ScreenShotDelegate { get; set; }

        private ICommand updateScreenShot_ClickCommand;
        internal ICommand UpdateScreenShotButton
        {
            get
            {
                return updateScreenShot_ClickCommand ?? (updateScreenShot_ClickCommand =
                    new RelayCommand<HubViewEventArgs>(
                        x =>
                        {
                            ScreenShot =
                                Extensions.GetWritableBitmap(
                                    Extensions.GetBitmapFromBytes(ScreenShotDelegate?.Invoke(ControlIdentifier)));
                        }));
            }
        }
    }
}
