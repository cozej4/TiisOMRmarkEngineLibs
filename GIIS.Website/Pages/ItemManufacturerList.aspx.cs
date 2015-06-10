using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_ItemManufacturerList : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewItemManufacturer") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["ItemManufacturer-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ItemManufacturer");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("ItemManufacturer-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["ItemManufacturerItem"];

                //grid header text
                gvItemManufacturer.Columns[0].HeaderText = wtList["ItemManufacturerGTIN"];
                gvItemManufacturer.Columns[1].HeaderText = wtList["ItemManufacturerItem"];
                gvItemManufacturer.Columns[2].HeaderText = wtList["ItemManufacturerManufacturer"];
                gvItemManufacturer.Columns[3].HeaderText = wtList["ItemManufacturerBaseUOM"];
                gvItemManufacturer.Columns[4].HeaderText = wtList["ItemManufacturerAlt1UOM"];
                gvItemManufacturer.Columns[5].HeaderText = wtList["ItemManufacturerAlt1Qty"];
                gvItemManufacturer.Columns[6].HeaderText = wtList["ItemManufacturerAlt2UOM"];
                gvItemManufacturer.Columns[7].HeaderText = wtList["ItemManufacturerAlt2Qty"];
                gvItemManufacturer.Columns[8].HeaderText = wtList["ItemManufacturerGTINParent"];
                gvItemManufacturer.Columns[10].HeaderText = wtList["ItemManufacturerIsActive"];
                gvItemManufacturer.Columns[9].HeaderText = wtList["ItemManufacturerNotes"];
                gvItemManufacturer.Columns[11].HeaderText = wtList["ItemManufacturerModifiedBy"];
                gvExport.Columns[0].HeaderText = wtList["ItemManufacturerGTIN"];
                gvExport.Columns[1].HeaderText = wtList["ItemManufacturerItem"];
                gvExport.Columns[2].HeaderText = wtList["ItemManufacturerManufacturer"];
                gvExport.Columns[3].HeaderText = wtList["ItemManufacturerBaseUOM"];
                gvExport.Columns[4].HeaderText = wtList["ItemManufacturerAlt1UOM"];
                gvExport.Columns[5].HeaderText = wtList["ItemManufacturerAlt1Qty"];
                gvExport.Columns[6].HeaderText = wtList["ItemManufacturerAlt2UOM"];
                gvExport.Columns[7].HeaderText = wtList["ItemManufacturerAlt2Qty"];
                gvExport.Columns[8].HeaderText = wtList["ItemManufacturerGTINParent"];
                gvExport.Columns[9].HeaderText = wtList["ItemManufacturerIsActive"];
                gvExport.Columns[10].HeaderText = wtList["ItemManufacturerNotes"];
                gvExport.Columns[11].HeaderText = wtList["ItemManufacturerModifiedBy"];

                //actions
               //this.btnAddNew.Visible = actionList.Contains("AddNewItemManufacturer");
                //this.btnSearch.Visible = actionList.Contains("SearchItemManufacturer");

                //Page Title
                this.lblTitle.Text = wtList["ItemManufacturerListTitle"];

                //buttons
                this.btnAddNew.Text = wtList["ItemManufacturerAddNewButton"];
                this.btnSearch.Text = wtList["ItemManufacturerSearchButton"];
                this.btnExcel.Text = wtList["ItemManufacturerListExcelButton"];

                //message
                this.lblWarning.Text = wtList["ItemManufacturerSearchWarningText"];

                //gridview databind
                int itemId = -1;
                odsItemManufacturer.SelectParameters.Clear();
                odsItemManufacturer.SelectParameters.Add("itemId", itemId.ToString());
                //Session["ItemManufacturerList-ItemId"] = "-1";
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

        odsItemManufacturer.SelectParameters.Clear();
        odsItemManufacturer.SelectParameters.Add("itemId", itemId.ToString());
        //Session["ItemManufacturerList-ItemId"] = itemId;
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("ItemManufacturer.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }
    protected void gvItemManufacturer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvItemManufacturer.PageIndex = e.NewPageIndex;
    }
    protected void gvItemManufacturer_DataBound(object sender, EventArgs e)
    {
        if (gvItemManufacturer.Rows.Count <= 0)
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
        //if (Session["ItemManufacturerList-ItemId"] != null)
        //{
            int itemId = int.Parse(ddlItem.SelectedValue);
            int maximumRows = int.MaxValue;
            int startRowIndex = 0;

            List<ItemManufacturer> list = ItemManufacturer.GetPagedItemManufacturerList(itemId, ref maximumRows, ref startRowIndex);
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
       // }
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