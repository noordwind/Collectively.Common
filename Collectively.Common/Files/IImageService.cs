using System.Collections.Generic;

namespace Collectively.Common.Files
{
    public interface IImageService
    {
        IDictionary<string,File> ProcessImage(File file);
    }
}