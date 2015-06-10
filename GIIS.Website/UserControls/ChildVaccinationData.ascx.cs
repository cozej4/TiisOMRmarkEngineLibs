using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using GIIS.DataLayer;
public partial class UserControls_ChildVaccinationData : System.Web.UI.UserControl
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
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["ChildVaccinationData-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ChildVaccinationData");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("ChildVaccinationData-dictionary" + language, wtList);
                }


                //grid header text
                gvVaccinationEvent.Columns[1].HeaderText = wtList["ChildVaccinationDataDose"];
                gvVaccinationEvent.Columns[2].HeaderText = wtList["ChildVaccinationDataLot"];
                gvVaccinationEvent.Columns[3].HeaderText = wtList["ChildVaccinationDataHealthCenter"];
                gvVaccinationEvent.Columns[4].HeaderText = wtList["ChildVaccinationDataDate"];
                gvVaccinationEvent.Columns[5].HeaderText = wtList["ChildVaccinationDataDone"];
                gvVaccinationEvent.Columns[6].HeaderText = wtList["ChildVaccinationDataReason"];


            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    protected void gvVaccinationEvent_DataBound(object sender, EventArgs e)
    {
        if (gvVaccinationEvent.Rows.Count > 0)
        {
            //string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(GIIS.DataLayer.Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
            //((BoundField)gvVaccinationEvent.Columns[4]).DataFormatString = "{0:" + dateformat + "}";

            foreach (GridViewRow gvr in gvVaccinationEvent.Rows)
            {
                CheckBox chkStatus = (CheckBox)gvr.Cells[5].FindControl("chkStatus");
                HyperLink hl = (HyperLink)gvr.Cells[1].FindControl("hlDose");
                if (!chkStatus.Checked)
                {
                    gvr.Cells[2].Text = String.Empty;
                    gvr.Cells[4].Text = String.Empty;
                    if (gvr.Cells[7].Text == "0")
                    {
                        gvr.Cells[3].Text = String.Empty;
                        hl.Enabled = false;
                        hl.ForeColor = System.Drawing.Color.Gray;
                    }
                }
                gvr.Cells[7].Visible = false;
            }
            gvVaccinationEvent.HeaderRow.Cells[7].Visible = false;
        }
    }
}