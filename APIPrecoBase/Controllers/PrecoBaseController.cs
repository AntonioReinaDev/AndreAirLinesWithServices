using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIPrecoBase.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Services;
using Newtonsoft.Json;

namespace APIPrecoBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrecoBaseController : ControllerBase
    {
        private readonly PrecoBaseService _precoBaseService;

        public PrecoBaseController(PrecoBaseService personService)
        {
            _precoBaseService = personService;
        }

        [HttpGet]
        [Authorize(Roles = "GetPrecoBase")]
        public ActionResult<List<PrecoBase>> Get() =>
            _precoBaseService.Get();

        [HttpGet("{id:length(24)}", Name = "GetCliente")]
        [Authorize(Roles = "GetIdPrecoBase")]
        public ActionResult<PrecoBase> Get(string id)
        {
            var cliente = _precoBaseService.Get(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpGet("busca")]
        public ActionResult<PrecoBase> GetVooChaves(string siglaorigem, string sigladestino)
        {
            var cliente = _precoBaseService.GetPrecoBase(siglaorigem, sigladestino);

            if (cliente == null)
            {
                return NotFound();
                throw new Exception();
            }

            return cliente;
        }

        [HttpPost]
        [Authorize(Roles = "PostPrecoBase")]
        public async Task<ActionResult<PrecoBase>> CreateAsync(PrecoBase precobase)
        {
            try
            {
                var usuario = await ConsultaAPI.BuscaUsuarioAsync(precobase.LoginUser);
                if (usuario.Login == null)
                    return NotFound("Este usuario não existe");
                if (usuario.Funcao.Nome != "Administrador")
                    return BadRequest("Este usuario nao tem autorização para cadastrar precobase");
            }
            catch
            {
                return NotFound("API DE USUARIOS ESTA FORA DO AR");
            }

            try
            {
                var origem = await ConsultaAPI.BuscaAeroportoAsync(precobase.Origem.Sigla);
                if (origem == null)
                    return NotFound("Não existe aeroporto com a sigla de origem escolhida!");
                precobase.Origem = origem;

                var destino = await ConsultaAPI.BuscaAeroportoAsync(precobase.Destino.Sigla);
                if (destino == null)
                    return NotFound("Não existe aeroporto com a sigla de destino escolhida!");
                precobase.Destino = destino;
            }
            catch
            {
                return StatusCode(408, "API DOS AEROPORTOS ESTA FORA DO AR");
            }

            ConsultaAPI.RegistraLog(new Log(precobase.LoginUser, null, JsonConvert.SerializeObject(precobase), "Create"));
            _precoBaseService.Create(precobase);

            return CreatedAtRoute("GetCliente", new { id = precobase.Id.ToString() }, precobase);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "PutPrecoBase")]
        public IActionResult Update(string id, PrecoBase personIn)
        {
            var cliente = _precoBaseService.Get(id);

            if (cliente == null)
            {
                return NotFound();
            }

            ConsultaAPI.RegistraLog(new Log(personIn.LoginUser, JsonConvert.SerializeObject(cliente), JsonConvert.SerializeObject(personIn), "Update"));
            _precoBaseService.Update(id, personIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "DeletePrecoBase")]
        public IActionResult Delete(string id)
        {
            var person = _precoBaseService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            ConsultaAPI.RegistraLog(new Log(person.LoginUser, JsonConvert.SerializeObject(person), null, "Delete"));
            _precoBaseService.Remove(person.Id);

            return NoContent();
        }
    }
}
