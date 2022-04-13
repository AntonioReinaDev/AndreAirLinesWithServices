using System.Collections.Generic;
using APIFuncao.Utils;
using Models;
using MongoDB.Driver;

namespace APIFuncao.Services
{
    public class FuncaoService
    {
        private readonly IMongoCollection<Funcao> _precobase;

        public FuncaoService(IProjMongoDotnetDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _precobase = database.GetCollection<Funcao>(settings.PersonCollectionName);
        }

        public List<Funcao> Get() =>
            _precobase.Find(person => true).ToList();

        public Funcao Get(string id) =>
            _precobase.Find<Funcao>(cliente => cliente.Id == id).FirstOrDefault();

        public Funcao GetNome(string nome) =>
            _precobase.Find<Funcao>(cliente => (cliente.Nome == nome)).FirstOrDefault();

        public Funcao Create(Funcao cliente)
        {
            _precobase.InsertOne(cliente);
            return cliente;
        }

        public void Update(string id, Funcao clienteIn) =>
            _precobase.ReplaceOne(cliente => cliente.Id == id, clienteIn);

        public void Remove(Funcao clienteIn) =>
            _precobase.DeleteOne(cliente => cliente.Id == clienteIn.Id);

        public void Remove(string id) =>
            _precobase.DeleteOne(cliente => cliente.Id == id);
    }
}
