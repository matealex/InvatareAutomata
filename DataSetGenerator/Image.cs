using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DataSetGenerator;

namespace DataSetGenerator
{

	public class Image
	{
		string path;
		int size;
		int scale;
		Bitmap image;

		public Image(string path, int size, int scale)
		{
			this.path = path;
			this.size = size;
			this.scale = scale;

			image = new Bitmap(2 * size * scale, 2 * size * scale);
		}

		public void SetPixel(PointD point, Color color)
		{
			var t = ImageCoordinates(point);
			image.SetPixel(t.X, t.Y, color);
		}

		public void Save()
		{
			image.Save(path);
		}

		private Point ImageCoordinates(PointD point)
		{
			var x = (int)(scale * (point.Point.X + (double)size));
			var y = (int)(scale * ((double)size - point.Point.Y));
			return new Point(x, y);
		}

	}
}