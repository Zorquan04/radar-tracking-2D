namespace RadarTracking2D.Core.Generation;

public class ObjectGenerator(int? seed = null)
{
    private readonly Random _random = seed.HasValue ? new Random(seed.Value) : new Random();

    public void AddObject(byte[,] image, int centerX, int centerY, int radius, byte peakIntensity = 220)
    {
        int height = image.GetLength(0);
        int width = image.GetLength(1);

        for (int y = centerY - radius; y <= centerY + radius; y++)
        {
            for (int x = centerX - radius; x <= centerX + radius; x++)
            {
                if (x < 0 || y < 0 || x >= width || y >= height)
                    continue;

                double dx = x - centerX;
                double dy = y - centerY;
                double distance = Math.Sqrt(dx * dx + dy * dy);

                if (distance > radius)
                    continue;

                double factor = 1.0 - (distance / radius);
                byte intensity = (byte)(peakIntensity * factor);

                // we do not overwrite the brighter signal
                if (image[y, x] < intensity)
                    image[y, x] = intensity;
            }
        }
    }
}