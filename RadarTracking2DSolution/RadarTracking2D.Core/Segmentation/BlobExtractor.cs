using RadarTracking2D.Core.Data;

namespace RadarTracking2D.Core.Segmentation;

public class BlobExtractor
{
    private readonly ConnectedComponentLabeling _ccl = new();

    public List<Blob> Extract(bool[,] binaryImage, NeighborhoodType neighborhood = NeighborhoodType.Four, int minPixelCount = 5)
    {
        var blobs = _ccl.Process(binaryImage, neighborhood);

        // filtration of small debris (noise)
        return blobs.Where(b => b.PixelCount >= minPixelCount).ToList();
    }
}