namespace RadarTracking2D.DataGen;

public class RadarSimulator(int width, int height, int objectCount)
{
    private readonly Random random = new();

    public int[,] GenerateFrame()
    {
        var frame = new int[width, height];

        // background noise
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
            frame[x, y] = random.Next(0, 20); // low noise values

        // adding objects
        for (int i = 0; i < objectCount; i++)
        {
            int objX = random.Next(0, width);
            int objY = random.Next(0, height);
            int intensity = random.Next(200, 256);

            // object as a small 3x3 square
            for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            {
                int px = Math.Clamp(objX + dx, 0, width - 1);
                int py = Math.Clamp(objY + dy, 0, height - 1);
                frame[px, py] = intensity;
            }
        }

        return frame;
    }

    // display in the console
    public void PrintFrame(int[,] frame)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(frame[x, y] > 50 ? "#" : ".");
            }
            Console.WriteLine();
        }
    }
}