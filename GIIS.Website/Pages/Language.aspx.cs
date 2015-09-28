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
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _Language : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewLanguage") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Language-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Language");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Language-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["LanguageName"];
                this.lblAbbrevation.Text = wtList["LanguageAbbrevation"];
                this.lblNotes.Text = wtList["LanguageNotes"];
                this.lblIsActive.Text = wtList["LanguageIsActive"];
                this.lblWritingDirection.Text = wtList["LanguageWritingDirection"];
                this.lblNameEnglish.Text = wtList["LanguageNameEnglish"];

                //grid header text
                gvLanguage.Columns[1].HeaderText = wtList["LanguageName"];
                gvLanguage.Columns[2].HeaderText = wtList["LanguageAbbrevation"];
                gvLanguage.Columns[5].HeaderText = wtList["LanguageNotes"];
                gvLanguage.Columns[6].HeaderText = wtList["LanguageIsActive"];
                gvLanguage.Columns[3].HeaderText = wtList["LanguageWritingDirection"];
                gvLanguage.Columns[4].HeaderText = wtList["LanguageNameEnglish"];
                gvExport.Columns[1].HeaderText = wtList["LanguageName"];
                gvExport.Columns[2].HeaderText = wtList["LanguageAbbrevation"];
                gvExport.Columns[5].HeaderText = wtList["LanguageNotes"];
                gvExport.Columns[6].HeaderText = wtList["LanguageIsActive"];
                gvExport.Columns[3].HeaderText = wtList["LanguageWritingDirection"];
                gvExport.Columns[4].HeaderText = wtList["LanguageNameEnglish"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddLanguage");
                this.btnEdit.Visible = actionList.Contains("EditLanguage");
                this.btnRemove.Visible = actionList.Contains("RemoveLanguage");

                //buttons
                this.btnAdd.Text = wtList["LanguageAddButton"];
                this.btnEdit.Text = wtList["LanguageEditButton"];
                this.btnRemove.Text = wtList["LanguageRemoveButton"];
                this.btnExcel.Text = wtList["LanguageExcelButton"];

                //message
                this.lblSuccess.Text = wtList["LanguageSuccessText"];
                this.lblWarning.Text = wtList["LanguageWarningText"];
                this.lblError.Text = wtList["LanguageErrorText"];

                //validators
                cvLanguage.ErrorMessage = wtList["LanguageMandatory"];

                //Page Title
                this.lblTitle.Text = wtList["LanguagePageTitle"];

                //selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    Language o = Language.GetLanguageById(id);
                    txtName.Text = o.Name;
                    txtNotes.Text = o.Notes;
                    txtAbbrevation.Text = o.Abbrevation;
                    txtNameEnglish.Text = o.NameEnglish;
                    txtWritingDirection.Text = o.WritingDirection;
                    rblIsActive.Items[0].Selected = o.IsActive;
                    rblIsActive.Items[1].Selected = !o.IsActive;
                    gridview_Databind();
                    btnAdd.Visible = false;
                }
                else
                {
                    // where =  "1 = 1";
                    gridview_Databind();
                    btnEdit.Visible = false;
                    btnRemove.Visible = false;
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    protected void gridview_Databind()
    {
        //gridview databind
        odsLanguage.SelectParameters.Clear();
        //odsLanguage.SelectParameters.Add("maximumRows", "10");
        //odsLanguage.SelectParameters.Add("startRowIndex", "0");
        odsLanguage.SelectParameters.Add("where", "1=1");
        odsLanguage.DataBind();
        gvLanguage.DataSource = odsLanguage;
        gvLanguage.DataBind();
        Session["Language-Where"] = "1 = 1";
    }
    protected bool AbbrevationExists(string abbr)
    {
        if (Language.GetLanguageByAbbrevation(abbr) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
    protected bool NameExists(string name)
    {
        if (Language.GetLanguageByName(name) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                if (AbbrevationExists(txtAbbrevation.Text.Replace("'", @"''")))
                    return;
                if (NameExists(txtName.Text.Replace("'", @"''")))
                    return;
                Language o = new Language();

                o.Name = txtName.Text.Replace("'", @"''");
                o.Abbrevation = txtAbbrevation.Text.Replace("'", @"''");
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                o.WritingDirection = txtWritingDirection.Text.Replace("'", @"''");
                o.NameEnglish = txtNameEnglish.Text.Replace("'", @"''");

                int i = Language.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gridview_Databind();
                    ClearControls(this);
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = true;
                    lblError.Visible = false;
                }

                //insert into word translate
                
                StringBuilder sb = new StringBuilder();
                List<WordTranslate> wtList = WordTranslate.GetWordByLanguage(1);
                foreach (WordTranslate wt in wtList)
                {
                    sb.AppendLine(string.Format(@" INSERT INTO ""WORD_TRANSLATE""(""LANGUAGE_ID"", ""CODE"", ""NAME"", ""PAGE_NAME"") VALUES ({0}, '{1}', '{2}', '{3}'); ", i, wt.Code, wt.Name, wt.PageName));
                }

                DBManager.ExecuteScalarCommand(sb.ToString(), CommandType.Text, null);


            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int id = -1;
                string _id = Request.QueryString["id"].ToString();
                int.TryParse(_id, out id);
                int userId = CurrentEnvironment.LoggedUser.Id;

                Language o = Language.GetLanguageById(id);
                if (AbbrevationExists(txtAbbrevation.Text.Replace("'", @"''")) && (o.Abbrevation != txtAbbrevation.Text.Trim()))
                    return;
                if (NameExists(txtName.Text.Replace("'", @"''")) && (o.Name != txtName.Text.Trim()))
                    return;
                o.Name = txtName.Text.Replace("'", @"''");
                o.Abbrevation = txtAbbrevation.Text.Replace("'", @"''");
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                o.WritingDirection = txtWritingDirection.Text.Replace("'", @"''");
                o.NameEnglish = txtNameEnglish.Text.Replace("'", @"''");

                int i = Language.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gridview_Databind();
                    //ClearControls(this);
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = true;
                    lblError.Visible = false;
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

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            int id = -1;
            string _id = Request.QueryString["id"].ToString();
            int.TryParse(_id, out id);
            int userId = CurrentEnvironment.LoggedUser.Id;

            int i = Language.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gridview_Databind();
                //ClearControls(this);
            }
            else
            {
                lblSuccess.Visible = false;
                lblWarning.Visible = true;
                lblError.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }

    protected void gvLanguage_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLanguage.PageIndex = e.NewPageIndex;
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

    protected void gvLanguage_DataBound(object sender, System.EventArgs e)
    {
        if (gvLanguage.Rows.Count > 0)
            btnExcel.Visible = true;
        else
            btnExcel.Visible = true;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        if (Session["Language-Where"] != null)
        {
            string where = Session["Language-Where"].ToString();
            if (!string.IsNullOrEmpty(where))
            {
                int maximumRows = int.MaxValue;
                int startRowIndex = 0;

                //List<Language> list = Language.GetPagedLanguageList(ref maximumRows, ref startRowIndex, where);
                //gvExport.DataSource = list;
                //gvExport.DataBind();
                //gvExport.Visible = true;
            }

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Language.xls");
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
