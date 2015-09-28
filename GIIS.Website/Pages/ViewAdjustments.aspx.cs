//*******************************************************************************
//Copyright 2015 TIIS - Tanzania Immunization Information System
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
 //******************************************************************************
using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_ViewAdjustments : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewAdjustments") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["ViewAdjustments-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ViewAdjustments");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("ViewAdjustments-dictionary" + language, wtList);
                }

                //controls
                this.lblFirstDate.Text = wtList["ViewAdjustmentsFirstDate"];
                this.lblEndDate.Text = wtList["ViewAdjustmentsEndDate"];
                this.lblWarning.Text = wtList["ViewAdjustmentsWarningText"];
                this.lblAdjustmentReason.Text = wtList["ViewAdjustmentsReason"];

                //grid header text
                gvTransactionLines.Columns[1].HeaderText = wtList["ViewAdjustmentsHealthFacility"];
                gvTransactionLines.Columns[2].HeaderText = wtList["ViewAdjustmentsTransactionDate"];
                gvTransactionLines.Columns[3].HeaderText = wtList["ViewAdjustmentsItemLot"];
                gvTransactionLines.Columns[4].HeaderText = wtList["ViewAdjustmentsItem"];
                gvTransactionLines.Columns[5].HeaderText = wtList["ViewAdjustmentsQuantity"];
                gvTransactionLines.Columns[6].HeaderText = wtList["ViewAdjustmentsReason"];

                gvExport.Columns[1].HeaderText = wtList["ViewAdjustmentsHealthFacility"];
                gvExport.Columns[2].HeaderText = wtList["ViewAdjustmentsTransactionDate"];
                gvExport.Columns[3].HeaderText = wtList["ViewAdjustmentsItemLot"];
                gvExport.Columns[4].HeaderText = wtList["ViewAdjustmentsItem"];
                gvExport.Columns[5].HeaderText = wtList["ViewAdjustmentsQuantity"];
                gvExport.Columns[6].HeaderText = wtList["ViewAdjustmentsReason"];

                //buttons
                this.btnSearch.Text = wtList["ViewAdjustmentsSearchButton"];
                this.btnExcel.Text = wtList["ViewAdjustmentsExcelButton"];
                this.btnPrint.Text = wtList["ViewAdjustmentsPrintButton"];

                //Page Title
                this.lblTitle.Text = wtList["ViewAdjustmentsPageTitle"];

                //validators
                ConfigurationDate cd = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
                ceFirstDate.Format = cd.DateFormat;
                revFirstDate.ErrorMessage = cd.DateFormat;
                revFirstDate.ValidationExpression = cd.DateExpresion;
                ceEndDate.Format = cd.DateFormat;
                revEndDate.ErrorMessage = cd.DateFormat;
                revEndDate.ValidationExpression = cd.DateExpresion;

                int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;

                string _hfId = (string)Request.QueryString["hfId"];
                if (!String.IsNullOrEmpty(_hfId))
                    int.TryParse(_hfId, out hfId);
                HttpContext.Current.Session["datefrom"] = txtFirstDate.Text;
                HttpContext.Current.Session["dateto"] = txtEndDate.Text;
                HttpContext.Current.Session["adjustmentReason"] = ddlAdjustmentReason.SelectedValue;
                HttpContext.Current.Session["ViewAdjustments-Where"] = " and 1 = 1";
                
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    protected void ValidateDates(object sender, ServerValidateEventArgs e)
    {
        ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
        if (txtFirstDate.Text != String.Empty || txtEndDate.Text != String.Empty)
        {
            DateTime datefrom = DateTime.MinValue;
            if (txtFirstDate.Text != String.Empty)
                datefrom = DateTime.ParseExact(txtFirstDate.Text, dateformat.DateFormat, CultureInfo.CurrentCulture);
            DateTime dateto = DateTime.MaxValue;
            if (txtEndDate.Text != String.Empty)
                dateto = DateTime.ParseExact(txtEndDate.Text, dateformat.DateFormat, CultureInfo.CurrentCulture);
            //if ((txtFirstDate.Text != String.Empty) && (txtEndDate.Text != String.Empty))
            e.IsValid = datefrom <= dateto && datefrom <= DateTime.Today.Date;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (CurrentEnvironment.LoggedUser != null)
        {
            int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;

            string _hfId = (string)Request.QueryString["hfId"];
            if (!String.IsNullOrEmpty(_hfId))
                int.TryParse(_hfId, out hfId);

            string where = string.Format(@" AND ""HEALTH_FACILITY_ID"" = {0} ", hfId);

            if (ddlAdjustmentReason.SelectedValue != "-1")
            {
                where += string.Format(@" AND ""ADJUSTMENT_ID"" = {0} ", ddlAdjustmentReason.SelectedValue);
                HttpContext.Current.Session["adjustmentReason"] = ddlAdjustmentReason.SelectedValue;
            }

            string dateFormat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString();

            DateTime datefrom = new DateTime();
            DateTime dateto = new DateTime();
            string transactionDate;
            if ((txtFirstDate.Text != String.Empty) && (txtEndDate.Text != String.Empty))
            {
                datefrom = DateTime.ParseExact(txtFirstDate.Text, dateFormat, CultureInfo.CurrentCulture);
                dateto = DateTime.ParseExact(txtEndDate.Text, dateFormat, CultureInfo.CurrentCulture);
                transactionDate = string.Format(@" AND ""TRANSACTION_DATE"" between '{0}' and '{1}'", datefrom.ToString("yyyy-MM-dd"), dateto.ToString("yyyy-MM-dd"));
            }
            else if (txtFirstDate.Text != String.Empty)
            {
                datefrom = DateTime.ParseExact(txtFirstDate.Text, dateFormat, CultureInfo.CurrentCulture);
                transactionDate = string.Format(@" AND ""TRANSACTION_DATE"" >= '{0}' ", datefrom.ToString("yyyy-MM-dd"));
            }
            else if (txtEndDate.Text != String.Empty)
            {
                dateto = DateTime.ParseExact(txtEndDate.Text, dateFormat, CultureInfo.CurrentCulture);
                transactionDate = string.Format(@" AND ""TRANSACTION_DATE"" <= '{0}' ", dateto.ToString("yyyy-MM-dd"));
            }
            else
                transactionDate = "";

            where += transactionDate;
            
            odsTransactionLines.SelectParameters.Clear();
            odsTransactionLines.SelectParameters.Add("where", where);

            HttpContext.Current.Session["ViewAdjustments-Where"] = where;
            HttpContext.Current.Session["datefrom"] = datefrom;
            HttpContext.Current.Session["dateto"] = dateto;

            //List<TransactionLines> transactionList = TransactionLines.GetAdjustmentsByDateInterval(where);
            //gvTransactionLines.DataSource = transactionList;
            //gvTransactionLines.DataBind();
        }
    }

    protected void gvTransactionLines_DataBinding(object sender, EventArgs e)
    {
        string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;

        ((BoundField)gvTransactionLines.Columns[2]).DataFormatString = "{0:" + dateformat + "}";
        ((BoundField)gvExport.Columns[2]).DataFormatString = "{0:" + dateformat + "}";
    }

    protected void gvTransactionLines_DataBound(object sender, EventArgs e)
    {
        if (gvTransactionLines.Rows.Count <= 0)
        {
            lblWarning.Visible = true;
            btnExcel.Visible = false;
            btnPrint.Visible = false;
        }
        else
        {
            btnExcel.Visible = true;
            lblWarning.Visible = false;
            btnPrint.Visible = true;
        }
    }

    protected void gvTransactionLines_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTransactionLines.PageIndex = e.NewPageIndex;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        if (Session["ViewAdjustments-Where"] != null)
        {
            string where = Session["ViewAdjustments-Where"].ToString();
            if (!string.IsNullOrEmpty(where))
            {
                int maximumRows = int.MaxValue;
                int startRowIndex = 0;

                List<TransactionLines> transactionList = TransactionLines.GetPagedAdjustmentsByDateInterval(ref maximumRows, ref startRowIndex, where);
                gvExport.DataSource = transactionList;
                gvExport.DataBind();
                gvExport.Visible = true;
            }

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=ViewAdjustments.xls");
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

        string queryString = "PrintViewAdjustments.aspx?hfId=" + hfId;
        string newWin = "window.open('" + queryString + "');";
        ClientScript.RegisterStartupScript(this.GetType(), "pop", newWin, true);

    }
}