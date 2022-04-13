using System.Collections.Generic;
using APIPassageiro.Utils;
using Models;
using MongoDB.Driver;

namespace APIPassageiro
{
    public class PassageiroService
    {
        private readonly IMongoCollection<Passageiro> _passageiros;

        public PassageiroService(IProjMongoDotnetDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _passageiros = database.GetCollection<Passageiro>(settings.PersonCollectionName);
        }

        public List<Passageiro> Get() =>
            _passageiros.Find(person => true).ToList();

        public Passageiro Get(string id) =>
            _passageiros.Find<Passageiro>(cliente => cliente.Id == id).FirstOrDefault();

        public Passageiro GetCPF(string cpf) =>
            _passageiros.Find<Passageiro>(cliente => cliente.Cpf == cpf).FirstOrDefault();

        public Passageiro Create(Passageiro cliente)
        {
            _passageiros.InsertOne(cliente);
            return cliente;
        }

        public void Update(string id, Passageiro clienteIn) =>
            _passageiros.ReplaceOne(cliente => cliente.Id == id, clienteIn);

        public void Remove(Passageiro clienteIn) =>
            _passageiros.DeleteOne(cliente => cliente.Id == clienteIn.Id);

        public void Remove(string id) =>
            _passageiros.DeleteOne(cliente => cliente.Id == id);
    }
}
