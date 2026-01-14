namespace RadarTracking2D.Core.Data;

public class Blob
{
    public int Id { get; } // unique identifier
    public List<(int X, int Y)> Pixels { get; } = new(); // all pixels belonging to this blob

    public Blob(int id)
    {
        Id = id; // assign ID at creation
    }
}