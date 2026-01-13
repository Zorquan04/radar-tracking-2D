using RadarTracking2D.Core.Data;

namespace RadarTracking2D.Core.ImageProcessing;

public class ThresholdingService
{
    private readonly OtsuThreshold _otsu = new();

    public int CalculateThreshold(RadarFrame frame, int manualOffset = 0)
    {
        var histogram = Histogram.FromFrame(frame);
        int otsuThreshold = _otsu.Compute(histogram);

        int modifiedThreshold = otsuThreshold + manualOffset;

        return Math.Clamp(modifiedThreshold, 0, 255);
    }

    public bool[,] ApplyThreshold(RadarFrame frame, int threshold)
    {
        var binary = new bool[frame.Height, frame.Width];

        for (int y = 0; y < frame.Height; y++)
        {
            for (int x = 0; x < frame.Width; x++)
            {
                binary[y, x] = frame.GetPixel(x, y) >= threshold;
            }
        }

        return binary;
    }
}