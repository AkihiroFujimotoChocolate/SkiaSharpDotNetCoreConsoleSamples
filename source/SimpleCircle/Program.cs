using SkiaSharp;
using System;
using System.IO;

namespace SimpleCircle
{
    class Program
    {
        const string OutputFileName = "output.png";
        const int ImageWidth = 500;
        const int ImageHeight = 300;

        static void Main(string[] args)
        {
            var surface = SKSurface.Create(new SKImageInfo(ImageWidth, ImageHeight));
            Draw(surface);
            using var stream = File.OpenWrite(OutputFileName);
            Save(stream, surface);
        }

        // Save surface as a png image file.
        private static void Save(Stream target, SKSurface surface)
        {
            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            data.SaveTo(target);
        }

        // Draw image to surface
        private static void Draw(SKSurface surface)
        {
            // Create a blank canvas
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            // Find the canvas bounds
            var canvasBounds = canvas.DeviceClipBounds;

            // Create an SKPaint object to draw circle
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Red,
                StrokeWidth = 25
            };

            // Draw the circle
            canvas.DrawCircle(canvasBounds.MidX, canvasBounds.MidY, 100, paint);

            // Modify the SKPaint object to fill the circle
            paint.Style = SKPaintStyle.Fill;
            paint.Color = SKColors.Blue;

            // Fill the circle
            canvas.DrawCircle(canvasBounds.MidX, canvasBounds.MidY, 100, paint);
        }
    }
}
