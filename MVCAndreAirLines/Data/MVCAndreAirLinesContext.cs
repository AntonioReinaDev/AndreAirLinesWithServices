using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace MVCAndreAirLines.Data
{
    public class MVCAndreAirLinesContext : DbContext
    {
        public MVCAndreAirLinesContext (DbContextOptions<MVCAndreAirLinesContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Aeroporto> Aeroporto { get; set; }

        public DbSet<Models.Airport> Airport { get; set; }
    }
}
