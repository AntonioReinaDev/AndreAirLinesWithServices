using System.Collections.Generic;
using APIUsuarios.Utils;
using Models;
using MongoDB.Driver;

namespace APIUsuarios.Service
{
    public class UsuarioService
    {
        private readonly IMongoCollection<Usuario> _precobase;

        public UsuarioService(IProjMongoDotnetDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _precobase = database.GetCollection<Usuario>(settings.PersonCollectionName);
        }

        public List<Usuario> Get() =>
            _precobase.Find(person => true).ToList();

        public Usuario Get(string id) =>
            _precobase.Find<Usuario>(cliente => cliente.Id == id).FirstOrDefault();

        public Usuario GetLogin(string login) =>
            _precobase.Find<Usuario>(cliente => (cliente.Login == login)).FirstOrDefault();

        public Usuario Create(Usuario cliente)
        {
            _precobase.InsertOne(cliente);
            return cliente;
        }

        public void Update(string id, Usuario clienteIn) =>
            _precobase.ReplaceOne(cliente => cliente.Id == id, clienteIn);

        public void Remove(Usuario clienteIn) =>
            _precobase.DeleteOne(cliente => cliente.Id == clienteIn.Id);

        public void Remove(string id) =>
            _precobase.DeleteOne(cliente => cliente.Id == id);
    }
}
