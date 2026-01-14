using RadarTracking2D.Core.Data;

namespace RadarTracking2D.Core;

public class ThresholdingService
{
    // Otsu's method – automatically compute threshold from histogram
    public int CalculateThreshold(RadarFrame frame)
    {
        int[] histogram = new int[256];
        for (int y = 0; y < frame.Height; y++)
            for (int x = 0; x < frame.Width; x++)
                histogram[frame.GetPixel(x, y)]++;

        int total = frame.Width * frame.Height;
        float sum = 0;
        for (int t = 0; t < 256; t++)
            sum += t * histogram[t];

        float sumB = 0;
        int wB = 0, wF = 0;
        float maxVar = 0;
        int threshold = 0;

        for (int t = 0; t < 256; t++)
        {
            wB += histogram[t];
            if (wB == 0) continue;

            wF = total - wB;
            if (wF == 0) break;

            sumB += t * histogram[t];

            float mB = sumB / wB;           // mean of background
            float mF = (sum - sumB) / wF;   // mean of foreground

            float varBetween = wB * wF * (mB - mF) * (mB - mF);
            if (varBetween > maxVar)         // pick t that maximizes between-class variance
            {
                maxVar = varBetween;
                threshold = t;
            }
        }

        if (maxVar == 0) // all pixels same
            return Array.FindIndex(histogram, h => h > 0);

        return threshold;
    }

    // apply computed threshold → binary image
    public bool[,] ApplyThreshold(RadarFrame frame, int threshold)
    {
        bool[,] binary = new bool[frame.Height, frame.Width];
        for (int y = 0; y < frame.Height; y++)
            for (int x = 0; x < frame.Width; x++)
                binary[y, x] = frame.GetPixel(x, y) >= threshold; // true = object
        return binary;
    }
}