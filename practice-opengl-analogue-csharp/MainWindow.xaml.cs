using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
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
        private Bitmap _bitmap;

        public MainWindow() {
            InitializeComponent();
            _image = RenderImage;
            RenderOptions.SetBitmapScalingMode(RenderImage, BitmapScalingMode.HighQuality);
        }

        private void DrawDots() {
            for (var i = 0; i < 50; i++) _bitmap.SetPixel(_random.Next(Size), _random.Next(Size), RandomColor());
        }

        private Color RandomColor() {
            var color = Color.FromArgb(_random.Next((int) Math.Pow(2, 32) - 1));
            return color;
        }

        private void DrawLines() {
            _bitmap = _bitmap.DrawLine(new Vector2(30, 18), new Vector2(134, 428), Color.Green);
            _bitmap = _bitmap.DrawLine(new Vector2(134 + 5, 428), new Vector2(30 + 5, 18), Color.Red);
            for (var i = 0; i < 50; i++)
                _bitmap = _bitmap.DrawLine(new Vector2(_random.Next(Size), _random.Next(Size)),
                    new Vector2(_random.Next(Size), _random.Next(Size)), RandomColor());
        }

        private void MakeTexture(object sender, RoutedEventArgs e) {
            _bitmap = new Bitmap(Size, Size, PixelFormat.Format24bppRgb);

            Enum.TryParse(((Button) sender).CommandParameter.ToString(), out ActionType t);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            switch (t) {
                case ActionType.None:
                    break;
                case ActionType.DrawDot:
                    DrawDots();
                    break;
                case ActionType.DrawLines:
                    DrawLines();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            TextMsBlock.Text = $"Made in {1000f * stopwatch.ElapsedTicks / Stopwatch.Frequency} ms";
            stopwatch.Stop();

            _image.Source = _bitmap.BitmapImage();
        }
    }

    public enum ActionType {
        None,
        DrawDot,
        DrawLines
    }
}