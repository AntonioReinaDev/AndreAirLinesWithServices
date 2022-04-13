using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Models.Services
{
    public static class ConsultaAPI
    {
        private static HttpClient APIConnection = new HttpClient();
        private static HttpResponseMessage GetRestposta = new HttpResponseMessage();

        public static async Task<Passageiro> BuscaPassageiroAsync(string cpf)
        {
            GetRestposta = await APIConnection.GetAsync("https://localhost:44340/api/Passageiro/busca?cpf=" + cpf);
            var passageiro = JsonConvert.DeserializeObject<Passageiro>(await GetRestposta.Content.ReadAsStringAsync());
            if (passageiro.Cpf == null)
                return null;
            return passageiro;
        }
        public static async Task<Voo> BuscaVooAsync(string origem, string destino)
        {
            GetRestposta = await APIConnection.GetAsync("https://localhost:44307/api/Voo/busca?siglaorigem=" + origem + "&sigladestino=" + destino);
            var voo = JsonConvert.DeserializeObject<Voo>(await GetRestposta.Content.ReadAsStringAsync());
            if (voo.Origem == null || voo.Destino == null)
                return null;
            return voo;
        }
        public static async Task<Classe> BuscaClasseAsync(string codigo)
        {
            GetRestposta = await APIConnection.GetAsync("https://localhost:44305/api/Classe/busca?descricao=" + codigo);
            var classe = JsonConvert.DeserializeObject<Classe>(await GetRestposta.Content.ReadAsStringAsync());
            if (classe.Descricao == null)
                return null;
            return classe;
        }
        public static async Task<PrecoBase> BuscaPrecoBaseAsync(string origem, string destino)
        {
            GetRestposta = await APIConnection.GetAsync("https://localhost:44364/api/PrecoBase/busca?siglaorigem=" + origem + "&sigladestino=" + destino);
            var precobase = JsonConvert.DeserializeObject<PrecoBase>(await GetRestposta.Content.ReadAsStringAsync());
            if (precobase.Origem == null || precobase.Destino == null)
                return null;
            return precobase;
        }
        public static async Task<Aeroporto> BuscaAeroportoAsync(string sigla)
        {
            GetRestposta = await APIConnection.GetAsync("https://localhost:44353/api/Aeroporto/busca?sigla=" + sigla);
            var aeroporto = JsonConvert.DeserializeObject<Aeroporto>(await GetRestposta.Content.ReadAsStringAsync());
            if (aeroporto.Sigla == null)
                return null;
            return aeroporto;
        }
        public static async Task<IEnumerable<Aeroporto>> BuscaAeroportoAsync()
        {
            GetRestposta = await APIConnection.GetAsync("https://localhost:44353/api/Aeroporto");
            var aeroporto = JsonConvert.DeserializeObject<IEnumerable<Aeroporto>>(await GetRestposta.Content.ReadAsStringAsync());
            if (aeroporto == null)
                return null;
            return aeroporto;
        }
        public static async Task<Aeronave> BuscaAeronaveAsync(string code)
        {
            GetRestposta = await APIConnection.GetAsync("https://localhost:44359/api/Aeronave/busca?codigo=" + code);
            var aeronave = JsonConvert.DeserializeObject<Aeronave>(await GetRestposta.Content.ReadAsStringAsync());
            if (aeronave.Codigo == null)
                return null;
            return aeronave;
        }
        public static async Task<Usuario> BuscaUsuarioAsync(string login)
        {
            GetRestposta = await APIConnection.GetAsync("https://localhost:44385/api/Usuario/busca?login=" + login);
            var usuario = JsonConvert.DeserializeObject<Usuario>(await GetRestposta.Content.ReadAsStringAsync());
            if (usuario == null)
                return null;
            return usuario;
        }
        public static async Task<List<Usuario>> BuscaUsuariosAsync()
        {
            GetRestposta = await APIConnection.GetAsync("https://localhost:44385/api/Usuario");
            var usuario = JsonConvert.DeserializeObject<List<Usuario>>(await GetRestposta.Content.ReadAsStringAsync());
            if (usuario == null)
                return null;
            return usuario;
        }
        public static async Task<Funcao> BuscaFuncaoAsync(string funcao)
        {
            GetRestposta = await APIConnection.GetAsync("https://localhost:44386/api/Funcao/busca?nome=" + funcao);
            var funcaoobj = JsonConvert.DeserializeObject<Funcao>(await GetRestposta.Content.ReadAsStringAsync());

            if (funcaoobj == null)
                return null;
            return funcaoobj;
        }
        public static void RegistraLog(Log log)
        {
            APIConnection.PostAsJsonAsync("https://localhost:44310/api/Log", log);
        }
    }
}
