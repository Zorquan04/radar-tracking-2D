using RadarTracking2D.Core.Tracking;
using System.Windows;
using System.Windows.Media;

namespace RadarTracking2D.WPF.Models;

public class TrackVisual
{
    public int Id { get; }
    public Point Position { get; private set; }
    public double StdDevX { get; private set; }
    public double StdDevY { get; private set; }
    public Color Color { get; }

    private readonly List<Point> _rawHistory = new();
    public List<Point> History { get; } = new();

    private static readonly Random _rand = new();
    private static readonly Dictionary<int, Color> _idToColor = new();

    public TrackVisual(Track core)
    {
        Id = core.Id;

        if (!_idToColor.TryGetValue(Id, out Color color))
        {
            color = Color.FromRgb((byte)_rand.Next(50, 255), (byte)_rand.Next(50, 255), (byte)_rand.Next(50, 255));
            _idToColor[Id] = color;
        }
        Color = color;

        AddPosition(core);
    }

    public void Update(Track core) => AddPosition(core);

    private void AddPosition(Track core)
    {
        var rawPoint = new Point(core.Distribution.MeanX, core.Distribution.MeanY);
        _rawHistory.Add(rawPoint);

        StdDevX = core.Distribution.StdDevX;
        StdDevY = core.Distribution.StdDevY;

        UpdateScaledHistory(1, 1);
    }

    public void UpdateScaledHistory(double scaleX, double scaleY)
    {
        History.Clear();
        foreach (var p in _rawHistory)
            History.Add(new Point(p.X * scaleX, p.Y * scaleY));

        if (_rawHistory.Count > 0)
        {
            var last = _rawHistory[_rawHistory.Count - 1];
            Position = new Point(last.X * scaleX, last.Y * scaleY);
        }
    }
}