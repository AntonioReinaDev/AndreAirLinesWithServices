using System.Collections.Generic;
using APIPrecoBase.Utils;
using Models;
using MongoDB.Driver;

namespace APIPrecoBase.Services
{
    public class PrecoBaseService
    {
        private readonly IMongoCollection<PrecoBase> _precobase;

        public PrecoBaseService(IProjMongoDotnetDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _precobase = database.GetCollection<PrecoBase>(settings.PersonCollectionName);
        }

        public List<PrecoBase> Get() =>
            _precobase.Find(person => true).ToList();

        public PrecoBase Get(string id) =>
            _precobase.Find<PrecoBase>(cliente => cliente.Id == id).FirstOrDefault();

        public PrecoBase GetPrecoBase(string siglaorigem, string sigladestino) =>
            _precobase.Find<PrecoBase>(cliente => (cliente.Origem.Sigla == siglaorigem) && (cliente.Destino.Sigla == sigladestino)).FirstOrDefault();

        public PrecoBase Create(PrecoBase cliente)
        {
            _precobase.InsertOne(cliente);
            return cliente;
        }

        public void Update(string id, PrecoBase clienteIn) =>
            _precobase.ReplaceOne(cliente => cliente.Id == id, clienteIn);

        public void Remove(PrecoBase clienteIn) =>
            _precobase.DeleteOne(cliente => cliente.Id == clienteIn.Id);

        public void Remove(string id) =>
            _precobase.DeleteOne(cliente => cliente.Id == id);
    }
}
