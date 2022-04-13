using System.Collections.Generic;
using Models;

namespace APIAirportDapper.Repository
{
    public interface IAirportRepository
    {
        bool Add(Airport airport);
        List<Airport> GetAll();
        Airport GetByCode(string code);
    }
}
