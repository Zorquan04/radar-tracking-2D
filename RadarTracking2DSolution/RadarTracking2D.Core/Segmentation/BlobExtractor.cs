using RadarTracking2D.Core.Statistics;

namespace RadarTracking2D.Core.Segmentation;

public class BlobExtractor
{
    private readonly ConnectedComponentLabeling _ccl = new();

    public List<BlobStatistics> Extract(bool[,] binary)
    {
        var blobs = _ccl.Process(binary);
        return blobs.Select(b => new BlobStatistics(b)).ToList();
    }
}