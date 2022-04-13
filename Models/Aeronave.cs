using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Aeronave
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public int Capacidade { get; set; }
        public string LoginUser { get; set; }
    }
}
