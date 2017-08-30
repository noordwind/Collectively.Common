using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Types;
using Serilog;

namespace Collectively.Common.Files
{
    public class FileResolver : IFileResolver
    {
        private static readonly ILogger Logger = Log.Logger;

        public Maybe<File> FromBase64(string base64, string name, string contentType)
        {
            if (base64.Empty())
                return new Maybe<File>();
            if (name.Empty())
                return new Maybe<File>();
            if (contentType.Empty())
                return new Maybe<File>();

            var startIndex = 0;
            if (base64.Contains(","))
                startIndex = base64.IndexOf(",", StringComparison.CurrentCultureIgnoreCase) + 1;

            var base64String = base64.Substring(startIndex);
            var bytes = Convert.FromBase64String(base64String);
            
            return File.Create(name, contentType, bytes);
        }

        public async Task<Maybe<Stream>> FromUrlAsync(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var result = await httpClient.GetAsync(url);
                    var stream = await result.Content.ReadAsStreamAsync();

                    return stream;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"There was an error when trying to get a stream from URL: {url}.");

                return Stream.Null;
            }
        }
    }
}