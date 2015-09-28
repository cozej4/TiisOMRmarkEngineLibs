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
using GIIS.DataLayer;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_LoggedUser : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string language = CurrentEnvironment.Language;
        int languageId = int.Parse(language);
        Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["LoggedUser-dictionary" + language];
        if (wtList == null)
        {
            List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "LoggedUser");
            wtList = new Dictionary<string, string>();
            foreach (WordTranslate vwt in wordTranslateList)
                wtList.Add(vwt.Code, vwt.Name);
            HttpContext.Current.Cache.Insert("LoggedUser-dictionary" + language, wtList);
        }
        this.lblUser.Text = wtList["LoggedUserUser"];
        this.lblHealthFacility.Text = wtList["LoggedUserHealthFacility"];
        this.lnkUserProfile.Text = wtList["LoggedUserChangePassword"];
        this.lnkLogout.Text = wtList["LoggedUserLogOut"];

        if (CurrentEnvironment.LoggedUser != null)
        {
            this.lblLoggedUser.Text = CurrentEnvironment.LoggedUser.Username;
            lblCurrentHealthFacility.Text = CurrentEnvironment.LoggedUser.HealthFacility.Name;
        }
        else
            this.pnlLoggedId.Visible = false;

    }
    protected void lnkLogout_Click(object sender, System.EventArgs e)
    {
        CurrentEnvironment.LoggedUser = null;
        // FormsAuthentication.SignOut();

        // Clear the output cache cookie...by expiring it
        HttpContext.Current.Response.Cookies["UserId"].Expires = DateTime.Now.AddDays(-1);
        HttpContext.Current.Session.Abandon();

        Response.Redirect(this.ResolveUrl("~/Default.aspx"), false);
    }

    protected void lnkUserProfile_Click(object sender, System.EventArgs e)
    {
        if (CurrentEnvironment.LoggedUser != null)
            Response.Redirect(this.ResolveUrl(string.Format("~/Pages/EditUser.aspx?id={0}", CurrentEnvironment.LoggedUser.Id)), false);
    }
}