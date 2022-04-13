using System.Collections.Generic;
using APIAeroporto.Utils;
using Models;
using MongoDB.Driver;

namespace APIAeroporto.Service
{
    public class AeroportoService
    {
        private readonly IMongoCollection<Aeroporto> _aeroporto;

        public AeroportoService(IProjMongoDotnetDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _aeroporto = database.GetCollection<Aeroporto>(settings.PersonCollectionName);
        }

        public List<Aeroporto> Get() =>
            _aeroporto.Find(person => true).ToList();

        public Aeroporto Get(string id) =>
            _aeroporto.Find<Aeroporto>(cliente => cliente.Id == id).FirstOrDefault();

        public Aeroporto GetSigla(string sligla) =>
            _aeroporto.Find<Aeroporto>(cliente => cliente.Sigla == sligla).FirstOrDefault();

        public Aeroporto Create(Aeroporto cliente)
        {
            _aeroporto.InsertOne(cliente);
            return cliente;
        }

        public void Update(string id, Aeroporto clienteIn) =>
            _aeroporto.ReplaceOne(cliente => cliente.Id == id, clienteIn);

        public void Remove(Aeronave clienteIn) =>
            _aeroporto.DeleteOne(cliente => cliente.Id == clienteIn.Id);

        public void Remove(string id) =>
            _aeroporto.DeleteOne(cliente => cliente.Id == id);
    }
}
