using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace GIIS.DataLayer
{
public partial class VWordTranslate
{

#region Properties
public Int32 LanguageId { get; set; }
public string Code { get; set; }
public string PageName { get; set; }
public string WordName { get; set; }
#endregion

#region GetData
public static List<VWordTranslate> GetVWordTranslateList(Int32 userId, string machineName)
{
try
{
string query = @"SELECT * FROM ""V_WORD_TRANSLATE"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetVWordTranslateAsList(dt, userId, machineName);
}
catch (Exception ex)
{
Log.InsertEntity("VWordTranslate", "GetVWordTranslateList", 1,ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static List<VWordTranslate> GetPagedVWordTranslateList(int maximumRows, int startRowIndex, string where, Int32 userId, string machineName)
{
try
{
string query = @"SELECT * FROM ""V_WORD_TRANSLATE"" WHERE " + where + " OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetVWordTranslateAsList(dt, userId, machineName);
}
catch (Exception ex)
{
Log.InsertEntity("VWordTranslate", "GetPagedVWordTranslateList", 1,ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static int GetCountVWordTranslateList(string where, Int32 userId, string machineName, int maximumRows, int startRowIndex)
{
try
{
string query = @"SELECT COUNT(*) FROM ""V_WORD_TRANSLATE"" WHERE " + where + ";";
object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
return int.Parse(count.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("VWordTranslate", "GetCountVWordTranslateList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
#endregion

#region Helper Methods
public static VWordTranslate GetVWordTranslateAsObject(DataTable dt, Int32 userId, string machineName)
{
foreach (DataRow row in dt.Rows)
{
try
{
VWordTranslate o = new VWordTranslate();
o.LanguageId = Helper.ConvertToInt(row["Language_Id"]);
o.Code = row["Code"].ToString();
o.PageName = row["Page_Name"].ToString();
o.WordName = row["Word_Name"].ToString();
return o;
}
catch (Exception ex)
{
Log.InsertEntity("VWordTranslate", "GetVWordTranslateAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<VWordTranslate> GetVWordTranslateAsList(DataTable dt, Int32 userId, string machineName)
{
List<VWordTranslate> oList = new List<VWordTranslate>();
foreach (DataRow row in dt.Rows)
{
try
{
VWordTranslate o = new VWordTranslate();
o.LanguageId = Helper.ConvertToInt(row["Language_Id"]);
o.Code = row["Code"].ToString();
o.PageName = row["Page_Name"].ToString();
o.WordName = row["Word_Name"].ToString();
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("VWordTranslate", "GetVWordTranslateAsList", 1,ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
