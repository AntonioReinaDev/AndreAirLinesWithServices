using System.Collections.Generic;
using APIClasse.Utils;
using Models;
using MongoDB.Driver;

namespace APIClasse.Service
{
    public class ClasseService
    {
        private readonly IMongoCollection<Classe> _classes;

        public ClasseService(IProjMongoDotnetDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _classes = database.GetCollection<Classe>(settings.PersonCollectionName);
        }

        public List<Classe> Get() =>
            _classes.Find(person => true).ToList();

        public Classe Get(string id) =>
            _classes.Find<Classe>(cliente => cliente.Id == id).FirstOrDefault();

        public Classe GetDescricao(string descricao) =>
            _classes.Find<Classe>(cliente => cliente.Descricao == descricao).FirstOrDefault();

        public Classe Create(Classe cliente)
        {
            _classes.InsertOne(cliente);
            return cliente;
        }

        public void Update(string id, Classe clienteIn) =>
            _classes.ReplaceOne(cliente => cliente.Id == id, clienteIn);

        public void Remove(Classe clienteIn) =>
            _classes.DeleteOne(cliente => cliente.Id == clienteIn.Id);

        public void Remove(string id) =>
            _classes.DeleteOne(cliente => cliente.Id == id);
    }
}
