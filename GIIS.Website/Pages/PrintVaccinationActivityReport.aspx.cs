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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.DataLayer;
using System.IO;
using System.Text;
using System.Globalization;
using System.Data;
using System.Linq;

public partial class Pages_PrintVaccinationActivityReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            if (CurrentEnvironment.LoggedUser != null)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);

                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["VaccinationActivityReport-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "VaccinationActivityReport");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("VaccinationActivityReport-dictionary" + language, wtList);
                }

                this.lblTitle.Text = wtList["PrintVaccinationActivityReportPageTitle"];
                this.lbItem.Text = wtList["VaccinationActivityReportItem"];
                this.lbYear.Text = wtList["VaccinationActivityReportYear"];

                string jan = wtList["Jan"];
                string feb = wtList["Feb"];
                string mar = wtList["Mar"];
                string apr = wtList["Apr"];
                string may = wtList["May"];
                string jun = wtList["Jun"];
                string jul = wtList["Jul"];
                string aug = wtList["Aug"];
                string sep = wtList["Sep"];
                string oct = wtList["Oct"];
                string nov = wtList["Nov"];
                string dec = wtList["Dec"];

                string vaccine = wtList["Vaccine"];
                string total = wtList["Total"];
                string planned = wtList["Planned"];

                string months = string.Format(@" ""{12}"" text, ""{0}"" int, ""{1}"" int, ""{2}"" int, ""{3}"" int, ""{4}"" int, ""{5}"" int, ""{6}"" int, ""{7}"" int, ""{8}"" int, ""{9}"" int, ""{10}"" int, ""{11}"" int ", jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec, vaccine);

                Session["__Months"] = months;
                Session["__Total"] = total;
                Session["__Planned"] = planned;
                string where="";
                string tmp="";
                if (HttpContext.Current.Session["Year"].ToString() != String.Empty)
                    lblYear.Text = HttpContext.Current.Session["Year"].ToString();
                else
                    lblYear.Text = DateTime.Today.Date.Year.ToString();

                if (HttpContext.Current.Session["VaccinationActivityReport-Item"] != String.Empty)
                    lblItem.Text = HttpContext.Current.Session["VaccinationActivityReport-Item"].ToString();

                if (HttpContext.Current.Session["VaccinationActivityReport-Where"].ToString() != String.Empty)
                    where = HttpContext.Current.Session["VaccinationActivityReport-Where"].ToString();
                 if (HttpContext.Current.Session["VaccinationActivityReport-TempWhere"].ToString() != String.Empty)
                    tmp = HttpContext.Current.Session["VaccinationActivityReport-TempWhere"].ToString();

                DataTable dt = VaccinationActivity.GetVaccinationActivity(languageId, where, Session["__Months"].ToString());
                DataTable source = GetDataTable(dt, tmp, languageId);

                gvReport.DataSource = source;
                gvReport.DataBind();

                string url = VaccinationActivityReportGraphHelper.GetUrl(source, this.lblTitle.Text);

                iActivityReport.Text = String.Format("<img src='{0}' alt='' />", url.Replace(" ", ""));

            }
            else
                Response.Redirect("Default.aspx", false);
        }
    }


    public override void VerifyRenderingInServerForm(Control control)
    {
        //Verifies that the control is rendered
    }

    protected void gvReport_DataBound(object sender, EventArgs e)
    {
        if (gvReport.Rows.Count > 0)
            foreach (GridViewRow gvr in gvReport.Rows)
                gvr.Cells[0].Visible = false;
        gvReport.HeaderRow.Cells[0].Visible = false;

    }
    public DataTable GetDataTable(DataTable dt, string tmp, int languageId)
    {
        string _total = Session["__Total"].ToString();
        string _planned = Session["__Planned"].ToString();

        DataColumn total = new DataColumn(_total, System.Type.GetType("System.Int32"));
        DataColumn planned = new DataColumn(_planned, System.Type.GetType("System.Int32"));

        dt.Columns.Add(total);
        dt.Columns.Add(planned);

        foreach (DataRow dr in dt.Rows)
        {
            string where = tmp;

            int s = 0;
            for (int index = 2; index <= 13; index++)
            {
                if (dr[index] != DBNull.Value)
                {
                    int v = int.Parse(dr[index].ToString());
                    s += v;
                }
            }
            dr[14] = s;


            where += string.Format(@" AND ""DOSE_ID"" = {0}", dr[0]);
            where += string.Format(@" AND ""VACCINATION_APPOINTMENT"".""SCHEDULED_DATE"" between '{0}-01-01' and now()", lblYear.Text);

            dr[15] = VaccinationActivity.GetPlanedVaccinationActivity(languageId, where);
        }

        System.Data.DataRow drow = dt.Rows[dt.Rows.Count - 1];
        for (int index = 2; index <= 15; index++)
        {
            int sum = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[index] != DBNull.Value)
                {
                    sum += int.Parse(dr[index].ToString());
                }
            }
            drow[index] = sum;
        }

        dt.AcceptChanges();

        return dt;
    }

}