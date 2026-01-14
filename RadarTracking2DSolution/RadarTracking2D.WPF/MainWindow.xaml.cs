using RadarTracking2D.Core.Data;
using RadarTracking2D.Core.Generation;
using RadarTracking2D.Core.ImageProcessing;
using RadarTracking2D.Core.Segmentation;
using RadarTracking2D.Core.Tracking;
using RadarTracking2D.WPF.Models;
using RadarTracking2D.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RadarTracking2D.WPF.Views;

public partial class MainWindow : Window
{
    private readonly RadarViewModel _viewModel = new();
    private readonly RadarFrameGenerator _generator = new();
    private readonly ThresholdingService _thresholding = new();
    private readonly BlobExtractor _extractor = new();
    private readonly Tracker _tracker = new();

    private readonly List<RadarFrame> _frames = new();
    private int _currentFrame = 0;
    private readonly DispatcherTimer _timer = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _viewModel;

        // generate some test frames
        for (int i = 0; i < 10; i++)
            _frames.Add(_generator.Generate(100, 100, 3));

        _timer.Interval = TimeSpan.FromMilliseconds(500);
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (_currentFrame >= _frames.Count)
        {
            _timer.Stop();
            return;
        }

        ProcessRadarFrame(_frames[_currentFrame]);
        _currentFrame++;
    }

    private void ProcessRadarFrame(RadarFrame frame)
    {
        // 1. Thresholding
        int threshold = _thresholding.CalculateThreshold(frame);
        bool[,] binary = _thresholding.ApplyThreshold(frame, threshold);

        // 2. Blob extraction
        var blobs = _extractor.Extract(binary);

        // 3. Tracking
        _tracker.ProcessFrame(blobs);

        // 4. Update ViewModel
        _viewModel.UpdateTracks(_tracker.GetTracks());

        // 5. Draw on canvas
        DrawTracks(_viewModel.Tracks);
    }

    public void DrawTracks(IEnumerable<TrackVisual> tracks)
    {
        RadarCanvas.Children.Clear();

        double scaleX = RadarCanvas.ActualWidth / 100.0;
        double scaleY = RadarCanvas.ActualHeight / 100.0;

        foreach (var t in tracks)
        {
            t.AddPosition();
            t.UpdateScaledHistory(scaleX, scaleY);

            // Gaussian ellipse
            var ellipse = new Ellipse
            {
                Width = t.StdDevX * 4 * scaleX,
                Height = t.StdDevY * 4 * scaleY,
                Fill = new SolidColorBrush(Color.FromArgb(60, t.Color.R, t.Color.G, t.Color.B))
            };
            Canvas.SetLeft(ellipse, t.Position.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, t.Position.Y - ellipse.Height / 2);
            RadarCanvas.Children.Add(ellipse);

            // Tracking point
            var dot = new Ellipse
            {
                Width = 6,
                Height = 6,
                Fill = new SolidColorBrush(t.Color)
            };
            Canvas.SetLeft(dot, t.Position.X - 3);
            Canvas.SetTop(dot, t.Position.Y - 3);
            RadarCanvas.Children.Add(dot);

            // History lines
            for (int i = 1; i < t.History.Count; i++)
            {
                var line = new Line
                {
                    X1 = t.History[i - 1].X,
                    Y1 = t.History[i - 1].Y,
                    X2 = t.History[i].X,
                    Y2 = t.History[i].Y,
                    Stroke = new SolidColorBrush(t.Color),
                    StrokeThickness = 1
                };
                RadarCanvas.Children.Add(line);
            }

            // Motion prediction
            var pred = t.GetCoreTrack().Predict();
            var predLine = new Line
            {
                X1 = t.Position.X,
                Y1 = t.Position.Y,
                X2 = pred.PredX * scaleX,
                Y2 = pred.PredY * scaleY,
                Stroke = new SolidColorBrush(t.Color) { Opacity = 0.3 },
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection { 4, 2 }
            };
            RadarCanvas.Children.Add(predLine);
        }
    }
}