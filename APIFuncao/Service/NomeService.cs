

namespace APIFuncao.Services
{
    public class NomeService
    {
        public static bool VerificaFuncaoNome(string nome, FuncaoService _funcaoService)
        {
            if (_funcaoService.GetNome(nome) != null)
                return false;
            return true;
        }
    }
}
