using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class Pages_ItemLotBalanceInHealthFacilities : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewItemLotBalanceInHealthFacilities") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["HealthFacilityItemLotBalance-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "HealthFacilityItemLotBalance");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("HealthFacilityItemLotBalance-dictionary" + language, wtList);
                }

                int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;

                //controls
                lblItemCategoryId.Text = wtList["HealthFacilityItemLotBalanceItemCategory"];
                lblItemId.Text = wtList["HealthFacilityItemLotBalanceItem"];
                lblItemLot.Text = wtList["HealthFacilityItemLotBalanceItemLot"];

                //grid header text
                gvHealthFacilityBalance.Columns[1].HeaderText = wtList["HealthFacilityItemLotBalanceHealthFacility"];
                gvHealthFacilityBalance.Columns[2].HeaderText = wtList["HealthFacilityItemLotBalanceReceived"];
                gvHealthFacilityBalance.Columns[3].HeaderText = wtList["HealthFacilityItemLotBalanceDistributed"];
                gvHealthFacilityBalance.Columns[4].HeaderText = wtList["HealthFacilityItemLotBalanceUsed"];
                gvHealthFacilityBalance.Columns[5].HeaderText = wtList["HealthFacilityItemLotBalanceWasted"];
                gvHealthFacilityBalance.Columns[6].HeaderText = wtList["HealthFacilityItemLotBalanceBalance"];

                //buttons
                this.btnSearch.Text = wtList["HealthFacilityItemLotBalanceSearchButton"];
                this.btnExcel.Text = wtList["HealthFacilityItemLotBalanceExcelButton"];
                this.btnPrint.Text = wtList["HealthFacilityItemLotBalancePrintButton"];

                //message
                this.lblWarning.Text = wtList["HealthFacilityItemLotBalanceWarningText"];

                //Page Title
                this.lblTitle.Text = wtList["ItemLotBalanceInHealthFacilitiesPageTitle"];


            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    protected void ddlItems_SelectedIndexChanged(object sender, EventArgs e)
    {
        string itemId = ddlItems.SelectedValue;
        odsItemLot.SelectParameters.Clear();
        odsItemLot.SelectParameters.Add("itemId", itemId);
        odsItemLot.SelectParameters.Add("hfId", CurrentEnvironment.LoggedUser.HealthFacilityId.ToString());
        odsItemLot.DataBind();
        HttpContext.Current.Session["item"] = itemId;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemLot.SelectedIndex != 0)
            {
                int itemlotId = int.Parse(ddlItemLot.SelectedValue);
                int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
                HttpContext.Current.Session["itemlot"] = itemlotId;
                odsHealthFacilityBalance.SelectParameters.Clear();
                odsHealthFacilityBalance.SelectParameters.Add("hfId", hfId.ToString());
                odsHealthFacilityBalance.SelectParameters.Add("itemlotid", itemlotId.ToString());
            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlItemCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        odsItemLot.SelectParameters.Clear();
        odsItemLot.SelectParameters.Add("itemId", "-1");
        odsItemLot.SelectParameters.Add("hfId", CurrentEnvironment.LoggedUser.Id.ToString());
        ddlItemLot.SelectedIndex = 0;
    }
    protected void gvHealthFacilityBalance_DataBound(object sender, EventArgs e)
    {
        if (gvHealthFacilityBalance.Rows.Count > 0)
        {
            btnExcel.Visible = true;
            btnPrint.Visible = true;
        }
        else
        {
            btnExcel.Visible = false;
            btnPrint.Visible = false;
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=ItemLotBalanceInHealthFacilities.xls");
        Response.Charset = "";

        // If you want the option to open the Excel file without saving then
        // comment out the line below
        // Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/ms-excel";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gvHealthFacilityBalance.RenderControl(htmlWrite);
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
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        int hfId;
        if (HttpContext.Current.Session["hfId"] != null)
            hfId = int.Parse(HttpContext.Current.Session["hfId"].ToString());
        else
            hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;

        string queryString = "PrintItemLotBalanceInHealthFacilies.aspx?hfId=" + hfId;
        string newWin = "window.open('" + queryString + "');";
        ClientScript.RegisterStartupScript(this.GetType(), "pop", newWin, true);

    }
}