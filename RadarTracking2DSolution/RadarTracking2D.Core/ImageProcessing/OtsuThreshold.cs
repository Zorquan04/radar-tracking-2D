namespace RadarTracking2D.Core.ImageProcessing;

public class OtsuThreshold
{
    public int Compute(Histogram histogram)
    {
        var probabilities = histogram.GetNormalized();

        double sum = 0.0;
        for (int i = 0; i < 256; i++)
            sum += i * probabilities[i];

        double sumBackground = 0.0;
        double weightBackground = 0.0;

        double maxVariance = 0.0;
        int threshold = 0;

        for (int t = 0; t < 256; t++)
        {
            weightBackground += probabilities[t];
            if (weightBackground == 0)
                continue;

            double weightForeground = 1.0 - weightBackground;
            if (weightForeground == 0)
                break;

            sumBackground += t * probabilities[t];

            double meanBackground = sumBackground / weightBackground;
            double meanForeground = (sum - sumBackground) / weightForeground;

            double varianceBetween =
                weightBackground * weightForeground *
                Math.Pow(meanBackground - meanForeground, 2);

            if (varianceBetween > maxVariance)
            {
                maxVariance = varianceBetween;
                threshold = t;
            }
        }

        return threshold;
    }
}