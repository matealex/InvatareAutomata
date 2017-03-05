using System;
using System.Collections.Generic;
using System.Drawing;

namespace Lab1
{
    public class PointsGenerator
    {
        private readonly Random random;
        private readonly List<Zone> zones;
        private readonly int range;

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
                var pa = random.NextDouble();
                if (gx > pa)
                {
                    return (float) x;
                }
            }
        }
    }
}