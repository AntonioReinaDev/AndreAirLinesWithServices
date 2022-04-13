namespace APIPrecoBase.Utils
{
    public interface IProjMongoDotnetDatabaseSettings
    {
        string PersonCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }

    }
}