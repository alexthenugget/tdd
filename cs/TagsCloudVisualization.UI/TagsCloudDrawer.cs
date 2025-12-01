using System.Drawing;
using TagCloudDrawer.Extensions;

namespace TagCloudDrawer;

public class TagsCloudDrawer
{
    private readonly Color backgroundColor;
    private readonly Color fillingColor;
    private readonly Color outlineColor;
    private const int Indentation = 50;
    
    public TagsCloudDrawer(Color? backgroundColor = null, Color? fillingColor = null, Color? outlineColor = null)
    {
        this.backgroundColor = backgroundColor ?? Color.White;
        this.fillingColor = fillingColor ?? Color.LightSkyBlue;
        this.outlineColor = outlineColor ?? Color.CornflowerBlue;
    }

    public void Draw(List<Rectangle> rectangles, string filename)
    {
        if (rectangles.Count == 0)
        {
            return;
        }
        var cloudBounds = rectangles.GetBounds();
        
        var width = cloudBounds.Width + 2 * Indentation;
        var height = cloudBounds.Height + 2 * Indentation;
        var imageSize = new Size(width, height);
        
        using var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        using var pen = new Pen(outlineColor);
        using var brush = new SolidBrush(fillingColor);
        
        graphics.Clear(backgroundColor);
        
        foreach (var rect in rectangles)
        {
            var drawRectangle = new Rectangle(
                rect.X - cloudBounds.X + Indentation,
                rect.Y - cloudBounds.Y + Indentation,
                rect.Width,
                rect.Height);
            graphics.FillRectangle(brush, drawRectangle);
            graphics.DrawRectangle(pen, drawRectangle);
        }
        bitmap.Save(filename);
    }
}