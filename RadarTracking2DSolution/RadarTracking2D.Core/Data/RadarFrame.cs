namespace RadarTracking2D.Core.Data;

public class RadarFrame(int width, int height)
{
    public int Width { get; } = width;
    public int Height { get; } = height;

    // [y, x] – convenient for browsing by rows
    public byte[,] Intensities { get; } = new byte[height, width];

    public byte GetPixel(int x, int y) => Intensities[y, x];

    public void SetPixel(int x, int y, byte value)
    {
        Intensities[y, x] = value;
    }
}