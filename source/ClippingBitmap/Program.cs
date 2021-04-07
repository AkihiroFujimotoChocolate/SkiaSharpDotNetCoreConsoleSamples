using ClippingBitmap.Properties;
using SkiaSharp;
using System;
using System.IO;
using System.Net.Http;

namespace SimpleCircle
{
    class Program
    {
        const string OutputFileNameCircle = "output_circle.png";

        const int ImageWidth = 1280;
        const int ImageHeight = 800;

        static void Main(string[] args)
        {
            using (var surface = SKSurface.Create(new SKImageInfo(ImageWidth, ImageHeight))){

                using (var inputStream = new MemoryStream(Resources.computer_game_gaming_computer))
                {
                    Draw(surface, inputStream);
                }
                using (var outputStreamLocal = File.OpenWrite(OutputFileNameCircle))
                {
                    Save(outputStreamLocal, surface);
                }
            };
        }

        // Save surface as a png image file.
        private static void Save(Stream target, SKSurface surface)
        {
            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            data.SaveTo(target);
        }


        // Draw image to surface
        private static void Draw(SKSurface surface, Stream source)
        {
            // Create a blank canvas
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.LightGray);

            // Find the canvas bounds
            var canvasBounds = canvas.DeviceClipBounds;

            using (var bitmap = SKBitmap.Decode(source))
            using (var image = CreateCircleClippedImage(bitmap))
            {
                float length = Math.Min(canvasBounds.Width, canvasBounds.Height) / 2f;
                var rect = new SKRect()
                {
                    Location = new SKPoint((canvasBounds.MidX - length) / 2f, (canvasBounds.MidY - length) / 2f),
                    Size = new SKSize(length, length)
                };
                canvas.DrawImage(image, rect);
            }
        }

        // Create Clipped Image
        private static SKImage CreateCircleClippedImage(SKBitmap bitmap)
        {
            int diameter = Math.Min(bitmap.Width, bitmap.Height);
            float radius = diameter / 2f;
            using (var surface = SKSurface.Create(new SKImageInfo(diameter, diameter)))
            {
                // Create a blank canvas
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.Transparent);

                using (var path = new SKPath())
                {
                    // set circle path
                    path.AddCircle(radius, radius, radius);
                    canvas.ClipPath(path, SKClipOperation.Intersect);

                    // Draw bitmap
                    canvas.DrawBitmap(bitmap, 0, 0);
                };
                return surface.Snapshot();
            }
        }
    }
}
