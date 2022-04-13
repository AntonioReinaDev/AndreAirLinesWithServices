using System.Collections.Generic;
using APIAeronave.Utils;
using Models;
using MongoDB.Driver;

namespace APIAeronave.Service
{
    public class AeronaveService
    {
        private readonly IMongoCollection<Aeronave> _aeronaves;

        public AeronaveService(IProjMongoDotnetDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _aeronaves = database.GetCollection<Aeronave>(settings.PersonCollectionName);
        }

        public List<Aeronave> Get() =>
            _aeronaves.Find(person => true).ToList();

        public Aeronave Get(string id) =>
            _aeronaves.Find<Aeronave>(cliente => cliente.Id == id).FirstOrDefault();

        public Aeronave GetCodigo(string codigo) =>
            _aeronaves.Find<Aeronave>(cliente => cliente.Codigo == codigo).FirstOrDefault();

        public Aeronave Create(Aeronave cliente)
        {
            _aeronaves.InsertOne(cliente);
            return cliente;
        }

        public void Update(string id, Aeronave clienteIn) =>
            _aeronaves.ReplaceOne(cliente => cliente.Id == id, clienteIn);

        public void Remove(Aeronave clienteIn) =>
            _aeronaves.DeleteOne(cliente => cliente.Id == clienteIn.Id);

        public void Remove(string id) =>
            _aeronaves.DeleteOne(cliente => cliente.Id == id);
    }
}
