using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

/// <summary>
/// Summary description for CurrentEnvironment
/// </summary>
public class CurrentEnvironment
{
    public CurrentEnvironment()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static GIIS.DataLayer.User LoggedUser
    {
        get
        {
            //return GIIS.DataLayer.User.GetUserById(1);
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["__LOGGED_USER"] != null)
                return HttpContext.Current.Session["__LOGGED_USER"] as GIIS.DataLayer.User;
            else
                return null;
        }
        set
        {
            HttpContext.Current.Session["__LOGGED_USER"] = value;
            HttpContext.Current.Session["__CURRENT_USER"] = value;

            if (value != null)
            {
                // Set the cookie used by the output cache...it's cleared on sign out.
                HttpContext.Current.Response.Cookies["UserId"].Value = value.Id.ToString();
            }
            else
            {
                // Clear the output cache cookie...by expiring it
                HttpContext.Current.Response.Cookies["UserId"].Expires = DateTime.Now.AddDays(-1);
            }
        }
    }

    public static string Language
    {
        get
        {
            //if (HttpContext.Current.Session["__language"] == null)
            //{
            //    HttpContext.Current.Session["__language"] = "English";
            //}
            //return HttpContext.Current.Session["__language"].ToString();
            if (HttpContext.Current.Session["__language"] == null)
            {
                HttpContext.Current.Session["__language"] = Configuration.GetConfigurationByName("Language").Value;
            }
            return HttpContext.Current.Session["__language"].ToString();
        }
    }

    /// <summary>
    /// To be used for in all page_load events of all pages to grant\access action within the page
    /// to the user.
    /// </summary>
    /// <param name="actionName">Name of page's action user wants to perform.</param>
    /// <returns>True when the page's action is loaded, and false otherwise</returns>
    /// <remarks>By 'LOAD' i mean set visible to the user</remarks>
    public static bool LoadActions(string actionName)
    {
     //  return true;

        if (LoggedUser == null)
            return false;

        Actions dtAction = GIIS.DataLayer.Actions.GetActionsByName(actionName);
        List<UserRole> dt = UserRole.GetUserRoleListByUserId(LoggedUser.Id);
        int actionId = 0;
        if (dtAction == null)
        {
            return false;
        }
        else
        {
            actionId = dtAction.Id;
        }

        foreach (UserRole userrole in dt)
        {
            if (RoleAction.Exists(userrole.RoleId, actionId) > 0)
            {
                return true;
            }
        }

        return false;
    }

    public static void CreateMessageAlert(System.Web.UI.Page senderPage, string alertMsg, string alertKey)
    {
        string strScript = "<script language=JavaScript>alert('" + alertMsg + "')</script>";
        if (!(senderPage.IsStartupScriptRegistered(alertKey)))
        {
            senderPage.RegisterStartupScript(alertKey, strScript);
        }
    }
}