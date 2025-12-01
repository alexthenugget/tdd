using System.Drawing;

namespace TagCloudDrawer.Extensions;

public static class RectangleExtensions
{
    public static Rectangle GetBounds(this IEnumerable<Rectangle> rectangles)
    {
        var minX = rectangles.Min(r => r.Left);
        var maxX = rectangles.Max(r => r.Right);
        var minY = rectangles.Min(r => r.Top);
        var maxY = rectangles.Max(r => r.Bottom);

        return new Rectangle(minX, minY, maxX - minX, maxY - minY);
    }
}