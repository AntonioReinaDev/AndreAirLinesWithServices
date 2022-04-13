using System.Collections.Generic;
using System.Threading.Tasks;
using APILog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Services;

namespace APILog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly LogService _logService;

        public LogController(LogService personService)
        {
            _logService = personService;
        }

        [HttpGet]
        [Authorize(Roles = "GetLog")]
        public ActionResult<List<Log>> Get() =>
            _logService.Get();

        [HttpGet("{id:length(24)}", Name = "GetCliente")]
        public ActionResult<Log> Get(string id)
        {
            var cliente = _logService.Get(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpPost]
        public async Task<ActionResult<Log>> CreateAsync(Log log)
        {
            try
            {
                var usuario = await ConsultaAPI.BuscaUsuarioAsync(log.Usuario);
                if (usuario == null)
                    return NotFound("Este usuário não existe");
            }
            catch
            {
                return StatusCode(408, "API DE USUARIOS ESTA FORA DO AR");
            }


            _logService.Create(log);

            return CreatedAtRoute("GetCliente", new { id = log.Id.ToString() }, log);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Log personIn)
        {
            var cliente = _logService.Get(id);

            if (cliente == null)
            {
                return NotFound();
            }

            _logService.Update(id, personIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var person = _logService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            _logService.Remove(person.Id);

            return NoContent();
        }
    }
}
