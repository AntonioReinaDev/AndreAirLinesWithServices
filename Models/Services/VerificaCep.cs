using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Models.Services
{
    public class VerificaCep
    {
        static readonly HttpClient client = new HttpClient();
        public async static Task<Endereco> CEPVerify(string cep)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://viacep.com.br/ws/" + cep + "/json");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var endereco = JsonConvert.DeserializeObject<Endereco>(responseBody);

                return endereco;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nExceprion Caught!");
                Console.WriteLine("Message: {0}", e.Message);
                return null;
                throw;
            }
        }
    }
}
