using RadarTracking2D.Core.Statistics;
using RadarTracking2D.Core.Tracking;
using System.Drawing;

public class Track
{
    public int Id { get; }
    public GaussianDistribution Distribution { get; set; }
    public Point? PreviousPosition { get; private set; }
    public int Age { get; set; } = 0;
    public bool UpdatedThisFrame { get; set; } = false;
    public MotionModel Motion { get; } = new MotionModel();

    public Track(int id, GaussianDistribution distribution)
    {
        Id = id;
        Distribution = distribution;
    }

    // update track with new measurement
    public void Update(GaussianDistribution newDistribution)
    {
        if (Distribution != null)
        {
            PreviousPosition = new Point((int)Distribution.MeanX, (int)Distribution.MeanY);
            Motion.Update(newDistribution.MeanX, newDistribution.MeanY, Distribution.MeanX, Distribution.MeanY);
        }
        else
        {
            Motion.Update(newDistribution.MeanX, newDistribution.MeanY, newDistribution.MeanX - 1, newDistribution.MeanY - 1);
        }

        Distribution = newDistribution;
        Age++;
    }

    // predict next position
    public (double PredX, double PredY) Predict()
    {
        return Motion.Predict(Distribution.MeanX, Distribution.MeanY);
    }
}