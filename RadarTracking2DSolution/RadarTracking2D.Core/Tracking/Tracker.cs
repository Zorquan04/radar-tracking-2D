using RadarTracking2D.Core.Statistics;

namespace RadarTracking2D.Core.Tracking;

public class Tracker
{
    private List<Track> _tracks = new();
    private List<Hypothesis> _activeHypotheses = new();
    private int _nextTrackId = 1;
    private readonly HypothesisEvaluator _evaluator = new();

    public Tracker()
    {
        _activeHypotheses.Add(new Hypothesis()); // root hypothesis
    }

    public void ProcessFrame(List<BlobStatistics> measurements)
    {
        var newHypotheses = new List<Hypothesis>();

        foreach (var hyp in _activeHypotheses)
        {
            // simple logic: each assignment measurement -> track or disturbance
            for (int i = 0; i < measurements.Count; i++)
            {
                // 1. assign to existing track
                foreach (var track in _tracks)
                {
                    var newHyp = hyp.Clone();
                    newHyp.Assignments[i] = track.Id;
                    newHyp.Probability = _evaluator.Evaluate(newHyp, measurements, _tracks);
                    newHypotheses.Add(newHyp);
                }

                // 2. assign as disruption
                var noiseHyp = hyp.Clone();
                noiseHyp.Assignments[i] = null;
                noiseHyp.Probability = _evaluator.Evaluate(noiseHyp, measurements, _tracks);
                newHypotheses.Add(noiseHyp);

                // 3. create a new track
                var newTrackHyp = hyp.Clone();
                int newId = _nextTrackId++;
                var dist = new GaussianDistribution(measurements[i].MeanX, measurements[i].MeanY, measurements[i].StdDevX, measurements[i].StdDevY);
                _tracks.Add(new Track(newId, dist));
                newTrackHyp.Assignments[i] = newId;
                newTrackHyp.Probability = _evaluator.Evaluate(newTrackHyp, measurements, _tracks);
                newHypotheses.Add(newTrackHyp);
            }
        }

        // selection of the most probable hypotheses
        _activeHypotheses = newHypotheses.OrderByDescending(h => h.Probability).Take(10).ToList(); // top 10 so it doesn't explode
    }

    public List<Track> GetTracks() => _tracks;
}