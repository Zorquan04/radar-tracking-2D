namespace RadarTracking2D.Core.Generation;

public class NoiseGenerator(int? seed = null)
{
    private readonly Random _random = seed.HasValue ? new Random(seed.Value) : new Random();

    public byte GenerateNoise(byte min = 0, byte max = 40)
    {
        return (byte)_random.Next(min, max + 1);
    }
}