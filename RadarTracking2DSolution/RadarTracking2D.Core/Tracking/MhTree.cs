using RadarTracking2D.Core.Tracking.Association;
using RadarTracking2D.Core.Statistics;

namespace RadarTracking2D.Core.Tracking;

// MHT tree – keeps all possible assignment hypotheses
public class MhTree
{
    public MhTreeNode Root { get; } = new(new Hypothesis());

    // expand tree with new measurements
    public void Expand(List<BlobStatistics> measurements, List<Track> tracks, ref int nextTrackId)
    {
        var leaves = Root.GetLeaves().ToList();

        foreach (var leaf in leaves)
        {
            foreach (var mIndex in Enumerable.Range(0, measurements.Count))
            {
                var measurement = measurements[mIndex];

                // no tracks yet → create new
                if (!tracks.Any())
                {
                    var hyp = leaf.Hypothesis.Clone();
                    hyp.Assignments[mIndex] = nextTrackId++;
                    leaf.AddChild(new MhTreeNode(hyp) { Probability = 1.0 });
                    continue;
                }

                // try assigning to existing tracks
                foreach (var track in tracks)
                {
                    var hyp = leaf.Hypothesis.Clone();
                    hyp.Assignments[mIndex] = track.Id;
                    double prob = ProbabilityCalculator.Evaluate(hyp, measurements, tracks);
                    leaf.AddChild(new MhTreeNode(hyp) { Probability = prob });
                }

                // measurement is noise
                var unassigned = leaf.Hypothesis.Clone();
                unassigned.Assignments[mIndex] = null;
                leaf.AddChild(new MhTreeNode(unassigned)
                {
                    Probability = ProbabilityCalculator.Evaluate(unassigned, measurements, tracks)
                });

                // create new track hypothesis (low priority)
                var newTrackHyp = leaf.Hypothesis.Clone();
                newTrackHyp.Assignments[mIndex] = nextTrackId++;
                leaf.AddChild(new MhTreeNode(newTrackHyp)
                {
                    Probability = 0.05
                });
            }
        }
    }

    // prune to maxLeaves best hypotheses
    public void Prune(int maxLeaves)
    {
        var bestLeaves = Root.GetLeaves().OrderByDescending(l => l.Probability).Take(maxLeaves).ToList();
        Root.Children.Clear();
        foreach (var leaf in bestLeaves)
            Root.AddChild(leaf);
    }
}