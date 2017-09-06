using System;
using System.Collections.Generic;
using System.IO;
using ImageSharp;
using Serilog;

namespace Collectively.Common.Files
{
    public class ImageService : IImageService
    {
        private static readonly ILogger Logger = Log.Logger;
        private static readonly double SmallSize = 200;
        private static readonly double MediumSize = 640;
        private static readonly double BigSize = 1200;

        public File ProcessImage(File file, double size)
        {
            using(var originalImage = Image.Load(file.Bytes))
            {
                return File.Create(file.Name, file.ContentType, 
                    ScaleImage(originalImage, size));
            }
        }

        public IDictionary<string, File> ProcessImage(File file)
        {
            Logger.Information($"Processing image: '{file.Name}', content type: '{file.ContentType}', " +
                         $"size: {file.SizeBytes} bytes.");

            using(var originalImage = Image.Load(file.Bytes))
            {
                var smallImage = ScaleImage(originalImage, SmallSize);
                var mediumImage = ScaleImage(originalImage, MediumSize);
                var bigImage = ScaleImage(originalImage, BigSize);
                var dictionary = new Dictionary<string, File>
                {
                    {"small", File.Create(file.Name, file.ContentType, smallImage)},
                    {"medium", File.Create(file.Name, file.ContentType, mediumImage)},
                    {"big", File.Create(file.Name, file.ContentType, bigImage)}
                };

                return dictionary;
            }
        }
            
        private byte[] ScaleImage(Image<Rgba32> image, double maxSize)
        {
            var ratioX = maxSize/image.Width;
            var ratioY = maxSize/image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(image.Width*ratio);
            var newHeight = (int)(image.Height*ratio);
            using (var stream = new MemoryStream())
            {
                image.Resize(newWidth, newHeight)
                     .Save(stream, ImageFormats.Jpeg);
                
                return stream.ToArray();
            }
        }
    }
}