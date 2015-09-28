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

public partial class Pages_ManufacturerList : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewManufacturerList") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Manufacturer-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Manufacturer");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Manufacturer-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["ManufacturerName"];
                this.lblCode.Text = wtList["ManufacturerCode"];


                //grid header text
                gvManufacturer.Columns[1].HeaderText = wtList["ManufacturerName"];
                gvManufacturer.Columns[2].HeaderText = wtList["ManufacturerCode"];
                //gvManufacturer.Columns[3].HeaderText = wtList["ManufacturerHl7Manufacturer"];
                gvManufacturer.Columns[4].HeaderText = wtList["ManufacturerIsActive"];
                gvManufacturer.Columns[3].HeaderText = wtList["ManufacturerNotes"];

                gvExport.Columns[1].HeaderText = wtList["ManufacturerName"];
                gvExport.Columns[2].HeaderText = wtList["ManufacturerCode"];
                //gvExport.Columns[3].HeaderText = wtList["ManufacturerHl7Manufacturer"];
                gvExport.Columns[4].HeaderText = wtList["ManufacturerIsActive"];
                gvExport.Columns[3].HeaderText = wtList["ManufacturerNotes"];

                //buttons
                this.btnSearch.Text = wtList["ManufacturerSearchButton"];
                this.btnAddNew.Text = wtList["ManufacturerAddNewButton"];
                this.btnExcel.Text = wtList["ManufacturerExcelButton"];

                //message
                this.lblWarning.Text = wtList["ManufacturerSearchWarningText"];

                //Page Title
                this.lblTitle.Text = wtList["ManufacturerListPageTitle"];

                odsManufacturer.SelectParameters.Clear();
                odsManufacturer.SelectParameters.Add("name", "");
                odsManufacturer.SelectParameters.Add("code", "");
                gvManufacturer.DataSourceID = "odsManufacturer";
                gvManufacturer.DataBind();
                lblWarning.Visible = false;

                Session["ManufacturerList-Name"] = "";
                Session["ManufacturerList-Code"] = "";
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("Manufacturer.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }
    protected void gvManufacturer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvManufacturer.PageIndex = e.NewPageIndex;
    }
    protected void gvManufacturer_DataBound(object sender, EventArgs e)
    {
        if (gvManufacturer.Rows.Count == 0)
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string name = txtName.Text.Replace("'", @"''");
        string code = txtCode.Text.Replace("'", @"''");

        odsManufacturer.SelectParameters.Clear();
        odsManufacturer.SelectParameters.Add("name", name.ToUpper());
        odsManufacturer.SelectParameters.Add("code", code.ToUpper());
        gvManufacturer.DataSourceID = "odsManufacturer";
        gvManufacturer.DataBind();

        Session["ManufacturerList-Name"] = name.ToUpper();
        Session["ManufacturerList-Code"] = code.ToUpper();
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string name = "";
        string code = "";

        if (Session["ManufacturerList-Name"] != null)
            name = Session["ManufacturerList-Name"].ToString();
        if (Session["ManufacturerList-Code"] != null)
            code = Session["ManufacturerList-Code"].ToString();

        int maximumRows = int.MaxValue;
        int startRowIndex = 0;

        List<Manufacturer> list = Manufacturer.GetPagedManufacturerList(name, code, ref maximumRows, ref startRowIndex);
        gvExport.DataSource = list;
        gvExport.DataBind();
        gvExport.Visible = true;

        if (list.Count >= 1)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=ManufacturerList.xls");
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


