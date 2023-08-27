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
        public SQLContext() : base(ConfigContext.ConnectionString) { }
    }
}
