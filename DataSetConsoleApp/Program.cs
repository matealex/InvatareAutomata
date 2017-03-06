using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DataSetGenerator;

namespace DataSetConsoleApp
{
    internal class Program
    {
        private static readonly int scaleFactor = 1;

        private static void Main(string[] args)
        {
            var zones = new List<Zone>
            {
                new Zone(new Point(150, 150), 10, 50, Color.Red),
                new Zone(new Point(-150, 150), 40, 50, Color.Blue),
                new Zone(new Point(-150, -150), 80, 100, Color.Green)
            };


            var interval = 400;
            var pointsGenerator = new PointsGenerator(zones, interval);
            var points = pointsGenerator.Generate(zones.Count*10000);

            var image = new Bitmap(2*interval*scaleFactor, 2*interval*scaleFactor);
            var g = Graphics.FromImage(image);

            foreach (var zone in zones)
            {
                foreach (var pointD in points.Where(p => p.Zone.Equals(zone)))
                {
                    var coordinates = ImageCoordinates(pointD, interval);
                    image.SetPixel(coordinates.X, coordinates.Y, zone.DrawColor);
                }

                var p1 = ImageCoordinates(new PointD
                {
                    Point = new PointF((float) (zone.Point.X - zone.XOffset/2),
                        (float) (zone.Point.Y + zone.YOffset/2))
                }, interval);

                var r1 = new Rectangle(
                    p1.X,
                    p1.Y,
                    (int) (zone.XOffset*scaleFactor),
                    (int) (zone.YOffset*scaleFactor));
                g.DrawRectangle(Pens.Yellow, r1);
            }

            image.Save("output.bmp");
        }

        private static Point ImageCoordinates(PointD point, int size)
        {
            var x = (int) (scaleFactor*(point.Point.X + (double) size));
            var y = (int) (scaleFactor*((double) size - point.Point.Y));
            return new Point(x, y);
        }
    }
}