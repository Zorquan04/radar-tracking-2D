namespace RadarTracking2D.Core.Statistics;

public class GaussianDistribution(double meanX, double meanY, double stdDevX, double stdDevY)
{
    public double MeanX { get; } = meanX;
    public double MeanY { get; } = meanY;
    public double StdDevX { get; } = stdDevX;
    public double StdDevY { get; } = stdDevY;

    // 2D Gaussian PDF – probability at (x,y)
    public double Probability(double x, double y)
    {
        double coeff = 1.0 / (2.0 * Math.PI * StdDevX * StdDevY);
        double exp = -(Math.Pow(x - MeanX, 2) / (2 * StdDevX * StdDevX) + Math.Pow(y - MeanY, 2) / (2 * StdDevY * StdDevY));
        return coeff * Math.Exp(exp);
    }
}