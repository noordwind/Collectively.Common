using System;
using System.Collections.Generic;
using System.IO;
using Serilog;
using Structure.Sketching;
using Structure.Sketching.Filters.Resampling;
using Structure.Sketching.Filters.Resampling.Enums;
using Structure.Sketching.Formats;

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
            using (var stream = new MemoryStream(file.Bytes))
            {
                var originalImage = new Image(stream);
                var resizedImage = ScaleImage(originalImage, size);

                return File.Create(file.Name, file.ContentType, resizedImage);
            }
        }

        public IDictionary<string, File> ProcessImage(File file)
        {
            Logger.Information($"Processing image, name:{file.Name}, contentType:{file.ContentType}, " +
                         $"sizeBytes:{file.SizeBytes}");

            using (var stream = new MemoryStream(file.Bytes))
            {
                var originalImage = new Image(stream);
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
            
        private byte[] ScaleImage(Image image, double maxSize)
        {
            var ratioX = maxSize/image.Width;
            var ratioY = maxSize/image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int) (image.Width* ratio);
            var newHeight = (int) (image.Height* ratio);

            using (var stream = new MemoryStream())
            {
                var newImage = new Image(image)
                    .Apply(new Resize(newWidth, newHeight, ResamplingFiltersAvailable.Bell));
                newImage.Save(stream, FileFormats.JPEG);

                return stream.ToArray();
            }
        }
    }
}