using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using practice_opengl_analogue_csharp;

namespace Extensions {
    public static class BitmapExtensions {
        public static BitmapImage BitmapImage(this Bitmap bitmap) {
            using (var memory = new MemoryStream()) {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    
        public static Bitmap DrawLine(this Bitmap bitmap, Vector2 p0, Vector2 p1,Color color) {
            var delta = p0 - p1;
            var steep = Math.Abs(delta.X) < Math.Abs(delta.Y);

            if (steep) {
                p0 = new Vector2(p0.Y, p0.X);
                p1 = new Vector2(p1.Y, p1.X);
            }

            if (p0.X > p1.X) {
                var temp = p1;
                p1 = p0;
                p0 = temp;
            }

            var x0 = (int) p0.X;
            var x1 = (int) p1.X;
            var y0 = (int) p0.Y;
            var y1 = (int) p1.Y;

            for (var x = x0; x < x1; x++) {
                var t = (float) (x - x0) / (x1 - x0);
                var y = (int) (y0 * (1 - t) + y1 * t);
                var yy = steep ? x : y;
                var xx = steep ? y : x;
                bitmap.SetPixel(xx, yy, color);
            }

            return bitmap;
        }
    }
}