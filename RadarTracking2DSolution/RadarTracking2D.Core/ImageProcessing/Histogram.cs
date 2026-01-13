using RadarTracking2D.Core.Data;

namespace RadarTracking2D.Core.ImageProcessing;

public class Histogram
{
    public int[] Values { get; } = new int[256];
    public int TotalPixels { get; private set; }

    public static Histogram FromFrame(RadarFrame frame)
    {
        var histogram = new Histogram();

        for (int y = 0; y < frame.Height; y++)
        {
            for (int x = 0; x < frame.Width; x++)
            {
                byte intensity = frame.GetPixel(x, y);
                histogram.Values[intensity]++;
                histogram.TotalPixels++;
            }
        }

        return histogram;
    }

    public double[] GetNormalized()
    {
        var normalized = new double[256];

        for (int i = 0; i < 256; i++)
        {
            normalized[i] = (double)Values[i] / TotalPixels;
        }

        return normalized;
    }
}