using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class Pages_ChildListNewSearch : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewChildList") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Child-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Child");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Child-dictionary" + language, wtList);
                }

                #region Person Configuration

                List<PersonConfiguration> pcList = PersonConfiguration.GetPersonConfigurationList();

                foreach (PersonConfiguration pc in pcList)
                {
                    if (pc.IsVisible == false)
                    {
                        Control lbl = FindMyControl(this, "lbl" + pc.ColumnName);
                        Control txt = FindMyControl(this, "txt" + pc.ColumnName);
                        //Control rbl = FindMyControl(this, "rbl" + pc.ColumnName);
                        Control tr = FindMyControl(this, "tr" + pc.ColumnName);

                        if (lbl != null)
                            lbl.Visible = false;

                        if (txt != null)
                            txt.Visible = false;

                        //if (rbl != null)
                        //    rbl.Visible = false;

                        if (tr != null)
                            tr.Visible = false;

                        for (int i = 1; i < gvChild.Columns.Count; i++)
                        {
                            if (gvChild.Columns[i].HeaderText == pc.ColumnName)
                            {
                                gvChild.Columns[i].Visible = false;
                                break;
                            }
                        }
                    }
                }
                string id1 = Configuration.GetConfigurationByName("IdentificationNo1").Value;
                string id2 = Configuration.GetConfigurationByName("IdentificationNo2").Value;
                string id3 = Configuration.GetConfigurationByName("IdentificationNo3").Value;
                #endregion

                //grid header text
                #region Grid Columns
                gvChild.Columns[1].HeaderText = wtList["ChildSystem"];
                gvChild.Columns[2].HeaderText = wtList["ChildFirstname1"];
                gvChild.Columns[3].HeaderText = wtList["ChildFirstname2"];
                gvChild.Columns[4].HeaderText = wtList["ChildLastname1"];
                gvChild.Columns[5].HeaderText = wtList["ChildLastname2"];
                gvChild.Columns[6].HeaderText = wtList["ChildBirthdate"];
                gvChild.Columns[7].HeaderText = wtList["ChildGender"];
                gvChild.Columns[8].HeaderText = wtList["ChildHealthcenter"];
                gvChild.Columns[9].HeaderText = wtList["ChildBirthplace"];
                gvChild.Columns[10].HeaderText = wtList["ChildCommunity"];
                gvChild.Columns[11].HeaderText = wtList["ChildDomicile"];
                gvChild.Columns[12].HeaderText = wtList["ChildStatus"];
                gvChild.Columns[13].HeaderText = wtList["ChildAddress"];
                gvChild.Columns[14].HeaderText = wtList["ChildPhone"];
                gvChild.Columns[15].HeaderText = wtList["ChildMobile"];
                gvChild.Columns[16].HeaderText = wtList["ChildEmail"];
                gvChild.Columns[17].HeaderText = wtList["ChildMother"];
                gvChild.Columns[18].HeaderText = wtList["ChildMotherFirstname"];
                gvChild.Columns[19].HeaderText = wtList["ChildMotherLastname"];
                gvChild.Columns[20].HeaderText = wtList["ChildFather"];
                gvChild.Columns[21].HeaderText = wtList["ChildFatherFirstname"];
                gvChild.Columns[22].HeaderText = wtList["ChildFatherLastname"];
                gvChild.Columns[23].HeaderText = wtList["ChildCaretaker"];
                gvChild.Columns[24].HeaderText = wtList["ChildCaretakerFirstname"];
                gvChild.Columns[25].HeaderText = wtList["ChildCaretakerLastname"];
                gvChild.Columns[26].HeaderText = wtList["ChildNotes"];
                gvChild.Columns[27].HeaderText = wtList["ChildIsActive"];
                gvChild.Columns[30].HeaderText = id1;
                gvChild.Columns[31].HeaderText = id2;
                gvChild.Columns[32].HeaderText = id3;
                gvChild.Columns[26].Visible = false;

                gvExel.Columns[1].HeaderText = wtList["ChildSystem"];
                gvExel.Columns[2].HeaderText = wtList["ChildFirstname1"];
                gvExel.Columns[3].HeaderText = wtList["ChildFirstname2"];
                gvExel.Columns[4].HeaderText = wtList["ChildLastname1"];
                gvExel.Columns[5].HeaderText = wtList["ChildLastname2"];
                gvExel.Columns[6].HeaderText = wtList["ChildBirthdate"];
                gvExel.Columns[7].HeaderText = wtList["ChildGender"];
                gvExel.Columns[8].HeaderText = wtList["ChildHealthcenter"];
                gvExel.Columns[9].HeaderText = wtList["ChildBirthplace"];
                gvExel.Columns[10].HeaderText = wtList["ChildCommunity"];
                gvExel.Columns[11].HeaderText = wtList["ChildDomicile"];
                gvExel.Columns[12].HeaderText = wtList["ChildStatus"];
                gvExel.Columns[13].HeaderText = wtList["ChildAddress"];
                gvExel.Columns[14].HeaderText = wtList["ChildPhone"];
                gvExel.Columns[15].HeaderText = wtList["ChildMobile"];
                gvExel.Columns[16].HeaderText = wtList["ChildEmail"];
                gvExel.Columns[17].HeaderText = wtList["ChildMother"];
                gvExel.Columns[18].HeaderText = wtList["ChildMotherFirstname"];
                gvExel.Columns[19].HeaderText = wtList["ChildMotherLastname"];
                gvExel.Columns[20].HeaderText = wtList["ChildFather"];
                gvExel.Columns[21].HeaderText = wtList["ChildFatherFirstname"];
                gvExel.Columns[22].HeaderText = wtList["ChildFatherLastname"];
                gvExel.Columns[23].HeaderText = wtList["ChildCaretaker"];
                gvExel.Columns[24].HeaderText = wtList["ChildCaretakerFirstname"];
                gvExel.Columns[25].HeaderText = wtList["ChildCaretakerLastname"];
                gvExel.Columns[26].HeaderText = wtList["ChildNotes"];
                gvExel.Columns[27].HeaderText = wtList["ChildIsActive"];
                gvExel.Columns[30].HeaderText = id1;
                gvExel.Columns[31].HeaderText = id2;
                gvExel.Columns[32].HeaderText = id3;
                gvExel.Columns[26].Visible = false;
                #endregion

                //message
                this.lblWarning.Text = wtList["ChildSearchWarningText"];

                //buttons
                //this.btnSearch.Text = wtList["ChildSearchAgainButton"];//Add this entry in DB
                this.btnExcel.Text = wtList["ChildExcelButton"];

                //Page Title
                this.lblTitle.Text = wtList["ChildListNewPageTitle"];

                odsChild.SelectParameters.Clear();

                if (HttpContext.Current.Session["_statusChildListNew"] != null)
                    odsChild.SelectParameters.Add("statusId", HttpContext.Current.Session["_statusChildListNew"].ToString());
                if (HttpContext.Current.Session["_birthdateFromChildListNew"] != null)
                    odsChild.SelectParameters.Add("birthdateFrom", HttpContext.Current.Session["_birthdateFromChildListNew"].ToString());
                if (HttpContext.Current.Session["_birthdateToChildListNew"] != null)
                    odsChild.SelectParameters.Add("birthdateTo", HttpContext.Current.Session["_birthdateToChildListNew"].ToString());
                if (HttpContext.Current.Session["_firstname1ChildListNew"] != null)
                    odsChild.SelectParameters.Add("firstname1", HttpContext.Current.Session["_firstname1ChildListNew"].ToString());
                if (HttpContext.Current.Session["_lastname1ChildListNew"] != null)
                    odsChild.SelectParameters.Add("lastname1", HttpContext.Current.Session["_lastname1ChildListNew"].ToString());
                if (HttpContext.Current.Session["_motherfirstname1ChildListNew"] != null)
                    odsChild.SelectParameters.Add("motherFirstname", HttpContext.Current.Session["_motherfirstname1ChildListNew"].ToString());
                if (HttpContext.Current.Session["_motherlastname1ChildListNew"] != null)
                    odsChild.SelectParameters.Add("motherLastname", HttpContext.Current.Session["_motherlastname1ChildListNew"].ToString());
                if (HttpContext.Current.Session["_idFieldsChildListNew"] != null)
                    odsChild.SelectParameters.Add("idFields", HttpContext.Current.Session["_idFieldsChildListNew"].ToString());
                if (HttpContext.Current.Session["_systemIdChildListNew"] != null)
                    odsChild.SelectParameters.Add("systemId", HttpContext.Current.Session["_systemIdChildListNew"].ToString());
                if (HttpContext.Current.Session["_barcodeIdChildListNew"] != null)
                    odsChild.SelectParameters.Add("barcodeId", HttpContext.Current.Session["_barcodeIdChildListNew"].ToString());
                if (HttpContext.Current.Session["_tempIdChildListNew"] != null)
                    odsChild.SelectParameters.Add("tempId", HttpContext.Current.Session["_tempIdChildListNew"].ToString());
                if (HttpContext.Current.Session["_healthCenterIdChildListNew"] != null)
                    odsChild.SelectParameters.Add("healthFacilityId", HttpContext.Current.Session["_healthCenterIdChildListNew"].ToString());
                if (HttpContext.Current.Session["_birthplace"] != null)
                    odsChild.SelectParameters.Add("birthplaceId", HttpContext.Current.Session["_birthplace"].ToString());
                else odsChild.SelectParameters.Add("birthplaceId", "");
                if (HttpContext.Current.Session["_community"] != null)
                    odsChild.SelectParameters.Add("communityId", HttpContext.Current.Session["_community"].ToString());
                else odsChild.SelectParameters.Add("communityId", "");
                if (HttpContext.Current.Session["_domicile"] != null)
                    odsChild.SelectParameters.Add("domicileId", HttpContext.Current.Session["_domicile"].ToString());
                else odsChild.SelectParameters.Add("domicileId", "");

                //if (odsChild.SelectParameters.Count > 3)  // why
                //{
                gvChild.DataSourceID = "odsChild";
                gvChild.DataBind();
                //}
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        
    }
    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ClearSession();
        odsChild.SelectParameters.Clear(); //maybe is not needed
        Response.Redirect("ChildListNew.aspx");
        Context.ApplicationInstance.CompleteRequest();
    }
    protected void ClearSession()
    {
        Session["_systemIdChildListNew"] = null;
        Session["_idFieldsChildListNew"] = null;
        Session["_barcodeIdChildListNew"] = null;
        Session["_tempIdChildListNew"] = null;
        Session["_firstname1ChildListNew"] = null;
        Session["_lastname1ChildListNew"] = null;
        Session["_motherfirstname1ChildListNew"] = null;
        Session["_motherlastname1ChildListNew"] = null;
        Session["_birthdateFromChildListNew"] = null;
        Session["_birthdateToChildListNew"] = null;
        Session["_healthCenterIdChildListNew"] = null;
        Session["_birthplace"] = null;
        Session["_domicile"] = null;
        Session["_community"] = null;
        Session["_statusChildListNew"] = null;
        Session["_hfChildSearch"] = null;
    }
    
    protected void gvChild_DataBound(object sender, System.EventArgs e)
    {
        if (gvChild.Rows.Count == 0)
        {
            lblWarning.Visible = true;
            btnExcel.Visible = false;
        }
        else
        {
            foreach (GridViewRow gvr in gvChild.Rows)
            {
                gvr.Cells[0].Visible = false;
            }
            btnExcel.Visible = true;
            lblWarning.Visible = false;
        }
    }
    protected void gvChild_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvChild.PageIndex = e.NewPageIndex;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        int maximumRows = int.MaxValue;
        int startRowIndex = 0;

        string IdFsearch = "";
        string SystemId = "";
        string BarcodeId = "";
        string TempId = "";
        string FtNsearch = "";
        string LtNsearch = "";
        string MFtNsearch = "";
        string MLtNsearch = "";

        DateTime dateto = new DateTime();
        DateTime datefrom = new DateTime();

        string format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString();

        string healthFacilityId = "";

        int domicileId = 0;
        int communityId = 0;
        int birthplaceId = 0;
        int statusId = 0;

        if (HttpContext.Current.Session["_idFieldsChildListNew"] != null)
            IdFsearch = HttpContext.Current.Session["_idFieldsChildListNew"].ToString();

        if (HttpContext.Current.Session["_firstname1ChildListNew"] != null)
            FtNsearch = HttpContext.Current.Session["_firstname1ChildListNew"].ToString();

        if (HttpContext.Current.Session["_lastname1ChildListNew"] != null)
            LtNsearch = HttpContext.Current.Session["_lastname1ChildListNew"].ToString();

        if (HttpContext.Current.Session["_motherfirstname1ChildListNew"] != null)
            MFtNsearch = HttpContext.Current.Session["_motherfirstname1ChildListNew"].ToString();

        if (HttpContext.Current.Session["_motherlastname1ChildListNew"] != null)
            MLtNsearch = HttpContext.Current.Session["_motherlastname1ChildListNew"].ToString();
        //todo: format date is to be reviwed
        if (HttpContext.Current.Session["_birthdateFromChildListNew"] != null)
            datefrom = DateTime.ParseExact(HttpContext.Current.Session["_birthdateFromChildListNew"].ToString(), "yyyy-MM-dd", CultureInfo.CurrentCulture);

        if (HttpContext.Current.Session["_birthdateToChildListNew"] != null)
            dateto = DateTime.ParseExact(HttpContext.Current.Session["_birthdateToChildListNew"].ToString(), "yyyy-MM-dd", CultureInfo.CurrentCulture);

        if (HttpContext.Current.Session["_statusChildListNew"] != null)
            statusId = int.Parse(HttpContext.Current.Session["_statusChildListNew"].ToString());

        if (HttpContext.Current.Session["_healthCenterIdChildListNew"] != null)
            healthFacilityId = HttpContext.Current.Session["_healthCenterIdChildListNew"].ToString();

        if (HttpContext.Current.Session["_birthplace"] != null)
            birthplaceId = int.Parse(HttpContext.Current.Session["_birthplace"].ToString());

        if (HttpContext.Current.Session["_community"] != null)
            communityId = int.Parse(HttpContext.Current.Session["_community"].ToString());

        if (HttpContext.Current.Session["_domicile"] != null)
            domicileId = int.Parse(HttpContext.Current.Session["_domicile"].ToString());

        if (HttpContext.Current.Session["_systemIdChildListNew"] != null)
            SystemId = HttpContext.Current.Session["_systemIdChildListNew"].ToString();

        if (HttpContext.Current.Session["_barcodeIdChildListNew"] != null)
            BarcodeId = HttpContext.Current.Session["_barcodeIdChildListNew"].ToString();

        if (HttpContext.Current.Session["_tempIdChildListNew"] != null)
            TempId = HttpContext.Current.Session["_tempIdChildListNew"].ToString();

        List<Child> list = Child.GetPagedChildList(statusId, datefrom, dateto, FtNsearch, LtNsearch, IdFsearch, healthFacilityId, birthplaceId, communityId, domicileId, MFtNsearch, MLtNsearch, SystemId, BarcodeId, TempId, ref maximumRows, ref startRowIndex);
        gvExel.DataSource = list;
        gvExel.DataBind();
        gvExel.Visible = true;


        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=ChildListNew.xls");
        Response.Charset = "";

        // If you want the option to open the Excel file without saving then
        // comment out the line below
        // Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/ms-excel";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gvExel.RenderControl(htmlWrite);
        Response.Write(stringWrite.ToString());
        Response.End();

        gvExel.Visible = false;
    }
    
    private static Control FindMyControl(Control Root, string Id)
    {
        if (Id == "txtIsActive")
            return null;

        if (Root.ID == Id)
            return Root;

        foreach (Control Ctl in Root.Controls)
        {
            Control FoundCtl = FindMyControl(Ctl, Id);
            if (FoundCtl != null)
                return FoundCtl;
        }

        return null;
    }
    protected override void Render(HtmlTextWriter writer)
    {
        // Ensure that the control is nested in a server form.
        if ((Page != null))
        {
            Page.VerifyRenderingInServerForm(this);
        }

        base.Render(writer);
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for the specified ASP.NET
        //     server control at run time. 
    }
   
}