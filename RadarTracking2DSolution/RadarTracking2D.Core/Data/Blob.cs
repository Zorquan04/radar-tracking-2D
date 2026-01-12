namespace RadarTracking2D.Core.Data;

public class Blob
{
    public int Label { get; set; }
    public List<(int X, int Y)> Pixels { get; } = new();

    public double MeanX => Pixels.Any() ? Pixels.Average(p => p.X) : 0;
    public double MeanY => Pixels.Any() ? Pixels.Average(p => p.Y) : 0;

    public double StdDevX => Pixels.Any() ? Math.Sqrt(Pixels.Average(p => Math.Pow(p.X - MeanX, 2))) : 0;
    public double StdDevY => Pixels.Any() ? Math.Sqrt(Pixels.Average(p => Math.Pow(p.Y - MeanY, 2))) : 0;
}