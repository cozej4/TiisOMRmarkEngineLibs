using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace GIIS.DataLayer
{
    public partial class Weight
    {

        #region Properties
        public Int32 Id { get; set; }
        public Int32 Day { get; set; }
        public double SD4neg { get; set; }
        public double SD3neg { get; set; }
        public double SD2neg { get; set; }
        public double SD1neg { get; set; }
        public double SD0 { get; set; }
        public double SD1 { get; set; }
        public double SD2 { get; set; }
        public double SD3 { get; set; }
        public double SD4 { get; set; }
        public string Gender { get; set; }

        #endregion

        #region GetData
        public static List<Weight> GetWeightList()
        {
            try
            {
                string query = @"SELECT * FROM ""WEIGHT"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetWeightAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Weight", "GetWeightList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Weight> GetPagedWeightList(ref int maximumRows, ref int startRowIndex, string where)
        {
            try
            {
                string query = @"SELECT * FROM ""WEIGHT"" WHERE " + where + " OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetWeightAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Weight", "GetPagedWeightList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountWeightList(string where)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""WEIGHT"" WHERE " + where + ";";
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Weight", "GetCountWeightList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static Weight GetWeightById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""WEIGHT"" WHERE ""ID"" = " + i + "";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetWeightAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Weight", "GetWeightById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
          public static Weight GetWeight(int days, string gender)
        {
            try
            {
                string query = @"SELECT * FROM ""WEIGHT"" WHERE ""DAY""= " + days + @" AND ""GENDER"" = '" + gender + "'";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetWeightAsObject(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CRUD
        //public static int Insert(Weight o)
        //{
        //    try
        //    {
        //        string query = @"INSERT INTO ""WEIGHT"" (""DAY"", ""SD4neg"", ""SD3neg"", ""SD2neg"", ""SD1neg"", ""SD0"", ""SD1"", ""SD2"", ""SD3"", ""SD4"", ""GENDER"") VALUES (" + o.Day + @", " + o.SD4neg + @", " + o.SD3neg + @", " + o.SD2neg + @", " + o.SD1neg + @", " + o.Sd0 + @", " + o.Sd1 + @", " + o.Sd2 + @", " + o.Sd3 + @", " + o.Sd4 + @", " + o.Gender + @") returning ""ID"" ";
        //        object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
        //        Log.InsertEntity(1, "Success", id.ToString(), "Weight", "Insert");
        //        return int.Parse(id.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.InsertEntity("Weight", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
        //    }
        //    return -1;
        //}

        //public static int Update(Weight o, int id)
        //{
        //    try
        //    {
        //        string query = @"UPDATE ""WEIGHT"" SET ""ID"" = " + o.Id + @", ""DAY"" = " + o.Day + @", ""SD4neg"" = " + o.Sd4neg + @", ""SD3neg"" = " + o.Sd3neg + @", ""SD2neg"" = " + o.Sd2neg + @", ""SD1neg"" = " + o.Sd1neg + @", ""SD0"" = " + o.Sd0 + @", ""SD1"" = " + o.Sd1 + @", ""SD2"" = " + o.Sd2 + @", ""SD3"" = " + o.Sd3 + @", ""SD4"" = " + o.Sd4 + @", ""GENDER"" = " + o.Gender + @"WHERE ""ID"" = " + id;
        //        int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
        //        Log.InsertEntity(2, "Success", id.ToString(), "Weight", "Update");
        //        return rowAffected;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.InsertEntity("Weight", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
        //    }
        //    return -1;
        //}

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""WEIGHT"" WHERE ""ID"" = " + id;
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Weight", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static Weight GetWeightAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Weight o = new Weight();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Day = Helper.ConvertToInt(row["DAY"]);
                    o.SD4neg = Helper.ConvertToDecimal(row["SD4neg"]);
                    o.SD3neg = Helper.ConvertToDecimal(row["SD3neg"]);
                    o.SD2neg = Helper.ConvertToDecimal(row["SD2neg"]);
                    o.SD1neg = Helper.ConvertToDecimal(row["SD1neg"]);
                    o.SD0 = Helper.ConvertToDecimal(row["SD0"]);
                    o.SD1 = Helper.ConvertToDecimal(row["SD1"]);
                    o.SD2 = Helper.ConvertToDecimal(row["SD2"]);
                    o.SD3 = Helper.ConvertToDecimal(row["SD3"]);
                    o.SD4 = Helper.ConvertToDecimal(row["SD4"]);
                    o.Gender = row["GENDER"].ToString();
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Weight", "GetWeightAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<Weight> GetWeightAsList(DataTable dt)
        {
            List<Weight> oList = new List<Weight>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Weight o = new Weight();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Day = Helper.ConvertToInt(row["DAY"]);
                    o.SD4neg = Helper.ConvertToDecimal(row["SD4neg"]);
                    o.SD3neg = Helper.ConvertToDecimal(row["SD3neg"]);
                    o.SD2neg = Helper.ConvertToDecimal(row["SD2neg"]);
                    o.SD1neg = Helper.ConvertToDecimal(row["SD1neg"]);
                    o.SD0 = Helper.ConvertToDecimal(row["SD0"]);
                    o.SD1 = Helper.ConvertToDecimal(row["SD1"]);
                    o.SD2 = Helper.ConvertToDecimal(row["SD2"]);
                    o.SD3 = Helper.ConvertToDecimal(row["SD3"]);
                    o.SD4 = Helper.ConvertToDecimal(row["SD4"]);
                    o.Gender = row["GENDER"].ToString();
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Weight", "GetWeightAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
