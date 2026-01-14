using RadarTracking2D.Core.Statistics;
using RadarTracking2D.Core.Tracking;
using System.Drawing;

public class Track
{
    public int Id { get; }
    public GaussianDistribution Distribution { get; set; }
    public Point? PreviousPosition { get; private set; }
    public int Age { get; private set; } = 0;
    public MotionModel Motion { get; } = new MotionModel();

    public Track(int id, GaussianDistribution distribution)
    {
        Id = id;
        Distribution = distribution;
    }

    public void Update(GaussianDistribution newDistribution)
    {
        if (Distribution != null)
        {
            PreviousPosition = new Point((int)Distribution.MeanX, (int)Distribution.MeanY);
            Motion.Update(newDistribution.MeanX, newDistribution.MeanY, Distribution.MeanX, Distribution.MeanY);
        }

        Distribution = newDistribution;
        Age++;
    }

    public (double PredX, double PredY) Predict()
    {
        return Motion.Predict(Distribution.MeanX, Distribution.MeanY);
    }
}