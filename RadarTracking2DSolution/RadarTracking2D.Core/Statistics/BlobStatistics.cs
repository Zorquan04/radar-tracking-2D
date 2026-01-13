using RadarTracking2D.Core.Data;

namespace RadarTracking2D.Core.Statistics;

public class BlobStatistics
{
    public int BlobLabel { get; }
    public int PixelCount { get; }
    public double MeanX { get; }
    public double MeanY { get; }
    public double StdDevX { get; }
    public double StdDevY { get; }

    public BlobStatistics(Blob blob)
    {
        BlobLabel = blob.Label;
        PixelCount = blob.PixelCount;

        MeanX = blob.Pixels.Average(p => p.X);
        MeanY = blob.Pixels.Average(p => p.Y);

        StdDevX = Math.Sqrt(blob.Pixels.Average(p => Math.Pow(p.X - MeanX, 2)));
        StdDevY = Math.Sqrt(blob.Pixels.Average(p => Math.Pow(p.Y - MeanY, 2)));
    }
}