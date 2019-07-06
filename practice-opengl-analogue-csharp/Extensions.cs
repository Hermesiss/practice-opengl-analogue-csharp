using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Media.Imaging;
using JeremyAnsel.Media.WavefrontObj;
using practice_opengl_analogue_csharp;

namespace Extensions {
    public static class ObjFileExtensions {
        public static Vector3 ToVector3(this ObjVector4 objVector4) {
            return new Vector3(objVector4.X, objVector4.Y, objVector4.Z);
        }

        public static Bounds3D Bounds(this ObjFile objFile) {
            var vertices = objFile.Vertices;
            var minX = vertices.Min(x => x.Position.X);
            var maxX = vertices.Max(x => x.Position.X);
            var minY = vertices.Min(x => x.Position.Y);
            var maxY = vertices.Max(x => x.Position.Y);
            var minZ = vertices.Min(x => x.Position.Z);
            var maxZ = vertices.Max(x => x.Position.Z);
            var size = new Vector3(maxX - minX, maxY - minY, maxZ - minZ);
            var center = new Vector3(maxX + minX, maxY + minY, maxZ + minZ) / 2;
            return new Bounds3D(center, size);
        }
    }

    public static class VectorExtensions {
        public static Vector2 Clamp(this Vector2 vector2, Vector2 min, Vector2 max) {
            var x = Utils.Clamp(min.X, max.X, vector2.X);
            var y = Utils.Clamp(min.Y, max.Y, vector2.Y);
            return new Vector2(x, y);
        }
    }

    public static class BitmapExtensions {
        public static BitmapImage BitmapImage(this Bitmap bitmap) {
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
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

        public static Bitmap DrawTriangle(this Bitmap bitmap, Vector2[] points, Color[] colors) {
            var pLength = points.Length;
            if (pLength != 3) throw new ArgumentException($"Passed {pLength} points instead of 3");
            for (var i = 0; i < pLength; i++) {
                bitmap = bitmap.DrawLine(points[i], points[(i + 1) % pLength], colors[i % colors.Length]);
            }

            return bitmap;
        }

        public static Bitmap DrawLine(this Bitmap bitmap, Vector2 p0, Vector2 p1, Color color) {
            var min = Vector2.Zero;
            var max = new Vector2(bitmap.Width - 1, bitmap.Height - 1);
            //p0 = p0.Clamp(min, max);
            //p1 = p1.Clamp(min, max);
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

            var dx = x1 - x0;
            var dy = y1 - y0;
            var dError2 = Math.Abs(dy) * 2;
            var error2 = 0;
            var y = y0;

            for (var x = x0; x <= x1; x++) {
                var yy = steep ? x : y;
                var xx = steep ? y : x;
                if (xx < bitmap.Width-1 && yy < bitmap.Height-1)
                    bitmap.SetPixel(xx, yy, color);

                error2 += dError2;
                if (error2 > dx) {
                    y += y1 > y0 ? 1 : -1;
                    error2 -= dx * 2;
                }
            }

            return bitmap;
        }
    }
}