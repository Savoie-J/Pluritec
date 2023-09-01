using P.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.Data
{
    public class SQLContext : DbContext
    {
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<SkippedSerialNumber> SkippedSerialNumbers { get; set; }
        public DbSet<Layer> Layers { get; set; }

        public SQLContext() : base(ConfigContext.ConnectionString) { }
    }
}
