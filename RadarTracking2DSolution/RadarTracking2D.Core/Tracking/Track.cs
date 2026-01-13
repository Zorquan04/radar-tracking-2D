using RadarTracking2D.Core.Statistics;

namespace RadarTracking2D.Core.Tracking;

public class Track(int id, GaussianDistribution distribution)
{
    public int Id { get; } = id;
    public GaussianDistribution Distribution { get; set; } = distribution;
    public int Age { get; private set; } = 0;

    public void Update(GaussianDistribution newDistribution)
    {
        Distribution = newDistribution;
        Age++;
    }

    public Track Clone()
    {
        var newDistribution = new GaussianDistribution(Distribution.MeanX, Distribution.MeanY, Distribution.StdDevX, Distribution.StdDevY);

        return new Track(Id, newDistribution);
    }

    // Track position prediction
    public (double PredX, double PredY) Predict()
    {
        // simple prediction – if you don't have speed, we return the current average
        return (Distribution.MeanX, Distribution.MeanY);
    }
}