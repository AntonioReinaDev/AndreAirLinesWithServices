namespace APIPassageiro.Utils
{
    public class ProjMongoDotnetDatabaseSettings : IProjMongoDotnetDatabaseSettings
    {
        public string PersonCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
