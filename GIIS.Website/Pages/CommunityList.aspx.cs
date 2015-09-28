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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.DataLayer;

public partial class _CommunityList : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewCommunityList") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Community-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Community");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Community-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["CommunityName"];

                //grid header text
                gvCommunity.Columns[1].HeaderText = wtList["CommunityName"];
                gvCommunity.Columns[3].HeaderText = wtList["CommunityIsActive"];
                gvCommunity.Columns[2].HeaderText = wtList["CommunityNotes"];

                gvExport.Columns[1].HeaderText = wtList["CommunityName"];
                gvExport.Columns[3].HeaderText = wtList["CommunityIsActive"];
                gvExport.Columns[2].HeaderText = wtList["CommunityNotes"];

                //actions
                this.btnAddNew.Visible = actionList.Contains("AddCommunity");
                //this.btnSearch.Visible = actionList.Contains("SearchCommunity");

                //buttons
                this.btnAddNew.Text = wtList["CommunityAddNewButton"];
                this.btnSearch.Text = wtList["CommunitySearchButton"];
                this.btnExcel.Text = wtList["CommunityExcelButton"];

                //Page Title
                this.lblTitle.Text = wtList["CommunityListPageTitle"];

                //message
                this.lblWarning.Text = wtList["CommunitySearchWarningText"];

                ////gridview databind
                //odsCommunity.SelectParameters.Clear();
                //odsCommunity.SelectParameters.Add("where", "1=1");
                //gvCommunity.DataSource = odsCommunity;
                //gvCommunity.DataBind();
                //lblWarning.Visible = false;
                //Session["CommunityList-Where"] = "1=1";
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
            odsCommunity.SelectParameters.Clear();
            odsCommunity.SelectParameters.Add("name", wsearch);
            Session["CommunityList-Name"] = wsearch;
        }
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("Community.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }
    protected void gvCommunity_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCommunity.PageIndex = e.NewPageIndex;
    }
    protected void gvCommunity_DataBound(object sender, System.EventArgs e)
    {
        if (gvCommunity.Rows.Count <= 0)
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
        if (Session["PlaceList-Name"] != null)
        {
            string name = Session["PlaceList-Name"].ToString();
            if (!string.IsNullOrEmpty(name))
            {
                int maximumRows = int.MaxValue;
                int startRowIndex = 0;

                List<Community> list = Community.GetPagedCommunityList(name, ref maximumRows, ref startRowIndex);
                gvExport.DataSource = list;
                gvExport.DataBind();
                gvExport.Visible = true;
            }

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=CommunityList.xls");
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
}