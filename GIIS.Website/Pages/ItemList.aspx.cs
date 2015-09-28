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
using System.Globalization;

public partial class _ItemList : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewItem") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Item-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Item");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Item-dictionary" + language, wtList);
                }

                //controls
                this.lblItemCategoryId.Text = wtList["ItemItemCategory"];
                this.lblName.Text = wtList["ItemName"];

                //grid header text
                gvItem.Columns[1].HeaderText = wtList["ItemName"];
                gvItem.Columns[2].HeaderText = wtList["ItemCode"];
                gvItem.Columns[3].HeaderText = wtList["ItemItemCategory"];
                //gvItem.Columns[4].HeaderText = wtList["ItemHl7Vaccine"];
                gvItem.Columns[4].HeaderText = wtList["ItemEntryDate"];
                gvItem.Columns[5].HeaderText = wtList["ItemExitDate"];
                gvItem.Columns[7].HeaderText = wtList["ItemIsActive"];
                gvItem.Columns[6].HeaderText = wtList["ItemNotes"];

                gvExport.Columns[1].HeaderText = wtList["ItemName"];
                gvExport.Columns[2].HeaderText = wtList["ItemCode"];
                gvExport.Columns[3].HeaderText = wtList["ItemItemCategory"];
                gvExport.Columns[4].HeaderText = wtList["ItemHl7Vaccine"];
                gvExport.Columns[5].HeaderText = wtList["ItemEntryDate"];
                gvExport.Columns[6].HeaderText = wtList["ItemExitDate"];
                gvExport.Columns[8].HeaderText = wtList["ItemIsActive"];
                gvExport.Columns[7].HeaderText = wtList["ItemNotes"];

                //actions
                this.btnAddNew.Visible = actionList.Contains("AddItem");
                //this.btnSearch.Visible = actionList.Contains("SearchItems");

                //buttons
                this.btnAddNew.Text = wtList["ItemAddNewButton"];
                this.btnSearch.Text = wtList["ItemSearchButton"];
                this.btnExcel.Text = wtList["ItemExcelButton"];

                //message
                this.lblWarning.Text = wtList["ItemWarningSearchText"];

                //Page Title
                this.lblTitle.Text = wtList["ItemListPageTitle"];

                //gridview databind
                odsItem.SelectParameters.Clear();
                odsItem.SelectParameters.Add("name", "");
                odsItem.SelectParameters.Add("itemCategoryId", "-1");
                gvItem.DataSourceID = "odsItem";
                gvItem.DataBind();
                lblWarning.Visible = false;

                Session["Item-Name"] = "";
                Session["Item-ItemCategoryId"] = "-1";
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string Nsearch = txtName.Text.Replace("'", @"''");
        string Csearch = ddlItemCategory.SelectedValue;

        odsItem.SelectParameters.Clear();
        odsItem.SelectParameters.Add("name", Nsearch.ToUpper());
        odsItem.SelectParameters.Add("itemCategoryId", Csearch);
        gvItem.DataSourceID = "odsItem";
        gvItem.DataBind();

        Session["Item-Name"] = Nsearch;
        Session["Item-ItemCategoryId"] = Csearch;
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("Item.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }
    protected void gvItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvItem.PageIndex = e.NewPageIndex;
    }

    protected void gvItem_Databound(object sender, EventArgs e)
    {

       if (gvItem.Rows.Count <= 0)
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
        string name = "";
        int itemCategoryId = -1;

        if (Session["Item-Name"] != null)
            name = Session["Item-Name"].ToString();
        if (Session["Item-ItemCategoryId"] != null)
            itemCategoryId = int.Parse(Session["Item-ItemCategoryId"].ToString());

        int maximumRows = int.MaxValue;
        int startRowIndex = 0;

        List<Item> list = Item.GetPagedItemList(name, itemCategoryId, ref maximumRows, ref startRowIndex);
        gvExport.DataSource = list;
        gvExport.DataBind();
        gvExport.Visible = true;

        if (list.Count >= 1)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Items.xls");
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
   
    protected void gvItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string exitDate = e.Row.Cells[5].Text;
           // DateTime date = DateTime.ParseExact(exitDate, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
            DateTime date = DateTime.ParseExact(exitDate, "dd-MMM-yyyy", CultureInfo.CurrentCulture);
            if (date.ToString("yyyy-MM-dd").Equals("0001-01-01"))
                e.Row.Cells[5].Text = String.Empty;
        }
    }
}