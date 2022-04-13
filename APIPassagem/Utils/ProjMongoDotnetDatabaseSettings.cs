namespace APIPassagem.Utils
{
    public class ProjMongoDotnetDatabaseSettings : IProjMongoDotnetDatabaseSettings
    {
        public string PersonCollectionName { get; set; } = "Passagem";
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "dbAndreAirLinesPassagem";
    }
}