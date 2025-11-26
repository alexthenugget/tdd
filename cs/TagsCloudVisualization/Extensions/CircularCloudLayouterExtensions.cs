using System.Drawing;

namespace TagsCloudVisualization;

public static class CircularCloudLayouterExtensions
{
    public static List<Rectangle> GenerateRectangles(this CircularCloudLayouter layouter, int count, Func<Size> sizeGenerator)
    {
        var result = new List<Rectangle>();
        for (int i = 0; i < count; i++)
        {
            result.Add(layouter.PutNextRectangle(sizeGenerator()));
        }
        return result;
    }
}