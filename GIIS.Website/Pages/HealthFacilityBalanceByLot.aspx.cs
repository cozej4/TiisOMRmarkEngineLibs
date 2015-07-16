using System.Linq;
using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using GIIS.BusinessLogic;
using System.Diagnostics;

public partial class Pages_HealthFacilityBalanceByLot : System.Web.UI.Page
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

            btnUpdateAndOrder.Visible = (actionList != null) && actionList.Contains("CreateOrderFromDrp") && (CurrentEnvironment.LoggedUser != null);

            if ((actionList != null) && actionList.Contains("ViewHealthFacilityBalance") && (CurrentEnvironment.LoggedUser != null))
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
                string _hfId = (string)Request.QueryString["hfId"];
                if (!String.IsNullOrEmpty(_hfId))
                    int.TryParse(_hfId, out hfId);

                lblHealthfacility.Visible = true;
                HealthFacility hf = HealthFacility.GetHealthFacilityById(hfId);
                lblHealthfacility.Text = hf.Name;


                if (CurrentEnvironment.LoggedUser.HealthFacilityId == hfId || (CurrentEnvironment.LoggedUser.HealthFacilityId == hf.ParentId && hf.VaccinationPoint))
                    btnUpdateBalance.Visible = true;
                else
                    btnUpdateBalance.Visible = false;

                odsHealthFacilityBalance.SelectParameters.Clear();
                odsHealthFacilityBalance.SelectParameters.Add("id", hfId.ToString());
                odsHealthFacilityBalance.DataBind();

                //grid header text
                gvHealthFacilityBalance.Columns[1].HeaderText = wtList["HealthFacilityItemLotBalanceGtin"];
                gvHealthFacilityBalance.Columns[2].HeaderText = wtList["HealthFacilityItemLotBalanceItem"];
                gvHealthFacilityBalance.Columns[3].HeaderText = wtList["HealthFacilityItemLotBalanceItemLot"];
                gvHealthFacilityBalance.Columns[4].HeaderText = wtList["HealthFacilityItemLotBalanceExpireDate"];
                //gvHealthFacilityBalance.Columns[5].HeaderText = wtList["HealthFacilityItemLotBalanceAMC"];
                //gvHealthFacilityBalance.Columns[6].HeaderText = wtList["HealthFacilityItemLotBalanceSafetyStock"];
                gvHealthFacilityBalance.Columns[7].HeaderText = wtList["HealthFacilityItemLotBalanceWasted"];
                gvHealthFacilityBalance.Columns[8].HeaderText = wtList["HealthFacilityItemLotBalanceBalance"];
                //gvHealthFacilityBalance.Columns[9].HeaderText = wtList["HealthFacilityItemLotBalanceDaysOfInventory"];
                gvHealthFacilityBalance.Columns[10].HeaderText = wtList["HealthFacilityBaseUom"];
                gvHealthFacilityBalance.Columns[11].HeaderText = wtList["HealthFacilityItemLotBalanceQuantity"];

                // TODO: JF - Translate the columns for stock management
                
                
             
                //buttons
                this.btnUpdateBalance.Text = wtList["HealthFacilityItemLotBalanceUpdateBalance"];
                //this.btnEdit.Text = wtList["HealthFacilityBalanceEditButton"];
                this.btnExcel.Text = wtList["HealthFacilityItemLotBalanceExcelButton"];
                this.btnPrint.Text = wtList["HealthFacilityItemLotBalancePrintButton"];

                //message
                this.lblWarning.Text = wtList["HealthFacilityBalanceByLotWarningText"];

                //Page Title
                this.lblTitle.Text = wtList["HealthFacilityByLotPageTitle"];
                this.lblSuccess.Text = wtList["HealthFacilityItemLotBalanceSuccessText"];
                this.lblError.Text = wtList["HealthFacilityItemLotBalanceErrorText"];

                ceDate.Format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revDate.ErrorMessage = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revDate.ValidationExpression = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateExpresion;

                ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
                txtDate.Text = DateTime.Today.ToString(dateformat.DateFormat.ToString());

            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    protected void btnUpdateBalance_Click(object sender, EventArgs e)
    {
        try
        {
            int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
            string _hfId = (string)Request.QueryString["hfId"];
            if (!String.IsNullOrEmpty(_hfId))
                int.TryParse(_hfId, out hfId);

            foreach (GridViewRow gr in gvHealthFacilityBalance.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtQty = (TextBox)gr.FindControl("txtQty");
                    if (!string.IsNullOrEmpty(txtQty.Text))
                    {
                        UInt32 qty = UInt32.Parse(txtQty.Text.Trim());
                        string gtin = gr.Cells[1].Text;
                        string lotno = gr.Cells[3].Text;

                        DateTime date = DateTime.ParseExact(txtDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                        
                        HealthFacility hf = HealthFacility.GetHealthFacilityById(hfId);
                        StockManagementLogic sml = new StockManagementLogic();
                        ItemTransaction st = sml.StockCount(hf, gtin, lotno, qty, CurrentEnvironment.LoggedUser.Id, date);
                        int i = st.Id;

                        if (i > 0)
                        {
                            lblSuccess.Visible = true;
                            lblError.Visible = false;
                        }
                        else
                        {
                            lblSuccess.Visible = false;
                            lblError.Visible = true;
                        }

                    }
                }
            }

            odsHealthFacilityBalance.SelectParameters.Clear();
            odsHealthFacilityBalance.SelectParameters.Add("id", hfId.ToString());
            odsHealthFacilityBalance.DataBind();
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblError.Visible = true;
        }
    }

    protected void gvHealthFacilityBalance_DataBound(object sender, EventArgs e)
    {
        if (gvHealthFacilityBalance.Rows.Count == 0)
        {
            lblWarning.Visible = true;
            btnExcel.Visible = false;
            btnPrint.Visible = false;
        }
        else
        {
            lblWarning.Visible = false;
            btnExcel.Visible = true;
            //btnPrint.Visible = true;

        }

    }
    protected void gvHealthFacilityBalance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblExpireDate = (Label)e.Row.FindControl("lblExpireDate");
            ItemLot lot = ItemLot.GetItemLotByGtinAndLotNo(e.Row.Cells[1].Text, e.Row.Cells[3].Text);
            if (lot != null)
                lblExpireDate.Text = lot.ExpireDate.ToString("dd-MMM-yyyy");
            int days = int.Parse(Configuration.GetConfigurationByName("LimitNumberOfDaysBeforeExpire").Value);
            if (lot.ExpireDate < DateTime.Today.Date)
                e.Row.Cells[4].BackColor = System.Drawing.Color.OrangeRed;
            else if (lot.ExpireDate < DateTime.Today.Date.AddDays(days))
                e.Row.Cells[4].BackColor = System.Drawing.Color.Yellow;
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=HealthFacilityBalanceByLot.xls");
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

        string queryString = "PrintHealthFacilityBalanceByLot.aspx?hfId=" + hfId;
        string newWin = "window.open('" + queryString + "');";
        ClientScript.RegisterStartupScript(this.GetType(), "pop", newWin, true);
        HttpContext.Current.Session["hfId"] = null;
    }
    /// <summary>
    /// Update an order
    /// </summary>
    protected void btnUpdateAndOrder_Click(object sender, EventArgs e)
    {

        // Update the balance
        // TODO: This should be in a BLL object eventually
        this.btnUpdateBalance_Click(sender, e);

        try
        {
           
            int hfId;
            if (Request.QueryString["hfId"] != null)
                hfId = int.Parse(Request.QueryString["hfId"].ToString());
            else
                hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;

            OrderManagementLogic oml = new OrderManagementLogic();
            var orderData = oml.RequestOrderFromDrp(HealthFacility.GetHealthFacilityById(hfId), CurrentEnvironment.LoggedUser.Id);
            // Redirect to the transfer order we created
            Response.Redirect(String.Format("{0}?OrderNum={1}", "TransferOrderDetail.aspx", orderData.OrderNum));
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            lblSuccess.Visible = false;
            lblError.Visible = true;
        }
    }
}