using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.Data
{
    public static class ConfigContext
    {
#if DEBUG
        private const string _env = "dev";
#else
        private const string _env = "prod";
#endif
        private static ConnectionStringSettings SQLSettings => ConfigurationManager.ConnectionStrings[_env];
        public static string ConnectionString => SQLSettings.ConnectionString;
        public static string ProviderName => SQLSettings.ProviderName;
    }
}
