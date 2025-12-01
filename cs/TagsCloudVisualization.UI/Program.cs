using System.Drawing;
using TagCloudDrawer;

namespace TagsCloudVisualization.UI;

class Program
{
    private static readonly Point Center = new Point(2, 2);
    
    static void Main(string[] args)
    {
        GenerateElongatedRectangles();
        GenerateSmallRectangles();
        GenerateDifferentRectangles();
    }
    private static void GenerateElongatedRectangles()
    {
        var layouter = new CircularCloudLayouter(Center);
        
        var rects = layouter.PutRectangles(100, () => 
            new Size(Random.Shared.Next(50, 150), Random.Shared.Next(20, 40)));

        var visualizer = new TagsCloudDrawer();
        visualizer.Draw(rects, "sample1_with_elongated_rects.png");
    }

    private static void GenerateSmallRectangles()
    {
        var layouter = new CircularCloudLayouter(Center);
        
        var rects = layouter.PutRectangles(200, () => 
            new Size(Random.Shared.Next(10, 30), Random.Shared.Next(10, 30)));

        var visualizer = new TagsCloudDrawer(
            backgroundColor: Color.Black,
            fillingColor: Color.DarkGreen,
            outlineColor: Color.LimeGreen
        );
        visualizer.Draw(rects, "sample2_with_small_rects.png");
    }

    private static void GenerateDifferentRectangles()
    {
        var layouter = new CircularCloudLayouter(Center);
        var rects = new List<Rectangle>();
        
        rects.AddRange(layouter.PutRectangles(10, () => 
            new Size(Random.Shared.Next(100, 200), Random.Shared.Next(50, 100))));
        
        rects.AddRange(layouter.PutRectangles(50, () => 
            new Size(Random.Shared.Next(10, 30), Random.Shared.Next(10, 30))));

        var visualizer = new TagsCloudDrawer(
            backgroundColor: Color.OldLace,
            fillingColor: Color.Orange,
            outlineColor: Color.DarkRed
        );
        visualizer.Draw(rects, "sample3_with_different_rects.png");
    }
}