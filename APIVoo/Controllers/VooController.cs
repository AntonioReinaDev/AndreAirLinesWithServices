using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIVoo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Services;
using Newtonsoft.Json;

namespace APIVoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VooController : ControllerBase
    {
        private readonly VooService _vooService;

        public VooController(VooService personService)
        {
            _vooService = personService;
        }

        [HttpGet]
        [Authorize(Roles = "GetVoo")]
        public ActionResult<List<Voo>> Get() =>
            _vooService.Get();

        [HttpGet("{id:length(24)}", Name = "GetCliente")]
        [Authorize(Roles = "GetIdVoo")]
        public ActionResult<Voo> Get(string id)
        {
            var cliente = _vooService.Get(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpGet("busca")]
        public ActionResult<Voo> GetVooChaves(string siglaorigem, string sigladestino)
        {
            var cliente = _vooService.GetVoo(siglaorigem, sigladestino);

            if (cliente == null)
            {
                return NotFound();
                throw new Exception();
            }

            return cliente;
        }

        [HttpPost]
        [Authorize(Roles = "PostVoo")]
        public async Task<ActionResult<Voo>> CreateAsync(Voo voo)
        {
            try
            {
                var usuario = await ConsultaAPI.BuscaUsuarioAsync(voo.LoginUser);
                if (usuario.Login == null)
                    return NotFound("Este usuario não existe");
                if (usuario.Funcao.Nome != "Administrador" && usuario.Funcao.Nome != "Atendente")
                    return BadRequest("Este usuario nao tem autorização para cadastrar precobase");
            }
            catch
            {
                StatusCode(408, "API DE USUARIOS ESTA FORA DO AR");
            }

            try
            {
                var origem = await ConsultaAPI.BuscaAeroportoAsync(voo.Origem.Sigla);
                if (origem == null)
                    return NotFound("Não existe aeroporto com a sigla de origem escolhida!");
                voo.Origem = origem;

                var destino = await ConsultaAPI.BuscaAeroportoAsync(voo.Destino.Sigla);
                if (destino == null)
                    return NotFound("Não existe aeroporto com a sigla de destino escolhida!");
                voo.Destino = destino;
            }
            catch
            {
                return StatusCode(408, "API DOS AEROPORTOS ESTA FORA DO AR");
            }

            try
            {
                var aeronave = await ConsultaAPI.BuscaAeronaveAsync(voo.Aeronave.Codigo);
                if (aeronave == null)
                    return NotFound("Não existe aeronave com codigo escrito");
                voo.Aeronave = aeronave;
            }
            catch
            {
                return StatusCode(408, "API DAS AERONAVES ESTA FORA DO AR");
            }

            ConsultaAPI.RegistraLog(new Log(voo.LoginUser, null, JsonConvert.SerializeObject(voo), "Create"));
            _vooService.Create(voo);

            return CreatedAtRoute("GetCliente", new { id = voo.Id.ToString() }, voo);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "PutVoo")]
        public IActionResult Update(string id, Voo personIn)
        {
            var cliente = _vooService.Get(id);

            if (cliente == null)
            {
                return NotFound();
            }

            ConsultaAPI.RegistraLog(new Log(personIn.LoginUser, JsonConvert.SerializeObject(cliente), JsonConvert.SerializeObject(personIn), "Update"));
            _vooService.Update(id, personIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "DeleteVoo")]
        public IActionResult Delete(string id)
        {
            var person = _vooService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            ConsultaAPI.RegistraLog(new Log(person.LoginUser, JsonConvert.SerializeObject(person), null, "Delete"));
            _vooService.Remove(person.Id);

            return NoContent();
        }
    }
}
