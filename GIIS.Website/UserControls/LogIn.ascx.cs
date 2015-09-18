using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using GIIS.DataLayer;

public partial class UserControls_LogIn : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            string language = CurrentEnvironment.Language;
            int languageId = int.Parse(language);
            Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["LogIn-dictionary" + language];
            if (wtList == null)
            {
                List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "LogIn");
                wtList = new Dictionary<string, string>();
                foreach (WordTranslate vwt in wordTranslateList)
                    wtList.Add(vwt.Code, vwt.Name);
                HttpContext.Current.Cache.Insert("LogIn-dictionary" + language, wtList);
            }

            this.btnLogIn.Text = wtList["LogInButton"];
            this.lblWrongPassword.Text = wtList["LogInWrongCredentials"];
            //this.rfvUsername.ErrorMessage = wtList["LogInUsernameRequired"];
            //this.rfvPassword.ErrorMessage = wtList["LogInPasswordRequired"];

            //if (CurrentEnvironment.LoggedUser != null)
            //{
            //    pnlLogin.Visible = false;
            //   FormsAuthentication.RedirectFromLoginPage("/Pages/Default.aspx", false);
            //}
            //else
            //    pnlLogin.Visible = true;
            ddlLanguage.SelectedValue = CurrentEnvironment.Language;
        }
    }
    protected void LoginButton_Click(object sender, System.EventArgs e)
    {
        if (this.Page.IsValid)
        {
            lblWrongPassword.Visible = false;
            CurrentEnvironment.LoggedUser = GIIS.DataLayer.User.GetDataByUsernameAndPassword(this.txtUsername.Text, this.txtPassword.Text);
            if (CurrentEnvironment.LoggedUser != null)
            {
                CurrentEnvironment.LoggedUser.Isloggedin = true;
                CurrentEnvironment.LoggedUser.PrevLogin = CurrentEnvironment.LoggedUser.Lastlogin;
                CurrentEnvironment.LoggedUser.Lastlogin = DateTime.Now;
                User.Update(CurrentEnvironment.LoggedUser);
                FormsAuthentication.RedirectFromLoginPage(this.txtUsername.Text, false);
            }
            else
            {
                lblWrongPassword.Visible = true;
            }
        }
    }
    protected void ddlLanguages_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["__language"] = ddlLanguage.SelectedValue;
    }
    //protected void btnForgotPassword_Click(object sender, EventArgs e)
    //{
    //    string url = string.Format("ResetUserPassword.aspx");
    //    Response.Redirect(url, false);
    //    Context.ApplicationInstance.CompleteRequest();
    //}
}