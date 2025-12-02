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

    public Bitmap Draw(IEnumerable<Rectangle> rectangles)
    {
        var rectsList = rectangles.ToList();
        if (rectsList.Count == 0)
        {
            var emptySize = new Size(150, 150); 
        
            var emptyBitmap = new Bitmap(emptySize.Width, emptySize.Height);
            
            using var emptyGraphics = Graphics.FromImage(emptyBitmap);
        
            emptyGraphics.Clear(backgroundColor);
        
            return emptyBitmap;
        }
        
        var cloudBounds = rectsList.GetBounds();
        
        var width = cloudBounds.Width + 2 * Indentation;
        var height = cloudBounds.Height + 2 * Indentation;
        
        var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);
        using var pen = new Pen(outlineColor);
        using var brush = new SolidBrush(fillingColor);
        
        graphics.Clear(backgroundColor);
        
        foreach (var rect in rectsList)
        {
            var drawRectangle = new Rectangle(
                rect.X - cloudBounds.X + Indentation,
                rect.Y - cloudBounds.Y + Indentation,
                rect.Width,
                rect.Height);
            graphics.FillRectangle(brush, drawRectangle);
            graphics.DrawRectangle(pen, drawRectangle);
        }
        return bitmap;
    }
}