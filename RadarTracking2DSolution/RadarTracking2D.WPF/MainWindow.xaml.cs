using RadarTracking2D.Core.Generation;
using RadarTracking2D.Core.ImageProcessing;
using RadarTracking2D.Core.Segmentation;
using RadarTracking2D.Core.Statistics;
using RadarTracking2D.Core.Tracking;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RadarTracking2D.WPF;

public partial class MainWindow : Window
{
    private RadarFrameGenerator _generator = new();
    private ThresholdingService _thresholding = new();
    private BlobExtractor _extractor = new();
    private Tracker _tracker = new();

    private int _frameWidth = 100;
    private int _frameHeight = 100;
    private int _objectCount = 2;

    public MainWindow()
    {
        InitializeComponent();
        RunSimulation();
    }

    private async void RunSimulation()
    {
        int frameCount = 10;

        for (int f = 0; f < frameCount; f++)
        {
            var frame = _generator.Generate(_frameWidth, _frameHeight, _objectCount);
            int threshold = _thresholding.CalculateThreshold(frame);
            var binary = _thresholding.ApplyThreshold(frame, threshold);
            var blobs = _extractor.Extract(binary);
            var blobStats = blobs.Select(b => new BlobStatistics(b)).ToList();

            _tracker.ProcessFrame(blobStats);

            DrawFrame(binary, _tracker.GetTracks());

            await Task.Delay(500); // half a second between frames
        }
    }

    private void DrawFrame(bool[,] binary, List<Core.Tracking.Track> tracks)
    {
        RadarCanvas.Children.Clear();

        // drawing a binary frame
        for (int y = 0; y < _frameHeight; y++)
        {
            for (int x = 0; x < _frameWidth; x++)
            {
                if (binary[y, x])
                {
                    var rect = new Rectangle
                    {
                        Width = 2,
                        Height = 2,
                        Fill = Brushes.White
                    };
                    Canvas.SetLeft(rect, x * 4);
                    Canvas.SetTop(rect, y * 4);
                    RadarCanvas.Children.Add(rect);
                }
            }
        }

        // drawing tracks
        foreach (var track in tracks)
        {
            var ellipse = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Red,
                Opacity = 0.5
            };
            Canvas.SetLeft(ellipse, track.Distribution.MeanX * 4 - 5);
            Canvas.SetTop(ellipse, track.Distribution.MeanY * 4 - 5);
            RadarCanvas.Children.Add(ellipse);
        }
    }
}