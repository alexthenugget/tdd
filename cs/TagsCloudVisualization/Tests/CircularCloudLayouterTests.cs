using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private Point center;
    private CircularCloudLayouter layouter;
    private Size defaultSize;
    
    [SetUp]
    public void SetUp()
    {
        center = new Point(2, 2);
        layouter = new CircularCloudLayouter(center);
        defaultSize = new Size(6, 4);
    }
    
    [Test]
    public void PutNextRectangle_FirstRectangle_ShouldBePlacedInTheCenter()
    {
        var expectedX = center.X - defaultSize.Width / 2;
        var expectedY = center.Y - defaultSize.Height / 2;
        var expectedRect = new Rectangle(expectedX, expectedY, defaultSize.Width, defaultSize.Height);

        var result = layouter.PutNextRectangle(defaultSize);

        result.Should().Be(expectedRect);
    }

    [Test]
    public void PutNextRectangle_SecondRectangle_ShouldNotIntersectTheCentralRectangle()
    {
        var firstRectangle = layouter.PutNextRectangle(defaultSize);
        var secondRectangle = layouter.PutNextRectangle(defaultSize);
        
        firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
    }
    
    [Test]
    public void PutNextRectangle_MultipleRectangles_ShouldNotIntersectOtherRectangles()
    {
        var rectangles = new List<Rectangle>();
        
        for (int i = 0; i < 50; i++)
        {
            var rectangle = layouter.PutNextRectangle(defaultSize);
            rectangles.Should().NotContain(r => r.IntersectsWith(rectangle));
            rectangles.Add(rectangle);
        }
    }
    
    [Test]
    public void PutNextRectangle_RectanglesWithDifferentSizes_ShouldNotIntersectOtherRectangles()
    {
        var random = new Random();
        var rectangles = layouter.GenerateRectangles(50, () => 
            new Size(random.Next(5, 50), random.Next(5, 50)));
        
        foreach (var r1 in rectangles)
        {
            foreach (var r2 in rectangles)
            {
                if (r1 == r2) continue;
                r1.IntersectsWith(r2).Should().BeFalse();
            }
        }
    }

    [Test]
    public void PutNextRectangle_MultipleRectangles_ShouldBeCircular()
    {
        var random = new Random();
        var rectangles = layouter.GenerateRectangles(50, () => 
            new Size(random.Next(5, 50), random.Next(5, 50)));
        
        var top = rectangles.Min(r => r.Top);
        var bottom = rectangles.Max(r => r.Bottom);
        var left = rectangles.Min(r => r.Left);
        var right = rectangles.Max(r => r.Right);
        
        var width = right - left;
        var height = bottom - top;
        
        double ratio = (double)width / height;
        
        ratio.Should().BeInRange(0.5, 2.0);
    }

    [Test]
    public void Layout_MultipleRectangles_ShouldBePlacedTightly()
    {
        var random = new Random(12345);
        var rectangles = layouter.GenerateRectangles(50, () => 
            new Size(random.Next(5, 50), random.Next(5, 50)));

        var density = rectangles.GetDensity(center);

        density.Should().BeGreaterThan(0.5);
    }
}