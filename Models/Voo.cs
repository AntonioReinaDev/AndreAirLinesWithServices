using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Voo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public Aeroporto Destino { get; set; }
        public Aeroporto Origem { get; set; }
        public Aeronave Aeronave { get; set; }
        public DateTime HoraEmbarque { get; set; }
        public DateTime HoraDesembarque { get; set; }
        public string LoginUser { get; set; }
    }
}
