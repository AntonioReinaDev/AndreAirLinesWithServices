using System.Collections.Generic;
using APIAirportDapper.Service;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace APIAirportDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly AirportService _airportService;

        public AirportController(AirportService personService)
        {
            _airportService = personService;
        }

        [HttpGet]
        public ActionResult<List<Airport>> Get() =>
            _airportService.GetAll();

        [HttpGet("{code}")]
        public ActionResult<Airport> GetCode(string code) =>
            _airportService.GetbyCode(code);
    }
}
