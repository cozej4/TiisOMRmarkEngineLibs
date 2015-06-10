using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.DataLayer;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string str = Request.ServerVariables.Get("SCRIPT_NAME");
        int index = str.LastIndexOf("/");
        string page = str.Substring(index + 1);
        string language = CurrentEnvironment.Language;
        int languageId = int.Parse(language);


        if (CurrentEnvironment.LoggedUser != null)
            Response.Redirect("/Pages/Default.aspx", false);


    }
   
    
}
