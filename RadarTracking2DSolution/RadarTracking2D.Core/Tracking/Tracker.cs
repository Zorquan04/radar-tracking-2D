using RadarTracking2D.Core.Statistics;

namespace RadarTracking2D.Core.Tracking;

public class Tracker
{
    private List<Track> _tracks = new();
    private readonly MhTree _tree = new();
    private int _nextTrackId = 1;

    public Tracker() { }

    public void ProcessFrame(List<BlobStatistics> measurements)
    {
        if (measurements.Count == 0) return;

        // we develop a tree of hypotheses
        _tree.Expand(measurements, _tracks, ref _nextTrackId);

        // we collect the most probable leaves
        var leaves = _tree.Root.GetLeaves().OrderByDescending(l => l.Probability).Take(10).ToList(); // we limit the top 10 hypotheses

        var updatedTracks = new Dictionary<int, Track>();

        foreach (var leaf in leaves)
        {
            foreach (var kv in leaf.Hypothesis.Assignments)
            {
                int mIndex = kv.Key;
                int? trackId = kv.Value;

                // skip assignments that are out of bounds for this frame
                if (mIndex < 0 || mIndex >= measurements.Count)
                    continue;

                if (trackId == null) 
                    continue; // measurement as a disturbance

                var meas = measurements[mIndex];

                if (updatedTracks.ContainsKey(trackId.Value))
                {
                    // already updated
                    var existing = updatedTracks[trackId.Value];
                    existing.Distribution = new GaussianDistribution(meas.MeanX, meas.MeanY, meas.StdDevX, meas.StdDevY);
                }
                else
                {
                    var existingGlobal = _tracks.FirstOrDefault(t => t.Id == trackId.Value);
                    if (existingGlobal != null)
                    {
                        existingGlobal.Distribution = new GaussianDistribution(meas.MeanX, meas.MeanY, meas.StdDevX, meas.StdDevY);
                        updatedTracks[trackId.Value] = existingGlobal;
                    }
                    else
                    {
                        var newTrack = new Track(trackId.Value, new GaussianDistribution(meas.MeanX, meas.MeanY, meas.StdDevX, meas.StdDevY));
                        updatedTracks[trackId.Value] = newTrack;

                        if (trackId.Value >= _nextTrackId)
                            _nextTrackId = trackId.Value + 1;
                    }
                }
            }
        }

        // track list synchronization
        _tracks = updatedTracks.Values.ToList();
    }

    public List<Track> GetTracks() => _tracks;
}