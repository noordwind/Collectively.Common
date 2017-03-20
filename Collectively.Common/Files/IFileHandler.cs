using System;
using System.Threading.Tasks;
using Collectively.Common.Types;

namespace Collectively.Common.Files
{
    public interface IFileHandler
    {
        Task UploadAsync(File file, string newName, Action<string> onUploaded = null);
        Task<Maybe<FileStreamInfo>> GetFileStreamInfoAsync(Guid remarkId, string size);
        Task<Maybe<FileStreamInfo>> GetFileStreamInfoAsync(string fileId);
        Task DeleteAsync(string name);
    }
}