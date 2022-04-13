

namespace APIAeroporto.Service
{
    public class SiglaService
    {
        public static bool VerificaAeroportoSigla(string sigla, AeroportoService _aeroportoService)
        {
            if (_aeroportoService.GetSigla(sigla) != null)
                return false;
            return true;
        }
    }
}
