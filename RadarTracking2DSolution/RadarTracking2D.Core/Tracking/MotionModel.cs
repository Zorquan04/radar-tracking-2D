namespace RadarTracking2D.Core.Tracking;

public class MotionModel(double velX = 0, double velY = 0)
{
    public double VelX { get; private set; } = velX;
    public double VelY { get; private set; } = velY;

    // position prediction based on previous position
    public (double PredX, double PredY) Predict(double currentX, double currentY)
    {
        return (currentX + VelX, currentY + VelY);
    }

    // speed update based on offset
    public void Update(double newX, double newY, double oldX, double oldY)
    {
        VelX = newX - oldX;
        VelY = newY - oldY;
    }
}