using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _WordTranslate : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewWordTranslate") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["WordTranslate-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "WordTranslate");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("WordTranslate-dictionary" + language, wtList);
                }

               // controls
                this.lblLanguageId.Text = wtList["WordTranslateLanguage"];
                //this.lblCode.Text = wtList["WordTranslateCode"];
                this.lblPageName.Text = wtList["WordTranslatePageName"];

                //grid header text
                gvWordTranslate.Columns[1].HeaderText = wtList["WordTranslateCode"];
                gvWordTranslate.Columns[2].HeaderText = wtList["WordTranslateWord"];
                gvWordTranslate.Columns[3].HeaderText = wtList["WordTranslateName"];


               // message
                this.lblSuccess.Text = wtList["WordTranslateSuccessText"];
                this.lblError.Text = wtList["WordTranslateErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["WordTranslatePageTitle"];
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindWordTranslate();
    }

    protected void gvWordTranslate_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvWordTranslate.PageIndex = e.NewPageIndex;
    }

    protected void gvWordTranslate_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvWordTranslate.EditIndex = e.NewEditIndex;
        BindWordTranslate();
        lblSuccess.Visible = false;
        lblError.Visible = false;
    }

    protected void gvWordTranslate_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int index = gvWordTranslate.EditIndex;
        GridViewRow row = gvWordTranslate.Rows[index];
        Label txtId = (Label)gvWordTranslate.Rows[e.RowIndex].FindControl("txtId");
        TextBox txtName = (TextBox)gvWordTranslate.Rows[e.RowIndex].FindControl("txtName");

        WordTranslate o = WordTranslate.GetWordTranslateById(int.Parse(txtId.Text));
        o.Name = txtName.Text;
        int i = WordTranslate.Update(o);

        if (i > 0)
        {
            lblSuccess.Visible = true;
            lblError.Visible = false;
        }
        else
        {
            lblSuccess.Visible = false;
            lblError.Visible = true;
        }

        gvWordTranslate.EditIndex = -1;
        BindWordTranslate();
    }

    protected void gvWordTranslate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvWordTranslate.EditIndex = -1;
        BindWordTranslate();
    }

    private void BindWordTranslate()
    {
        string where = string.Format(@" ""LANGUAGE_ID"" = '{0}' AND ""PAGE_NAME"" = '{1}' ", ddlLanguages.SelectedValue, ddlPage.SelectedValue);
        //if (!string.IsNullOrEmpty(txtCode.Text))
        //    where += string.Format(@" AND UPPER(""CODE"") LIKE '%{0}%' ", txtCode.Text.ToUpper().Replace("'", @"''"));
        where += string.Format(@" ORDER BY ""CODE"" ");
        List<WordTranslate> wtList = WordTranslate.GetWordTranslateList(where);

        gvWordTranslate.DataSource = wtList;
        gvWordTranslate.DataBind();
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
                if (c is DropDownList)
                    (c as DropDownList).SelectedIndex = 0;
            }
        }
    }
    protected void ddlPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindWordTranslate();
    }
    protected void ddlLanguages_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindWordTranslate();
    }
}