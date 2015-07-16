using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_GtinHfStockPolicy : System.Web.UI.Page
{
    //static String healthFacilityId;

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

            if ((actionList != null) && actionList.Contains("ViewGtinHfStockPolicy") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["GtinHfStockPolicy-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "GtinHfStockPolicy");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("GtinHfStockPolicy-dictionary" + language, wtList);
                }

                //controls
                lblGtin.Text = wtList["GtinHfStockPolicyGTIN"];
                lblHealthFacility.Text = wtList["GtinHfStockPolicyHealthFacility"];
                lblReorderQty.Text = wtList["GtinHfStockPolicyReorderQty"];
                lblLeadTime.Text = wtList["GtinHfStockPolicyLeadTime"];
                lblConsumptionLogic.Text = wtList["GtinHfStockPolicyConsumptionLogic"];
                //lblSafetyStock.Text = wtList["GtinHfStockPolicySafetyStock"];

                //grid header text
                gvGtinHfStockPolicy.Columns[0].HeaderText = wtList["GtinHfStockPolicyGTIN"];
                gvGtinHfStockPolicy.Columns[1].HeaderText = wtList["GtinHfStockPolicyHealthFacility"];
                gvGtinHfStockPolicy.Columns[3].HeaderText = wtList["GtinHfStockPolicyReorderQty"];
                gvGtinHfStockPolicy.Columns[2].HeaderText = wtList["GtinHfStockPolicyLeadTime"];
                gvGtinHfStockPolicy.Columns[5].HeaderText = wtList["GtinHfStockPolicyConsumptionLogic"];
                // gvGtinHfStockPolicy.Columns[4].HeaderText = wtList["GtinHfStockPolicySafetyStock"];

                //buttons
                this.btnEdit.Text = wtList["GtinHfStockPolicyEditButton"];

                //message
                lblSuccess.Text = wtList["GtinHfStockPolicySuccessText"];
                lblWarning.Text = wtList["GtinHfStockPolicyWarningText"];
                lblError.Text = wtList["GtinHfStockPolicyErrorText"];

                //Page Title
                lblTitle.Text = wtList["GtinHfStockPolicyPageTitle"];

                //selected object
                string _hf = Request.QueryString["hfId"];
                string _gtin = Request.QueryString["gtin"];

                if (!String.IsNullOrEmpty(_hf) && !String.IsNullOrEmpty(_gtin))
                {
                    GtinHfStockPolicy o = GtinHfStockPolicy.GetGtinHfStockPolicyByHealthFacilityCodeAndGtin(_hf, _gtin);
                    ddlGtin.SelectedValue = o.Gtin;
                    ddlGtin.DataBind();

                    ddlHealthFacility.SelectedValue = o.HealthFacilityCode;
                    LoadReorderQuantity();
                    if (!string.IsNullOrEmpty(o.ReorderQty.ToString()))
                        ddlReorderQty.SelectedValue = o.ReorderQty.ToString();

                    if (o.LeadTime != null)
                        txtLeadTime.Text = o.LeadTime.ToString();
                    ddlConsumptionLogic.SelectedValue = o.ConsumptionLogic;
                    txtSafetyStock.Text = o.SafetyStock.ToString();
                    gvGtinHfStockPolicy.DataBind();

                    btnEdit.Visible = true;
                    ddlGtin.Enabled = false;
                    ddlHealthFacility.Enabled = false;
                }

                LoadReorderQuantity();
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string _hf = Request.QueryString["hfId"];
                string _gtin = Request.QueryString["gtin"];

                if (!String.IsNullOrEmpty(_hf) && !String.IsNullOrEmpty(_gtin))
                {
                    GtinHfStockPolicy o = GtinHfStockPolicy.GetGtinHfStockPolicyByHealthFacilityCodeAndGtin(_hf, _gtin);
                    o.Gtin = ddlGtin.SelectedValue.ToString();
                    o.HealthFacilityCode = ddlHealthFacility.SelectedValue;
                    double rq = -1;
                    double.TryParse(ddlReorderQty.SelectedValue, out rq);
                    o.ReorderQty = rq;
                    int lt = -1;
                    if (!String.IsNullOrEmpty(txtLeadTime.Text))
                    {
                        int.TryParse(txtLeadTime.Text, out lt);
                        o.LeadTime = lt;
                    }
                    o.ConsumptionLogic = ddlConsumptionLogic.SelectedValue.ToString();
                    double ss = -1;
                    if (!String.IsNullOrEmpty(txtSafetyStock.Text))
                    {
                        double.TryParse(txtSafetyStock.Text, out ss);
                        o.SafetyStock = ss;
                    }
                    //if (!String.IsNullOrEmpty(txtNotes.Text))
                    //    o.Notes = txtNotes.Text.ToString();

                    int i = GtinHfStockPolicy.Update(o);
                    if (i > 0)
                    {
                        lblSuccess.Visible = true;
                        lblWarning.Visible = false;
                        lblError.Visible = false;
                        gvGtinHfStockPolicy.DataBind();
                        ClearControls(this);
                    }
                    else
                    {
                        lblSuccess.Visible = false;
                        lblWarning.Visible = true;
                        lblError.Visible = false;
                    }
                }
                else
                {
                    GtinHfStockPolicy o = new GtinHfStockPolicy();
                    o.Gtin = ddlGtin.SelectedValue.ToString();
                    o.HealthFacilityCode = ddlHealthFacility.SelectedValue;
                    double rq = -1;
                    double.TryParse(ddlReorderQty.SelectedValue, out rq);
                    o.ReorderQty = rq;
                    int lt = -1;
                    if (!String.IsNullOrEmpty(txtLeadTime.Text))
                    {
                        int.TryParse(txtLeadTime.Text, out lt);
                        o.LeadTime = lt;
                    }
                    o.ConsumptionLogic = ddlConsumptionLogic.SelectedValue.ToString();
                    double ss = -1;
                    if (!String.IsNullOrEmpty(txtSafetyStock.Text))
                    {
                        double.TryParse(txtSafetyStock.Text, out ss);
                        o.SafetyStock = ss;
                    }
                    //if (!String.IsNullOrEmpty(txtNotes.Text))
                    //    o.Notes = txtNotes.Text.ToString();

                    int i = 0;

                    GtinHfStockPolicy o1 = GtinHfStockPolicy.GetGtinHfStockPolicyByHealthFacilityCodeAndGtin(ddlHealthFacility.SelectedValue, ddlGtin.SelectedValue);
                    if (o1 == null)
                        i = GtinHfStockPolicy.Insert(o);
                    else
                        i = GtinHfStockPolicy.Update(o);

                    if (i > 0)
                    {
                        lblSuccess.Visible = true;
                        lblWarning.Visible = false;
                        lblError.Visible = false;
                        gvGtinHfStockPolicy.DataBind();
                        ClearControls(this);
                    }
                    else
                    {
                        lblSuccess.Visible = false;
                        lblWarning.Visible = true;
                        lblError.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }

    protected void HealthCenters_ValueSelected(object sender, System.EventArgs e)
    {
        //int _id = -1;
        //int.TryParse(txtHealthcenterId.SelectedItemID.ToString(), out _id);
        //HealthFacility hf = HealthFacility.GetHealthFacilityById(_id);
        //healthFacilityId = hf.Code;
    }

    private void ClearControls(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
                ClearControls(c);
            else
            {
                if (c is TextBox)
                    (c as TextBox).Text = "";
                //if (c is CheckBox)
                //    (c as CheckBox).Checked = false;
                if (c is DropDownList)
                    (c as DropDownList).SelectedIndex = 0;
            }
        }
    }

    protected void gvGtinHfStockPolicy_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvGtinHfStockPolicy.PageIndex = e.NewPageIndex;
    }

    protected void ddlGtin_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadReorderQuantity();
        LoadExistingValues();
    }

    protected void ddlHealthFacility_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadExistingValues();
    }

    private void LoadReorderQuantity()
    {
        ItemManufacturer im = ItemManufacturer.GetItemManufacturerByGtin(ddlGtin.SelectedValue);
        if (im != null)
        {
            List<ReorderQuantity> list = new List<ReorderQuantity>();

            ReorderQuantity rqDose = new ReorderQuantity();
            rqDose.Quantity = 1;
            rqDose.Name = "1 " + im.BaseUom;
            list.Add(rqDose);

            ReorderQuantity rqVial = new ReorderQuantity();
            rqVial.Quantity = im.Alt1QtyPer;
            rqVial.Name = im.Alt1QtyPer + " " + im.BaseUom + " - 1 " + im.Alt1Uom;
            list.Add(rqVial);

            if (!string.IsNullOrEmpty(im.Alt2Uom))
            {
                ReorderQuantity rqBox = new ReorderQuantity();
                rqBox.Quantity = im.Alt2QtyPer;
                rqBox.Name = im.Alt2QtyPer + " " + im.BaseUom + " - 1 " + im.Alt2Uom;
                list.Add(rqBox);
            }
            //txtReorderQty.Text = im.Alt2QtyPer.ToString();
            ddlReorderQty.DataSource = list;
            ddlReorderQty.DataValueField = "Quantity";
            ddlReorderQty.DataTextField = "Name";
            ddlReorderQty.DataBind();
        }
    }

    private void LoadExistingValues()
    {
        GtinHfStockPolicy o = GtinHfStockPolicy.GetGtinHfStockPolicyByHealthFacilityCodeAndGtin(ddlHealthFacility.SelectedValue, ddlGtin.SelectedValue);
        if (o != null)
        {
            if (!string.IsNullOrEmpty(o.ReorderQty.ToString()))
                ddlReorderQty.SelectedValue = o.ReorderQty.ToString();

            if (o.SafetyStock != null)
                txtSafetyStock.Text = o.SafetyStock.ToString();
            else
                txtSafetyStock.Text = string.Empty;
            if (o.LeadTime != null)
                txtLeadTime.Text = o.LeadTime.ToString();
            else
                txtLeadTime.Text = string.Empty;
        }
    }

    private struct ReorderQuantity
    {
        public int Quantity { get; set; }
        public string Name { get; set; }
    }
}