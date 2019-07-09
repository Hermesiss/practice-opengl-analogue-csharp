using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Extensions;
using JeremyAnsel.Media.WavefrontObj;
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
        private List<String> _stages;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public MainWindow() {
            ConsoleAllocator.ShowConsoleWindow();
            InitializeComponent();
            _image = RenderImage;
            RenderOptions.SetBitmapScalingMode(RenderImage, BitmapScalingMode.HighQuality);
        }

        private void DrawDots() {
            for (var i = 0; i < 50; i++) _bitmap.SetPixel(RandInsideTexture(), RandInsideTexture(), RandomColor());
            AddStage("Drawing");
        }

        private int RandInsideTexture() {
            return _random.Next(Size);
        }

        private Color RandomColor() {
            var color = Color.FromArgb(_random.Next((int) Math.Pow(2, 32) - 1));
            return color;
        }

        private void DrawLines() {
            _bitmap.DrawLine(new Vector2(30, 18), new Vector2(134, 428), Color.Green);
            _bitmap.DrawLine(new Vector2(134 + 5, 428), new Vector2(30 + 5, 18), Color.Red);
            for (var i = 0; i < 50; i++)
                _bitmap.DrawLine(new Vector2(_random.Next(Size), _random.Next(Size)),
                    new Vector2(_random.Next(Size), _random.Next(Size)), RandomColor());
            AddStage("Drawing");
        }

        private void DrawModelWireframe() {
            var m = ObjFile.FromFile("Models/african_head.obj");
            AddStage("Loading");

            var bounds = m.Bounds();
            var scale = _bitmap.Width / bounds.BiggestSize() - 8;
            var shift = Vector3.One * _bitmap.Width / 2;
            
            AddStage("Bounds");

            foreach (var face in m.Faces) {
                var indicesCount = face.Vertices.Count;

                for (var i = 0; i < indicesCount; i++) {
                    var vertex0 = face.Vertices[i].Vertex;

                    var p0 = m.Vertices[vertex0 - 1].Position.ToVector3();
                    p0 *= scale;
                    p0 += shift;

                    var vertex1 = face.Vertices[(i + 1) % indicesCount].Vertex;

                    var p1 = m.Vertices[vertex1 - 1].Position.ToVector3();
                    p1 *= scale;
                    p1 += shift;
                    
                    _bitmap.DrawLine(new Vector2(p0.X, p0.Y),
                        new Vector2(p1.X, p1.Y), Color.White);
                }
            }
            AddStage("Drawing");
        }

        private void MakeTexture(object sender, RoutedEventArgs e) {
            _bitmap = new Bitmap(Size, Size, PixelFormat.Format24bppRgb);

            Enum.TryParse(((Button) sender).CommandParameter.ToString(), out ActionType t);
            
            _stages = new List<string>();
            _stopwatch.Restart();

            switch (t) {
                case ActionType.None:
                    break;
                case ActionType.DrawDot:
                    DrawDots();
                    break;
                case ActionType.DrawLines:
                    DrawLines();
                    break;
                case ActionType.DrawModelWireframe:
                    DrawModelWireframe();
                    break;
                case ActionType.DrawTriangles:
                    DrawTriangles();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            TextMsBlock.Text = String.Join(" ", _stages);
            _stopwatch.Stop();

            _image.Source = _bitmap.BitmapImage();
        }

        private void DrawTriangles() {
            var triCount = _random.Next(3) + 1;
            for (int i = 0; i < triCount; i++) {
                var points = new[] {
                    new Vector2(RandInsideTexture(), RandInsideTexture()),
                    new Vector2(RandInsideTexture(), RandInsideTexture()),
                    new Vector2(RandInsideTexture(), RandInsideTexture())
                };
                var colors = new[] {
                    Color.Red,
                    Color.Green, 
                    Color.Blue
                };

                _bitmap.DrawTriangle(points, colors);
            }

            AddStage("Drawing");
        }

        private void AddStage(string stageName) {
            _stages.Add($"{stageName}: {1000f * _stopwatch.ElapsedTicks / Stopwatch.Frequency:F2} ms");
            _stopwatch.Restart();
        }
    }

    public enum ActionType {
        None,
        DrawDot,
        DrawLines,
        DrawModelWireframe,
        DrawTriangles
    }
}