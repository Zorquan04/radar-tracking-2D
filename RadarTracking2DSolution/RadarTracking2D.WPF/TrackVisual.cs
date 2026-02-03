using System.Windows;
using System.Windows.Media;

namespace RadarTracking2D.WPF.Models;

// TrackVisual stores WPF-related info for a core Track
public class TrackVisual
{
    public int Id { get; }
    public Point Position { get; private set; }
    public double StdDevX { get; private set; }
    public double StdDevY { get; private set; }
    public Color Color { get; }

    private readonly Track _coreTrack;

    private readonly List<Point> _rawHistory = new();
    public List<Point> History { get; } = new();
    private const int MaxHistoryPoints = 3;

    private static readonly Random _rand = new();
    private static readonly Dictionary<int, Color> _idToColor = new();

    public TrackVisual(Track core, Point? initialPoint = null)
    {
        _coreTrack = core;
        Id = core.Id;

        if (!_idToColor.TryGetValue(Id, out Color color))
        {
            color = Color.FromRgb((byte)_rand.Next(50, 255), (byte)_rand.Next(50, 255), (byte)_rand.Next(50, 255));
            _idToColor[Id] = color;
        }
        Color = color;

        if (initialPoint.HasValue)
            AddHistoryPoint(initialPoint.Value);
        else
            AddPosition(); // normalny track
    }

    // Add current position to history
    public void AddPosition(Point? pt = null)
    {
        Point newPt = pt ?? new Point(_coreTrack.Distribution.MeanX, _coreTrack.Distribution.MeanY);
        _rawHistory.Add(newPt);
        if (_rawHistory.Count > MaxHistoryPoints)
            _rawHistory.RemoveAt(0);

        StdDevX = _coreTrack.Distribution.StdDevX;
        StdDevY = _coreTrack.Distribution.StdDevY;
    }

    // Update history for display scaling
    public void UpdateScaledHistory(double scaleX, double scaleY)
    {
        History.Clear();
        foreach (var p in _rawHistory)
            History.Add(new Point(p.X * scaleX, p.Y * scaleY));

        if (_rawHistory.Count > 0)
        {
            var last = _rawHistory[^1];
            Position = new Point(last.X * scaleX, last.Y * scaleY);
        }
    }

    // Add a specific history point (used for manual tracks)
    public void AddHistoryPoint(Point pt)
    {
        _rawHistory.Add(pt);
        if (_rawHistory.Count > MaxHistoryPoints)
            _rawHistory.RemoveAt(0);
    }

    public Track GetCoreTrack() => _coreTrack;
}