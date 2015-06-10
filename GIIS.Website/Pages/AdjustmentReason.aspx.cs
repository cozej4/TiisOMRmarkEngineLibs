using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _AdjustmentReason : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewAdjustmentReason") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["AdjustmentReason-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "AdjustmentReason");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("AdjustmentReason-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["AdjustmentReasonName"];
                this.lblIsActive.Text = wtList["AdjustmentReasonIsActive"];
                this.lblNotes.Text = wtList["AdjustmentReasonNotes"];
                this.lblPositive.Text = wtList["AdjustmentReasonPositive"];

                //grid header text
                gvAdjustmentReason.Columns[1].HeaderText = wtList["AdjustmentReasonName"];
                gvAdjustmentReason.Columns[2].HeaderText = wtList["AdjustmentReasonPositive"];
                gvAdjustmentReason.Columns[3].HeaderText = wtList["AdjustmentReasonNotes"];
                gvAdjustmentReason.Columns[4].HeaderText = wtList["AdjustmentReasonIsActive"];
                gvExport.Columns[1].HeaderText = wtList["AdjustmentReasonName"];
                gvExport.Columns[2].HeaderText = wtList["AdjustmentReasonPositive"];
                gvExport.Columns[3].HeaderText = wtList["AdjustmentReasonNotes"];
                gvExport.Columns[4].HeaderText = wtList["AdjustmentReasonIsActive"];
                //actions
                this.btnAdd.Visible = actionList.Contains("AddAdjustmentReason");
                this.btnEdit.Visible = actionList.Contains("EditAdjustmentReason");
                this.btnRemove.Visible = actionList.Contains("RemoveAdjustmentReason");

                //buttons
                this.btnAdd.Text = wtList["AdjustmentReasonAddButton"];
                this.btnEdit.Text = wtList["AdjustmentReasonEditButton"];
                this.btnRemove.Text = wtList["AdjustmentReasonRemoveButton"];
                this.btnExcel.Text = wtList["AdjustmentReasonExcelButton"];

                //message
                this.lblSuccess.Text = wtList["AdjustmentReasonSuccessText"];
                this.lblWarning.Text = wtList["AdjustmentReasonWarningText"];
                this.lblError.Text = wtList["AdjustmentReasonErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["AdjustmentReasonPageTitle"];

                //validators
               rfvName.ErrorMessage = wtList["AdjustmentReasonMandatory"];
                //selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    AdjustmentReason o = AdjustmentReason.GetAdjustmentReasonById(id);
                    txtName.Text = o.Name;
                    txtNotes.Text = o.Notes;
                    rblIsActive.Items[0].Selected = o.IsActive;
                    rblIsActive.Items[1].Selected = !o.IsActive;
                    btnAdd.Visible = false;
                    chkPositive.Checked = o.Positive;
                }
                else
                {
                    btnEdit.Visible = false;
                    btnRemove.Visible = false;
                    Session["AdjustmentReason-Where"] = "1 = 1";
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

                AdjustmentReason o = new AdjustmentReason();

                o.Name = txtName.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                o.Positive = chkPositive.Checked;
                int i = AdjustmentReason.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvAdjustmentReason.DataBind();
                    Session["AdjustmentReason-Where"] = "1 = 1";
                    ClearControls(this);
                    chkPositive.Checked = false;
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

                AdjustmentReason o = AdjustmentReason.GetAdjustmentReasonById(id);

                o.Name = txtName.Text.Replace("'", @"''");
                if (id == 99)
                    o.IsActive = true;
                else
                    o.IsActive = bool.Parse(rblIsActive.SelectedValue);

                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                o.Positive = chkPositive.Checked;

                int i = AdjustmentReason.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvAdjustmentReason.DataBind();
                    Session["AdjustmentReason-Where"] = "1 = 1";
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
            int i = 0;
            if (id != 99)
                i = AdjustmentReason.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gvAdjustmentReason.DataBind();
                Session["AdjustmentReason-Where"] = "1 = 1";
                ClearControls(this);
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

    protected void gvAdjustmentReason_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAdjustmentReason.PageIndex = e.NewPageIndex;
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

    protected void gvAdjustmentReason_DataBound(object sender, System.EventArgs e)
    {
        if (gvAdjustmentReason.Rows.Count > 0)
            btnExcel.Visible = true;
        else
            btnExcel.Visible = false;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        if (Session["AdjustmentReason-Where"] != null)
        {
            string where = Session["AdjustmentReason-Where"].ToString();
            if (!string.IsNullOrEmpty(where))
            {
                int maximumRows = int.MaxValue;
                int startRowIndex = 0;

                //List<AdjustmentReason> list = AdjustmentReason.GetPagedAdjustmentReasonList(ref maximumRows, ref startRowIndex, where);
                //gvExport.DataSource = list;
                //gvExport.DataBind();
                //gvExport.Visible = true;
            }

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=AdjustmentReason.xls");
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
