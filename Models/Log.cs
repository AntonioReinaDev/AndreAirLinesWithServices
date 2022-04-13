using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Log
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Usuario { get; set; }
        public string EntidadeAntes { get; set; }
        public string EntidadeDepois { get; set; }
        public string Operacao { get; set; }
        public DateTime Data { get; set; }

        public Log(string usuario, string entidadeAntes, string entidadeDepois, string operacao)
        {
            Usuario = usuario;
            EntidadeAntes = entidadeAntes;
            EntidadeDepois = entidadeDepois;
            Operacao = operacao;
            Data = DateTime.Now;
        }
    }
}
