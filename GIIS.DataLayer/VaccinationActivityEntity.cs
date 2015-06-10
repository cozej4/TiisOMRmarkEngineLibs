using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GIIS.DataLayer
{
    public class VaccinationActivity
    {
        public static DataTable GetVaccinationActivity(int languageId, string where, string months)
        {
            try
            {
                string query = string.Format(@" Select * from crosstab('

SELECT ""DOSE"".""ID"", replace(""DOSE"".""FULLNAME"", '' '', '''') AS Vaksina,  
EXTRACT(MONTH FROM ""VACCINATION_EVENT"".""VACCINATION_DATE"")::INTEGER as MUAJI,  
CAST(Count(*) AS TEXT) AS VLERA
FROM ""DOSE""
LEFT OUTER JOIN ""VACCINATION_EVENT"" ON ""VACCINATION_EVENT"".""DOSE_ID"" = ""DOSE"".""ID""
LEFT OUTER JOIN ""HEALTH_FACILITY"" ON ""VACCINATION_EVENT"".""HEALTH_FACILITY_ID"" = ""HEALTH_FACILITY"".""ID""  
LEFT OUTER JOIN ""CHILD"" ON ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID""  

WHERE ""CHILD"".""STATUS_ID"" = 1
AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = ''True''
AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = 0
AND ""VACCINE_LOT_ID"" > 0
{0}

GROUP BY ""DOSE"".""ID"",""DOSE"".""FULLNAME"", EXTRACT(MONTH FROM ""VACCINATION_EVENT"".""VACCINATION_DATE"")::INTEGER ORDER BY 1,3',
'select m from generate_series(1, 12) m')
AS kolonat(""ID"" int, {1})

Union    Select 100 as ""ID"", 'Total' as ""Vaksina"", 0,0,0,0,0,0,0,0,0,0,0,0       Order by ""ID"" ; ", where, months);

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationActivity", "GetVaccinationActivity", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                return null;
            }
        }

        public static DataTable GetVaccinationActivity(int languageId, string where, string months, int itemId)
        {
            try
            {
                string query = string.Format(@" Select * from crosstab('

SELECT ""DOSE"".""ID"", replace(""DOSE"".""FULLNAME"", '' '', '''') AS Vaksina,  
EXTRACT(MONTH FROM ""VACCINATION_EVENT"".""VACCINATION_DATE"")::INTEGER as MUAJI,  
CAST(Count(*) AS TEXT) AS VLERA
FROM ""DOSE""
LEFT OUTER JOIN ""VACCINATION_EVENT"" ON ""VACCINATION_EVENT"".""DOSE_ID"" = ""DOSE"".""ID""
LEFT OUTER JOIN ""HEALTH_FACILITY"" ON ""VACCINATION_EVENT"".""HEALTH_FACILITY_ID"" = ""HEALTH_FACILITY"".""ID""  
LEFT OUTER JOIN ""CHILD"" ON ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID""
LEFT OUTER JOIN ""ITEM_LOT"" ON ""ITEM_LOT"".""ID"" =  ""VACCINATION_EVENT"".""VACCINE_LOT_ID""

WHERE ""CHILD"".""STATUS_ID"" = 1
AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = ''True''
AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = 0
AND ""VACCINE_LOT_ID"" > 0
AND ""ITEM_LOT"".""ITEM_ID"" = {2}
{0}

GROUP BY ""DOSE"".""ID"",""DOSE"".""FULLNAME"", EXTRACT(MONTH FROM ""VACCINATION_EVENT"".""VACCINATION_DATE"")::INTEGER ORDER BY 1,3',
'select m from generate_series(1, 12) m')
AS kolonat(""ID"" int, {1})

Union    Select 100 as ""ID"", 'Total' as ""Vaksina"", 0,0,0,0,0,0,0,0,0,0,0,0       Order by ""ID"" ; ", where, months, itemId);

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationActivity", "GetVaccinationActivity", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                return null;
            }
        }

        public static int GetPlanedVaccinationActivity(int languageId, string where)
        {
            try
            {
                string query = string.Format(@" Select Count(*) from ""VACCINATION_EVENT"", ""VACCINATION_APPOINTMENT"", ""HEALTH_FACILITY"", ""CHILD""
where ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID""
AND ""VACCINATION_EVENT"".""APPOINTMENT_ID"" = ""VACCINATION_APPOINTMENT"".""ID""
AND ""CHILD"".""HEALTHCENTER_ID"" = ""HEALTH_FACILITY"".""ID""
AND ""CHILD"".""STATUS_ID"" = 1 {0}", where);

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationActivity", "GetPlanedVaccinationActivity", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}