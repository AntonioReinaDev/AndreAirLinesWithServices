using System.Collections.Generic;
using System.Threading.Tasks;
using APIPassagem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Services;
using Newtonsoft.Json;

namespace APIPassagem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassagemController : ControllerBase
    {
        private readonly PassagemService _passagemService;

        public PassagemController(PassagemService personService)
        {
            _passagemService = personService;
        }

        [HttpGet]
        [Authorize(Roles = "GetPassagem")]
        public ActionResult<List<Passagem>> Get() =>
            _passagemService.Get();

        [HttpGet("{id:length(24)}", Name = "GetCliente")]
        [Authorize(Roles = "GetIdPassagem")]
        public ActionResult<Passagem> Get(string id)
        {
            var cliente = _passagemService.Get(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpPost]
        [Authorize(Roles = "PostPassagem")]
        public async Task<ActionResult<Passagem>> CreateAsync(Passagem passagem)
        {
            try
            {
                var usuario = await ConsultaAPI.BuscaUsuarioAsync(passagem.LoginUser);
                if (usuario.Login == null)
                    return NotFound("Este usuario não existe");
                if (usuario.Funcao.Nome != "Administrador" && usuario.Funcao.Nome != "Atendente")
                    return BadRequest("Este usuario nao tem autorização para cadastrar precobase");
            }
            catch
            {
                return NotFound("API DE USUARIOS ESTA FORA DO AR");
            }
            try
            {
                var voo = await ConsultaAPI.BuscaVooAsync(passagem.Voo.Origem.Sigla, passagem.Voo.Destino.Sigla);
                if (voo == null)
                    return NotFound("Não existe voo com estas informaçoes");
                passagem.Voo = voo;
            }
            catch
            {
                return StatusCode(408, "API DOS VOOS ESTA FORA DO AR");
            }

            try
            {
                var passageiro = await ConsultaAPI.BuscaPassageiroAsync(passagem.Passageiro.Cpf);
                if (passageiro == null)
                    return NotFound("Não existe passageiro com este cpf");
                passagem.Passageiro = passageiro;
            }
            catch
            {
                return StatusCode(408, "API DE PASSAGEIROS FORA DE AR!");
            }

            try
            {
                var classe = await ConsultaAPI.BuscaClasseAsync(passagem.Classe.Descricao);
                if (classe == null)
                    return NotFound("Não existe classse com esta descricao");
                passagem.Classe = classe;
            }
            catch
            {
                return StatusCode(408, "API DAS CLASSES ESTA FORA DO AR");
            }

            try
            {
                var precoBase = await ConsultaAPI.BuscaPrecoBaseAsync(passagem.PrecoBase.Origem.Sigla, passagem.PrecoBase.Destino.Sigla);
                if (precoBase == null)
                    return NotFound("Não existe um prec=ço base para esta viagem");
                passagem.PrecoBase = precoBase;
            }
            catch
            {
                return StatusCode(408, "API DOS PRECOS BASES DO AR");
            }

            passagem.ValorTotal = (passagem.PrecoBase.Valor * ((passagem.Classe.Valor - 100) / 100) * -1) * (((passagem.PromocaoPorcentagem - 100) / 100) * -1);

            ConsultaAPI.RegistraLog(new Log(passagem.LoginUser, null, JsonConvert.SerializeObject(passagem), "Create"));
            _passagemService.Create(passagem);

            return CreatedAtRoute("GetCliente", new { id = passagem.Id.ToString() }, passagem);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "PutPassagem")]
        public IActionResult Update(string id, Passagem personIn)
        {
            var cliente = _passagemService.Get(id);

            if (cliente == null)
            {
                return NotFound();
            }

            ConsultaAPI.RegistraLog(new Log(personIn.LoginUser, JsonConvert.SerializeObject(cliente), JsonConvert.SerializeObject(personIn), "Update"));
            _passagemService.Update(id, personIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "DeletePassagem")]
        public IActionResult Delete(string id)
        {
            var person = _passagemService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            ConsultaAPI.RegistraLog(new Log(person.LoginUser, JsonConvert.SerializeObject(person), null, "Delete"));
            _passagemService.Remove(person.Id);

            return NoContent();
        }
    }
}
