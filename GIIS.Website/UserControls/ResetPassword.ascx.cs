using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using GIIS.DataLayer;

public partial class UserControls_ResetPassword : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            string language = CurrentEnvironment.Language;
            int languageId = int.Parse(language);
            Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["ResetPassword-dictionary" + language];
            if (wtList == null)
            {
                List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ResetPassword");
                wtList = new Dictionary<string, string>();
                foreach (WordTranslate vwt in wordTranslateList)
                    wtList.Add(vwt.Code, vwt.Name);
                HttpContext.Current.Cache.Insert("ResetPassword-dictionary" + language, wtList);
            }

            ////controls            
            //this.lblUsernameNotFound.Text = wtList["ResetPasswordUsernameNotFound"];
            //this.lblSuccess.Text = wtList["ResetPasswordSuccess"];
            //this.lblWarning.Text = wtList["ResetPasswordWarning"];
            //this.lblError.Text = wtList["ResetPasswordError"];

            ////actions
            //this.btnResetPassword.Text = wtList["ResetPasswordButton"];

            ////validators
            //cvRequiredFields.ErrorMessage = wtList["ResetPasswordMandatoryFields"];
            //revNewPassword.ErrorMessage = wtList["ResetPasswordPasswordValidator"];
            //cvPasswordMatch.ErrorMessage = wtList["ResetPasswordPasswordMatch"];
        }
    }

    protected void btnResetPassword_Click(object sender, System.EventArgs e)
    {
        try
        {
            if (this.Page.IsValid)
            {
                User user = null;
                if (!String.IsNullOrEmpty(this.txtUsername.Text.Trim().Replace("'", @"''")))
                {
                    user = User.GetByUsername(this.txtUsername.Text.Trim().Replace("'", @"''"));
                    if (user != null)
                    {
                        user.Password = Helper.ComputeHash(this.txtNewPassword.Text.Replace("'", @"''"));
                        int i = User.Update(user);
                        if (i > 0)
                        {
                            this.lblSuccess.Visible = true;
                            this.lblWarning.Visible = false;
                            this.lblError.Visible = false;
                            lblUsernameExists.Visible = false;
                            ClearControls(this);

                            Page.ClientScript.RegisterStartupScript(this.GetType(), "ResetPass", "alert('Password reset successful !');window.location = 'Default.aspx';", true);

                            //FormsAuthentication.RedirectToLoginPage();
                        }
                        else
                        {
                            this.lblSuccess.Visible = false;
                            this.lblWarning.Visible = true;
                            this.lblError.Visible = false;
                            lblUsernameExists.Visible = false;
                        }
                    }
                    else
                    {
                        lblUsernameExists.Visible = true;
                        this.lblSuccess.Visible = false;
                        this.lblWarning.Visible = false;
                        this.lblError.Visible = false;
                    }
                }
            }
        }
        catch(Exception ex)
        {   
            this.lblSuccess.Visible = false;
            this.lblWarning.Visible = false;
            this.lblError.Visible = true;
            lblUsernameExists.Visible = false;
        }
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
                //if (c is DropDownList)
                //    (c as DropDownList).SelectedIndex = 0;
            }
        }
    }
}