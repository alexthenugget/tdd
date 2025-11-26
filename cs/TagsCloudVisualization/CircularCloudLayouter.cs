using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point center;
    private readonly List<Rectangle> rectangles;
    
    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        this.rectangles = new List<Rectangle>();
    }
    
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var spiralPoints = GetSpiralPoints();
        foreach (var point in spiralPoints)
        {
            var newPoint = new Point(point.X -  rectangleSize.Width/2, point.Y - rectangleSize.Height/2);
            var rectangle = new Rectangle(newPoint, rectangleSize);
            if (!IsIntersecting(rectangle))
            {
                var compactedRectangle = CompactRectangle(rectangle);
                rectangles.Add(compactedRectangle);
                return compactedRectangle;
            }
        }
        throw new Exception("Не найдено место для нового прямоугольника.");
    }

    private IEnumerable<Point> GetSpiralPoints()
    {
        double angle = 0;
        const double spiralStep = 0.5;
        const double angleStep = 0.1;
        while (true)
        {
            var radius = spiralStep * angle;
            var x = center.X + (int)(radius * Math.Cos(angle));
            var y = center.Y + (int)(radius * Math.Sin(angle));
            
            yield return new Point(x, y);
            
            angle += angleStep;
        }
    }

    private Rectangle CompactRectangle(Rectangle rectangle)
    {
        bool wasShifted;
        do
        {
            wasShifted = false;
            
            var rectCenter = GetRectangleCenter(rectangle);

            var dx = center.X - rectCenter.X;
            var dy = center.Y - rectCenter.Y;
            
            var stepX = dx == 0 ? 0 : Math.Sign(dx);
            var stepY = dy == 0 ? 0 : Math.Sign(dy);

            if (stepX != 0)
            {
                var newRectX = rectangle with { X = rectangle.X + stepX };
                if (!IsIntersecting(newRectX))
                {
                    rectangle = newRectX;
                    wasShifted = true;
                }
            }

            if (stepY != 0)
            {
                var newRectY = rectangle with { Y = rectangle.Y + stepY };
                if (!IsIntersecting(newRectY))
                {
                    rectangle = newRectY;
                    wasShifted = true;
                }
            }

        } while (wasShifted);

        return rectangle;
    }
    
    private bool IsIntersecting(Rectangle rectangle)
    {
        return rectangles.Any(r => r.IntersectsWith(rectangle));
    }
    
    private Point GetRectangleCenter(Rectangle rect)
    {
        return new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
    }
}