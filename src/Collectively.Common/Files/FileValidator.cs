using System;
using System.IO;
using ImageSharp;
using Serilog;

namespace Collectively.Common.Files
{
    public class FileValidator : IFileValidator
    {
        private static readonly ILogger Logger = Log.Logger;

        public bool IsImage(File file)
        {
            try
            {
                using(var image = Image.Load(file.Bytes))
                {
                    return image.Width > 0 && image.Height > 0;
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "Error while reading image from stream");
                
                return false;
            }
        }
    }
}