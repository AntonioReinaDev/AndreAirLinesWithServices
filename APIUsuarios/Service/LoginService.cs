

using APIUsuarios.Service;

namespace APIUsuarios.Services
{
    public class LoginService
    {
        public static bool VerificaUsuarioLogin(string login, UsuarioService _usuarioService)
        {
            if (_usuarioService.GetLogin(login) != null)
                return false;
            return true;
        }
    }
}
