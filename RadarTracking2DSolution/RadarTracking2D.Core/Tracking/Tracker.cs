using RadarTracking2D.Core.Statistics;

namespace RadarTracking2D.Core.Tracking;

// main tracker: MHT + motion model + Gaussian updates
public class Tracker
{
    private readonly Dictionary<int, Track> _tracks = new();
    private readonly List<Track> _manualTracks = new();
    private readonly MhTree _tree = new MhTree();
    private int _nextTrackId = 1;
    public int GetNextId() => _nextTrackId++;

    public void AddManualTrack(Track track)
    {
        _manualTracks.Add(track);
    }

    public void ProcessFrame(List<BlobStatistics> measurements)
    {
        if (!measurements.Any())
        {
            Console.WriteLine("No measurements -> frame skipped");
            return;
        }
        
        _tree.Expand(measurements, _tracks.Values.ToList(), ref _nextTrackId);
        _tree.Prune(8);

        // pick best leaves
        var leaves = _tree.Root.GetLeaves().OrderByDescending(l => l.Probability).Take(5).ToList();

        foreach (var leaf in leaves)
        {
            foreach (var kv in leaf.Hypothesis.Assignments)
            {
                int mIndex = kv.Key;
                int? trackId = kv.Value;

                if (trackId == null || mIndex < 0 || mIndex >= measurements.Count)
                    continue;

                var meas = measurements[mIndex];

                if (!_tracks.TryGetValue(trackId.Value, out var track))
                {
                    // new track
                    track = new Track(trackId.Value, new GaussianDistribution(meas.MeanX, meas.MeanY, meas.StdDevX, meas.StdDevY));
                    _tracks[trackId.Value] = track;
                }
                else
                {
                    // update existing track
                    var oldX = track.Distribution.MeanX;
                    var oldY = track.Distribution.MeanY;
                    track.Update(new GaussianDistribution(meas.MeanX, meas.MeanY, meas.StdDevX, meas.StdDevY));
                }
            }
        }

        foreach (var t in _tracks.Values.Concat(_manualTracks))
        {
            if (t.UpdatedThisFrame)
                t.Age = 0;
            else
                t.Age += 1;
        }
    }

    public List<Track> GetTracks() => _tracks.Values.Concat(_manualTracks).ToList();
}