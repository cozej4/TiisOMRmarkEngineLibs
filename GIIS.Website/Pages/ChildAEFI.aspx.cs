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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
public partial class Pages_ChildAEFI : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            List<string> actionList = null;
            string sessionNameAction = "";
            if (CurrentEnvironment.LoggedUser != null)
            {
                sessionNameAction = "__GIS_actionList_" + CurrentEnvironment.LoggedUser.Id;
                actionList = (List<string>)Session[sessionNameAction];
            }

            if ((actionList != null) && actionList.Contains("ViewVaccinationEvent") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["ChildAefi-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ChildAefi");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("VaccinationAppointment-dictionary" + language, wtList);
                }

                //controls
                this.chbxChildAEFI.Text = wtList["ChildAefiDoneAefi"];
                this.lblDate.Text = wtList["ChildAefiDate"];
                this.lblNotes.Text = wtList["ChildAefiNotes"];

                //grid header text
                gvVaccinationAppointment.Columns[1].HeaderText = wtList["ChildAefiVaccines"];
                gvVaccinationAppointment.Columns[2].HeaderText = wtList["ChildAefiVaccineDate"];
                gvVaccinationAppointment.Columns[3].HeaderText = wtList["ChildAefiHealthCenter"];
                gvVaccinationAppointment.Columns[4].HeaderText = wtList["ChildAefiDone"];

                gvVccEvent.Columns[1].HeaderText = wtList["ChildAefiVaccines"];
                gvVccEvent.Columns[2].HeaderText = wtList["ChildAefiVaccineDate"];
                gvVccEvent.Columns[3].HeaderText = wtList["ChildAefiHealthCenter"];
                gvVccEvent.Columns[4].HeaderText = wtList["ChildAefiDone"];
                gvVccEvent.Columns[5].HeaderText = wtList["ChildAefiAEFI"];
                gvVccEvent.Columns[6].HeaderText = wtList["ChildAefiAEFIDate"];
                gvVccEvent.Columns[7].HeaderText = wtList["ChildAefiNotes"];

                //page title
                this.lblTitle.Text = wtList["ChildAefiPageTitle"];
                //actions

                //buttons
                this.btnEdit.Text = wtList["ChildAefiEditButton"];
                //this.btnClear.Text = wtList["ChildAefiClearButton"];

                //messages
                this.lblInfo.Text = wtList["ChildAefiInfoText"];
                this.lblSuccess.Text = wtList["ChildAefiSuccessText"];
                this.lblWarning.Text = wtList["ChildAefiWarningText"];
                this.lblError.Text = wtList["ChildAefiErrorText"];
                this.cvAEFIdate.ErrorMessage = wtList["ChildAefiDateValidator"];

                ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
                ceDate.Format = dateformat.DateFormat;
                revDate.ErrorMessage = dateformat.DateFormat;
                revDate.ValidationExpression = dateformat.DateExpresion;
                ceDate.EndDate = DateTime.Today.Date;

                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);

                    odsVaccinationAppointment.SelectParameters.Clear();
                    odsVaccinationAppointment.SelectParameters.Add("childId", id.ToString());
                    odsVaccinationAppointment.DataBind();

                    if (gvVaccinationAppointment.Rows.Count <= 0)
                    {
                        chbxChildAEFI.Visible = false;
                        lblDate.Visible = false;
                        txtDate.Visible = false;
                        lblNotes.Visible = false;
                        txtNotes.Visible = false;
                        btnSave.Visible = false;
                        lblInfo.Visible = true;
                        btnEdit.Visible = false;

                     
                    }
                    btnEdit.Visible = false;
                    btnClear.Visible = false;
                    txtDate.Text = DateTime.Today.ToString(dateformat.DateFormat);
                    odsVccEvent.SelectParameters.Clear();
                    odsVccEvent.SelectParameters.Add("childId", id.ToString());
                    odsVccEvent.DataBind();
                }
                int appId = -1;
                string _appid = Request.QueryString["appId"];
                if (!String.IsNullOrEmpty(_appid))
                {
                    int.TryParse(_appid, out appId);
                    VaccinationAppointment app = VaccinationAppointment.GetVaccinationAppointmentById(appId);
                    HttpContext.Current.Session["__AppId"] = appId;
                    chbxChildAEFI.Visible = true;
                    lblDate.Visible = true;
                    txtDate.Visible = true;
                    lblNotes.Visible = true;
                    txtNotes.Visible = true;
                    btnSave.Visible = false;
                    btnEdit.Visible = true;
                    btnClear.Visible = false;
                    lblInfo.Visible = false;

                    chbxChildAEFI.Checked = app.Aefi;
                    if (app.AefiDate.ToString("yyyy-MM-dd") != "0001-01-01")
                        txtDate.Text = app.AefiDate.ToString(dateformat.DateFormat);
                    txtNotes.Text = app.Notes;
                    odsVccEvent.SelectParameters.Clear();
                    odsVccEvent.SelectParameters.Add("childId", app.ChildId.ToString());
                    odsVccEvent.DataBind();

                }
            }
        }
    }

    protected void gvVaccinationAppointment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HttpContext.Current.Session["__AppId"] = e.Row.Cells[0].Text;
            HttpContext.Current.Session["__VaccinationDate"] = DateTime.ParseExact(e.Row.Cells[2].Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            e.Row.Cells[0].Visible = false;
        }
    }

    protected void gvVaccinationAppointment_DataBound(object sender, EventArgs e)
    {
        if (gvVaccinationAppointment.Rows.Count > 0)
        gvVaccinationAppointment.HeaderRow.Cells[0].Visible = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                if (HttpContext.Current.Session["__AppId"] != null)
                {
                    int appId = int.Parse(HttpContext.Current.Session["__AppId"].ToString());

                    VaccinationAppointment va = VaccinationAppointment.GetVaccinationAppointmentById(appId);
                    va.Aefi = chbxChildAEFI.Checked;
                    DateTime date = DateTime.ParseExact(txtDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.InvariantCulture);
                    va.AefiDate = date;
                    va.Notes = txtNotes.Text;
                    va.ModifiedBy = CurrentEnvironment.LoggedUser.Id;
                    va.ModifiedOn = DateTime.Now;

                    int updated = VaccinationAppointment.Update(va);

                    if (updated >= 1)
                    {

                        odsVccEvent.SelectParameters.Clear();
                        odsVccEvent.SelectParameters.Add("childId", va.ChildId.ToString());

                        odsVccEvent.DataBind();

                        lblSuccess.Visible = true;
                        lblWarning.Visible = false;
                        lblError.Visible = false;
                    }
                    else
                    {
                        lblSuccess.Visible = false;
                        lblWarning.Visible = false;
                        lblError.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }

    protected void ValidateAEFIDate(object sender, ServerValidateEventArgs e)
    {
        if (Page.IsValid)
        {
            for (int rowIndex = 0; rowIndex < gvVaccinationAppointment.Rows.Count; rowIndex++)
            {
                if (HttpContext.Current.Session["__VaccinationDate"] != null)
                {
                    DateTime vaccinationDate = DateTime.ParseExact(HttpContext.Current.Session["__VaccinationDate"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
                    DateTime aefiDate = DateTime.ParseExact(txtDate.Text, dateformat.DateFormat, CultureInfo.InvariantCulture);

                    e.IsValid = aefiDate >= vaccinationDate && aefiDate <= DateTime.Today.Date;

                    if (e.IsValid == false)
                        break;
                }
            }
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int appId = -1;
                string _appid = Request.QueryString["appId"];
                if (!String.IsNullOrEmpty(_appid))
                {
                    int.TryParse(_appid, out appId);
                    VaccinationAppointment va = VaccinationAppointment.GetVaccinationAppointmentById(appId);
                    va.Aefi = chbxChildAEFI.Checked;
                    DateTime date = DateTime.ParseExact(txtDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                    if (va.Aefi)
                    {
                        va.AefiDate = date;
                        va.Notes = txtNotes.Text;
                    }
                    else
                    {
                        va.AefiDate = DateTime.Parse("0001-01-01");
                        va.Notes = String.Empty;
                    }

                    va.ModifiedBy = CurrentEnvironment.LoggedUser.Id;
                    va.ModifiedOn = DateTime.Now;

                    int updated = VaccinationAppointment.Update(va);

                    if (updated >= 1)
                    {

                        odsVccEvent.SelectParameters.Clear();
                        odsVccEvent.SelectParameters.Add("childId", va.ChildId.ToString());

                        odsVccEvent.DataBind();

                        if (va.Aefi)
                        {
                            txtDate.Text = string.Empty;
                            txtNotes.Text = string.Empty;
                        }

                        lblSuccess.Visible = true;
                        lblWarning.Visible = false;
                        lblError.Visible = false;
                    }
                    else
                    {
                        lblSuccess.Visible = false;
                        lblWarning.Visible = false;
                        lblError.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }
    protected void gvVccEvent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string _aefidate = e.Row.Cells[6].Text;
            DateTime aefiDate = DateTime.Parse("0001-01-01");
            if (!String.IsNullOrEmpty(_aefidate) && _aefidate != "&nbsp;")
                aefiDate = DateTime.ParseExact(e.Row.Cells[6].Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);

            if (aefiDate.ToString("yyyy-MM-dd") == "0001-01-01")
                e.Row.Cells[6].Text = string.Empty;

        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.chbxChildAEFI.Checked = false;
        this.txtDate.Text = string.Empty;
        this.txtNotes.Text = string.Empty;
    }
}