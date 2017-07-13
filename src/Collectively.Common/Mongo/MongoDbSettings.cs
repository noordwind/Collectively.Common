namespace Collectively.Common.Mongo
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public bool Seed { get; set; }
    }
}