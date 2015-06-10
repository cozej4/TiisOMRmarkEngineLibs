using GIIS.DataLayer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Pages_RunReport : System.Web.UI.Page
{

    public string jasperAction = String.Empty;
    public string jasperXLSAction = String.Empty;

    private void SetReportLayout(Decimal id, String format)
    {
        try
        {


            string command = "SELECT * FROM \"REPORT\" WHERE \"ID\" = @Id";
            using (var dt = DBManager.ExecuteReaderCommand(command, System.Data.CommandType.Text, new List<Npgsql.NpgsqlParameter>() {
            new NpgsqlParameter("Id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = id }
        }))
            {
                using (var rdr = dt.CreateDataReader())
                {
                    if (rdr.Read())
                    {
                        this.jasperAction = String.Format("{0}/{1}.{2}", ConfigurationManager.AppSettings["JasperServer"], rdr["JASPER_ID"], Request.QueryString["format"]);
                        this.lblTitle.Text = rdr["REPORT_NAME"].ToString();
                        this.lblReportName.Text = rdr["REPORT_NAME"].ToString();
                        this.lblReportDescription.Text = rdr["DESCRIPTION"].ToString();
                    }
                    else
                        Response.Redirect("/Default.aspx");
                }
            }

            /// Command
            command = "SELECT * FROM \"REPORT_PARAMETERS\" A INNER JOIN \"REPORT_PARAMETER_INPUT_TYPE\" B ON (A.\"INPUT_TYPE\" = B.\"ID\") WHERE \"REPORT_ID\" = @Id ORDER BY \"PARM_ID\"";
            using (var dt = DBManager.ExecuteReaderCommand(command, System.Data.CommandType.Text, new List<Npgsql.NpgsqlParameter>() {
            new NpgsqlParameter("Id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = id }
        }))
            {
                using (var rdr = dt.CreateDataReader())
                {
                    while (rdr.Read())
                    {
                        var inputControl = new HtmlGenericControl(rdr["TAG"].ToString());
                        if (rdr["TYPE"] != DBNull.Value)
                            inputControl.Attributes.Add("type", rdr["TYPE"].ToString());
                        inputControl.Attributes.Add("id", rdr["PARM_ID"].ToString());
                        inputControl.Attributes.Add("style", "z-index:8");

                        // Value?
                        if (rdr["COMPLETE_SOURCE"] != DBNull.Value)
                        {
                            var contextParms = new List<NpgsqlParameter>() {
                                new NpgsqlParameter("@FacilityId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.HealthFacilityId },
                                new NpgsqlParameter("@FacilityCode", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.HealthFacility.Code },
                                new NpgsqlParameter("@UserId", NpgsqlTypes.NpgsqlDbType.Integer) { Value = CurrentEnvironment.LoggedUser.Id }
                            };
                            if (rdr["TAG"].ToString() == "input")
                            {
                                inputControl.Attributes.Add("value", DBManager.ExecuteScalarCommand(rdr["COMPLETE_SOURCE"].ToString(), System.Data.CommandType.Text, contextParms).ToString());
                            }
                            else
                            {
                                using (var idt = DBManager.ExecuteReaderCommand(rdr["COMPLETE_SOURCE"].ToString(), System.Data.CommandType.Text, contextParms))
                                {
                                    using (var irdr = idt.CreateDataReader())
                                    {
                                        while (irdr.Read())
                                        {
                                            var opt = new HtmlGenericControl("option");
                                            inputControl.Controls.Add(opt);
                                            opt.Attributes.Add("value", irdr[0].ToString());
                                            opt.InnerText = irdr[1].ToString();
                                        }
                                    }
                                }
                            }
                        }

                        inputControl.Attributes.Add("name", rdr["PARM_ID"].ToString());
                        inputControl.Attributes.Add("title", rdr["DESCRIPTION"].ToString());
                        // Hidden
                        if (rdr["TYPE"].ToString() == "hidden")
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
        catch {}
    
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

            this.SetReportLayout(Decimal.Parse(Request.QueryString["reportId"]), Request.QueryString["format"]);
        }
        else
        {
            Response.Redirect("Default.aspx");
        }
    }
}