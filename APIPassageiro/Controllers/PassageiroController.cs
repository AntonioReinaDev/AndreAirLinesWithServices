using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Services;
using Newtonsoft.Json;

namespace APIPassageiro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassageiroController : ControllerBase
    {
        private readonly PassageiroService _passageiroService;

        public PassageiroController(PassageiroService personService)
        {
            _passageiroService = personService;
        }

        [HttpGet]
        [Authorize(Roles = "GetPassageiro")]
        public ActionResult<List<Passageiro>> Get() =>
            _passageiroService.Get();

        [HttpGet("{id:length(24)}", Name = "GetCliente")]
        [Authorize(Roles = "GetIdPassageiro")]
        public ActionResult<Passageiro> Get(string id)
        {
            var cliente = _passageiroService.Get(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpGet("busca")]
        public ActionResult<Passageiro> GetPassageiroCPF(string cpf)
        {
            var cliente = _passageiroService.GetCPF(cpf);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpPost]
        [Authorize(Roles = "PostPassageiro")]
        public async Task<ActionResult<Passageiro>> CreateAsync(Passageiro passageiro)
        {
            try
            {
                var usuario = await ConsultaAPI.BuscaUsuarioAsync(passageiro.LoginUser);
                if (usuario.Login == null)
                    return NotFound("Este usuario não existe");
                if (usuario.Funcao.Nome != "Administrador" && usuario.Funcao.Nome != "Atendente")
                    return BadRequest("Este usuario nao tem autorização para cadastrar precobase");
            }
            catch
            {
                return NotFound("API DE USUARIOS ESTA FORA DO AR");
            }

            if (!CPFService.ValidaCPF(passageiro.Cpf, _passageiroService))
                return BadRequest("CPF Inválido!");
            if (!CPFService.VerificaPassageiroCPF(passageiro.Cpf, _passageiroService))
                return BadRequest("Passageiro ja cadastrado");

            var end = await VerificaCep.CEPVerify(passageiro.Endereco.Cep);
            if (end.Localidade != null)
            {
                int num = passageiro.Endereco.Numero;
                passageiro.Endereco = new Endereco(end.Localidade, end.Logradouro, end.Bairro, end.Uf, end.Complemento, end.Cep);
                passageiro.Endereco.Numero = num;
            }

            ConsultaAPI.RegistraLog(new Log(passageiro.LoginUser, null, JsonConvert.SerializeObject(passageiro), "Create"));
            _passageiroService.Create(passageiro);

            return CreatedAtRoute("GetCliente", new { id = passageiro.Id.ToString() }, passageiro);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "PutPassageiro")]
        public IActionResult Update(string id, Passageiro personIn)
        {
            var cliente = _passageiroService.Get(id);

            if (cliente == null)
            {
                return NotFound();
            }

            ConsultaAPI.RegistraLog(new Log(personIn.LoginUser, JsonConvert.SerializeObject(cliente), JsonConvert.SerializeObject(personIn), "Update"));
            _passageiroService.Update(id, personIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "DeletePassageiro")]
        public IActionResult Delete(string id)
        {
            var person = _passageiroService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            ConsultaAPI.RegistraLog(new Log(person.LoginUser, JsonConvert.SerializeObject(person), null, "Delete"));
            _passageiroService.Remove(person.Id);

            return NoContent();
        }
    }
}
