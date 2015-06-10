using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.DataLayer;

public partial class Pages_HealthFacilityListForBalance : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            List<string> actionList = null;
            string sessionNameAction = "";
            if (CurrentEnvironment.LoggedUser != null)
            {
                sessionNameAction = "__GIS_actionList_" + CurrentEnvironment.LoggedUser.Id;
                actionList = (List<string>)Session[sessionNameAction];
            }

            if ((actionList != null) && actionList.Contains("ViewHealthFacilityList") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["HealthFacility-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "HealthFacility");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("HealthFacility-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["HealthFacilityName"];
                this.lblCode.Text = wtList["HealthFacilityCode"];

                //grid header text
                gvHealthFacility.Columns[1].HeaderText = wtList["HealthFacilityName"];
                gvHealthFacility.Columns[2].HeaderText = wtList["HealthFacilityCode"];
                gvHealthFacility.Columns[3].HeaderText = wtList["HealthFacilityParent"];
                gvHealthFacility.Columns[4].HeaderText = wtList["HealthFacilityTopLevel"];
                gvHealthFacility.Columns[5].HeaderText = wtList["HealthFacilityLeaf"];
                gvHealthFacility.Columns[6].HeaderText = wtList["HealthFacilityVaccinationPoint"];
                //gvHealthFacility.Columns[7].HeaderText = wtList["HealthFacilityListViewBalance"];


                //actions
                // this.btnSearch.Visible = actionList.Contains("SearchHealthFacility");

                //buttons
                this.btnSearch.Text = wtList["HealthFacilitySearchButton"];

                //message
                this.lblWarning.Text = wtList["HealthFacilitySearchWarningText"];

                //Page Title
                // this.lblTitle.Text = wtList["HealthFacilityListPageTitle"];

                //gridview databind
                odsHealthFacility.SelectParameters.Clear();


                if (HttpContext.Current.Session["HealthFacilityList-Name"] != null)
                    odsHealthFacility.SelectParameters.Add("name", HttpContext.Current.Session["HealthFacilityList-Name"].ToString().ToUpper());
                else odsHealthFacility.SelectParameters.Add("name", "");

                if (HttpContext.Current.Session["HealthFacilityList-Code"] != null)
                    odsHealthFacility.SelectParameters.Add("code", HttpContext.Current.Session["HealthFacilityList-Code"].ToString().ToUpper());
                else odsHealthFacility.SelectParameters.Add("code", "");

                Session["_healthfacility"] = CurrentEnvironment.LoggedUser.HealthFacilityId;
                string s = HealthFacility.GetAllChildsForOneHealthFacility(CurrentEnvironment.LoggedUser.HealthFacilityId);

                //if (HttpContext.Current.Session["HealthFacilityList-HFID"] != null)
                //    odsHealthFacility.SelectParameters.Add("hfid", HttpContext.Current.Session["HealthFacilityList-HFID"].ToString().ToUpper());
                //else odsHealthFacility.SelectParameters.Add("hfid", s);

                odsHealthFacility.SelectParameters.Add("hfid", s);

                gvHealthFacility.DataSourceID = "odsHealthFacility";
                gvHealthFacility.DataBind();
            }
            else
            {

                Response.Redirect("Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string wsearch = txtName.Text.Replace("'", @"''");
        string csearch = txtCode.Text.Replace("'", @"''");

        string s = string.Empty;
        string hf = string.Empty;
        if (Session["_healthfacility"] != null)
            hf = Session["_healthfacility"].ToString();

        if (!string.IsNullOrEmpty(hf))
        {
            s = HealthFacility.GetAllChildsForOneHealthFacility(int.Parse(hf));
        }

        odsHealthFacility.SelectParameters.Clear();
        odsHealthFacility.SelectParameters.Add("name", wsearch.ToUpper());
        odsHealthFacility.SelectParameters.Add("code", csearch.ToUpper());
        odsHealthFacility.SelectParameters.Add("hfid", s);

        Session["HealthFacilityList-Name"] = wsearch;
        Session["HealthFacilityList-Code"] = csearch;
        Session["HealthFacilityList-HFID"] = s;
    }
    protected void gvHealthFacility_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHealthFacility.PageIndex = e.NewPageIndex;
    }

    protected void gvHealthFacility_DataBound(object sender, EventArgs e)
    {
        if (gvHealthFacility.Rows.Count == 0)
            lblWarning.Visible = true;
        else
            lblWarning.Visible = false;
    }
}