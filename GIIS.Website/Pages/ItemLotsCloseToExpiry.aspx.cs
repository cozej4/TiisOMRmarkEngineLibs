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

public partial class Pages_ItemLotsCloseToExpiry : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewItemLotsCloseToExpiry") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;

                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["ItemLotsCloseToExpiry-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ItemLotsCloseToExpiry");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("ItemLotsCloseToExpiry-dictionary" + language, wtList);
                }

                this.lblEntryDateFrom.Text = wtList["ItemLotsCloseToExpiryExpireDate"];
                this.lblItem.Text = wtList["ItemLotsCloseToExpiryItem"];

                //grid header text
                gvItemLot.Columns[1].HeaderText = wtList["ItemLotsCloseToExpiryLotNumber"];
                gvItemLot.Columns[2].HeaderText = wtList["ItemLotsCloseToExpiryItem"];
                gvItemLot.Columns[3].HeaderText = wtList["ItemLotsCloseToExpiryEntryDate"];
                gvItemLot.Columns[4].HeaderText = wtList["ItemLotsCloseToExpiryExpireDate"];
                gvItemLot.Columns[5].HeaderText = wtList["ItemLotsCloseToExpiryReceived"];
                gvItemLot.Columns[6].HeaderText = wtList["ItemLotsCloseToExpiryDistributed"];
                gvItemLot.Columns[7].HeaderText = wtList["ItemLotsCloseToExpiryUsed"];
                gvItemLot.Columns[8].HeaderText = wtList["ItemLotsCloseToExpiryWasted"];
                gvItemLot.Columns[9].HeaderText = wtList["ItemLotsCloseToExpiryBalance"];

                this.btnSearch.Text = wtList["ItemLotsCloseToExpirySearchButton"];
                this.btnExcel.Text = wtList["ItemLotsCloseToExpiryExcelButton"];
                this.btnPrint.Text = wtList["ItemLotsCloseToExpiryPrintButton"];

                //validators
                string format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                string expresion = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateExpresion;

                ceExpireDate.Format = format;
                revExpireDate.ErrorMessage = format;
                revExpireDate.ValidationExpression = expresion;
                //txtExpireDate.Text = DateTime.Today.AddMonths(1).ToString(format);

                //message
                this.lblWarning.Text = wtList["ItemLotsCloseToExpirySearchWarningText"];

                //Page Title
                this.lblTitle.Text = wtList["ItemLotsCloseToExpiryListPageTitle"];

                //gridview databind
               string where = string.Format(@" AND ""EXPIRE_DATE"" <= '{0}' ", DateTime.Today.AddMonths(1).ToString("yyyy-MM-dd"));
               
                odsItemLot.SelectParameters.Clear();
                odsItemLot.SelectParameters.Add("hfId", CurrentEnvironment.LoggedUser.HealthFacilityId.ToString());
                odsItemLot.SelectParameters.Add("where", where);
                HttpContext.Current.Session["where"] = where;
            }
            else
            {
                Response.Redirect("Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
    }

    protected void gvItemLot_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblEntryDate = (Label)e.Row.FindControl("lblEntryDate");
            Label lblExpireDate = (Label)e.Row.FindControl("lblExpireDate");
            ItemLot lot = ItemLot.GetItemLotById(int.Parse(e.Row.Cells[10].Text));
            //lblEntryDate.Text = lot.EntryDate.ToString(dateformat);
            lblExpireDate.Text = lot.ExpireDate.ToString(dateformat);
        }
    }
    protected void gvItemLot_DataBound(object sender, EventArgs e)
    {
        if (gvItemLot.Rows.Count <= 0)
        {
            lblWarning.Visible = true;
            btnExcel.Visible = false;
            btnPrint.Visible = false;
        }
        else
        {
            btnExcel.Visible = true;
            btnPrint.Visible = true;
            lblWarning.Visible = false;
            foreach (GridViewRow gvr in gvItemLot.Rows)
            {
                gvr.Cells[10].Visible = false;
            }
            gvItemLot.HeaderRow.Cells[10].Visible = false;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (CurrentEnvironment.LoggedUser != null)
        {
            string where = String.Empty;
            string format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString();

            if (ddlItem.SelectedIndex != 0)
            {
                where += @" AND ""ITEM_ID"" = " + int.Parse(ddlItem.SelectedValue);
                HttpContext.Current.Session["item"] = ddlItem.SelectedValue;
            }
            if (txtExpireDate.Text != String.Empty)
            {
                DateTime expireDate = DateTime.ParseExact(txtExpireDate.Text, format, CultureInfo.CurrentCulture);
                where += string.Format(@" AND ""EXPIRE_DATE"" <= '{0}' ", expireDate.ToString("yyyy-MM-dd"));
                HttpContext.Current.Session["expiredate"] = txtExpireDate.Text;
            }
            else
            {
                where += string.Format(@" AND ""EXPIRE_DATE"" <= '{0}' ", DateTime.Today.AddMonths(1).ToString("yyyy-MM-dd"));
            }

            HttpContext.Current.Session["where"] = where;
            odsItemLot.SelectParameters.Clear();
            odsItemLot.SelectParameters.Add("hfId", CurrentEnvironment.LoggedUser.HealthFacilityId.ToString());
            odsItemLot.SelectParameters.Add("where", where);
                        
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=ItemLotsCloseToExpiry.xls");
            Response.Charset = "";

            // If you want the option to open the Excel file without saving then
            // comment out the line below
            // Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            gvItemLot.RenderControl(htmlWrite);
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
        int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
        string queryString = "PrintItemLotsCloseToExpiry.aspx?hfId=" + hfId;
        string newWin = "window.open('" + queryString + "');";
        ClientScript.RegisterStartupScript(this.GetType(), "pop", newWin, true);
        
    }
}