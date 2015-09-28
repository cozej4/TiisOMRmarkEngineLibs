//*******************************************************************************
//Copyright 2015 TIIS - Tanzania Immunization Information System
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
 //******************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
public partial class WordTranslate
{

#region Properties
public Int32 Id { get; set; }
public Int32 LanguageId { get; set; }
public string Code { get; set; }
public string Name { get; set; }
public string PageName { get; set; }
public Language Language
{
get
{
if (this.LanguageId > 0)
return Language.GetLanguageById(this.LanguageId);
else
return null;
}
}
public string EnglishVersion
{
    get
    {
        return WordTranslate.GetWordByLanguageAndCode(1, this.Code);
    }
}
#endregion

#region GetData
public static List<WordTranslate> GetWordTranslateList()
{
try
{
string query = @"SELECT * FROM ""WORD_TRANSLATE"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetWordTranslateAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("WordTranslate", "GetWordTranslateList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetWordTranslateForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""WORD_TRANSLATE"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("WordTranslate", "GetWordTranslateForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static WordTranslate GetWordTranslateById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""WORD_TRANSLATE"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("WordTranslate", i.ToString(), 4, DateTime.Now, 1);
return GetWordTranslateAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("WordTranslate", "GetWordTranslateById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(WordTranslate o)
{
try
{
string query = @"INSERT INTO ""WORD_TRANSLATE"" (""LANGUAGE_ID"", ""CODE"", ""NAME"", ""PAGE_NAME"") VALUES (@LanguageId, @Code, @Name, @PageName) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@LanguageId", DbType.Int32)  { Value = (object)o.LanguageId ?? DBNull.Value },
new NpgsqlParameter("@Code", DbType.String)  { Value = (object)o.Code ?? DBNull.Value },
new NpgsqlParameter("@Name", DbType.String)  { Value = (object)o.Name ?? DBNull.Value },
new NpgsqlParameter("@PageName", DbType.String)  { Value = (object)o.PageName ?? DBNull.Value }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("WordTranslate", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("WordTranslate", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(WordTranslate o)
{
try
{
string query = @"UPDATE ""WORD_TRANSLATE"" SET ""ID"" = @Id, ""LANGUAGE_ID"" = @LanguageId, ""CODE"" = @Code, ""NAME"" = @Name, ""PAGE_NAME"" = @PageName WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@LanguageId", DbType.Int32)  { Value = (object)o.LanguageId ?? DBNull.Value },
new NpgsqlParameter("@Code", DbType.String)  { Value = (object)o.Code ?? DBNull.Value },
new NpgsqlParameter("@Name", DbType.String)  { Value = (object)o.Name ?? DBNull.Value },
new NpgsqlParameter("@PageName", DbType.String)  { Value = (object)o.PageName ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("WordTranslate", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("WordTranslate", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""WORD_TRANSLATE"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("WordTranslate", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("WordTranslate", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region Helper Methods
public static WordTranslate GetWordTranslateAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
WordTranslate o = new WordTranslate();
o.Id = Helper.ConvertToInt(row["ID"]);
o.LanguageId = Helper.ConvertToInt(row["LANGUAGE_ID"]);
o.Code = row["CODE"].ToString();
o.Name = row["NAME"].ToString();
o.PageName = row["PAGE_NAME"].ToString();
return o;
}
catch (Exception ex)
{
Log.InsertEntity("WordTranslate", "GetWordTranslateAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<WordTranslate> GetWordTranslateAsList(DataTable dt)
{
List<WordTranslate> oList = new List<WordTranslate>();
foreach (DataRow row in dt.Rows)
{
try
{
WordTranslate o = new WordTranslate();
o.Id = Helper.ConvertToInt(row["ID"]);
o.LanguageId = Helper.ConvertToInt(row["LANGUAGE_ID"]);
o.Code = row["CODE"].ToString();
o.Name = row["NAME"].ToString();
o.PageName = row["PAGE_NAME"].ToString();
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("WordTranslate", "GetWordTranslateAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
