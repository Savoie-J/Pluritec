using Newtonsoft.Json;
using P.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace P.Web.Business
{
    public class MdbContext : IDisposable
    {
        private readonly string _mdbFile;

        private string ConnectionString => $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={_mdbFile}";

        private readonly OleDbConnection _connection;

        public MdbContext(string mdbFile)
        {
            this._mdbFile = mdbFile;
            this._connection = new OleDbConnection(this.ConnectionString);
            this._connection.Open();
        }

        public void Dispose()
        {
            this._connection.Dispose();
        }

        private List<LayerData> GetLayerData(OleDbCommand cmd)
        {
            List<dynamic> data = new List<dynamic>();

            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                // loop through each row
                while (reader.Read())
                {
                    dynamic flexible = new ExpandoObject();
                    IDictionary<string, object> fields = (IDictionary<string, object>)flexible;

                    // loop through each column
                    for (int index = 0; index < reader.FieldCount; index++)
                    {
                        string name = reader.GetName(index);
                        object value = reader.GetValue(index);
                        fields.Add(name, value);
                    }
                    data.Add(flexible);
                }
            }

            string serialized = JsonConvert.SerializeObject(data);
            List<LayerData> results = JsonConvert.DeserializeObject<List<LayerData>>(serialized);

            return results;
        }

        private const string _query_GetMeasurementData = @"
SELECT * FROM Layers";

        public List<LayerData> GetMeasurementData()
        {
            using (OleDbCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = _query_GetMeasurementData;

                return GetLayerData(cmd);
            }
        }
    }
}