using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_ItemLotListNew : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewItemLot") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["ItemLot-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ItemLot");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("ItemLot-dictionary" + language, wtList);
                }

                //controls
                lblItem.Text = wtList["ItemLotItem"];
                lblGtin.Text = wtList["ItemLotGTIN"];

                //grid header text
                gvItemLotNew.Columns[0].HeaderText = wtList["ItemLotGTIN"];
                gvItemLotNew.Columns[1].HeaderText = wtList["ItemLotLotNumber"];
                gvItemLotNew.Columns[2].HeaderText = wtList["ItemLotItem"];
                gvItemLotNew.Columns[3].HeaderText = wtList["ItemLotExpireDate"];
                gvItemLotNew.Columns[4].HeaderText = wtList["ItemLotNotes"];

                gvExport.Columns[0].HeaderText = wtList["ItemLotGTIN"];
                gvExport.Columns[1].HeaderText = wtList["ItemLotLotNumber"];
                gvExport.Columns[2].HeaderText = wtList["ItemLotItem"];
                gvExport.Columns[3].HeaderText = wtList["ItemLotExpireDate"];
                gvExport.Columns[4].HeaderText = wtList["ItemLotNotes"];
                
                //actions
                //btnAddNew.Visible = actionList.Contains("AddNewItemLot");
                //btnSearch.Visible = actionList.Contains("SearchItemLot");

                //Page Title
                this.lblTitle.Text = wtList["ItemLotListTitle"];

                //buttons
                this.btnAddNew.Text = wtList["ItemLotAddNewButton"];
                this.btnSearch.Text = wtList["ItemLotSearchButton"];
                this.btnExcel.Text = wtList["ItemLotListExcelButton"];

                //message
               this.lblWarning.Text = wtList["ItemLotSearchWarningText"];

                //gridview databind
                int itemId = -1;
                string gtin = "";
                odsItemLotNew.SelectParameters.Clear();
                odsItemLotNew.SelectParameters.Add("itemId", itemId.ToString());
                odsItemLotNew.SelectParameters.Add("gtin", gtin);
                gvItemLotNew.DataBind();
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        int itemId = int.Parse(ddlItem.SelectedValue);
        string gtin = ddlGtin.SelectedValue;

        odsItemLotNew.SelectParameters.Clear();
        odsItemLotNew.SelectParameters.Add("itemId", itemId.ToString());
        odsItemLotNew.SelectParameters.Add("gtin", gtin );
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("ItemLotNew.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        int itemId = int.Parse(ddlItem.SelectedValue);
        string gtin = ddlGtin.SelectedValue;
        int maximumRows = int.MaxValue;
        int startRowIndex = 0;

        List<ItemLot> list = ItemLot.GetPagedItemLotList(itemId, gtin, ref maximumRows, ref startRowIndex);
        gvExport.DataSource = list;
        gvExport.DataBind();
        gvExport.Visible = true;

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=ItemManufacturerList.xls");
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

    protected void gvItemLot_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvItemLotNew.PageIndex = e.NewPageIndex;
    }
    protected void gvItemLot_DataBound(object sender, EventArgs e)
    {
        if (gvItemLotNew.Rows.Count <= 0)
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