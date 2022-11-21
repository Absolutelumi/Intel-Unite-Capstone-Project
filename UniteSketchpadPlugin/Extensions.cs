using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace UnitePluginTest
{
    internal static class Extensions
    {
        internal static Bitmap GetBitmapFromBytes(byte[] bytes)
        {
            if (bytes == null)
                return null;
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                stream.Seek(0, SeekOrigin.Begin);
                return new Bitmap(stream);
            }
        }

        internal static WriteableBitmap GetWritableBitmap(Bitmap bitmap)
        {
            if (bitmap == null) return null;
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var writableBitmap = new WriteableBitmap(bitmapData.Width, bitmapData.Height, 96, 96, PixelFormats.Bgr32, null);
            try
            {
                writableBitmap.WritePixels(new Int32Rect(0, 0, bitmapData.Width, bitmapData.Height), bitmapData.Scan0, bitmapData.Height * bitmapData.Stride, bitmapData.Stride, 0, 0);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
            bitmap.Dispose();
            return writableBitmap;
        }
    }
}
