using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Hatches
{
    public class Dots : IHatch
    {
        static Dots()
        {
            bmp = CreateBitmap();
        }
        private static SKBitmap bmp;
        private static SKBitmap CreateBitmap()
        {
            var bmp = new SKBitmap(20, 20);
            using var paint = new SKPaint() { Color = Colors.White.ToSKColor() };
            using var path = new SKPath();
            using var canvas = new SKCanvas(bmp);

            paint.IsAntialias = true; // AA is especially important for circles, it seems to do little for the other shapes

            canvas.Clear(Colors.Black.ToSKColor());
            canvas.DrawCircle(5, 5, 5, paint);

            return bmp;
        }

        public SKShader GetShader(Color backgroundColor, Color hatchColor, PixelRect rect)
        {
            return SKShader.CreateBitmap(
                bmp,
                SKShaderTileMode.Repeat,
                SKShaderTileMode.Repeat,
                SKMatrix.CreateScale(0.5f, 0.5f))
                    .WithColorFilter(Drawing.GetMaskColorFilter(hatchColor, backgroundColor));
        }
    }
}
