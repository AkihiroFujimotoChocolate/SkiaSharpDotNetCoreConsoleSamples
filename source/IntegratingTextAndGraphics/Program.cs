using SkiaSharp;
using System;
using System.IO;

namespace IntegratingTextAndGraphics
{
    class Program
    {
        const string HelloMessage = "Hello SkiaSharp!";
        const string OutputFileName = "output.png";
        const int ImageWidth = 500;
        const int ImageHeight = 200;

        static void Main(string[] args)
        {
            var surface = SKSurface.Create(new SKImageInfo(ImageWidth, ImageHeight));
            Draw(surface, HelloMessage);
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
        private static void Draw(SKSurface surface, string message)
        {
            // Create a blank canvas
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            // Create an SKPaint object to display the text
            var textPaint = new SKPaint
            {
                Color = SKColors.Chocolate
            };

            // Find the canvas bounds
            var canvasBounds = canvas.DeviceClipBounds;

            // Adjust TextSize property so text is 90% of screen width
            float textWidth = textPaint.MeasureText(message);
            textPaint.TextSize = 0.9f * canvasBounds.Size.Width * textPaint.TextSize / textWidth;

            // Find the text bounds
            var textBounds = new SKRect();
            textPaint.MeasureText(message, ref textBounds);

            // Calculate offsets to center the text on the screen
            float xText = canvasBounds.MidX - textBounds.MidX;
            float yText = canvasBounds.MidY - textBounds.MidY;

            // And draw the text
            canvas.DrawText(message, xText, yText, textPaint);


            // Create a new SKRect object for the frame around the text
            var frameRect = textBounds;
            frameRect.Offset(xText, yText);
            frameRect.Inflate(10, 10);

            // Create an SKPaint object to display the frame
            var framePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 5,
                Color = SKColors.Blue
            };

            // Draw one frame
            canvas.DrawRoundRect(frameRect, 20, 20, framePaint);

            // Inflate the frameRect and draw another
            frameRect.Inflate(10, 10);
            framePaint.Color = SKColors.DarkBlue;
            canvas.DrawRoundRect(frameRect, 30, 30, framePaint);
        }
    }
}
