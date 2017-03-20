using Collectively.Common.Types;

namespace Collectively.Common.Files
{
    public interface IFileResolver
    {
        Maybe<File> FromBase64(string base64, string name, string contentType);
    }
}