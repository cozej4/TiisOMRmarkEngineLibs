using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class Pages_FindDuplications : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewFindDuplications") && (CurrentEnvironment.LoggedUser != null))
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

                //controls
                #region Controls

                this.chbFirstname.Text = wtList["ChildFirstname1"];
                this.chbLastname.Text = wtList["ChildLastname1"];
                this.chbBirthdate.Text = wtList["ChildBirthdate"];
                this.chbGender.Text = wtList["ChildGender"];


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
                gvChild.Columns[10].HeaderText = wtList["ChildStatus"];
                gvChild.Columns[11].HeaderText = wtList["ChildMotherFirstname"];
                gvChild.Columns[12].HeaderText = wtList["ChildMotherLastname"];
                gvChild.Columns[13].HeaderText = wtList["ChildFatherFirstname"];
                gvChild.Columns[14].HeaderText = wtList["ChildFatherLastname"];
                gvChild.Columns[15].HeaderText = id1;
                gvChild.Columns[16].HeaderText = id2;
                gvChild.Columns[17].HeaderText = id3;
                #endregion

                //buttons
                this.btnSearch.Text = wtList["ChildSearchButton"];
                this.btnExcel.Text = wtList["ChildExcelButton"];

                //Page Title
                this.lblTitle.Text = wtList["ChildFindDuplications"];

                //message
                this.lblWarningText.Text = wtList["ChildSearchWarningText"];

            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (chbBirthdate.Checked || chbFirstname.Checked || chbGender.Checked)
            {
                HealthFacility hf = HealthFacility.GetHealthFacilityById(CurrentEnvironment.LoggedUser.HealthFacilityId);
                
                odsChild.SelectParameters.Clear();
                odsChild.SelectParameters.Add("birthdateFlag", chbBirthdate.Checked.ToString());
                odsChild.SelectParameters.Add("firstnameFlag", chbFirstname.Checked.ToString());
                odsChild.SelectParameters.Add("genderFlag", chbGender.Checked.ToString());
                odsChild.SelectParameters.Add("healthFacilityId", hf.Id.ToString());

                gvChild.DataSource = odsChild;
                gvChild.DataBind();
                lblWarning.Visible = false;
            }
            else
                lblWarning.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void gvChild_DataBinding(object sender, System.EventArgs e)
    {
        string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
        ((BoundField)gvChild.Columns[6]).DataFormatString = "{0:" + dateformat + "}";

    }

    protected void gvChild_DataBound(object sender, EventArgs e)
    {
        if (gvChild.Rows.Count <= 0)
            lblWarningText.Visible = true;
        else
        {
            btnExcel.Visible = false;
            lblWarningText.Visible = false;
            lblWarning.Visible = false;
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=FindDuplications.xls");
        Response.Charset = "";

        // If you want the option to open the Excel file without saving then
        // comment out the line below
        // Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/ms-excel";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gvChild.RenderControl(htmlWrite);
        Response.Write(stringWrite.ToString());
        Response.End();
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