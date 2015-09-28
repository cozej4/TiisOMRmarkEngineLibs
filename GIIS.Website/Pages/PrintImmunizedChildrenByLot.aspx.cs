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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.DataLayer;
using System.IO;
using System.Text;
using System.Globalization;


public partial class Pages_PrintImmunizedChildrenByLot : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            if (CurrentEnvironment.LoggedUser != null)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Child-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Child");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Child-dictionary" + language, wtList);
                }
                #region Person Configuration

                List<PersonConfiguration> pcList = PersonConfiguration.GetPersonConfigurationList();

                foreach (PersonConfiguration pc in pcList)
                {
                    if (pc.IsVisible == false)
                    {
                        Control lbl = FindMyControl(this, "lbl" + pc.ColumnName);
                        Control txt = FindMyControl(this, "txt" + pc.ColumnName);
                        Control tr = FindMyControl(this, "tr" + pc.ColumnName);

                        if (lbl != null)
                            lbl.Visible = false;

                        if (txt != null)
                            txt.Visible = false;



                        if (tr != null)
                            tr.Visible = false;

                        for (int i = 1; i < gvChild.Columns.Count; i++)
                        {
                            if (gvChild.Columns[i].HeaderText == pc.ColumnName)
                            {
                                gvChild.Columns[i].Visible = false;
                                break;
                            }
                        }
                    }
                }
                string id1 = Configuration.GetConfigurationByName("IdentificationNo1").Value;
                string id2 = Configuration.GetConfigurationByName("IdentificationNo2").Value;
                string id3 = Configuration.GetConfigurationByName("IdentificationNo3").Value;
                #endregion

                this.lbHealthFacility.Text = wtList["ImmunizedChildrenByLotHealthFacility"];
                this.lbLotId.Text = wtList["ImmunizedChildrenByLotItemLot"];
                this.lbChildNo.Text = wtList["ImmunizedChildrenByLotChildrenNo"];

                this.lblTitle.Text = wtList["PrintImmunizedChildrenByLotPageTitle"];

                //grid header text
                #region Grid Columns
                gvChild.Columns[1].HeaderText = wtList["ChildSystem"];
                gvChild.Columns[2].HeaderText = wtList["ChildFirstname1"];
                gvChild.Columns[3].HeaderText = wtList["ChildFirstname2"];
                gvChild.Columns[4].HeaderText = wtList["ChildLastname1"];
                gvChild.Columns[5].HeaderText = wtList["ChildLastname2"];
                gvChild.Columns[6].HeaderText = wtList["ChildBirthdate"];
                gvChild.Columns[7].HeaderText = wtList["ChildGender"];
                gvChild.Columns[8].HeaderText = wtList["ChildHealthcenter"];
                gvChild.Columns[9].HeaderText = wtList["ChildBirthplace"];
                gvChild.Columns[10].HeaderText = wtList["ChildCommunity"];
                gvChild.Columns[11].HeaderText = wtList["ChildDomicile"];
                gvChild.Columns[12].HeaderText = wtList["ChildStatus"];
                gvChild.Columns[13].HeaderText = wtList["ChildAddress"];
                gvChild.Columns[14].HeaderText = wtList["ChildPhone"];
                gvChild.Columns[15].HeaderText = wtList["ChildMobile"];
                gvChild.Columns[16].HeaderText = wtList["ChildEmail"];
                gvChild.Columns[17].HeaderText = wtList["ChildMother"];
                gvChild.Columns[18].HeaderText = wtList["ChildMotherFirstname"];
                gvChild.Columns[19].HeaderText = wtList["ChildMotherLastname"];
                gvChild.Columns[20].HeaderText = wtList["ChildFather"];
                gvChild.Columns[21].HeaderText = wtList["ChildFatherFirstname"];
                gvChild.Columns[22].HeaderText = wtList["ChildFatherLastname"];
                gvChild.Columns[23].HeaderText = wtList["ChildCaretaker"];
                gvChild.Columns[24].HeaderText = wtList["ChildCaretakerFirstname"];
                gvChild.Columns[25].HeaderText = wtList["ChildCaretakerLastname"];
                gvChild.Columns[26].HeaderText = wtList["ChildNotes"];
                gvChild.Columns[27].HeaderText = wtList["ChildIsActive"];
                gvChild.Columns[30].HeaderText = id1;
                gvChild.Columns[31].HeaderText = id2;
                gvChild.Columns[32].HeaderText = id3;

                #endregion

                int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
                string _hfId = (string)Request.QueryString["hfId"];
                if (!String.IsNullOrEmpty(_hfId))
                    int.TryParse(_hfId, out hfId);

                int lotId = 0;
                lotId =Helper.ConvertToInt( HttpContext.Current.Session["ImmunizedChildrenByLot-lotID"].ToString());
                
                lblHealthFacility.Text = HealthFacility.GetHealthFacilityById(hfId).Name;
                if (lotId != 0)
                lblLotId.Text = ItemLot.GetItemLotById(lotId).LotNumber;
                lblChildNo.Text = Child.GetCountImmunizedChildrenByLot(hfId, lotId).ToString();
                int maximumRows = int.MaxValue;
                int startRowIndex = 0;

                List<Child> list = Child.GetImmunizedChildrenByLot(ref maximumRows, ref startRowIndex, hfId, lotId);
                gvChild.DataSource = list;
                gvChild.DataBind();

            }
            else
                Response.Redirect("Default.aspx", false);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //Verifies that the control is rendered
    }

    private static Control FindMyControl(Control Root, string Id)
    {
        if (Id == "txtIsActive")
            return null;

        if (Root.ID == Id)
            return Root;

        foreach (Control Ctl in Root.Controls)
        {
            Control FoundCtl = FindMyControl(Ctl, Id);
            if (FoundCtl != null)
                return FoundCtl;
        }

        return null;
    }
    protected void gvChild_DataBound(object sender, System.EventArgs e)
    {
        string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
        ((BoundField)gvChild.Columns[6]).DataFormatString = "{0:" + dateformat + "}";

    }
}