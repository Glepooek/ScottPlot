namespace ScottPlot.Markers;

internal class FilledTriangleUp : IMarker
{
    public void Render(SKCanvas canvas, SKPaint paint, Pixel center, float size, MarkerStyle markerStyle)
    {
        // Length of each side of triangle = size
        float radius = (float)(size / 1.732); // size / sqrt(3)
        float xOffset = (float)(radius * 0.866); // r * sqrt(3)/2
        float yOffset = radius / 2;

        // Top, right, and left vertices
        SKPoint[] pointsList =
        {
            new (center.X, center.Y - radius),
            new (center.X + xOffset, center.Y + yOffset),
            new (center.X - xOffset, center.Y + yOffset),
        };

        SKPath path = new();
        path.AddPoly(pointsList);

        PixelRect rect = new(center, radius);
        Drawing.FillPath(canvas, paint, path, markerStyle.FillStyle, rect);
        Drawing.DrawPath(canvas, paint, path, markerStyle.OutlineStyle);
    }
}
