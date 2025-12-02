using System.Drawing;

namespace TagsCloudVisualization;

public static class CircularCloudLayouterExtensions
{
    public static List<Rectangle> PutRectangles(this CircularCloudLayouter layouter, int count, Func<Size> sizeGenerator)
    {
        var result = new List<Rectangle>();
        for (var i = 0; i < count; i++)
        {
            result.Add(layouter.PutNextRectangle(sizeGenerator()));
        }
        return result;
    }
}