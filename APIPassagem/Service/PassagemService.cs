using System.Collections.Generic;
using APIPassagem.Utils;
using Models;
using MongoDB.Driver;

namespace APIPassagem.Services
{
    public class PassagemService
    {
        private readonly IMongoCollection<Passagem> _passagem;

        public PassagemService(IProjMongoDotnetDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _passagem = database.GetCollection<Passagem>(settings.PersonCollectionName);
        }

        public List<Passagem> Get() =>
            _passagem.Find(person => true).ToList();

        public Passagem Get(string id) =>
            _passagem.Find<Passagem>(cliente => cliente.Id == id).FirstOrDefault();

        public Passagem Create(Passagem cliente)
        {
            _passagem.InsertOne(cliente);
            return cliente;
        }

        public void Update(string id, Passagem clienteIn) =>
            _passagem.ReplaceOne(cliente => cliente.Id == id, clienteIn);

        public void Remove(Passagem clienteIn) =>
            _passagem.DeleteOne(cliente => cliente.Id == clienteIn.Id);

        public void Remove(string id) =>
            _passagem.DeleteOne(cliente => cliente.Id == id);
    }
}
