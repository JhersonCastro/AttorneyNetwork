using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace AttorneyNetwork.Database
{
    public class Context
    {
        private string connectionString = AttorneyNetwork.Properties.Settings.Default.ConnectionString;

        public int ExecuteNonQuery(string query, Dictionary<string, object> sDictionary = null)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    if (sDictionary != null)
                    {
                        foreach (KeyValuePair<string, object> entry in sDictionary)
                        {
                            command.Parameters.Add(entry.Key, entry.Value);
                        }
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }

        public DataSet executeSelect(string query, Dictionary<string, string> parameters = null)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand(query, conn);
                    if (parameters != null)
                    {
                        foreach (var entry in parameters)
                        {
                            cmd.Parameters.Add(entry.Key, entry.Value);
                        }
                    }
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
