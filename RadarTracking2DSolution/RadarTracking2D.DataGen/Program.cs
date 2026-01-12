namespace RadarTracking2D.DataGen;

class Program
{
    static void Main()
    {
        var simulator = new RadarSimulator(width: 20, height: 10, objectCount: 3);

        var frame = simulator.GenerateFrame();

        simulator.PrintFrame(frame);

        Console.WriteLine("Frame generated successfully!");
    }
}
