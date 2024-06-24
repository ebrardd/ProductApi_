namespace ProductApi_.Configs
{
    public class MongoDBSettings : IMongoDBSettings
    {
        public string DatabaseName { get; set; }
        public string CollectionName1 { get; set; }
        public string CollectionName2 { get; set; }
        public string ConnectionUri { get; set; }
    }
}
