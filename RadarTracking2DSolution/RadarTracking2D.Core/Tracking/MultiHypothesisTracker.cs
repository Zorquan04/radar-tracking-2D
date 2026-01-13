using RadarTracking2D.Core.Data;
using RadarTracking2D.Core.Statistics;

namespace RadarTracking2D.Core.Tracking;

public class MultiHypothesisTracker
{
    private readonly List<Track> _tracks = new();
    private readonly double _gateDistance = 15.0; // permissible distance
    private int _nextId = 1;

    public IReadOnlyList<Track> Tracks => _tracks;

    public void ProcessFrame(List<Detection> detections)
    {
        var usedDetections = new HashSet<Detection>();

        // assigning detection to existing tracks
        foreach (var track in _tracks)
        {
            // position prediction
            var pred = track.Predict(); // returns (PredX, PredY) as ValueTuple
            double predX = pred.PredX;
            double predY = pred.PredY;

            var closest = detections.Where(d => !usedDetections.Contains(d)).OrderBy(d => Math.Sqrt(Math.Pow(d.X - predX, 2) + Math.Pow(d.Y - predY, 2))).FirstOrDefault();

            if (closest != null && Distance(predX, predY, closest) <= _gateDistance)
            {
                usedDetections.Add(closest);

                // track update with new measurement
                var newDist = new GaussianDistribution(closest.X, closest.Y, track.Distribution.StdDevX, track.Distribution.StdDevY);
                track.Update(newDist);
            }
        }

        // creating new tracks for unassigned detections
        foreach (var d in detections)
        {
            if (!usedDetections.Contains(d))
            {
                var dist = new GaussianDistribution(d.X, d.Y, 1.0, 1.0);
                _tracks.Add(new Track(_nextId++, dist));
            }
        }
    }

    private double Distance(double x1, double y1, Detection d) => Math.Sqrt(Math.Pow(x1 - d.X, 2) + Math.Pow(y1 - d.Y, 2));
}