using System.Collections.Generic;
using System.Security.Authentication;
using Autofac;
using MongoDB.Driver;

namespace Collectively.Common.Mongo
{
    public class DocumentDbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c, p) =>
            {
                var settings = c.Resolve<MongoDbSettings>();
                var clientSettings = new MongoClientSettings();
                clientSettings.Server = new MongoServerAddress(settings.Host, 10250);
                clientSettings.UseSsl = true;
                clientSettings.SslSettings = new SslSettings();
                clientSettings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;
                var identity = new MongoInternalIdentity(settings.Database, settings.Username);
                var evidence = new PasswordEvidence(settings.Password);
                clientSettings.Credentials = new List<MongoCredential>()
                {
                    new MongoCredential("SCRAM-SHA-1", identity, evidence)
                };

                return new MongoClient(clientSettings);
            }).SingleInstance();

            builder.Register((c, p) =>
            {
                var mongoClient = c.Resolve<MongoClient>();
                var settings = c.Resolve<MongoDbSettings>();
                var database = mongoClient.GetDatabase(settings.Database);

                return database;
            }).As<IMongoDatabase>();
        }
    }
}