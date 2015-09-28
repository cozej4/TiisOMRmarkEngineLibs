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

public partial class Pages_UserList : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewUserList") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language); //Language.GetLanguageByName(language).Id;
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["User-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "User");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("User-dictionary" + language, wtList);
                }

                //controls
                this.lblUsername.Text = wtList["UserUsername"];

                //actions
                //this.btnSearch.Visible = actionList.Contains("SearchUsers");
                this.btnAddNew.Visible = actionList.Contains("AddUser");

                //buttons
                this.btnSearch.Text = wtList["UserSearchButton"];
                this.btnAddNew.Text = wtList["UserAddNewButton"];
                this.btnExcel.Text = wtList["UserExcelButton"];

                //message
                this.lblWarningSearch.Text = wtList["UserSearchWarningText"];

                //Page Title
                this.lblTitle.Text = wtList["UserListPageTitle"];

                gvUser.Columns[1].HeaderText = wtList["UserUsername"];
                gvUser.Columns[3].HeaderText = wtList["UserFirstname"];
                gvUser.Columns[4].HeaderText = wtList["UserLastname"];
                gvUser.Columns[7].HeaderText = wtList["UserIsActive"];
                gvUser.Columns[6].HeaderText = wtList["UserNotes"];
                gvUser.Columns[5].HeaderText = wtList["UserEmail"];
                gvUser.Columns[2].HeaderText = wtList["UserHealthFacility"];

                gvExport.Columns[1].HeaderText = wtList["UserUsername"];
                gvExport.Columns[3].HeaderText = wtList["UserFirstname"];
                gvExport.Columns[4].HeaderText = wtList["UserLastname"];
                gvExport.Columns[7].HeaderText = wtList["UserIsActive"];
                gvExport.Columns[6].HeaderText = wtList["UserNotes"];
                gvExport.Columns[5].HeaderText = wtList["UserEmail"];
                gvExport.Columns[2].HeaderText = wtList["UserHealthFacility"];

              
                int hcId = CurrentEnvironment.LoggedUser.HealthFacilityId;
             
                odsUser.SelectParameters.Clear();
                odsUser.SelectParameters.Add("username", string.Empty);
                odsUser.SelectParameters.Add("hfid", hcId.ToString());

                Session["UserList-username"] = string.Empty;
                Session["UserList-HfId"] = hcId;
            }
            else
            {
                Response.Redirect("Default.aspx", false);
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string wsearch = txtUsername.Text.Replace("'", @"''");
        
        int hcId = CurrentEnvironment.LoggedUser.HealthFacilityId;
       
        odsUser.SelectParameters.Clear();
        odsUser.SelectParameters.Add("username", wsearch.ToUpper());
        odsUser.SelectParameters.Add("hfid", hcId.ToString());

        Session["UserList-username"] = wsearch.ToUpper();
        Session["UserList-HfId"] = hcId;
    }

    protected void gvUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUser.PageIndex = e.NewPageIndex;
    }
    protected void gvUser_Databound(object sender, System.EventArgs e)
    {
        if (gvUser.Rows.Count <= 0)
        {
            lblWarningSearch.Visible = true;
            btnExcel.Visible=false;}
        else{
            btnExcel.Visible = true;
            lblWarningSearch.Visible = false;
    }
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("User.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string username = "";
        string hfid = "";

        if (Session["UserList-username"] != null)
            username = Session["UserList-username"].ToString();

        if (Session["UserList-HfId"] != null)
            hfid = Session["UserList-HfId"].ToString();

        int maximumRows = int.MaxValue;
        int startRowIndex = 0;

        List<GIIS.DataLayer.User> list = GIIS.DataLayer.User.GetPagedUserList(username, int.Parse(hfid), ref maximumRows, ref startRowIndex);
        gvExport.DataSource = list;
        gvExport.DataBind();
        gvExport.Visible = true;

        if (list.Count >= 1)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=UserList.xls");
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