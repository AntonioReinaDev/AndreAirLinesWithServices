using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIAeroporto.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Services;
using Newtonsoft.Json;

namespace APIAeroporto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AeroportoController : ControllerBase
    {
        private readonly AeroportoService _aeroportoService;

        public AeroportoController(AeroportoService personService)
        {
            _aeroportoService = personService;
        }

        [HttpGet]
        [Authorize(Roles = "GetAeroporto")]
        public ActionResult<List<Aeroporto>> Get() =>
            _aeroportoService.Get();

        [HttpGet("{id:length(24)}", Name = "GetCliente")]
        [Authorize(Roles = "GetIdAeroporto")]
        public ActionResult<Aeroporto> Get(string id)
        {
            var cliente = _aeroportoService.Get(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpGet("busca")]
        public ActionResult<Aeroporto> GetPassageiroCPF(string sigla)
        {
            var cliente = _aeroportoService.GetSigla(sigla);

            if (cliente == null)
            {
                return NotFound();
                throw new Exception();
            }

            return cliente;
        }

        [HttpPost]
        [Authorize(Roles = "PostAeroporto")]
        public async Task<ActionResult<Aeroporto>> CreateAsync(Aeroporto aeroporto)
        {
            try
            {
                var usuario = await ConsultaAPI.BuscaUsuarioAsync(aeroporto.LoginUser);
                if (usuario.Login == null)
                    return NotFound("Este usuario não existe");
                if (usuario.Funcao.Nome != "Administrador")
                    return BadRequest("Este usuario nao tem autorização para cadastrar precobase");
            }
            catch
            {
                return NotFound("API DE USUARIOS ESTA FORA DO AR");
            }

            if (!SiglaService.VerificaAeroportoSigla(aeroporto.Sigla, _aeroportoService))
                return BadRequest("Já existe um aeroporto com a sigla escolhida");

            var end = await VerificaCep.CEPVerify(aeroporto.Endereco.Cep);
            if (end != null)
            {
                int num = aeroporto.Endereco.Numero;
                aeroporto.Endereco = new Endereco(end.Localidade, end.Logradouro, end.Bairro, end.Uf, end.Complemento, end.Cep);
                aeroporto.Endereco.Numero = num;
            }

            var airport = await AirportsQuery.GetAirportAsync(aeroporto.Sigla);
            if (airport != null)
            {
                aeroporto.Endereco.Localidade = airport.City;
                aeroporto.Endereco.Continente = airport.Continent;
                aeroporto.Endereco.Pais = airport.Country;
            }

            ConsultaAPI.RegistraLog(new Log(aeroporto.LoginUser, null, JsonConvert.SerializeObject(aeroporto), "Create"));
            _aeroportoService.Create(aeroporto);

            return CreatedAtRoute("GetCliente", new { id = aeroporto.Id.ToString() }, aeroporto);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "PutAeroporto")]
        public IActionResult Update(string id, Aeroporto personIn)
        {
            var cliente = _aeroportoService.Get(id);

            if (cliente == null)
            {
                return NotFound();
            }

            ConsultaAPI.RegistraLog(new Log(personIn.LoginUser, JsonConvert.SerializeObject(cliente), JsonConvert.SerializeObject(personIn), "Update"));
            _aeroportoService.Update(id, personIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "DeleteAeroporto")]
        public IActionResult Delete(string id)
        {
            var person = _aeroportoService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            ConsultaAPI.RegistraLog(new Log(person.LoginUser, JsonConvert.SerializeObject(person), null, "Delete"));
            _aeroportoService.Remove(person.Id);

            return NoContent();
        }
    }
}
