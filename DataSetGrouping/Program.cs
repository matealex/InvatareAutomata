using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using DataSetGenerator;

namespace DataSetGrouping
{
	class MainClass
	{
		static Random r;
		static List<Color> colors;


		public static void Main(string[] args)
		{
			r = new Random();
			colors = new List<Color>
			{
				Color.Red,Color.Green,Color.Blue,Color.Yellow,Color.Brown,Color.Pink,
				Color.Azure,Color.Black,Color.DarkRed,Color.Violet
			};
			int limit = 400;

			////////
			/// READ FILE

			StreamReader file = new StreamReader("output.txt");

			var points = new List<PointF>();
			string t = string.Empty;

			while ((t = file.ReadLine()) != null)
			{
				var components = t.Split(' ');
				float x = float.Parse(components[0]);
				float y = float.Parse(components[1]);
				points.Add(new PointF { X = x, Y = y });
			}

			////////
			/// GENERATE CENTROIDS

			List<Zone> centroids = new List<Zone>();

			var centroidNumber = r.Next(2, colors.Count);

			for (int i = 0; i < centroidNumber; i++)
			{
				centroids.Add(GenerateCentroid(limit));
			}

			Console.WriteLine("initial centroids");
			foreach (var c in centroids)
			{
				Console.WriteLine(c.Point.X + " " + c.Point.Y);
			}


			////////

			int iteration = 0;
			double previousE = float.MaxValue;

			while (true)
			{

				var groupedPoints = new List<PointD>();
				foreach (var p in points)
				{
					var min = double.MaxValue;
					double sim = 0;
					Zone bestCentroid = null;

					foreach (var c in centroids)
					{
						sim = Similarity(p, c);
						if (sim < min)
						{
							min = sim;
							bestCentroid = c;
						}
					}

					groupedPoints.Add(new PointD { Point = p, Zone = bestCentroid });
				}

				////////
				var terminate = true;

				List<Zone> newCentroids = new List<Zone>();
				foreach (var c in centroids)
				{
					try
					{
						var newPoint = WeightCenter(groupedPoints.Where(p => p.Zone.Equals(c)).ToList());
						newCentroids.Add(new Zone(newPoint, 0, 0, c.DrawColor));
					}
					catch
					{
						terminate = false;
						colors.Add(c.DrawColor);
						var newC = GenerateCentroid(limit);
						newCentroids.Add(newC);
					}
				}
				centroids = newCentroids;


				//Console.WriteLine("new centroids");
				//foreach (var c in centroids)
				//{
				//	Console.WriteLine(c.Point.X + " " + c.Point.Y);
				//}

				////////

				double currentE = 0;
				foreach (var p in groupedPoints)
				{
					currentE += Convergence(p.Point, p.Zone);
				}

				////////
				/// SAVE IMAGE

				iteration++;
				DataSetGenerator.Image image = new DataSetGenerator.Image(iteration + ".bmp", limit, 1);
				foreach (var p in groupedPoints)
				{
					image.SetPixel(p);
				}
				foreach (var c in centroids)
				{
					image.DrawCircle(new PointD { Point = c.Point, Zone = c });
				}
				image.Save();

				////////





				if (Math.Abs(currentE - previousE) < 1  && terminate)
				{
					return;
				}


				Console.WriteLine(Math.Abs(currentE - previousE));
				previousE = currentE;
			}
		}

		public static Zone GenerateCentroid(int limit)
		{
			int x = (int)(2 * limit * r.NextDouble() - limit);
			int y = (int)(2 * limit * r.NextDouble() - limit);
			var colorIndex = r.Next(0, colors.Count);
			var color = colors[colorIndex];
			colors.RemoveAt(colorIndex);

			return new Zone(new Point { X = x, Y = y }, 0, 0, color);
		}

		public static double Similarity(PointF point, Zone centroid)
		{
			//return DistanceEuclid(point, centroid);
			return DistanceCos(point, centroid);
		}

		public static double Convergence(PointF point, Zone centroid)
		{
			return DistanceEuclid(point, centroid);
		}

		public static double DistanceEuclid(PointF point, Zone centroid)
		{
			return Math.Sqrt((point.X - centroid.Point.X) * (point.X - centroid.Point.X) +
									(point.Y - centroid.Point.Y) * (point.Y - centroid.Point.Y));
		}

		public static double DistanceManhattan(PointF point, Zone centroid)
		{
			return Math.Abs(point.X - centroid.Point.X) +
					   Math.Abs(point.Y - centroid.Point.Y);
		}

		public static double DistanceCos(PointF point, Zone centroid)
		{
			return 1 - ((point.X * centroid.Point.X + point.Y * centroid.Point.Y) /
				(Math.Sqrt((point.X * point.X + point.Y * point.Y)) *
				 Math.Sqrt((centroid.Point.X * centroid.Point.X + centroid.Point.Y * centroid.Point.Y))));
		}

		public static Point WeightCenter(List<PointD> points)
		{
			if (points.Count == 0)
			{
				throw new Exception();
			}

			float x = 0, y = 0;

			foreach (var p in points)
			{
				x += p.Point.X;
				y += p.Point.Y;
			}
			x = x / points.Count;
			y = y / points.Count;

			return new Point { X = (int)x, Y = (int)y };
		}
	}
}
