using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Collectively.Common.Types;
using Serilog;

namespace Collectively.Common.Files
{
    public class AwsS3FileHandler : IFileHandler
    {
        private static readonly ILogger Logger = Log.Logger;
        private readonly IAmazonS3 _client;
        private readonly AwsS3Settings _settings;

        public AwsS3FileHandler(IAmazonS3 client, AwsS3Settings settings)
        {
            _client = client;
            _settings = settings;
        }

        public async Task UploadAsync(File file, string newName, Action<string,string> onUploaded = null)
        {
            Logger.Information($"Uploading file {file.Name} -> {newName} to AWS S3 bucket: {_settings.Bucket}.");
            var baseUrl = $"https://{_settings.Bucket}.s3.{_settings.Region}.amazonaws.com";
            var fullUrl = $"{baseUrl}/{newName}";
            using (var stream = new MemoryStream(file.Bytes))
            {
                await _client.UploadObjectFromStreamAsync(_settings.Bucket, newName,
                    stream, new Dictionary<string, object>());
            }
            Logger.Information($"Completed uploading file {file.Name} -> {newName} to AWS S3 bucket: {_settings.Bucket}.");
            onUploaded?.Invoke(baseUrl, fullUrl);
        }

        public async Task DeleteAsync(string name)
        {
            Logger.Information($"Deleting file {name} from AWS S3 bucket: {_settings.Bucket}.");
            var url = $"https://s3.{_settings.Region}.amazonaws.com/{_settings.Bucket}/";
            await _client.DeleteObjectAsync(_settings.Bucket, name);
            Logger.Information($"Completed deleting file {name} from AWS S3 bucket: {_settings.Bucket}.");
        }
    }
}