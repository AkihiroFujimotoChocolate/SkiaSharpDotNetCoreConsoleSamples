using SkiaSharp;
using System.IO;
using Topten.RichTextKit;

namespace SimpleCircle
{
    class Program
    {

        const string OutputFileEn = "output_En.png";
        const string OutputFileJpEmj = "output_JpEmj.png";

        const string MessageEn = "Hello,RichTextKit!";
        const string MessageJpEmj = "🌞こんにちは、リッチテキスト！絵文字いけるかな？🍲";

        const int ImageWidth = 200;
        const int ImageHeight = 500;

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            using (var surface = SKSurface.Create(new SKImageInfo(ImageWidth, ImageHeight)))
            {
                Draw(surface, MessageEn);
                using (var stream = File.OpenWrite(OutputFileEn))
                    Save(stream, surface);
                Draw(surface, MessageJpEmj);
                using (var stream = File.OpenWrite(OutputFileJpEmj))
                    Save(stream, surface);
            }
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
            canvas.Clear(SKColors.LightGray);

            // Find the canvas bounds
            var canvasBounds = canvas.DeviceClipBounds;

            // Create the text block
            var tb = new TextBlock();

            // Configure layout properties
            tb.MaxWidth = canvasBounds.Width * 0.8f;
            tb.Alignment = TextAlignment.Left;

            var style = new Style()
            {
                FontSize = 24
            };

            // Add text to the text block
            tb.AddText(message, style);

            // Paint the text block
            tb.Paint(canvas, new SKPoint(canvasBounds.Width * 0.1f, canvasBounds.Height * 0.1f));
        }
    }
}
