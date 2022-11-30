using System;
using System.Drawing;

namespace UniteSketchpadPlugin.EventArg
{
    internal class HubViewEventArgs : EventArgs
    {
        internal Bitmap ScreenShot { get; set; }
    }
}
