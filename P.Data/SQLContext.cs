using P.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
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

        private const string _query_LookupJobNumber = @"
SELECT TOP 1 
	m.[PartNumber] [PartNumber]
FROM 
	[904646d9742f].[Remote].[dbo].[JobNumbers] l 
	LEFT JOIN [904646d9742f].[Remote].[dbo].[PartNumbers] m ON m.[ID] = l.[ID]
WHERE
	l.[JobNumber] = @JobNumber
ORDER BY 
	m.[Date] DESC";

        public string LookupPartNumber(string jobNumber)
        {
            if (string.IsNullOrWhiteSpace(jobNumber))
                return null;

            string connectionString = ConfigurationManager.ConnectionStrings["dev"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(_query_LookupJobNumber, connection))
                {
                    command.Parameters.Add(new SqlParameter("JobNumber", jobNumber));

                    string result = command.ExecuteScalar() as string;

                    return result;
                }
            }
        }
    }
}