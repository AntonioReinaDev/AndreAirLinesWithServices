using System.Collections.Generic;
using System.Text;
using APILog.Utils;
using Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace APILog.Services
{
    public class LogService
    {
        private readonly IMongoCollection<Log> _log;
        private readonly ConnectionFactory _factory = new ConnectionFactory { HostName = "localhost"};
        private const string QUEUE_NAME = "messagelogs";

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
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.QueueDeclare(
                        queue: QUEUE_NAME,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );

                    var stringfieldMessage = JsonConvert.SerializeObject(cliente);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfieldMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: QUEUE_NAME,
                        basicProperties: null,
                        body: bytesMessage
                        );
                }
            }
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
