using System.Collections.Generic;
using System.Text;
using APILogsRabbitMQ.Utils;
using Models;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace APILogsRabbitMQ.Services
{
    public class LogService
    {
        private readonly IMongoCollection<Log> _log;

        public LogService(IProjMongoDotnetDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _log = database.GetCollection<Log>(settings.PersonCollectionName);
        }

        public List<Log> Get() =>
            _log.Find(person => true).ToList();

        public Log Get(string id) =>
            _log.Find<Log>(cliente => cliente.Id == id).FirstOrDefault();

        public Log Create(Log cliente)
        {
            _log.InsertOne(cliente);
            return cliente;
        }

        public void Update(string id, Log clienteIn) =>
            _log.ReplaceOne(cliente => cliente.Id == id, clienteIn);

        public void Remove(Log clienteIn) =>
            _log.DeleteOne(cliente => cliente.Id == clienteIn.Id);

        public void Remove(string id) =>
            _log.DeleteOne(cliente => cliente.Id == id);
    }
}
