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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _HealthFacilityBalance : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewHealthFacilityBalance") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["HealthFacilityBalance-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "HealthFacilityBalance");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("HealthFacilityBalance-dictionary" + language, wtList);
                }

                //grid header text
                gvHealthFacilityBalance.Columns[1].HeaderText = wtList["HealthFacilityBalanceItem"];
                gvHealthFacilityBalance.Columns[2].HeaderText = wtList["HealthFacilityBalanceReceived"];
                gvHealthFacilityBalance.Columns[3].HeaderText = wtList["HealthFacilityBalanceDistributed"];
                gvHealthFacilityBalance.Columns[4].HeaderText = wtList["HealthFacilityBalanceUsed"];
                gvHealthFacilityBalance.Columns[5].HeaderText = wtList["HealthFacilityBalanceWasted"];
                gvHealthFacilityBalance.Columns[6].HeaderText = wtList["HealthFacilityBalanceBalance"];

                //message
                this.lblWarning.Text = wtList["HealthFacilityBalanceWarningTextNoRecords"];

                //buttons
                //this.btnAdd.Text = wtList["HealthFacilityBalanceAddButton"];
                //this.btnEdit.Text = wtList["HealthFacilityBalanceEditButton"];
                this.btnExcel.Text = wtList["HealthFacilityBalanceExcelButton"];
                this.btnPrint.Text=wtList["HealthFacilityBalancePrintButton"];

                //Page Title
                this.lblTitle.Text = wtList["HealthFacilityBalancePageTitle"];

                int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;

                string _hfId = (string)Request.QueryString["hfId"];
                if (!String.IsNullOrEmpty(_hfId))
                    int.TryParse(_hfId, out hfId);
                
                odsHealthFacilityBalance.SelectParameters.Clear();
                odsHealthFacilityBalance.SelectParameters.Add("hfId", hfId.ToString());
                odsHealthFacilityBalance.DataBind();
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    protected void gvHealthFacilityBalance_DataBound(object sender, EventArgs e)
    {
        if (gvHealthFacilityBalance.Rows.Count <= 0)
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

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=HealthFacilityBalance.xls");
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

        string queryString = "PrintHealthFacilityBalance.aspx?hfId=" + hfId;
        string newWin = "window.open('" + queryString + "');";
        ClientScript.RegisterStartupScript(this.GetType(), "pop", newWin, true);
        HttpContext.Current.Session["hfId"] = null;
    }
}
