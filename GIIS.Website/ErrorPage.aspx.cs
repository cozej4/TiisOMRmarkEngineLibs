using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.DataLayer;

public partial class ErrorPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int userId = CurrentEnvironment.LoggedUser.Id;
        string language = CurrentEnvironment.Language;
        int languageId = int.Parse(language);
        Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["ErrorPage-dictionary" + language];
        if (wtList == null)
        {
            List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ErrorPage");
            wtList = new Dictionary<string, string>();
            foreach (WordTranslate vwt in wordTranslateList)
                wtList.Add(vwt.Code, vwt.Name);
            HttpContext.Current.Cache.Insert("ErrorPage-dictionary" + language, wtList);
        }

        //controls
        this.lbl1.Text = wtList["ErrorPageLabel1"];
        this.lbl2.Text = wtList["ErrorPageLabel2"];
        this.lbl3.Text = wtList["ErrorPageLabel3"];

    }
   
}