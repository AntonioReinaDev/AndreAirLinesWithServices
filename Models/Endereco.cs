using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Endereco
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Cep { get; set; }
        public string Localidade { get; set; }
        public string Logradouro { get; set; }
        public string Pais { get; set; }
        public string Bairro { get; set; }
        public int Numero { get; set; }
        public string Uf { get; set; }
        public string Complemento { get; set; }
        public string Continente { get; set; }


        //utilizado quando encontra o cep na api dos correios assim sendo sempre no Brasil
        public Endereco(string localidade, string logradouro, string bairro, string uf, string complemento, string cep)
        {
            Cep = cep;
            Localidade = localidade;
            Logradouro = logradouro;
            Bairro = bairro;
            Uf = uf;
            Complemento = complemento;
            Pais = "Brasil";
            Continente = "America do Sul";
        }
    }
}

