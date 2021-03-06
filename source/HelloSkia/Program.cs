using SkiaSharp;
using System;
using System.IO;

namespace HelloSkia
{
    class Program
    {
        const string  HelloMessage = "Hello Skia!";

        static void Main(string[] args)
        {
            // Create an SKSurface object
            var info = new SKImageInfo(300, 50);
            using (var surface = SKSurface.Create(info))
            {
                // Create a blank SKCanvas
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.LightBlue);

                // Draw the message to canvas
                var paint = new SKPaint
                {
                    Color = SKColors.Black,
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill,
                    TextAlign = SKTextAlign.Center,
                    TextSize = 24
                };
                var coord = new SKPoint(info.Width / 2, (info.Height + paint.TextSize) / 2);
                canvas.DrawText(HelloMessage, coord, paint);

                // Save the SKCanvas as png image file.
                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite("output.png"))
                {
                    data.SaveTo(stream);
                }
            }

        }
    }
}
