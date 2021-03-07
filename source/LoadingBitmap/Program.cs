using LoadingBitmap.Properties;
using SkiaSharp;
using System;
using System.IO;
using System.Net.Http;

namespace SimpleCircle
{
    class Program
    {
        const string OutputFileNameResource = "output_resource.png";
        const string InputImageURL = "https://dummyimage.com/500x100/000/ffff.png&text=Image+file+from+dummyimage.com";
        const string OutputFileNameWeb = "output_web.png";
        const string InputImageLocalFilePath = "input.png";
        const string OutputFileLocal = "output_local.png";

        const string AlternativeText = "Image could not be loaded";
 
        const int ImageWidth = 1280;
        const int ImageHeight = 800;

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var surface = SKSurface.Create(new SKImageInfo(ImageWidth, ImageHeight));


            // Load from resources
            try
            {
                using var inputStreamResource = new MemoryStream(Resources.irasutoya_food_sukiyaki_png);
                Draw(surface, inputStreamResource);

            }
            catch
            {
                Draw(surface, null);
            }
            using var outputStreamReesource = File.OpenWrite(OutputFileNameResource);
            Save(outputStreamReesource, surface);

            // Load from Web
            HttpClient httpClient = new HttpClient();
            try
            {
                using (var inputStreaWeb = await httpClient.GetStreamAsync(InputImageURL))
                using (var memStream = new MemoryStream())
                {
                    await inputStreaWeb.CopyToAsync(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);
                    Draw(surface, memStream);
                };
            }
            catch
            {
                Draw(surface, null);
            }
            using var outputStreamWeb = File.OpenWrite(OutputFileNameWeb);
            Save(outputStreamWeb, surface);

            // Load from local file system
            try
            {
                using (var inputStreamLocal = File.OpenRead(InputImageLocalFilePath))
                Draw(surface, inputStreamLocal);

            }
            catch
            {
                Draw(surface, null);
            }
            using var outputStreamLocal = File.OpenWrite(OutputFileLocal);
            Save(outputStreamLocal, surface);


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

            // Load bitmap from stream
            var bitmap = source != null ? SKBitmap.Decode(source) : null;
            if (bitmap != null)
            {
                float x = (canvasBounds.Size.Width - bitmap.Width) / 2;
                float y = (canvasBounds.Size.Height - bitmap.Height) / 2;
                canvas.DrawBitmap(bitmap, x, y);
            }
            else
            {
                // Create an SKPaint object to display the text
                var textPaint = new SKPaint
                {
                    Color = SKColors.Chocolate
                };

                // Adjust TextSize property so text is 90% of screen width
                float textWidth = textPaint.MeasureText(AlternativeText);
                textPaint.TextSize = 0.9f * canvasBounds.Size.Width * textPaint.TextSize / textWidth;

                // Find the text bounds
                var textBounds = new SKRect();
                textPaint.MeasureText(AlternativeText, ref textBounds);

                // Calculate offsets to center the text on the screen
                float xText = canvasBounds.MidX - textBounds.MidX;
                float yText = canvasBounds.MidY - textBounds.MidY;

                // And draw the text
                canvas.DrawText(AlternativeText, xText, yText, textPaint);

            }
        }
    }
}
