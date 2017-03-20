using Amazon;
using Amazon.S3;
using Autofac;
using Collectively.Common.Extensions;
using Microsoft.Extensions.Configuration;

namespace Collectively.Common.Files
{
    public class FilesModule : Module
    {
        private readonly IConfiguration _configuration;

        public FilesModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
                builder.RegisterType<FileValidator>().As<IFileValidator>().SingleInstance();
                builder.RegisterType<FileResolver>().As<IFileResolver>().SingleInstance();
                builder.RegisterType<ImageService>().As<IImageService>().SingleInstance();
                builder.RegisterInstance(_configuration.GetSettings<AwsS3Settings>()).SingleInstance();
                builder.RegisterType<AwsS3FileHandler>().As<IFileHandler>().SingleInstance();
                builder.Register(c =>
                {
                    var settings = c.Resolve<AwsS3Settings>();

                    return new AmazonS3Client(settings.AccessKey, settings.SecretKey,
                        RegionEndpoint.GetBySystemName(settings.Region));
                })
                .As<IAmazonS3>()
                .SingleInstance();
        }
    }
}