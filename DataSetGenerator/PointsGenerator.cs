using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DataSetGenerator
{
    public class PointsGenerator
    {
        private readonly Random random;
        private readonly int range;
        private readonly List<Zone> zones;

        public PointsGenerator(List<Zone> zones, int range)
        {
            random = new Random();
            this.zones = zones;
            this.range = range;
        }

        public List<PointD> Generate(int numberOfPoints)
        {
            var points = new List<PointD>();
            for (var i = 0; i < numberOfPoints; i++)
            {
                var zoneIndex = random.Next(0, zones.Count);
                points.Add(new PointD
                {
                    Point = GeneratePointForZone(zones[zoneIndex]),
                    Zone = zones[zoneIndex]
                }
                    );
            }

            var outputFile = new StreamWriter("output.txt");
            foreach (var pointD in points)
            {
                outputFile.WriteLine($"{pointD.Point.X} {pointD.Point.Y} {zones.IndexOf(pointD.Zone)}");
            }

            return points;
        }

        private PointF GeneratePointForZone(Zone zone)
        {
            var x = GenerateValue(zone.Point.X, zone.XOffset);
            var y = GenerateValue(zone.Point.Y, zone.YOffset);
            return new PointF(x, y);
        }

        private float GenerateValue(double m, double sigma)
        {
            while (true)
            {
                var x = 2*range*random.NextDouble() - range;
                var gx = Math.Pow(Math.E, -Math.Pow(m - x, 2)/(2*Math.Pow(sigma, 2)));

				var pa = random.Next(0, 100000) / 100000.0;
                if (gx >= pa)
                {
                    return (float) x;
                }
            }
        }
    }
}