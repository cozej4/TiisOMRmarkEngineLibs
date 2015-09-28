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

public partial class Pages_PrintNotImmunizedChildren : System.Web.UI.Page
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
                        //Control lbl = FindMyControl(this, "lbl" + pc.ColumnName);
                        //Control txt = FindMyControl(this, "txt" + pc.ColumnName);
                        ////Control rbl = FindMyControl(this, "rbl" + pc.ColumnName);
                        //Control tr = FindMyControl(this, "tr" + pc.ColumnName);

                        //if (lbl != null)
                        //    lbl.Visible = false;

                        //if (txt != null)
                        //    txt.Visible = false;

                        //if (rbl != null)
                        //    rbl.Visible = false;

                        //if (tr != null)
                        //    tr.Visible = false;

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

                #endregion

                this.lbHealthFacility.Text = wtList["ChildHealthcenter"];
                this.lbVaccine.Text = wtList["ChildVaccine"];
               
                this.lblTitle.Text = wtList["PrintNotImmunizedChildrenPageTitle"];

                //grid header text
                #region Grid Columns
                gvChild.Columns[1].HeaderText = wtList["ChildNonVaccinationReason"];
                gvChild.Columns[2].HeaderText = wtList["ChildSystem"];
                gvChild.Columns[3].HeaderText = wtList["ChildFirstname1"];
                gvChild.Columns[4].HeaderText = wtList["ChildFirstname2"];
                gvChild.Columns[5].HeaderText = wtList["ChildLastname1"];
                gvChild.Columns[6].HeaderText = wtList["ChildLastname2"];
                gvChild.Columns[7].HeaderText = wtList["ChildBirthdate"];
                gvChild.Columns[8].HeaderText = wtList["ChildGender"];
                gvChild.Columns[9].HeaderText = wtList["ChildHealthcenter"];
                gvChild.Columns[10].HeaderText = wtList["ChildBirthplace"];
                gvChild.Columns[11].HeaderText = wtList["ChildMotherFirstname"];
                gvChild.Columns[12].HeaderText = wtList["ChildMotherLastname"];
                gvChild.Columns[13].HeaderText = wtList["ChildFatherFirstname"];
                gvChild.Columns[14].HeaderText = wtList["ChildFatherLastname"];

                #endregion

                int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
                string _hfId = (string)Request.QueryString["hfId"];
                if (!String.IsNullOrEmpty(_hfId))
                    int.TryParse(_hfId, out hfId);
                
                lblHealthFacility.Text = HealthFacility.GetHealthFacilityById(hfId).Name;
                lblVaccine.Text = HttpContext.Current.Session["vaccine"].ToString();
                
                int maximumRows = int.MaxValue;
                int startRowIndex = 0;
                string where = HttpContext.Current.Session["whereExcelNotImmunized"].ToString();
                List<Child> list = Child.GetNotImmunizedChildren(ref maximumRows, ref startRowIndex, where);
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

    protected void gvChild_DataBound(object sender, System.EventArgs e)
    {
        string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
        ((BoundField)gvChild.Columns[7]).DataFormatString = "{0:" + dateformat + "}";

    }
}