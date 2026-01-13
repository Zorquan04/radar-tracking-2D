using RadarTracking2D.Core.Data;

namespace RadarTracking2D.Core.Segmentation;

public class ConnectedComponentLabeling
{
    private int _currentLabel;
    private readonly Dictionary<int, int> _labelEquivalences = new();

    public List<Blob> Process(
        bool[,] binaryImage,
        NeighborhoodType neighborhood = NeighborhoodType.Four)
    {
        int height = binaryImage.GetLength(0);
        int width = binaryImage.GetLength(1);

        int[,] labels = new int[height, width];
        _currentLabel = 1;
        _labelEquivalences.Clear();

        // First pass
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (!binaryImage[y, x])
                    continue;

                var neighbors = GetNeighborLabels(labels, x, y, neighborhood);
                if (neighbors.Count == 0)
                {
                    labels[y, x] = _currentLabel;
                    _labelEquivalences[_currentLabel] = _currentLabel;
                    _currentLabel++;
                }
                else
                {
                    int minLabel = neighbors.Min();
                    labels[y, x] = minLabel;

                    foreach (var label in neighbors)
                        Union(minLabel, label);
                }
            }
        }

        // Second pass + blob building
        var blobs = new Dictionary<int, Blob>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int label = labels[y, x];
                if (label == 0)
                    continue;

                int root = Find(label);

                if (!blobs.ContainsKey(root))
                    blobs[root] = new Blob(root);

                blobs[root].Pixels.Add((x, y));
            }
        }

        return blobs.Values.ToList();
    }

    // helpers
    private List<int> GetNeighborLabels(int[,] labels, int x, int y, NeighborhoodType neighborhood)
    {
        var result = new List<int>();

        void TryAdd(int nx, int ny)
        {
            if (nx >= 0 && ny >= 0 && ny < labels.GetLength(0) && nx < labels.GetLength(1))
            {
                int label = labels[ny, nx];
                if (label > 0)
                    result.Add(label);
            }
        }

        // 4-neighborhood
        TryAdd(x - 1, y);     // left
        TryAdd(x, y - 1);     // up

        if (neighborhood == NeighborhoodType.Eight)
        {
            TryAdd(x - 1, y - 1);
            TryAdd(x + 1, y - 1);
        }

        return result;
    }

    private int Find(int label)
    {
        if (_labelEquivalences[label] != label)
            _labelEquivalences[label] = Find(_labelEquivalences[label]);

        return _labelEquivalences[label];
    }

    private void Union(int a, int b)
    {
        int rootA = Find(a);
        int rootB = Find(b);

        if (rootA != rootB)
            _labelEquivalences[rootB] = rootA;
    }
}