using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.DataLayer;

public partial class _PlaceList : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewPlaceList") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Place-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Place");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Place-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["PlaceName"];

                //grid header text
                gvPlace.Columns[1].HeaderText = wtList["PlaceName"];
                gvPlace.Columns[2].HeaderText = wtList["PlaceParent"];
                gvPlace.Columns[3].HeaderText = wtList["PlaceLeaf"];
                gvPlace.Columns[4].HeaderText = wtList["PlaceNotes"];
                gvPlace.Columns[5].HeaderText = wtList["PlaceIsActive"];
                gvExport.Columns[1].HeaderText = wtList["PlaceName"];
                gvExport.Columns[2].HeaderText = wtList["PlaceParent"];
                gvExport.Columns[3].HeaderText = wtList["PlaceLeaf"];
                gvExport.Columns[4].HeaderText = wtList["PlaceNotes"];
                gvExport.Columns[5].HeaderText = wtList["PlaceIsActive"];

                //actions
                this.btnAddNew.Visible = actionList.Contains("AddPlace");
                //this.btnSearch.Visible = actionList.Contains("SearchPlace");

                //Page Title
                this.lblTitle.Text = wtList["PlaceListPageTitle"];

                //buttons
                this.btnAddNew.Text = wtList["PlaceAddNewButton"];
                this.btnSearch.Text = wtList["PlaceSearchButton"];
                this.btnExcel.Text = wtList["PlaceListExcelButton"];

                //message
                this.lblWarning.Text = wtList["PlaceSearchWarningText"];

                //gridview databind
                Session["PlaceList-Where"] = "";
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string wsearch = txtName.Text.ToUpper().Replace("'", @"''");
        if (!String.IsNullOrEmpty(wsearch))
        {
            odsPlace.SelectParameters.Clear();
            odsPlace.SelectParameters.Add("name", wsearch);
            Session["PlaceList-Name"] = wsearch;
        }
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("Place.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }
    protected void gvPlace_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPlace.PageIndex = e.NewPageIndex;
    }
    protected void gvPlace_DataBound(object sender, EventArgs e)
    {
        if (gvPlace.Rows.Count <= 0)
        {
            lblWarning.Visible = true;
            btnExcel.Visible = false;
        }
        else
        {
            btnExcel.Visible = true;
            lblWarning.Visible = false;
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string name = string.Empty;
        if (Session["PlaceList-Name"] != null)
                   name = Session["PlaceList-Name"].ToString();
            
                int maximumRows = int.MaxValue;
                int startRowIndex = 0;

                List<Place> list = Place.GetPagedPlaceList(name, ref maximumRows, ref startRowIndex);
                gvExport.DataSource = list;
                gvExport.DataBind();
                gvExport.Visible = true;
            

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=PlaceList.xls");
            Response.Charset = "";

            // If you want the option to open the Excel file without saving then
            // comment out the line below
            // Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            gvExport.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();

            gvExport.Visible = false;
        
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