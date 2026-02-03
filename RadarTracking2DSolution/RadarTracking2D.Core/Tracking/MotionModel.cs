namespace RadarTracking2D.Core.Tracking;

// simple constant-velocity motion model
public class MotionModel(double velX = 0, double velY = 0)
{
    public double VelX { get; private set; } = velX;
    public double VelY { get; private set; } = velY;

    // update velocity based on movement
    public void Update(double newX, double newY, double oldX, double oldY)
    {
        VelX = newX - oldX;
        VelY = newY - oldY;
    }
}