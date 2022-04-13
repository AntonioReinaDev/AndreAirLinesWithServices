using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class PrecoBase
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public Aeroporto Origem { get; set; }
        public Aeroporto Destino { get; set; }
        public double Valor { get; set; }
        public DateTime DataInclusao { get; set; }
        public string LoginUser { get; set; }
    }
}
