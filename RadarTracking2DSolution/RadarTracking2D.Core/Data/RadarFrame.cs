namespace RadarTracking2D.Core.Data;

public class RadarFrame(int width, int height)
{
    public int Width { get; } = width;
    public int Height { get; } = height;

    public byte[,] Intensities { get; } = new byte[height, width]; // store pixel values [y, x]

    public byte GetPixel(int x, int y) => Intensities[y, x]; // read pixel

    public void SetPixel(int x, int y, byte value) => Intensities[y, x] = value; // write pixel
}