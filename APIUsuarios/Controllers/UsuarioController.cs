using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIUsuarios.Service;
using APIUsuarios.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Services;
using Newtonsoft.Json;

namespace APIUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService personService)
        {
            _usuarioService = personService;
        }

        [HttpGet]
        [Authorize(Roles = "GetUsuario")]
        public ActionResult<List<Usuario>> Get() =>
            _usuarioService.Get();

        [HttpGet("{id:length(24)}", Name = "GetCliente")]
        [Authorize(Roles = "GetIdUsuario")]
        public ActionResult<Usuario> Get(string id)
        {
            var cliente = _usuarioService.Get(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpGet("busca")]
        public ActionResult<Usuario> GetUsuarioLogin(string login)
        {
            var cliente = _usuarioService.GetLogin(login);

            if (cliente == null)
            {
                return NotFound();
                throw new Exception();
            }

            return cliente;
        }

        [HttpPost]
        [Authorize(Roles = "PostUsuario")]
        public async Task<ActionResult<Usuario>> CreateAsync(Usuario usuario)
        {
            try
            {
                var user = await ConsultaAPI.BuscaUsuarioAsync(usuario.LoginUser);
                if (user.Login == null)
                    return NotFound("Este usuario não existe");
                if (user.Funcao.Nome != "Administrador")
                    return BadRequest("Este usuario nao tem autorização para cadastrar precobase");
            }
            catch
            {
                return NotFound("API DE USUARIOS ESTA FORA DO AR");
            }

            if (!LoginService.VerificaUsuarioLogin(usuario.Login, _usuarioService))
                return BadRequest("Login ja esta sendo utlizado!");

            try
            {
                var funcao = await ConsultaAPI.BuscaFuncaoAsync(usuario.Funcao.Nome);
                if (funcao == null)
                    return NotFound("Esta função não existe");
                usuario.Funcao = funcao;
            }
            catch
            {
                return NotFound("API DAS FUNÇÕES ESTA FORA DO AR");
            }

            var end = await VerificaCep.CEPVerify(usuario.Endereco.Cep);
            if (end.Localidade != null)
            {
                int num = usuario.Endereco.Numero;
                usuario.Endereco = new Endereco(end.Localidade, end.Logradouro, end.Bairro, end.Uf, end.Complemento, end.Cep);
                usuario.Endereco.Numero = num;
            }

            ConsultaAPI.RegistraLog(new Log(usuario.LoginUser, null, JsonConvert.SerializeObject(usuario), "Create"));
            _usuarioService.Create(usuario);

            return CreatedAtRoute("GetCliente", new { id = usuario.Id.ToString() }, usuario);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "PutUsuario")]
        public IActionResult Update(string id, Usuario personIn)
        {
            var cliente = _usuarioService.Get(id);

            if (cliente == null)
            {
                return NotFound();
            }

            ConsultaAPI.RegistraLog(new Log(personIn.LoginUser, JsonConvert.SerializeObject(cliente), JsonConvert.SerializeObject(personIn), "Update"));
            _usuarioService.Update(id, personIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "DeleteUsuario")]
        public IActionResult Delete(string id)
        {
            var person = _usuarioService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            ConsultaAPI.RegistraLog(new Log(person.LoginUser, JsonConvert.SerializeObject(person), null, "Delete"));
            _usuarioService.Remove(person.Id);

            return NoContent();
        }
    }
}
