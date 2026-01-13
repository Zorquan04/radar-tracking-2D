namespace RadarTracking2D.Core.Data;

public readonly struct Pixel(int x, int y, byte intensity)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public byte Intensity { get; } = intensity;
}