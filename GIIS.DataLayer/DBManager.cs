using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Mono;
using Npgsql;
using NpgsqlTypes;
using System.Linq;
using System.Threading;

namespace GIIS.DataLayer
{

    public class DBManager
    {

        #region "Fields"
        #endregion
        private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["GiisConnectionString"].ConnectionString;


        #region "Properties"
        public static string ConnectionString
        {
            get { return connString; }

            set { connString = value; }
        }

        #endregion

        #region "Methods"


        public static DataTable ExecuteReaderCommand(string sqlCommand, CommandType commandType, List<NpgsqlParameter> parameters)
        {
            DataTable dataTable = new DataTable();
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sqlCommand, conn);
                cmd.CommandType = commandType;

                // Adding parameters to the stored procedure, if there are any
                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.Clear();
                    foreach (NpgsqlParameter iParam in parameters)
                    {
                        cmd.Parameters.Add(iParam);
                    }
                }

                try
                {
                    conn.Open();
                    dataTable.Load(cmd.ExecuteReader());
                }
                catch (Exception ex)
                {
                   throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }

            return dataTable;
        }

        public static int ExecuteNonQueryCommand(string sqlCommand, CommandType commandType, List<NpgsqlParameter> parameters)
        {
            int result = 0;

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sqlCommand, conn);
                cmd.CommandType = commandType;
                // Adding parameters to the stored procedure, if there are any
                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.Clear();
                    foreach (NpgsqlParameter iParam in parameters)
                    {
                        cmd.Parameters.Add(iParam);
                    }
                }

                try
                {
                    conn.Open();
#if DEBUG
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
#endif 
                    result = cmd.ExecuteNonQuery();
#if DEBUG
                    sw.Stop();
                    string parms = "", cmdDebug = cmd.CommandText;
                    if (parameters != null)
                        parameters.ForEach(o =>
                        {

                            if (o.Value is String || o.Value is DateTime)
                                cmdDebug = cmdDebug.Replace(o.ParameterName, String.Format("'{0}'", o.Value));

                            else
                                cmdDebug = cmdDebug.Replace(o.ParameterName, String.Format("{0}", o.Value ?? "NULL"));
                            parms += String.Format("{0}={1},", o.ParameterName, o.Value);
                        });
                    Trace.TraceInformation("{0} : {1} : {2}", cmd.CommandText, cmdDebug, sw.ElapsedMilliseconds);
#endif

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }

            return result;
        }

        public static object ExecuteScalarCommand(string sqlCommand, CommandType commandType, List<NpgsqlParameter> parameters)
        {
            object result = null;

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sqlCommand, conn);
                cmd.CommandType = commandType;
                // Adding parameters to the stored procedure, if there are any
                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.Clear();
                    foreach (NpgsqlParameter iParam in parameters)
                    {
                        cmd.Parameters.Add(iParam);
                    }
                }

                try
                {
                    conn.Open();
#if DEBUG
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
#endif 
                    result = cmd.ExecuteScalar();
#if DEBUG
                    sw.Stop();
                    string parms = "", cmdDebug = cmd.CommandText;
                    if(parameters != null)
                        parameters.ForEach(o =>
                        {

                            if (o.Value is String || o.Value is DateTime)
                                cmdDebug = cmdDebug.Replace(o.ParameterName, String.Format("'{0}'", o.Value));

                            else
                                cmdDebug = cmdDebug.Replace(o.ParameterName, String.Format("{0}", o.Value ?? "NULL"));
                            parms += String.Format("{0}={1},", o.ParameterName, o.Value);
                        });
                    Trace.TraceInformation("{0} : {1} : {2}", cmd.CommandText, cmdDebug, sw.ElapsedMilliseconds);

#endif
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }

            return result;
        }
        public static object ExecuteNonQueryImage(string sqlCommand, byte[] content)
        {
            int result = 0;
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sqlCommand, conn);
                cmd.CommandType = CommandType.Text;
                // Adding parameters to the stored procedure, if there are any

                cmd.Parameters.Clear();
                NpgsqlParameter param = new NpgsqlParameter(":content", NpgsqlDbType.Bytea);
                param.Value = content;
                cmd.Parameters.Add(param);

                try
                {
                    conn.Open();
#if DEBUG
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
#endif 
                    result = cmd.ExecuteNonQuery();
#if DEBUG
                    sw.Stop();
                    Trace.TraceInformation("{0} : {1} ", cmd.CommandText, sw.ElapsedMilliseconds);

#endif
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }

            return result;

        }

        #endregion
    }
}
