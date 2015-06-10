using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _Menu : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewMenu") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Menu-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Menu");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Menu-dictionary" + language, wtList);
                }

                //controls
                this.lblParentId.Text = wtList["MenuParent"];
                this.lblTitle.Text = wtList["MenuTitle"];
                this.lblNavigateUrl.Text = wtList["MenuNavigateUrl"];
                this.lblIsActive.Text = wtList["MenuIsActive"];
                this.lblDisplayOrder.Text = wtList["MenuDisplayOrder"];

                //grid header text
                gvMenu.Columns[1].HeaderText = wtList["MenuParent"];
                gvMenu.Columns[2].HeaderText = wtList["MenuTitle"];
                gvMenu.Columns[3].HeaderText = wtList["MenuNavigateUrl"];
                gvMenu.Columns[4].HeaderText = wtList["MenuIsActive"];
                gvMenu.Columns[5].HeaderText = wtList["MenuDisplayOrder"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddMenu");
                this.btnEdit.Visible = actionList.Contains("EditMenu");
                this.btnRemove.Visible = actionList.Contains("RemoveMenu");

                //buttons
                this.btnAdd.Text = wtList["MenuAddButton"];
                this.btnEdit.Text = wtList["MenuEditButton"];
                this.btnRemove.Text = wtList["MenuRemoveButton"];

                //message
                this.lblSuccess.Text = wtList["MenuSuccessText"];
                this.lblWarning.Text = wtList["MenuWarningText"];
                this.lblError.Text = wtList["MenuErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["MenuPageTitle"];
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

                GIIS.DataLayer.Menu o = new GIIS.DataLayer.Menu();

                o.ParentId = int.Parse(txtParentId.Text);
                o.Title = txtTitle.Text.Replace("'", @"''");
                o.NavigateUrl = txtNavigateUrl.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.DisplayOrder = int.Parse(txtDisplayOrder.Text);

                int i = GIIS.DataLayer.Menu.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvMenu.DataBind();
                    ClearControls(this);
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

                GIIS.DataLayer.Menu o = GIIS.DataLayer.Menu.GetMenuById(id);

                o.ParentId = int.Parse(txtParentId.Text);
                o.Title = txtTitle.Text.Replace("'", @"''");
                o.NavigateUrl = txtNavigateUrl.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.DisplayOrder = int.Parse(txtDisplayOrder.Text);

                int i = GIIS.DataLayer.Menu.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvMenu.DataBind();
                    ClearControls(this);
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

            int i = GIIS.DataLayer.Menu.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gvMenu.DataBind();
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

    protected void gvMenu_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMenu.PageIndex = e.NewPageIndex;
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
}
