using System;
using System.IO;
using Serilog;
using Structure.Sketching;

namespace Collectively.Common.Files
{
    public class FileValidator : IFileValidator
    {
        private static readonly ILogger Logger = Log.Logger;

        public bool IsImage(File file)
        {
            try
            {
                using (var stream = new MemoryStream(file.Bytes))
                {
                    var image = new Image(stream);

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