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
using GIIS.DataLayer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Diagnostics;
public partial class Pages_RunReport : System.Web.UI.Page
{

    public string jasperAction = String.Empty;
    public string jasperXLSAction = String.Empty;

    private void SetReportLayout(Decimal id, String format, List<String> actionList)
    {
        try
        {

            StringBuilder myRoles = new StringBuilder();
            actionList.ForEach(a => myRoles.AppendFormat("'{0}',", a));
            myRoles.Remove(myRoles.Length - 1, 1);

            // HACK: This should be placed in the DAL
            string command = "SELECT * FROM \"REPORT\" WHERE \"ID\" = @Id AND \"ACTION_ID\" IN (SELECT \"ID\" FROM \"ACTIONS\" WHERE \"NAME\" IN (" + myRoles.ToString() + "))";
            using (var dt = DBManager.ExecuteReaderCommand(command, System.Data.CommandType.Text, new List<Npgsql.NpgsqlParameter>() {
            new NpgsqlParameter("Id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = id }
        }))
            {
                using (var rdr = dt.CreateDataReader())
                {
                    if (rdr.Read())
                    {
                        this.jasperAction = String.Format("{0}/{1}", ConfigurationManager.AppSettings["JasperServer"], rdr["JASPER_ID"]);
                        this.lblTitle.Text = rdr["REPORT_NAME"].ToString();
                        this.lblReportName.Text = rdr["REPORT_NAME"].ToString();
                        this.lblReportDescription.Text = rdr["DESCRIPTION"].ToString();
                    }
                    else
                        Response.Redirect("/Default.aspx");
                }
            }

            /// Command
            command = "SELECT *, C.\"NAME\" AS ACTION FROM \"REPORT_PARAMETERS\" A INNER JOIN \"REPORT_PARAMETER_INPUT_TYPE\" B ON (A.\"INPUT_TYPE\" = B.\"ID\") INNER JOIN \"ACTIONS\" C ON (A.\"ACTION_ID\" = C.\"ID\") WHERE \"REPORT_ID\" = @Id ORDER BY \"PARM_ID\"";
            using (var dt = DBManager.ExecuteReaderCommand(command, System.Data.CommandType.Text, new List<Npgsql.NpgsqlParameter>() { new NpgsqlParameter("Id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = id }}))
            {
                using (var rdr = dt.CreateDataReader())
                {
                    while (rdr.Read())
                    {
                        //HtmlGenericControl backingInput = null;
                        var inputControl = new HtmlGenericControl(rdr["TAG"].ToString());

                        if (!actionList.Contains(rdr["ACTION"].ToString()))
                        {
                            //inputControl.Attributes.Add("readOnly", "readonly");
                            //inputControl.Attributes.Add("disabled", "disabled");
                            inputControl.Attributes["type"] = "hidden";
                            inputControl.TagName = "input";
                            // Used because readonly has no effect 
                            //backingInput = new HtmlGenericControl("input");
                            //backingInput.Attributes.Add("type", "hidden");
                            //backingInput.Attributes.Add("name", rdr["PARM_ID"].ToString());
                        }
                        else if (rdr["TYPE"] != DBNull.Value)
                            inputControl.Attributes.Add("type", rdr["TYPE"].ToString());

                        inputControl.Attributes.Add("id", rdr["PARM_ID"].ToString());
                        inputControl.Attributes.Add("style", "z-index:8");

                        // Value?
                        if (rdr["COMPLETE_SOURCE"] != DBNull.Value)
                        {
                            List<NpgsqlParameter> contextParms = new List<NpgsqlParameter>() {
                                new NpgsqlParameter("@FacilityId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.HealthFacilityId },
                                new NpgsqlParameter("@FacilityCode", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.HealthFacility.Code },
                                new NpgsqlParameter("@UserId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.Id }
                            },
                            contextParms2 = new List<NpgsqlParameter>() {
                                new NpgsqlParameter("@FacilityId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.HealthFacilityId },
                                new NpgsqlParameter("@FacilityCode", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.HealthFacility.Code },
                                new NpgsqlParameter("@UserId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.Id }
                            };

                            // Hidden value or input is just a value
                            if (rdr["TAG"].ToString() == "input" || inputControl.Attributes["type"] == "hidden")
                            {
                                inputControl.Attributes.Add("value", DBManager.ExecuteScalarCommand(rdr["COMPLETE_SOURCE"].ToString(), System.Data.CommandType.Text, contextParms).ToString());

                                //if (backingInput != null)
                                //    backingInput.Attributes.Add("value", DBManager.ExecuteScalarCommand(rdr["COMPLETE_SOURCE"].ToString(), System.Data.CommandType.Text, contextParms2).ToString());

                            }
                            else
                            {

                                // default value for drop down
                                String defaultValue = null;
                                if (rdr["DEFAULT"] != DBNull.Value)
                                    defaultValue = DBManager.ExecuteScalarCommand(rdr["DEFAULT"].ToString(), System.Data.CommandType.Text, contextParms).ToString();
                                
                                //if (backingInput != null)
                                //    backingInput.Attributes.Add("value", defaultValue);

                                Debug.WriteLine(rdr["COMPLETE_SOURCE"]);
                                // Populate drop down options
                                using (var idt = DBManager.ExecuteReaderCommand(rdr["COMPLETE_SOURCE"].ToString(), System.Data.CommandType.Text, contextParms2))
                                {
                                    using (var irdr = idt.CreateDataReader())
                                    {
                                        while (irdr.Read())
                                        {
                                            var opt = new HtmlGenericControl("option");
                                            inputControl.Controls.Add(opt);
                                            opt.Attributes.Add("value", irdr[0].ToString());
                                            if (defaultValue == irdr[0].ToString())
                                                opt.Attributes.Add("selected", "selected");
                                            opt.InnerText = irdr[1].ToString();
                                        }
                                    }
                                }
                            }
                        }

                        inputControl.Attributes.Add("name", rdr["PARM_ID"].ToString());
                        inputControl.Attributes.Add("title", rdr["DESCRIPTION"].ToString());
                        // Hidden
                        if (inputControl.Attributes["type"]  == "hidden")
                        {
                            this.reportInputs.Controls.Add(inputControl);
                            continue;
                        }
                        inputControl.Attributes.Add("class", "form-control");

                        // Label control
                        var labelControl = new Label()
                        {
                            Text = rdr["PARM_NAME"].ToString()
                        };


                        var row = new HtmlGenericControl("div");
                        row.Attributes.Add("class", "row");
                        row.Attributes.Add("style", "margin:5px");
                        var colMd4 = new HtmlGenericControl("div");
                        colMd4.Attributes.Add("class", "col-md-2");
                        var colMd8 = new HtmlGenericControl("div");
                        colMd8.Attributes.Add("class", "col-md-6");
                        row.Controls.Add(colMd4);
                        row.Controls.Add(colMd8);
                        colMd4.Controls.Add(labelControl);
                        colMd8.Controls.Add(inputControl);
                        //colMd8.Controls.Add(backingInput);
                        this.reportInputs.Controls.Add(row);


                        if (rdr["DATATYPE"].ToString() == "System.DateTime")
                            Page.RegisterStartupScript(rdr["PARM_ID"].ToString(),
                                string.Format(
                                "<script type=\"text/javascript\">Sys.Application.add_init(function() {{$create(Sys.Extended.UI.CalendarBehavior, {{\"format\":\"MM-dd-yyyy\",\"id\":\"ce_{0}\"}}, null, null, $get(\"{0}\"));}});</script>", rdr["PARM_ID"]));
                      //  $create(Sys.Extended.UI.CalendarBehavior, {"endDate":"Thu, 28 May 2015 00:00:00 GMT","format":"dd/MM/yyyy","id":"ctl00_ContentPlaceHolder1_ceBirthdateTo"}, null, null, $get("ctl00_ContentPlaceHolder1_txtBirthdateTo"));


                    }
                }
            }
        }
        catch(Exception e) {
            Debug.WriteLine(e.ToString());
        }
    
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        List<string> actionList = null;
        string sessionNameAction = "";
        if (CurrentEnvironment.LoggedUser != null)
        {
            sessionNameAction = "__GIS_actionList_" + CurrentEnvironment.LoggedUser.Id;
            actionList = (List<string>)Session[sessionNameAction];
        }

        if (CurrentEnvironment.LoggedUser != null)
        {
            int userId = CurrentEnvironment.LoggedUser.Id;
            string language = CurrentEnvironment.Language;
            int languageId = int.Parse(language);
            Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Reports-dictionary" + language];
            if (wtList == null)
            {
                List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Reports");
                wtList = new Dictionary<string, string>();
                foreach (WordTranslate vwt in wordTranslateList)
                    wtList.Add(vwt.Code, vwt.Name);
                HttpContext.Current.Cache.Insert("Reports-dictionary" + language, wtList);
            }

            this.SetReportLayout(Decimal.Parse(Request.QueryString["reportId"]), Request.QueryString["format"], actionList);
        }
        else
        {
            Response.Redirect("Default.aspx");
        }
    }
}