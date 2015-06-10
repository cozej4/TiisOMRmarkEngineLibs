using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class HealthFacilityType
    {
        public static HealthFacilityType GetHealthFacilityTypeByName(string name)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY_TYPE"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>() 
                { 
                    new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = name }
                };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityType", string.Format("RecordId: {0}", name), 4, DateTime.Now, 1);
                return GetHealthFacilityTypeAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityType", "GetHealthFacilityTypeByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static HealthFacilityType GetHealthFacilityTypeByCode(string code)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY_TYPE"" WHERE ""CODE"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>() 
                { 
                    new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = code }
                };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityType", string.Format("RecordId: {0}", code), 4, DateTime.Now, 1);
                return GetHealthFacilityTypeAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityType", "GetHealthFacilityTypeByCode", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<HealthFacilityType> GetPagedHealthFacilityTypeList(ref int maximumRows, ref int startRowIndex, string where)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY_TYPE"" WHERE " + where + @" ORDER BY ""NAME"" OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetHealthFacilityTypeAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("GetCountHealthFacilityTypeList", "GetPagedHealthFacilityTypeList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountHealthFacilityTypeList(string where)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""HEALTH_FACILITY_TYPE"" WHERE " + where + ";";
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("GetCountHealthFacilityTypeList", "GetCountGetCountHealthFacilityTypeListList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
