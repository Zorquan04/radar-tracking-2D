namespace RadarTracking2D.Core.Data;

public class TrackState(double meanX, double meanY, double stdX, double stdY)
{
    public double MeanX { get; set; } = meanX;
    public double MeanY { get; set; } = meanY;
    public double StdX { get; set; } = stdX;
    public double StdY { get; set; } = stdY;
    public double Probability { get; set; } = 1.0; // starting value

    public TrackState Clone() => new TrackState(MeanX, MeanY, StdX, StdY) { Probability = Probability };
}