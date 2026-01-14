namespace RadarTracking2D.Core.Data;

public class Blob
{
    public int Id { get; }
    public List<(int X, int Y)> Pixels { get; } = new();

    public Blob(int id)
    {
        Id = id;
    }
}