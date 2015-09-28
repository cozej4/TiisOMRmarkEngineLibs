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


public partial class _NonvaccinationReason : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewNonvaccinationReason") && (CurrentEnvironment.LoggedUser != null))
            { 
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["NonvaccinationReason-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "NonvaccinationReason");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("NonvaccinationReason-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["NonvaccinationReasonName"];
                this.lblCode.Text = wtList["NonvaccinationReasonCode"];
                this.lblIsActive.Text = wtList["NonvaccinationReasonIsActive"];
                this.lblNotes.Text = wtList["NonvaccinationReasonNotes"];
               // this.lblKeepChildDue.Text = wtList["NonvaccinationReasonKeepChildDue"];

                //grid header text
                gvNonvaccinationReason.Columns[1].HeaderText = wtList["NonvaccinationReasonName"];
                gvNonvaccinationReason.Columns[2].HeaderText = wtList["NonvaccinationReasonCode"];
               // gvNonvaccinationReason.Columns[3].HeaderText = wtList["NonvaccinationReasonKeepChildDue"];
                gvNonvaccinationReason.Columns[5].HeaderText = wtList["NonvaccinationReasonIsActive"];
                gvNonvaccinationReason.Columns[4].HeaderText = wtList["NonvaccinationReasonNotes"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddNonvaccinationReason");
                this.btnEdit.Visible = actionList.Contains("EditNonvaccinationReason");
                this.btnRemove.Visible = actionList.Contains("RemoveNonvaccinationReason");

                //buttons
                this.btnAdd.Text = wtList["NonvaccinationReasonAddButton"];
                this.btnEdit.Text = wtList["NonvaccinationReasonEditButton"];
                this.btnRemove.Text = wtList["NonvaccinationReasonRemoveButton"];
                this.btnExcel.Text=wtList["NonvaccinationReasonExcelButton"];

                //message
                this.lblSuccess.Text = wtList["NonvaccinationReasonSuccessText"];
                this.lblWarning.Text = wtList["NonvaccinationReasonWarningText"];
                this.lblError.Text = wtList["NonvaccinationReasonErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["NonvaccinationReasonPageTitle"];

                //selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    NonvaccinationReason o = NonvaccinationReason.GetNonvaccinationReasonById(id);
                    txtName.Text = o.Name;
                    rblKeepChildDue.SelectedValue = o.KeepChildDue.ToString();
                    txtCode.Text = o.Code;
                    txtNotes.Text = o.Notes;
                    rblIsActive.Items[0].Selected = o.IsActive;
                    rblIsActive.Items[1].Selected = !o.IsActive;
                    btnAdd.Visible = false;
                }
                else
                {
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


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                if (nameExists(txtName.Text.Replace("'", @"''")))
                    return;
                NonvaccinationReason o = new NonvaccinationReason();

                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.KeepChildDue = bool.Parse(rblKeepChildDue.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                o.Notes = txtNotes.Text.Replace("'", @"''");

                int i = NonvaccinationReason.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvNonvaccinationReason.DataBind();
                    ClearControls(this);
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = false;
                    lblError.Visible = true;
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

                NonvaccinationReason o = NonvaccinationReason.GetNonvaccinationReasonById(id);
                if (nameExists(txtName.Text.Replace("'", @"''")) && (o.Name != txtName.Text.Trim()))
                    return;
                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text.Replace("'", @"''");
                o.KeepChildDue = bool.Parse(rblKeepChildDue.SelectedValue);
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                o.Notes = txtNotes.Text.Replace("'", @"''");

                int i = NonvaccinationReason.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvNonvaccinationReason.DataBind();
                    //ClearControls(this);
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = false;
                    lblError.Visible = true;
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

            int i = NonvaccinationReason.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gvNonvaccinationReason.DataBind();
                ClearControls(this);
            }
            else
            {
                lblSuccess.Visible = false;
                lblWarning.Visible = false;
                lblError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }

    protected void gvNonvaccinationReason_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNonvaccinationReason.PageIndex = e.NewPageIndex;
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

    protected bool nameExists(string name)
    {
        if (NonvaccinationReason.GetNonvaccinationReasonByName(name) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }

    protected void gvNonvaccinationReason_DataBound(object sender, EventArgs e)
    {
        if (gvNonvaccinationReason.Rows.Count > 0)
            btnExcel.Visible = true;
        else
            btnExcel.Visible = false;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=NonVaccinationReason.xls");
        Response.Charset = "";

        // If you want the option to open the Excel file without saving then
        // comment out the line below
        // Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/ms-excel";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gvNonvaccinationReason.RenderControl(htmlWrite);
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
}
