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
using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class Pages_ImmunizedChildrenByLot : System.Web.UI.Page
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

                //controls
                lblHealthFacility.Text = wtList["ImmunizedChildrenByLotHealthFacility"];
                lblLotId.Text = wtList["ImmunizedChildrenByLotItemLot"];
                lblChildNo.Text = wtList["ImmunizedChildrenByLotChildrenNo"];
                lblTitle.Text = wtList["ImmunizedChildrenByLotPageTitle"];
                lblWarning.Text = wtList["ImmunizedChildrenByLotWarningText"];

                this.btnExcel.Text = wtList["ChildExcelButton"];
                this.btnPrint.Text = wtList["ChildPrintButton"];
               
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
                gvExport.Columns[1].HeaderText = wtList["ChildSystem"];
                gvExport.Columns[2].HeaderText = wtList["ChildFirstname1"];
                gvExport.Columns[3].HeaderText = wtList["ChildFirstname2"];
                gvExport.Columns[4].HeaderText = wtList["ChildLastname1"];
                gvExport.Columns[5].HeaderText = wtList["ChildLastname2"];
                gvExport.Columns[6].HeaderText = wtList["ChildBirthdate"];
                gvExport.Columns[7].HeaderText = wtList["ChildGender"];
                gvExport.Columns[8].HeaderText = wtList["ChildHealthcenter"];
                gvExport.Columns[9].HeaderText = wtList["ChildBirthplace"];
                gvExport.Columns[10].HeaderText = wtList["ChildCommunity"];
                gvExport.Columns[11].HeaderText = wtList["ChildDomicile"];
                gvExport.Columns[12].HeaderText = wtList["ChildStatus"];
                gvExport.Columns[13].HeaderText = wtList["ChildAddress"];
                gvExport.Columns[14].HeaderText = wtList["ChildPhone"];
                gvExport.Columns[15].HeaderText = wtList["ChildMobile"];
                gvExport.Columns[16].HeaderText = wtList["ChildEmail"];
                gvExport.Columns[17].HeaderText = wtList["ChildMother"];
                gvExport.Columns[18].HeaderText = wtList["ChildMotherFirstname"];
                gvExport.Columns[19].HeaderText = wtList["ChildMotherLastname"];
                gvExport.Columns[20].HeaderText = wtList["ChildFather"];
                gvExport.Columns[21].HeaderText = wtList["ChildFatherFirstname"];
                gvExport.Columns[22].HeaderText = wtList["ChildFatherLastname"];
                gvExport.Columns[23].HeaderText = wtList["ChildCaretaker"];
                gvExport.Columns[24].HeaderText = wtList["ChildCaretakerFirstname"];
                gvExport.Columns[25].HeaderText = wtList["ChildCaretakerLastname"];
                gvExport.Columns[26].HeaderText = wtList["ChildNotes"];
                gvExport.Columns[27].HeaderText = wtList["ChildIsActive"];
                gvExport.Columns[30].HeaderText = id1;
                gvExport.Columns[31].HeaderText = id2;
                gvExport.Columns[32].HeaderText = id3;
                #endregion
                
                int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
                string _hfId = (string)Request.QueryString["hfId"];
                if (!String.IsNullOrEmpty(_hfId))
                    int.TryParse(_hfId, out hfId);

                int lotId = 0;
                string _lotId = (string)Request.QueryString["lotId"];
                if (!String.IsNullOrEmpty(_lotId))
                    int.TryParse(_lotId, out lotId);

                if (hfId != 0)
                    txtHealthFacility.Text = HealthFacility.GetHealthFacilityById(hfId).Name;

                if (lotId != 0)
                txtLotId.Text = ItemLot.GetItemLotById(lotId).LotNumber;

                odsChild.SelectParameters.Clear();
                odsChild.SelectParameters.Add("hfId", hfId.ToString());
                odsChild.SelectParameters.Add("itemlotid", lotId.ToString());
                HttpContext.Current.Session["ImmunizedChildrenByLot-hfID"] = hfId.ToString();
                HttpContext.Current.Session["ImmunizedChildrenByLot-lotID"] = lotId.ToString();
                txtChildNo.Text = Child.GetCountImmunizedChildrenByLot(hfId, lotId).ToString();
               
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
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
            ((BoundField)gvExport.Columns[6]).DataFormatString = "{0:" + dateformat + "}";
        if (gvChild.Rows.Count <= 0)
        {
            btnExcel.Visible = false;
            btnPrint.Visible = false;
            lblWarning.Visible = true;
        }
        else
        {
            btnExcel.Visible = true;
            btnPrint.Visible = true;
            lblWarning.Visible = false;
    }
    }
    protected void gvChild_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvChild.PageIndex = e.NewPageIndex;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        if ((HttpContext.Current.Session["ImmunizedChildrenByLot-hfID"] != null) && (HttpContext.Current.Session["ImmunizedChildrenByLot-lotID"] != null))
        {
            string HF = HttpContext.Current.Session["ImmunizedChildrenByLot-hfID"].ToString();
            string LOT = HttpContext.Current.Session["ImmunizedChildrenByLot-lotID"].ToString();
            int hfid = Helper.ConvertToInt(HF);
            int lotid = Helper.ConvertToInt(LOT);

            if ((!string.IsNullOrEmpty(HF)) && (!string.IsNullOrEmpty(LOT)))
            {
                int maximumRows = int.MaxValue;
                int startRowIndex = 0;

                List<Child> list = Child.GetImmunizedChildrenByLot(ref maximumRows, ref startRowIndex, hfid, lotid);
                gvExport.DataSource = list;
                gvExport.DataBind();
                gvExport.Visible = true;
            }

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=ImmunizedChildrenByLot.xls");
            Response.Charset = "";

            // If you want the option to open the Excel file without saving then
            // comment out the line below
            // Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            gvExport.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();

            gvExport.Visible = false;
        }
    }

    protected override void Render(HtmlTextWriter writer)
    {
        // Ensure that the control is nested in a server form.
        if ((Page != null))
        {
            Page.VerifyRenderingInServerForm(this);
        }

        base.Render(writer);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for the specified ASP.NET
        //     server control at run time. 
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["ImmunizedChildrenByLot-hfID"] != null) 
        {
            string HF = HttpContext.Current.Session["ImmunizedChildrenByLot-hfID"].ToString();
            int hfId = Helper.ConvertToInt(HF);

            string queryString = "PrintImmunizedChildrenByLot.aspx?hfId=" + hfId ;
            string newWin = "window.open('" + queryString + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "pop", newWin, true);
        }
    }
}