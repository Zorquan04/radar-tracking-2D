using RadarTracking2D.Core.Tracking.Association;
using RadarTracking2D.Core.Statistics;

namespace RadarTracking2D.Core.Tracking;

public class MhTree
{
    public MhTreeNode Root { get; } = new(new Hypothesis());

    public void Expand(List<BlobStatistics> measurements, List<Track> tracks, ref int nextTrackId)
    {
        var leaves = Root.GetLeaves().ToList();

        foreach (var leaf in leaves)
        {
            foreach (var mIndex in Enumerable.Range(0, measurements.Count))
            {
                var measurement = measurements[mIndex];

                // no tracks -> we are creating a new one
                if (!tracks.Any())
                {
                    var hyp = leaf.Hypothesis.Clone();
                    hyp.Assignments[mIndex] = nextTrackId++;

                    leaf.AddChild(new MhTreeNode(hyp) { Probability = 1.0 });
                    continue;
                }

                // assignment to existing tracks
                foreach (var track in tracks)
                {
                    var hyp = leaf.Hypothesis.Clone();
                    hyp.Assignments[mIndex] = track.Id;

                    double prob = ProbabilityCalculator.Evaluate(hyp, measurements, tracks);

                    leaf.AddChild(new MhTreeNode(hyp) { Probability = prob });
                }

                // unassigned measurement (noise)
                var unassigned = leaf.Hypothesis.Clone();
                unassigned.Assignments[mIndex] = null;
                leaf.AddChild(new MhTreeNode(unassigned)
                {
                    Probability = ProbabilityCalculator.Evaluate(unassigned, measurements, tracks)
                });

                // new track
                var newTrackHyp = leaf.Hypothesis.Clone();
                newTrackHyp.Assignments[mIndex] = nextTrackId++;
                leaf.AddChild(new MhTreeNode(newTrackHyp)
                {
                    Probability = 0.05 // low priority
                });
            }
        }
    }

    public void Prune(int maxLeaves)
    {
        var bestLeaves = Root.GetLeaves().OrderByDescending(l => l.Probability).Take(maxLeaves).ToList();

        Root.Children.Clear();

        foreach (var leaf in bestLeaves)
            Root.AddChild(leaf);
    }
}