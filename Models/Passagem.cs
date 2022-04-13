using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Models
{
    public class Passagem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public Voo Voo { get; set; }
        public Passageiro Passageiro { get; set; }
        public double ValorTotal { get; set; }
        public PrecoBase PrecoBase { get; set; }
        public Classe Classe { get; set; }
        public DateTime DataCadastro { get; set; }
        public double PromocaoPorcentagem { get; set; }
        public string LoginUser { get; set; }
    }

}
