using RadarTracking2D.Core.Data;

namespace RadarTracking2D.Core.Generation;

public class RadarFrameGenerator
{
    private readonly NoiseGenerator _noiseGenerator = new();
    private readonly ObjectGenerator _objectGenerator = new();

    public RadarFrame Generate(
        int width,
        int height,
        int objectCount)
    {
        var frame = new RadarFrame(width, height);

        // Noise
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                frame.SetPixel(x, y, _noiseGenerator.GenerateNoise());
            }
        }

        // Objects
        var rand = new Random();

        for (int i = 0; i < objectCount; i++)
        {
            int cx = rand.Next(20, width - 20);
            int cy = rand.Next(20, height - 20);
            int radius = rand.Next(6, 12);

            _objectGenerator.AddObject(frame.Intensities, cx, cy, radius);
        }

        return frame;
    }
}