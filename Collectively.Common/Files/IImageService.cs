using System.Collections.Generic;

namespace Collectively.Common.Files
{
    public interface IImageService
    {
        File ProcessImage(File file, double size);
        IDictionary<string,File> ProcessImage(File file);
    }
}