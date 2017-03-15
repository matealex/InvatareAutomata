﻿using System.Collections.Generic;
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
                new Zone(new Point(350, 350), 5, 10, Color.Red),
                new Zone(new Point(150, 150), 10, 10, Color.Blue),
                new Zone(new Point(-150, 350), 10, 5, Color.Green)
            };

            var interval = 400;
            var pointsGenerator = new PointsGenerator(zones, interval);
            var points = pointsGenerator.Generate(zones.Count*10000);

			var image = new DataSetGenerator.Image("output.bmp", interval, scaleFactor);

            foreach (var zone in zones)
            {
                foreach (var pointD in points.Where(p => p.Zone.Equals(zone)))
                {
					image.SetPixel(pointD, zone.DrawColor);
                }
            }

            image.Save();
        }
    }

	
}