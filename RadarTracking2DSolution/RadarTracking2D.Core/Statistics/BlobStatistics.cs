using RadarTracking2D.Core.Data;

namespace RadarTracking2D.Core.Statistics;

public class BlobStatistics
{
    public double MeanX { get; }
    public double MeanY { get; }
    public double StdDevX { get; }
    public double StdDevY { get; }

    public BlobStatistics(Blob blob)
    {
        int n = blob.Pixels.Count;
        if (n == 0)
        {
            // empty blob → all zero
            MeanX = 0;
            MeanY = 0;
            StdDevX = 0;
            StdDevY = 0;
            return;
        }

        // average positions
        MeanX = blob.Pixels.Average(p => p.X);
        MeanY = blob.Pixels.Average(p => p.Y);

        // standard deviations
        StdDevX = Math.Sqrt(blob.Pixels.Average(p => (p.X - MeanX) * (p.X - MeanX)));
        StdDevY = Math.Sqrt(blob.Pixels.Average(p => (p.Y - MeanY) * (p.Y - MeanY)));
    }
}