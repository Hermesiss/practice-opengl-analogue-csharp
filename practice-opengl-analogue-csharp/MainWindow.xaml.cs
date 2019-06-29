using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using Extensions;
using Color = System.Drawing.Color;
using Image = System.Windows.Controls.Image;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace practice_opengl_analogue_csharp {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private const int Size = 512;
        private readonly Image _image;
        private readonly Random _random = new Random();

        public MainWindow() {
            InitializeComponent();
            _image = RenderImage;
            RenderOptions.SetBitmapScalingMode(RenderImage, BitmapScalingMode.HighQuality);
        }

        private void DrawDot() {
            var bitmap = new Bitmap(Size, Size, PixelFormat.Format24bppRgb);
            bitmap.SetPixel(_random.Next(Size), _random.Next(Size), Color.Red);

            _image.Source = bitmap.BitmapImage();
        }

        private void DrawLines() {
            var bitmap = new Bitmap(Size, Size, PixelFormat.Format24bppRgb);
            bitmap = bitmap.DrawLine(new Vector2(30, 18), new Vector2(134, 428), Color.Green);
            bitmap = bitmap.DrawLine(new Vector2(134 + 5, 428), new Vector2(30 + 5, 18), Color.Red);
            bitmap = bitmap.DrawLine(new Vector2(_random.Next(Size), _random.Next(Size)),
                new Vector2(_random.Next(Size), _random.Next(Size)), Color.Blue);
            _image.Source = bitmap.BitmapImage();
        }
        
        private void newTextureBtn_Click(object sender, RoutedEventArgs e) {
            DrawDot();
        }

        private void DrawLineBtn_OnClickBtn_Click(object sender, RoutedEventArgs e) {
            DrawLines();
        }
    }
}