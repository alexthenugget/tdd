using System.Drawing;

namespace TagsCloudVisualization.Tests;

public static class LayoutTestExtensions
{
    public static double GetTotalArea(this IEnumerable<Rectangle> rectangles)
    {
        return rectangles.Sum(r => r.Width * r.Height);
    }

    public static double GetCloudRadius(this IReadOnlyCollection<Rectangle> rectangles, Point center)
    {
        if (rectangles.Count == 0) return 0;

        return rectangles.Max(r => 
        {
            var dx = Math.Max(Math.Abs(center.X - r.Left), Math.Abs(center.X - r.Right));
            var dy = Math.Max(Math.Abs(center.Y - r.Top), Math.Abs(center.Y - r.Bottom));
            return Math.Sqrt(dx * dx + dy * dy);
        });
    }

    public static double GetDensity(this IReadOnlyCollection<Rectangle> rectangles, Point center)
    {
        var rectanglesArea = rectangles.GetTotalArea();
        var radius = rectangles.GetCloudRadius(center);
        var circleArea = Math.PI * Math.Pow(radius, 2);

        return circleArea == 0 ? 0 : rectanglesArea / circleArea;
    }
}