using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GIIS.DataLayer
{
    public class CohortCoverage
    {
        public static DataTable GetCohortCoverage(int languageId, string where, DateTime firstDate, DateTime today, DateTime enddDate, int vaccineId, int community, int domicile)
        {
            try
            {
                string helper = "";

                List<NonvaccinationReason> nvrList = NonvaccinationReason.GetNonvaccinationReasonList();
                foreach (NonvaccinationReason nvr in nvrList)
                {
                    string s = string.Format(@",( -- non vaccinated for a reason			
			                Select Count(*) from ""VACCINATION_EVENT"", ""CHILD""
			                where ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID"" AND ""DOSE_ID"" = v_parent_rec.""ID"" 
			                AND  ""BIRTHDATE"" between cast(firstdate as date) and cast(todate as date)   AND  ""VACCINATION_DATE"" between cast(firstdate as date) and cast(enddate as date) 
			                AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = 'False' AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = {0}  AND ""CHILD"".""STATUS_ID"" = 1
			                AND ""HEALTHCENTER_ID"" in ({1})
			                )", nvr.Id, where);

                    if (where == "1")
                        s = string.Format(@",( -- non vaccinated for a reason			
			                Select Count(*) from ""VACCINATION_EVENT"", ""CHILD""
			                where ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID"" AND ""DOSE_ID"" = v_parent_rec.""ID"" 
			                AND  ""BIRTHDATE"" between cast(firstdate as date) and cast(todate as date)   AND  ""VACCINATION_DATE"" between cast(firstdate as date) and cast(enddate as date) 
			                AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = 'False' AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = {0}  AND ""CHILD"".""STATUS_ID"" = 1
			                )", nvr.Id);

                    helper += (s + Environment.NewLine);
                }
                string optwhere = String.Empty;
                if (vaccineId != -1)
                    optwhere = String.Format(@" AND ""DOSE"".""ID"" = {0}", vaccineId);
                string conwhere = string.Empty;
                if (community != 0)
                    conwhere = String.Format(@" AND ""COMMUNITY_ID"" = {0}", community);
                if (domicile !=0)
                    conwhere += String.Format(@" AND ""DOMICILE_ID"" = {0}", domicile);

               
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format(@"CREATE OR REPLACE FUNCTION cohort_helper(optwhere text, firstdate text, todate text, enddate text) RETURNS void AS
                $BODY$
                DECLARE
                    v_parent_rec RECORD;
                BEGIN

                    FOR v_parent_rec IN SELECT ""DOSE"".* FROM ""DOSE"" join ""SCHEDULED_VACCINATION"" on ""DOSE"".""SCHEDULED_VACCINATION_ID"" = ""SCHEDULED_VACCINATION"".""ID"" 
				                WHERE ""ENTRY_DATE"" <= cast(todate as date) AND (""EXIT_DATE"" >= cast(todate as date) OR ""EXIT_DATE"" is null OR ""EXIT_DATE"" = '0001-01-01')
                               {1}  ORDER BY ""ID"" 
	                LOOP
        
		                INSERT INTO ""HELPER""

                        select
		                (
                           (
			                select ""ID"" FROM ""DOSE"" WHERE ""ID"" = v_parent_rec.""ID""
			
			                ), (
			                select ""FULLNAME"" FROM ""DOSE"" WHERE ""ID"" = v_parent_rec.""ID""
			
			                ), ( -- cohort
			
			                Select Count(*) from ""CHILD"" 
			                Where ""CHILD"".""STATUS_ID"" = 1
			                AND ""BIRTHDATE"" between cast(firstdate as date) and cast(todate as date)
			                AND ""HEALTHCENTER_ID"" in ({2}) {3}

			                ) ,( -- immunized
			  
			                Select Count(*) from ""VACCINATION_EVENT"", ""CHILD""
			                where ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID"" AND ""DOSE_ID"" = v_parent_rec.""ID"" 
			                AND  ""BIRTHDATE"" between cast(firstdate as date) and cast(todate as date) AND ""VACCINATION_DATE"" between cast(firstdate as date) and cast(enddate as date) 
			                AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = 'True'  AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = 0 AND ""CHILD"".""STATUS_ID"" = 1
			                AND ""HEALTHCENTER_ID"" in ({2}) {3}
			
			                )

                            {0}
		                );

                    END LOOP;
                    RETURN;
                END;
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;
                ", helper, optwhere, where,conwhere));

                if (where == "1")
                {
                    sb.Clear();
                    sb.AppendLine(string.Format(@"CREATE OR REPLACE FUNCTION cohort_helper(optwhere text, firstdate text, todate text, enddate text) RETURNS void AS
                $BODY$
                DECLARE
                    v_parent_rec RECORD;
                BEGIN

                    FOR v_parent_rec IN SELECT ""DOSE"".* FROM ""DOSE"" join ""SCHEDULED_VACCINATION"" on ""DOSE"".""SCHEDULED_VACCINATION_ID"" = ""SCHEDULED_VACCINATION"".""ID"" 
				                WHERE ""ENTRY_DATE"" <= cast(todate as date) AND (""EXIT_DATE"" >= cast(todate as date) OR ""EXIT_DATE"" is null OR ""EXIT_DATE"" = '0001-01-01')
                               {1}  ORDER BY ""ID"" 
	                LOOP
		                INSERT INTO ""HELPER""
                        select
		                (
                           (
			                select ""ID"" FROM ""DOSE"" WHERE ""ID"" = v_parent_rec.""ID""
			
			                ), (
			                select ""FULLNAME"" FROM ""DOSE"" WHERE ""ID"" = v_parent_rec.""ID""
			
			                ), ( -- cohort
			
			                Select Count(*) from ""CHILD"" 
			                Where ""CHILD"".""STATUS_ID"" = 1
			                AND ""BIRTHDATE"" between cast(firstdate as date) and cast(todate as date)
			                 {2}

			                ) ,( -- immunized
			  
			                Select Count(*) from ""VACCINATION_EVENT"", ""CHILD""
			                where ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID"" AND ""DOSE_ID"" = v_parent_rec.""ID"" 
			                AND  ""BIRTHDATE"" between cast(firstdate as date) and cast(todate as date) AND ""VACCINATION_DATE"" between cast(firstdate as date) and cast(enddate as date) 
			                AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = 'True'  AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = 0 AND ""CHILD"".""STATUS_ID"" = 1
			                 {2} 
                            )
                            {0}
		                );
                    END LOOP;
                    RETURN;
                END;
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;
                ", helper, optwhere, conwhere));
                }

                DBManager.ExecuteNonQueryCommand(sb.ToString(), CommandType.Text, null);

                string truncateQuery = string.Format(@"TRUNCATE ""HELPER"";");
                DBManager.ExecuteNonQueryCommand(truncateQuery, CommandType.Text, null);

                string funcQueryHelper = string.Format("SELECT cohort_helper('{0}', '{1}', '{2}', '{3}');", " ", firstDate.ToString("yyyy-MM-dd"), today.ToString("yyyy-MM-dd"), enddDate.ToString("yyyy-MM-dd"));
                DataTable dtfHelper = DBManager.ExecuteReaderCommand(funcQueryHelper, CommandType.Text, null);

                string funcQuery = string.Format(@"select * from ""HELPER"";");
                DataTable dtf = DBManager.ExecuteReaderCommand(funcQuery, CommandType.Text, null);
                
                return dtf;

                //string query = "";
                //if (dtf.Rows.Count != 0)
                //{
                //    query = string.Format("{1} Where 1=1 {0};", where, dtf.Rows[0][0].ToString());
                //}

                //DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                //return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("CohortCoverage", "GetCohortCoverage", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                return null;
            }
        }

        public static DataTable GetPlannedCoverage(int languageId, string where, DateTime firstDate, DateTime today, DateTime enddDate, int vaccineId, int domicile)
        {
            try
            {
                string helper = "";

                List<NonvaccinationReason> nvrList = NonvaccinationReason.GetNonvaccinationReasonList();
                foreach (NonvaccinationReason nvr in nvrList)
                {
                    string s = string.Format(@",( -- non vaccinated for a reason			
			                Select Count(*) from ""VACCINATION_EVENT"", ""CHILD""
			                where ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID"" AND ""DOSE_ID"" = v_parent_rec.""ID"" 
			                AND  ""SCHEDULED_DATE"" between cast(firstdate as date) and cast(todate as date)   AND  ""VACCINATION_DATE"" between cast(firstdate as date) and cast(enddate as date) 
			                AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = 'False' AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = {0}  AND ""CHILD"".""STATUS_ID"" = 1
			                AND ""HEALTHCENTER_ID"" in ({1})
			                )", nvr.Id, where);
                    if (where == "1")
                        s = string.Format(@",( -- non vaccinated for a reason			
			                Select Count(*) from ""VACCINATION_EVENT"", ""CHILD""
			                where ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID"" AND ""DOSE_ID"" = v_parent_rec.""ID"" 
			                AND  ""SCHEDULED_DATE"" between cast(firstdate as date) and cast(todate as date)   AND  ""VACCINATION_DATE"" between cast(firstdate as date) and cast(enddate as date) 
			                AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = 'False' AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = {0}  AND ""CHILD"".""STATUS_ID"" = 1
			                )", nvr.Id);

                    helper += (s + Environment.NewLine);
                }
                string optwhere = String.Empty;
                if (vaccineId != -1)
                    optwhere = String.Format(@" AND ""DOSE"".""ID"" = {0}", vaccineId);
                string conwhere = string.Empty;
                if (domicile != 0)
                    conwhere += String.Format(@" AND ""DOMICILE_ID"" = {0}", domicile);


                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format(@"CREATE OR REPLACE FUNCTION cohort_helper_planned(optwhere text, firstdate text, todate text, enddate text) RETURNS void AS
                $BODY$
                DECLARE
                    v_parent_rec RECORD;
                BEGIN

                    FOR v_parent_rec IN SELECT ""DOSE"".* FROM ""DOSE"" join ""SCHEDULED_VACCINATION"" on ""DOSE"".""SCHEDULED_VACCINATION_ID"" = ""SCHEDULED_VACCINATION"".""ID"" 
				                WHERE ""ENTRY_DATE"" <= cast(todate as date) AND (""EXIT_DATE"" >= cast(todate as date) OR ""EXIT_DATE"" is null OR ""EXIT_DATE"" = '0001-01-01')
                               {1}  ORDER BY ""ID"" 
	                LOOP
        
		                INSERT INTO ""HELPER""

                        select
		                (
                           (
			                select ""ID"" FROM ""DOSE"" WHERE ""ID"" = v_parent_rec.""ID""
			
			                ), (
			                select ""FULLNAME"" FROM ""DOSE"" WHERE ""ID"" = v_parent_rec.""ID""
			
			                ), ( -- planned
			
			                Select Count(*) from ""CHILD"", ""VACCINATION_EVENT""
			                Where ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID"" AND ""CHILD"".""STATUS_ID"" = 1
			                AND ""SCHEDULED_DATE"" between cast(firstdate as date) and cast(todate as date) AND  ""DOSE_ID"" = v_parent_rec.""ID""
			                AND ""HEALTHCENTER_ID"" in ({2}) {3}

			                ) ,( -- immunized
			  
			                Select Count(*) from ""VACCINATION_EVENT"", ""CHILD""
			                where ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID""  AND ""DOSE_ID"" = v_parent_rec.""ID"" 
			                AND  ""SCHEDULED_DATE"" between cast(firstdate as date) and cast(todate as date) AND ""VACCINATION_DATE"" between cast(firstdate as date) and cast(enddate as date) 
			                AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = 'True'  AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = 0 AND ""CHILD"".""STATUS_ID"" = 1
			                AND ""HEALTHCENTER_ID"" in ({2}) {3}
			
			                )

                            {0}
		                );

                    END LOOP;
                    RETURN;
                END;
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;
                ", helper, optwhere, where, conwhere));

                if (where == "1")
                {
                    sb.Clear();
                    sb.AppendLine(string.Format(@"CREATE OR REPLACE FUNCTION cohort_helper_planned(optwhere text, firstdate text, todate text, enddate text) RETURNS void AS
                $BODY$
                DECLARE
                    v_parent_rec RECORD;
                BEGIN

                    FOR v_parent_rec IN SELECT ""DOSE"".* FROM ""DOSE"" join ""SCHEDULED_VACCINATION"" on ""DOSE"".""SCHEDULED_VACCINATION_ID"" = ""SCHEDULED_VACCINATION"".""ID"" 
				                WHERE ""ENTRY_DATE"" <= cast(todate as date) AND (""EXIT_DATE"" >= cast(todate as date) OR ""EXIT_DATE"" is null OR ""EXIT_DATE"" = '0001-01-01')
                               {1}  ORDER BY ""ID"" 
	                LOOP
		                INSERT INTO ""HELPER""
                        select
		                (
                           (
			                select ""ID"" FROM ""DOSE"" WHERE ""ID"" = v_parent_rec.""ID""
			
			                ), (
			                select ""FULLNAME"" FROM ""DOSE"" WHERE ""ID"" = v_parent_rec.""ID""
			
			                ), ( -- planned
			
			                Select Count(*) from ""CHILD"", ""VACCINATION_EVENT""
			                Where ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID"" AND ""CHILD"".""STATUS_ID"" = 1
			                AND ""SCHEDULED_DATE"" between cast(firstdate as date) and cast(todate as date) AND  ""DOSE_ID"" = v_parent_rec.""ID""
			                 {2}
			                ) ,( -- immunized
			  
			                Select Count(*) from ""VACCINATION_EVENT"", ""CHILD""
			                where ""VACCINATION_EVENT"".""CHILD_ID"" = ""CHILD"".""ID""  AND ""DOSE_ID"" = v_parent_rec.""ID"" 
			                AND  ""SCHEDULED_DATE"" between cast(firstdate as date) and cast(todate as date) AND ""VACCINATION_DATE"" between cast(firstdate as date) and cast(enddate as date) 
			                AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = 'True'  AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = 0 AND ""CHILD"".""STATUS_ID"" = 1
			                {2}
			                )
                            {0}
		                );
                    END LOOP;
                    RETURN;
                END;
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;
                ", helper, optwhere, conwhere));
                }

                DBManager.ExecuteNonQueryCommand(sb.ToString(), CommandType.Text, null);

                string truncateQuery = string.Format(@"TRUNCATE ""HELPER"";");
                DBManager.ExecuteNonQueryCommand(truncateQuery, CommandType.Text, null);

                string funcQueryHelper = string.Format("SELECT cohort_helper_planned('{0}', '{1}', '{2}', '{3}');", " ", firstDate.ToString("yyyy-MM-dd"), today.ToString("yyyy-MM-dd"), enddDate.ToString("yyyy-MM-dd"));
                DataTable dtfHelper = DBManager.ExecuteReaderCommand(funcQueryHelper, CommandType.Text, null);

                string funcQuery = string.Format(@"select * from ""HELPER"";");
                DataTable dtf = DBManager.ExecuteReaderCommand(funcQuery, CommandType.Text, null);

                return dtf;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("CohortCoverage", "GetPlannedCoverage", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                return null;
            }
        }

        public static DataTable GetOpenVialReport(int hfId, DateTime firstDate, DateTime endDate, int vaccineId)
        {
            try
            {
                
                string truncateQuery = string.Format(@"TRUNCATE ""OPENVIAL_HELPER"";");
                DBManager.ExecuteNonQueryCommand(truncateQuery, CommandType.Text, null);

                string funcQueryHelper = string.Format("SELECT openvial_helper({0}, '{1}', '{2}', {3});", hfId, firstDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), vaccineId);
                DataTable dtfHelper = DBManager.ExecuteReaderCommand(funcQueryHelper, CommandType.Text, null);

                string funcQuery = string.Format(@"select * from ""OPENVIAL_HELPER"";");
                DataTable dtf = DBManager.ExecuteReaderCommand(funcQuery, CommandType.Text, null);

                return dtf;

           
            }
            catch (Exception ex)
            {
                Log.InsertEntity("CohortCoverage", "GetOpenVialReport", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                return null;
            }
        }

    }
}
