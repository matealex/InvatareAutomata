using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var zones = new List<Zone>
            {
                new Zone(new Point(150, 150), 10, 50, Color.Red),
                new Zone(new Point(-150, 150), 40, 50, Color.Blue),
                new Zone(new Point(-150, -150), 80, 100, Color.Green)
            };
            var interval = 400;
            var pointsGenerator = new PointsGenerator(zones, interval);
            var points = pointsGenerator.Generate(zones.Count*10000);

            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.Minimum = -interval;
            chart1.ChartAreas[0].AxisX.Maximum = interval;
            chart1.ChartAreas[0].AxisY.Minimum = -interval;
            chart1.ChartAreas[0].AxisY.Maximum = interval;

            foreach (var zone in zones)
            {
                var series = new Series
                {
                    ChartType = SeriesChartType.Point,
                    Color = zone.DrawColor
                };

                foreach (var pointD in points.Where(p => p.Zone.Equals(zone)))
                {
                    series.Points.AddXY(pointD.Point.X, pointD.Point.Y);
                }
                chart1.Series.Add(series);
            }

            var outputFile = new StreamWriter("output.txt");
            foreach (var pointD in points)
            {
                outputFile.WriteLine($"{pointD.Point.X} {pointD.Point.Y} {zones.IndexOf(pointD.Zone)}");
            }
        }
    }
}