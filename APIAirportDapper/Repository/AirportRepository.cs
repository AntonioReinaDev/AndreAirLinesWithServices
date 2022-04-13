using System.Collections.Generic;
using System.Data.SqlClient;
using APIAirportDapper.Config;
using Dapper;
using Models;

namespace APIAirportDapper.Repository
{
    public class AirportRepository : IAirportRepository
    {
        private string _conn;

        public AirportRepository()
        {
            _conn = DataBaseConfiguration.Get();
        }

        public bool Add(Airport airport)
        {
            bool status = false;

            using (var db = new SqlConnection(_conn))
            {
                db.Open();
                db.Execute(Airport.INSERT, airport);
                status = true;
            }

            return status;
        }

        public List<Airport> GetAll()
        {
            using (var db = new SqlConnection(_conn))
            {
                db.Open();
                var airports = db.Query<Airport>(Airport.SELECTALL);
                return (List<Airport>)airports;
            }
        }

        public Airport GetByCode(string code)
        {
            using (var db = new SqlConnection(_conn))
            {
                db.Open();
                try
                {
                    Airport airport = db.QueryFirst<Airport>(Airport.SELECTBYCODE + "'" + code + "'");
                    return airport;
                }
                catch
                {
                    return null;
                }

            }
        }
    }
}
