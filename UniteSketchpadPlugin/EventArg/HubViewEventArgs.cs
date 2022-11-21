using System;
using System.Drawing;

namespace UnitePluginTest.EventArg
{
    internal class HubViewEventArgs : EventArgs
    {
        internal Bitmap ScreenShot { get; set; }
    }
}
