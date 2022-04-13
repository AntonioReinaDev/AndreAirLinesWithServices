

namespace APIAeronave.Service
{
    public class CodigoService
    {
        public static bool VerificaAeronaveSigla(string sigla, AeronaveService _aeronaveService)
        {
            if (_aeronaveService.GetCodigo(sigla) != null)
                return false;
            return true;
        }
    }
}
