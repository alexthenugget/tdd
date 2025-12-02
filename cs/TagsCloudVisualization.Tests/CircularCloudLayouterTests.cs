using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagCloudDrawer;

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
    
    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            SaveFailedTestImage();
    }
    
    private void SaveFailedTestImage()
    {
        var testName = TestContext.CurrentContext.Test.Name;
        var projectPath = TestContext.CurrentContext.TestDirectory;
        var filename = Path.Combine(projectPath, $"{testName}_Failed.png");

        var visualizer = new TagsCloudDrawer(Color.White, Color.Red, Color.DarkRed);
        
        using var bitmap = visualizer.Draw(layouter.Rectangles);
        bitmap.Save(filename);
        
        TestContext.Out.WriteLine($"Tag cloud visualization saved to file {filename}");
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
        for (int i = 0; i < 50; i++)
        {
            var rectangle = layouter.PutNextRectangle(defaultSize);
            
            layouter.Rectangles.Should().NotContain(r => r != rectangle && r.IntersectsWith(rectangle));
        }
    }
    
    [Test]
    public void PutNextRectangle_RectanglesWithDifferentSizes_ShouldNotIntersectOtherRectangles()
    {
        layouter.PutRectangles(50, () => 
            new Size(Random.Shared.Next(5, 50), Random.Shared.Next(5, 50)));

        var rects = layouter.Rectangles;

        for (var i = 0; i < rects.Count; i++)
        {
            for (var j = i + 1; j < rects.Count; j++)
            {
                var r1 = rects[i];
                var r2 = rects[j];
                r1.IntersectsWith(r2).Should().BeFalse();
            }
        }
    }

    [Test]
    public void PutNextRectangle_MultipleRectangles_ShouldBeCircular()
    {
        layouter.PutRectangles(150, () => 
            new Size(Random.Shared.Next(5, 50), Random.Shared.Next(5, 50)));
            
        var rectsList = layouter.Rectangles;

        var totalArea = rectsList.GetTotalArea();
        var expectedRadius = Math.Sqrt(totalArea / Math.PI);
        var realRadius = rectsList.GetCloudRadius(center);

        (realRadius / expectedRadius).Should().BeLessThan(1.35);
    }

    [Test]
    public void Layout_MultipleRectangles_ShouldBePlacedTightly()
    {
        layouter.PutRectangles(50, () => 
            new Size(Random.Shared.Next(5, 50), Random.Shared.Next(5, 50)));
        
        var density = layouter.Rectangles.GetDensity(center);

        density.Should().BeGreaterThan(0.5);
    }
}