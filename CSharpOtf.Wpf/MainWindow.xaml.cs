using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OpenGL.TextDrawing;
using OpenGL.TextDrawing.Cff.Type2Charstring;
using Path = System.Windows.Shapes.Path;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public const string FontFileFilter = "*.otf";
        public const string FontFilesDirectory = "./Resources";

        public OpenTypeParser Parser { get; set; }
        public FileSystemWatcher FileSystemWatcher { get; }

        private int _selectedCharstringIndex;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int SelectedCharstringIndex
        {
            get => _selectedCharstringIndex;
            set
            {
                _selectedCharstringIndex = value;
                SelectedCharstring = SelectedCharstringIndex < Parser.Charstrings.Count ?
                    Parser.Charstrings[SelectedCharstringIndex] :
                    Parser.Charstrings[0];
                RenderSelectedCharstring(SelectedCharstring);
                OnPropertyChanged();
            }
        }

        private T2ProgramContext _selectedCharstring;
        public T2ProgramContext SelectedCharstring
        {
            get => _selectedCharstring;
            set
            {
                _selectedCharstring = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<FileInfo> _fontFiles;
        public IEnumerable<FileInfo> FontFiles
        {
            get => _fontFiles;
            set
            {
                _fontFiles = value;
                OnPropertyChanged();

                if (FontFiles.Any())
                {
                    SelectedFontFile = value.First();
                }
            }
        }

        private FileInfo _selectedFontFile;

        public FileInfo SelectedFontFile
        {
            get => _selectedFontFile;
            set
            {
                if (value == null)
                {
                    return;
                }

                if (_selectedFontFile?.FullName == value?.FullName)
                {
                    return;
                }

                _selectedFontFile = value;
                Parser.Parse(value.FullName);
                SelectedCharstringIndex = 0;
                OnPropertyChanged();
            }
        }

        private List<T2ProgramContext> _textCharstrings = new();
        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                RenderText(Text);
                OnPropertyChanged();
            }
        }

        private void RenderText(string text)
        {
            _textCharstrings.Clear();
            TextCanvas.Children.Clear();

            var scale = new ScaleTransform(0.1, -0.1);
            var translateBase = new TranslateTransform(0, -1000);

            foreach (var symbol in text)
            {
                var lastCharstring = _textCharstrings.LastOrDefault();
                var lastTranslate = TextCanvas.Children.Count == 0 ?
                    default :
                    ((TransformGroup)TextCanvas.Children[^1].RenderTransform).Children.OfType<TranslateTransform>().FirstOrDefault();
                var translate = new TranslateTransform((lastCharstring?.Width!.Value ?? 0) + (lastTranslate?.X ?? 0), translateBase.Y);
                var transform = new TransformGroup();
                transform.Children.Add(translate);
                transform.Children.Add(scale);

                var charstring = Parser.Charstrings.First(item => symbol.ToString() == " " ?
                    item.Name == "space" :
                    item.Name == symbol.ToString());
                var chatstringPath = CreateCharstringPath(transform, charstring);

                TextCanvas.Children.Add(chatstringPath);
                _textCharstrings.Add(charstring);
            }
        }

        private Path CharstringPath { get; set; }
        private List<Path> StemPaths { get; set; }
        private Path WidthPath { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Parser = new OpenTypeParser();

            FileSystemWatcher = new FileSystemWatcher(".", FontFileFilter)
            {
                IncludeSubdirectories = true,
                EnableRaisingEvents = true,
            };
            FileSystemWatcher.Created += (s, e) =>
            {
                App.Current.Dispatcher.Invoke(() => FontFiles = GetFontFiles(FontFilesDirectory));
            };
            FileSystemWatcher.Renamed += (s, e) =>
            {
                App.Current.Dispatcher.Invoke(() => FontFiles = GetFontFiles(FontFilesDirectory));
            };
            FileSystemWatcher.Deleted += (s, e) =>
            {
                App.Current.Dispatcher.Invoke(() => FontFiles = GetFontFiles(FontFilesDirectory));
            };

            FontFiles = GetFontFiles(FontFilesDirectory);
        }

        private IEnumerable<FileInfo> GetFontFiles(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return Enumerable.Empty<FileInfo>();
            }

            return Directory.EnumerateFiles(directoryPath, FontFileFilter).Select(item => new FileInfo(item));
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RenderSelectedCharstring(T2ProgramContext context)
        {
            var transform = new TransformGroup();
            transform.Children.Add(new ScaleTransform(0.3, -0.3));
            transform.Children.Add(new TranslateTransform(300, 400));

            CharstringPath = CreateCharstringPath(transform, context);
            StemPaths = CreateStemPaths(transform, context);
            WidthPath = CreateWidthPath(transform, context);

            Canvas.Children.Clear();
            Canvas.Children.Add(CharstringPath);

            foreach (var stemPath in StemPaths)
            {
                Canvas.Children.Add(stemPath);
            }

            Canvas.Children.Add(WidthPath);
        }

        private Path CreateWidthPath(TransformGroup transform, T2ProgramContext context)
        {
            var path = new Path()
            {
                Stroke = Brushes.DarkRed,
                StrokeThickness = 5,
                RenderTransform = transform
            };

            var pathGeometry = new PathGeometry();
            path.Data = pathGeometry;

            pathGeometry.AddGeometry(new LineGeometry(new Point(0, -1000), new Point(0, 1000)));
            pathGeometry.AddGeometry(new LineGeometry(new Point(context.Width ?? 0, -1000), new Point(context.Width ?? 0, 1000)));

            return path;
        }

        private static Path CreateCharstringPath(TransformGroup transform, T2ProgramContext context)
        {
            var path = new Path
            {
                Stroke = Brushes.Black,
                StrokeThickness = 10,
                RenderTransform = transform
            };

            var pathGeometry = new PathGeometry();
            path.Data = pathGeometry;

            foreach (var pathSegment in context.Path.PathSegments)
            {
                //var pointColor = Brushes.Red;
                var pointWitdh = 10f;
                var startPoint1 = new Point(pathSegment.StartPoint.X - pointWitdh / 2, pathSegment.StartPoint.Y - pointWitdh / 2);
                var endPoint1 = new Point(pathSegment.EndPoint.X - pointWitdh / 2, pathSegment.EndPoint.Y - pointWitdh / 2);
                pathGeometry.AddGeometry(new RectangleGeometry(new Rect(startPoint1, new Size(pointWitdh, pointWitdh))));
                pathGeometry.AddGeometry(new RectangleGeometry(new Rect(endPoint1, new Size(pointWitdh, pointWitdh))));

                if (pathSegment.GetType() == typeof(T2LineSegment))
                {
                    var startPoint = new Point(pathSegment.StartPoint.X, pathSegment.StartPoint.Y);
                    var endPoint = new Point(pathSegment.EndPoint.X, pathSegment.EndPoint.Y);

                    pathGeometry.AddGeometry(new LineGeometry(startPoint, endPoint));
                }
                else if (pathSegment.GetType() == typeof(T2BezierCurve))
                {
                    var curve = (T2BezierCurve)pathSegment;
                    var point1 = new Point(curve.StartPoint.X, curve.StartPoint.Y);
                    var point2 = new Point(curve.ControlPoint1.X, curve.ControlPoint1.Y);
                    var point3 = new Point(curve.ControlPoint2.X, curve.ControlPoint2.Y);
                    var point4 = new Point(curve.EndPoint.X, curve.EndPoint.Y);

                    var point21 = new Point(point2.X - pointWitdh / 2, point2.Y - pointWitdh / 2);
                    pathGeometry.AddGeometry(new RectangleGeometry(new Rect(point21, new Size(pointWitdh, pointWitdh))));
                    var point31 = new Point(point3.X - pointWitdh / 2, point3.Y - pointWitdh / 2);
                    pathGeometry.AddGeometry(new RectangleGeometry(new Rect(point31, new Size(pointWitdh, pointWitdh))));

                    var figure = new PathFigure()
                    {
                        StartPoint = point1,
                    };

                    figure.Segments.Add(new BezierSegment(point2, point3, point4, true));
                    pathGeometry.Figures.Add(figure);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            return path;
        }

        private static List<Path> CreateStemPaths(TransformGroup transform, T2ProgramContext context)
        {
            var stemPaths = new List<Path>();

            foreach (var stem in context.Stems)
            {
                var stemPath = new Path()
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 3,
                    StrokeDashArray = new() { 5 },
                    RenderTransform = transform
                };

                var stemPathGeometry = new PathGeometry();
                stemPath.Data = stemPathGeometry;

                if (stem.IsVertical)
                {
                    var startPoint = new Point(stem.Stem1, 1000);
                    var endPoint = new Point(stem.Stem1, -1000);
                    stemPathGeometry.AddGeometry(new LineGeometry(startPoint, endPoint));

                    var startPoint1 = new Point(stem.Stem2, 1000);
                    var endPoint1 = new Point(stem.Stem2, -1000);
                    stemPathGeometry.AddGeometry(new LineGeometry(startPoint1, endPoint1));
                }
                else
                {
                    var startPoint = new Point(1000, stem.Stem1);
                    var endPoint = new Point(-1000, stem.Stem1);
                    stemPathGeometry.AddGeometry(new LineGeometry(startPoint, endPoint));

                    var startPoint1 = new Point(1000, stem.Stem2);
                    var endPoint1 = new Point(-1000, stem.Stem2);
                    stemPathGeometry.AddGeometry(new LineGeometry(startPoint1, endPoint1));
                }

                stemPaths.Add(stemPath);
            }

            return stemPaths;
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCharstringIndex == 0)
            {
                return;
            }

            SelectedCharstringIndex--;
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCharstringIndex == Parser.Charstrings.Count - 1)
            {
                return;
            }

            SelectedCharstringIndex++;
        }
    }
}
