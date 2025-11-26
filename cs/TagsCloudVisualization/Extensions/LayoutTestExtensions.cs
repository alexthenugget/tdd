using System.Drawing;

namespace TagsCloudVisualization.Tests;

public static class LayoutTestExtensions
{
    private static double GetTotalArea(this List<Rectangle> rectangles)
    {
        return rectangles.Sum(r => r.Width * r.Height);
    }

    private static double GetCloudRadius(this List<Rectangle> rectangles, Point center)
    {
        if (rectangles.Count == 0) return 0;

        return rectangles
            .SelectMany(r => new[]
            {
                new Point(r.Left, r.Top),
                new Point(r.Right, r.Top),
                new Point(r.Right, r.Bottom),
                new Point(r.Left, r.Bottom)
            })
            .Max(p => Math.Sqrt(Math.Pow(p.X - center.X, 2) + Math.Pow(p.Y - center.Y, 2)));
    }

    public static double GetDensity(this List<Rectangle> rectangles, Point center)
    {
        var rectanglesArea = rectangles.GetTotalArea();
        var radius = rectangles.GetCloudRadius(center);
        var circleArea = Math.PI * Math.Pow(radius, 2);

        return circleArea == 0 ? 0 : rectanglesArea / circleArea;
    }
}