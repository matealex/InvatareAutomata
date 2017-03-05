using System.Drawing;

namespace Lab1
{
    public class Zone
    {
        public Color DrawColor;
        public Point Point;
        public double XOffset;
        public double YOffset;

        public Zone(Point point, double xOffset, double yOffset, Color drawColor)
        {
            Point = point;
            XOffset = xOffset;
            YOffset = yOffset;
            DrawColor = drawColor;
        }
    }

    public struct PointD
    {
        public PointF Point;
        public Zone Zone;
    }
}