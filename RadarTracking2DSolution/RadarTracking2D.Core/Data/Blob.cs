namespace RadarTracking2D.Core.Data;

public class Blob(int label)
{
    public int Label { get; } = label;
    public List<(int X, int Y)> Pixels { get; } = new();

    public int PixelCount => Pixels.Count;
}