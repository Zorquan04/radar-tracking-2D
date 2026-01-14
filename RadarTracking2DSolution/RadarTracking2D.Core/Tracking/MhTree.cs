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

                // no tracks yet -> create new
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

                    // Boost slightly, but not too much
                    prob = Math.Max(prob, 0.1); // minimum probability for track assignment
                    if (leaf.Hypothesis.Assignments.ContainsValue(track.Id))
                        prob *= 1.5; // history gives a small bonus

                    leaf.AddChild(new MhTreeNode(hyp) { Probability = prob });
                }

                // measurement is noise
                var unassigned = leaf.Hypothesis.Clone();
                unassigned.Assignments[mIndex] = null;
                double noiseProb = ProbabilityCalculator.Evaluate(unassigned, measurements, tracks);
                noiseProb = Math.Max(noiseProb, 0.05);
                leaf.AddChild(new MhTreeNode(unassigned)
                {
                    Probability = noiseProb
                });

                // create new track hypothesis
                var newTrackHyp = leaf.Hypothesis.Clone();
                newTrackHyp.Assignments[mIndex] = nextTrackId++;
                leaf.AddChild(new MhTreeNode(newTrackHyp)
                {
                    Probability = 0.2
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