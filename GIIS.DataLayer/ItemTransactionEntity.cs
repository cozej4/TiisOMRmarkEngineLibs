using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class ItemTransaction
    {
        public static List<ItemTransaction> GetItemTransactionListByHealthFacilityCode(string hfcode)
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM_TRANSACTION"" where ""HEALTH_FACILITY_CODE"" = @hfcode ;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                    {
                    new NpgsqlParameter("@hfcode", DbType.String) { Value = hfcode }
                    };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetItemTransactionAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemTransaction", "GetItemTransactionList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
