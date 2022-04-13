

using System.Collections.Generic;
using APIVoo.Utils;
using Models;
using MongoDB.Driver;

namespace APIVoo.Services
{
    public class VooService
    {
        private readonly IMongoCollection<Voo> _voo;

        public VooService(IProjMongoDotnetDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _voo = database.GetCollection<Voo>(settings.PersonCollectionName);
        }

        public List<Voo> Get() =>
            _voo.Find(person => true).ToList();

        public Voo Get(string id) =>
            _voo.Find<Voo>(cliente => cliente.Id == id).FirstOrDefault();

        public Voo GetVoo(string siglaorigem, string sigladestino) =>
            _voo.Find<Voo>(cliente => (cliente.Origem.Sigla == siglaorigem) && (cliente.Destino.Sigla == sigladestino)).FirstOrDefault();

        public Voo Create(Voo cliente)
        {
            _voo.InsertOne(cliente);
            return cliente;
        }

        public void Update(string id, Voo clienteIn) =>
            _voo.ReplaceOne(cliente => cliente.Id == id, clienteIn);

        public void Remove(Voo clienteIn) =>
            _voo.DeleteOne(cliente => cliente.Id == clienteIn.Id);

        public void Remove(string id) =>
            _voo.DeleteOne(cliente => cliente.Id == id);
    }
}
