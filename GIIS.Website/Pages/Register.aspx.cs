using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Register : System.Web.UI.Page
{
    static String domicileId;
    protected void Domicile_ValueSelected(object sender, System.EventArgs e)
    {
        domicileId = txtDomicileId.SelectedItemID.ToString();
    }
    protected void Page_Load(object sender, System.EventArgs e)
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

            if ((actionList != null) && actionList.Contains("ViewRegister") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Register-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Register");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Register-dictionary" + language, wtList);
                }

                //Page Title
                this.lblTitle.Text = wtList["RegisterPageTitle"];
                this.btnExcel.Text = wtList["RegisterExcelButton"];

                this.lblYear.Text = wtList["RegisterYear"];
                this.lblFirstName.Text = wtList["Firstname"];
                this.lblLastname.Text = wtList["Lastname"];
                this.lblDomicileId.Text = wtList["RegisterDomicile"];


                int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
                string _hfId = (string)Request.QueryString["hfId"];
                if (!String.IsNullOrEmpty(_hfId))
                     int.TryParse(_hfId, out hfId);

                //string s = HealthFacility.GetAllChildsForOneHealthFacility(hfId);
                //string where = string.Format(" AND (\"ADMINISTRATION_ID\" in ({0})) ", s);
                //where += string.Format(@" AND EXTRACT(YEAR FROM to_date(""BIRTHDATE"", get_date_format())) = {0} ", DateTime.Today.Year);
                //gvRegister.DataSource = Register.GetRegister(languageId, where);
                //gvRegister.DataBind();

                if (Session["_whereRegister"] != null)
                {
                    string where = Session["_whereRegister"].ToString();
                    
                    gvRegister.DataSource = Register.GetRegister(languageId, where);
                    gvRegister.DataBind();
                }
            }
        }
       
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if ((CurrentEnvironment.LoggedUser != null))
        {
            int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
            string _hfId = Request.QueryString["hfId"].ToString();
            if (!String.IsNullOrEmpty(_hfId))
                int.TryParse(_hfId, out hfId);

            string s = HealthFacility.GetAllChildsForOneHealthFacility(hfId);

            string where = string.Format(" AND (\"ADMINISTRATION_ID\" in ({0})) ", s);

            if (!string.IsNullOrEmpty(txtFirstName.Text))
            {
                where += string.Format(@" AND UPPER(""FIRSTNAME"") LIKE '%{0}%' ", txtFirstName.Text.Replace("'", @"''").ToUpper());
            }

            if (!string.IsNullOrEmpty(txtLastname.Text))
            {
                where += string.Format(@" AND UPPER(""LASTNAME"") LIKE '%{0}%' ", txtLastname.Text.Replace("'", @"''").ToUpper());
            }

            if (domicileId != null)
                where += string.Format(@" AND ""DOMICILE_ID"" = {0} ", domicileId);

            int year = int.Parse(ddlYear.SelectedValue);
            if (year != -1)
                where += string.Format(@" AND EXTRACT(YEAR FROM to_date(""BIRTHDATE"", get_date_format())) = {0} ", year);

            //domicileId = null;

            //string language = CurrentEnvironment.Language;
            //int languageId = int.Parse(language);
            //gvRegister.DataSource = Register.GetRegister(languageId, where);
            //gvRegister.DataBind();

            Session["_whereRegister"] = where;

            Response.Redirect(Request.RawUrl);
        }
    }

    protected void gvRegister_DataBound(object sender, EventArgs e)
    {
        if (gvRegister.Rows.Count > 0)
        {
            btnExcel.Visible = true;

        }
        else
            btnExcel.Visible = false;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=Register.xls");
        Response.Charset = "";

        // If you want the option to open the Excel file without saving then
        // comment out the line below
        // Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/ms-excel";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gvRegister.RenderControl(htmlWrite);
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