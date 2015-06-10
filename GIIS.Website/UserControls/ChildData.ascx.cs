
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.DataLayer;

public partial class UserControls_ChildData : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            
        }
    }
    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        string language = CurrentEnvironment.Language;
        int languageId = int.Parse(language);
        Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["ChildData-dictionary" + language];
        if (wtList == null)
        {
            List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ChildData");
            wtList = new Dictionary<string, string>();
            foreach (WordTranslate vwt in wordTranslateList)
                wtList.Add(vwt.Code, vwt.Name);
            HttpContext.Current.Cache.Insert("ChildData-dictionary" + language, wtList);
        }

        string id1 = Configuration.GetConfigurationByName("IdentificationNo1").Value;

        if (e.Item.ItemType == ListItemType.Item)
        {
            Label lbSystemID = (Label)e.Item.FindControl("lbSystemID");
            lbSystemID.Text = wtList["ChildDataSystem"];
            Label lbIdentificationNo1 = (Label)e.Item.FindControl("lbIdentificationNo1");
            lbIdentificationNo1.Text = id1;
            Label lbFirstName = (Label)e.Item.FindControl("lbFirstName");
            lbFirstName.Text = wtList["ChildDataFirstname1"];
            Label lbLastName = (Label)e.Item.FindControl("lbLastName");
            lbLastName.Text = wtList["ChildDataLastname1"];
            Label lbBirthdate = (Label)e.Item.FindControl("lbBirthdate");
            lbBirthdate.Text = wtList["ChildDataBirthdate"];
            Label lbBirthplace = (Label)e.Item.FindControl("lbBirthplace");
            lbBirthplace.Text = wtList["ChildDataBirthplace"];
            Label lbGender = (Label)e.Item.FindControl("lbGender");
            lbGender.Text = wtList["ChildDataGender"];
            Label lbHealthFacility = (Label)e.Item.FindControl("lbHealthFacility");
            lbHealthFacility.Text = wtList["ChildDataHealthFacility"];
            Label lbMother = (Label)e.Item.FindControl("lbMotherName");
            lbMother.Text = wtList["ChildDataMother"];
            //Label lbFather = (Label)e.Item.FindControl("lbFatherName");
            //lbFather.Text = wtList["ChildDataFather"];
            //Label lbAddress = (Label)e.Item.FindControl("lbAddress");
            //lbAddress.Text = wtList["ChildDataAddress"];
          
        }
    }
}