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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Collections.Specialized;
using System.Data;
using GIIS.BusinessLogic;

public partial class Pages_RegisterVaccination : System.Web.UI.Page
{
    static String healthCenterId;

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
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["VaccinationEvent-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "VaccinationEvent");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("VaccinationEvent-dictionary" + language, wtList);
                }

                //controls
                this.lbChild.Text = wtList["VaccinationEventChild"];
                this.lbVaccineDose.Text = wtList["VaccinationEventDose"];
                this.lbHealthCenter.Text = wtList["VaccinationEventHealthFacility"];
                this.lbScheduledDate.Text = wtList["VaccinationEventScheduledDate"];
                this.lbChildBirthdate.Text = wtList["VaccinationEventBirthdate"];
                //this.cvVaccinationdate.ErrorMessage = wtList["VaccinationEventDateValidator"];

                //grid header text
                gvVaccinationEvent.Columns[1].HeaderText = wtList["VaccinationEventDose"];
                gvVaccinationEvent.Columns[2].HeaderText = wtList["VaccinationEventVaccineLot"];
                gvVaccinationEvent.Columns[3].HeaderText = wtList["VaccinationEventVaccinationDate"];
                gvVaccinationEvent.Columns[4].HeaderText = wtList["VaccinationEventVaccinationStatus"];
                gvVaccinationEvent.Columns[5].HeaderText = wtList["VaccinationEventNonvaccinationReason"];

                //actions
                this.btnEdit.Visible = actionList.Contains("EditVaccinationEvent");
                this.btnRemove.Visible = actionList.Contains("RemoveVaccinationEvent");

                //buttons
                this.btnEdit.Text = wtList["VaccinationEventEditButton"];
                this.btnRemove.Text = wtList["VaccinationEventRemoveButton"];

                //message
                this.lblSuccess.Text = wtList["VaccinationEventSuccessText"];
                this.lblWarning.Text = wtList["VaccinationEventWarningText"];
                this.lblError.Text = wtList["VaccinationEventErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["VaccinationEventPageTitle"];

                //selected object by id
                int appId = -1;
                string _appid = Request.QueryString["appId"];
                if (!String.IsNullOrEmpty(_appid))
                {
                    int.TryParse(_appid, out appId);
                    VaccinationAppointment app = VaccinationAppointment.GetVaccinationAppointmentById(appId);
                    PopulateControls(app);
                   
                }
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    id = int.Parse(_id);
                    VaccinationEvent v = VaccinationEvent.GetVaccinationEventById(id);
                    PopulateControls(v);
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    /// <summary>
    /// Populate controls on the page by vaccination event 
    /// </summary>
    /// <param name="v">The vaccination event that is being called</param>
    private void PopulateControls(VaccinationEvent v)
    {
        lnkChild.Text = v.Child.Name;
        lnkChild.PostBackUrl = "Child.aspx?id=" + v.Child.Id;
        lblChildBirthdate.Text = v.Child.Birthdate.ToString("dd-MMM-yyyy");
        chbxOutreach.Checked = v.Appointment.Outreach;
        string doses = v.Dose.Fullname;
        lblVaccineDose.Text = doses;
        lblScheduledDate.Text = v.ScheduledDate.ToString("dd-MMM-yyyy");
        lblHealthCenter.Text = v.HealthFacility.Name;

        if (v.VaccinationStatus || v.NonvaccinationReasonId != 0)
        {
            if (v.HealthFacilityId == CurrentEnvironment.LoggedUser.HealthFacilityId || v.HealthFacility.ParentId == CurrentEnvironment.LoggedUser.HealthFacilityId || CurrentEnvironment.LoggedUser.HealthFacility.ParentId == 0)
            {
                string s = HealthFacility.GetAllChildsForOneHealthFacility(CurrentEnvironment.LoggedUser.HealthFacilityId, true);
                string where = string.Format(@"AND ""ID"" in ( {0})", s);
                odsHealthF.SelectParameters.Clear();
                odsHealthF.SelectParameters.Add("ids", where);
                odsHealthF.DataBind();

                ddlHealthFacility.SelectedValue = v.HealthFacilityId.ToString();
            }
            else
            {
                lblWarning.Text = "You can not modify this record!";
                lblWarning.Visible = true;
                ddlHealthFacility.Visible = false;
                gvVaccinationEvent.Visible = false;
                btnEdit.Visible = false;
                btnRemove.Visible = false;
                lblHealthFacilityId.Visible = false;
                chbxOutreach.Visible = false;
            }

        }
        odsVaccinationEvent.SelectParameters.Clear();
        odsVaccinationEvent.SelectParameters.Add("appId", v.AppointmentId.ToString());

        tblsupp.Visible = false;
    }
    /// <summary>
    /// Populate Controls on the page by using the vaccination appointment
    /// </summary>
    /// <param name="app">The vaccination appointment that is being called</param>
    private void PopulateControls (VaccinationAppointment app)
    {
        List<VaccinationEvent> le = VaccinationEvent.GetVaccinationEventByAppointmentId(app.Id);
        lnkChild.Text = app.Child.Name;
        lnkChild.PostBackUrl = "Child.aspx?id=" + app.Child.Id;
        lblChildBirthdate.Text = app.Child.Birthdate.ToString("dd-MMM-yyyy");
        chbxOutreach.Checked = app.Outreach;
        string doses = "";
        VaccinationEvent o = null;
        foreach (VaccinationEvent ve in le)
        {
            doses += ve.Dose.Fullname + " ";
            lblHealthCenter.Text = ve.HealthFacility.Name;
            o = ve;
        }
        lblVaccineDose.Text = doses;
        lblScheduledDate.Text = app.ScheduledDate.ToString("dd-MMM-yyyy"); //ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat);

        if (!o.IsActive)
        {
            lblInfo.Visible = true;
            btnEdit.Visible = false;
            btnRemove.Visible = false;
            gvVaccinationEvent.Visible = false;
        }

        foreach (VaccinationEvent ve in le)
        {
            int datediff = DateTime.Today.Date.Subtract(ve.Child.Birthdate).Days;
            if (ve.Dose.FromAgeDefinitionId != 0 && ve.Dose.FromAgeDefinitionId != null)
                if (datediff < ve.Dose.FromAgeDefinition.Days)
                {
                    lblInfo.Visible = true;
                    break;
                }
            if (ve.Dose.ToAgeDefinitionId != 0 && ve.Dose.ToAgeDefinitionId != null)
                if (datediff > ve.Dose.ToAgeDefinition.Days)
                {
                    lblInfo.Visible = true;
                    break;
                }
        }

        // populate supplements table
        lblVitADate.Text = DateTime.Today.Date.ToString("dd-MMM-yyyy");
        lblMebendezolDate.Text = DateTime.Today.Date.ToString("dd-MMM-yyyy");

        ChildSupplements su = ChildSupplements.GetChildSupplementsByChild(app.Child.Id, DateTime.Today.Date); //get by date
        if (su != null)
        {
            chkVitA.Checked = su.Vita;
            chkMebendezol.Checked = su.Mebendezol;
        }
        else
        {
            chkVitA.Checked = false;
            chkMebendezol.Checked = false;
        }

        //populate facility dropdownlist

            int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
            HealthFacility hf = HealthFacility.GetHealthFacilityById(hfId);
            string where = string.Empty;
            if (!hf.TopLevel)
            {
                string s = HealthFacility.GetAllChildsForOneHealthFacility(hfId, true);
                where = string.Format(@"AND ""ID"" in ( {0})", s);
            }
            odsHealthF.SelectParameters.Clear();
            odsHealthF.SelectParameters.Add("ids", where);
            odsHealthF.DataBind();
            if (hf.VaccinationPoint)
                ddlHealthFacility.SelectedValue = hfId.ToString();

        //populate events grid
        odsVaccinationEvent.SelectParameters.Clear();
        odsVaccinationEvent.SelectParameters.Add("appId", app.Id.ToString());
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int i = 0;
                int childId = 0;
                for (int rowIndex = 0; rowIndex < gvVaccinationEvent.Rows.Count; rowIndex++)
                {
                    //extract the TextBox values
                    DropDownList ddlVaccineLot = (DropDownList)gvVaccinationEvent.Rows[rowIndex].Cells[2].FindControl("ddlVaccineLot");
                    TextBox txtVaccinationDate = (TextBox)gvVaccinationEvent.Rows[rowIndex].Cells[3].FindControl("txtVaccinationDate");
                    CheckBox chkDoneStatus = (CheckBox)gvVaccinationEvent.Rows[rowIndex].Cells[4].FindControl("chkStatus");
                    DropDownList ddlNonvaccinationReason = (DropDownList)gvVaccinationEvent.Rows[rowIndex].Cells[5].FindControl("ddlNonvaccinationReason");
                    Label lblId = (Label)gvVaccinationEvent.Rows[rowIndex].Cells[7].FindControl("lblId");
                    int id = int.Parse(lblId.Text);
                    VaccinationEvent v = VaccinationEvent.GetVaccinationEventById(id);
                    childId = v.ChildId;
                    if ((ddlVaccineLot.SelectedIndex > 0 || ddlNonvaccinationReason.SelectedIndex > 0) && gvVaccinationEvent.Rows[rowIndex].Visible)
                        i = UpdateOneRecord(id, int.Parse(ddlVaccineLot.SelectedValue), txtVaccinationDate.Text, chkDoneStatus.Checked, int.Parse(ddlNonvaccinationReason.SelectedValue));
                }
                if (i > 0)
                {
                    gvChild.Visible = true;
                    odsChild.SelectParameters.Clear();
                    odsChild.SelectParameters.Add("childId", childId.ToString());

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
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }
    private int UpdateOneRecord(int id, int vaccineLotId, string vaccinationDate, bool done, int nonvaccinationReason)
    {
        int i = -1;

        int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
        int userId = CurrentEnvironment.LoggedUser.Id;
        DateTime date = DateTime.ParseExact(vaccinationDate, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
      
        if (ddlHealthFacility.SelectedIndex != 0)
           hfId= int.Parse(ddlHealthFacility.SelectedValue);

        VaccinationLogic vl = new VaccinationLogic();
        VaccinationEvent ve = vl.UpdateVaccinationEvent(id, vaccineLotId, date, hfId, done, nonvaccinationReason, userId, DateTime.Now);

        if (ve != null)
        {
            UpdateVaccinationAppointment(ve.AppointmentId, chbxOutreach.Checked);
            UpdateSupplements(ve.ChildId);
            i = 1;
        }
        return i;
    }

    private void UpdateVaccinationAppointment(int appId, bool outreach)
    {
        VaccinationAppointment va = VaccinationAppointment.GetVaccinationAppointmentById(appId);
        va.Outreach = outreach;
        va.ModifiedOn = DateTime.Now;
        va.ModifiedBy = CurrentEnvironment.LoggedUser.Id;
        VaccinationAppointment.Update(va);
    }
    private void UpdateSupplements(int childId)
    {
        ChildSupplements su = ChildSupplements.GetChildSupplementsByChild(childId);
        if (su != null)
        {
            su.ChildId = childId;
            su.Vita = chkVitA.Checked;
            su.Mebendezol = chkMebendezol.Checked;
            su.Date = DateTime.Now;
            // or 
            // DateTime date = DateTime.ParseExact(lblVitADate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
            // su.Date = date;
            su.ModifiedOn = DateTime.Now;
            su.ModifiedBy = CurrentEnvironment.LoggedUser.Id;

            ChildSupplements.Update(su);
        }
        else
        {
            su = new ChildSupplements();
            su.ChildId = childId;
            su.Vita = chkVitA.Checked;
            su.Mebendezol = chkMebendezol.Checked;
            su.Date = DateTime.Now;

            // or 
            // DateTime date = DateTime.ParseExact(lblVitADate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
            // su.Date = date;
            su.ModifiedOn = DateTime.Now;
            su.ModifiedBy = CurrentEnvironment.LoggedUser.Id;

            ChildSupplements.Insert(su);
        }
    }

    private int RemoveOneRecord(int id)
    {
        int i = -1;
        int userId = CurrentEnvironment.LoggedUser.Id;

        VaccinationEvent o = VaccinationEvent.GetVaccinationEventById(id);

        VaccinationLogic vl = new VaccinationLogic();
        o = vl.RemoveVaccinationEvent(o, userId);

        if (o != null)
        {
            UpdateVaccinationAppointment(o.AppointmentId, false);
            i = 1;
        }
        return i;
    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int i = 0;
                for (int rowIndex = 0; rowIndex < gvVaccinationEvent.Rows.Count; rowIndex++)
                {
                    DropDownList ddlVaccineLot = (DropDownList)gvVaccinationEvent.Rows[rowIndex].Cells[2].FindControl("ddlVaccineLot");
                    Label lblId = (Label)gvVaccinationEvent.Rows[rowIndex].Cells[7].FindControl("lblId");
                    int id = int.Parse(lblId.Text);
                    if (gvVaccinationEvent.Rows[rowIndex].Visible)
                       i = RemoveOneRecord(id);
                }
                //remove supplements added on day
                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    //grid should be refreshed
                    gvVaccinationEvent.DataBind();
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = false;
                    lblError.Visible = true;
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

    protected void ValidateVaccinationDate(object sender, ServerValidateEventArgs e)
    {
        if (Page.IsValid)
        {
            for (int rowIndex = 0; rowIndex < gvVaccinationEvent.Rows.Count; rowIndex++)
            {
                //extract the TextBox values
                TextBox txtVaccinationDate = (TextBox)gvVaccinationEvent.Rows[rowIndex].Cells[3].FindControl("txtVaccinationDate");
                   Label lblId = (Label)gvVaccinationEvent.Rows[rowIndex].Cells[7].FindControl("lblId");
                    int id = int.Parse(lblId.Text);
                    VaccinationEvent v = VaccinationEvent.GetVaccinationEventById(id);
                   Child c = v.Child;
                ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));

                DateTime date = DateTime.ParseExact(txtVaccinationDate.Text, dateformat.DateFormat, CultureInfo.CurrentCulture);

                e.IsValid = date <= DateTime.Today && date >= c.Birthdate ;

                if (e.IsValid == false)
                    break;
            }
        }
    }
    protected void cvVaccinationEvent_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Page.IsValid)
        {
            for (int rowIndex = 0; rowIndex < gvVaccinationEvent.Rows.Count; rowIndex++)
            {
                //extract the TextBox values
                DropDownList ddlVaccineLot = (DropDownList)gvVaccinationEvent.Rows[rowIndex].Cells[2].FindControl("ddlVaccineLot");
                TextBox txtVaccinationDate = (TextBox)gvVaccinationEvent.Rows[rowIndex].Cells[3].FindControl("txtVaccinationDate");
                CheckBox chkDoneStatus = (CheckBox)gvVaccinationEvent.Rows[rowIndex].Cells[4].FindControl("chkStatus");
                DropDownList ddlNonvaccinationReason = (DropDownList)gvVaccinationEvent.Rows[rowIndex].Cells[5].FindControl("ddlNonvaccinationReason");
                Label lblId = (Label)gvVaccinationEvent.Rows[rowIndex].Cells[7].FindControl("lblId");
                int id = int.Parse(lblId.Text);

                if (gvVaccinationEvent.Rows[rowIndex].Visible)
                {
                    if (chkDoneStatus.Checked)
                        args.IsValid = (ddlVaccineLot.SelectedIndex != 0 && ddlHealthFacility.SelectedIndex != 0); // ----- is index 0
                    else
                        args.IsValid = (ddlVaccineLot.SelectedIndex == 0 && ddlNonvaccinationReason.SelectedIndex != 0 && ddlHealthFacility.SelectedIndex != 0); //

                    if (args.IsValid == false)
                        break;
                }
            }
        }
    }

    protected void btnImmunizationCard_Click(object sender, EventArgs e)
    {
        int id = -1;
        int childId = 0;
        string _id = Request.QueryString["id"];
        if (!String.IsNullOrEmpty(_id))
        {
            int.TryParse(_id, out id);
            VaccinationEvent o = VaccinationEvent.GetVaccinationEventById(id);
            childId = o.ChildId;
        }
        int appid = -1;
        string _appid = Request.QueryString["appid"];
        if (!String.IsNullOrEmpty(_appid))
        {
            int.TryParse(_appid, out appid);
            VaccinationAppointment o = VaccinationAppointment.GetVaccinationAppointmentById(appid);
            childId = o.ChildId;
        }
        Response.Redirect("ViewImmunizationCard.aspx?id=" + childId, false);

    }
    protected void gvVaccinationEvent_DataBound(object sender, EventArgs e)
    {
        if (gvVaccinationEvent.Rows.Count > 0)
        {
            foreach (GridViewRow gvr in gvVaccinationEvent.Rows)
            {
                gvr.Cells[6].Visible = false;
                gvr.Cells[0].Visible = false;
            }

            gvVaccinationEvent.HeaderRow.Cells[6].Visible = false;
            gvVaccinationEvent.HeaderRow.Cells[0].Visible = false;
            string _appid = Request.QueryString["appId"];
            if (!String.IsNullOrEmpty(_appid))
            {
                foreach (GridViewRow gvr in gvVaccinationEvent.Rows)
                {

                    Label lblId = (Label)gvr.FindControl("lblId");
                    VaccinationEvent ve = VaccinationEvent.GetVaccinationEventById(int.Parse(lblId.Text));

                    if ((!ve.VaccinationStatus) && (ve.NonvaccinationReasonId == 0 || ve.NonVaccinationReason.KeepChildDue == true))
                    {
                        gvr.Visible = true;
                        //int datediff = DateTime.Today.Date.Subtract(ve.Child.Birthdate).Days;
                        //if (ve.Dose.ToAgeDefinitionId != 0 && ve.Dose.ToAgeDefinitionId != null)
                        //    if (datediff > ve.Dose.ToAgeDefinition.Days)
                        //        gvr.Visible = false;
                    }
                    else
                        gvr.Visible = false;
                }
            }

            string _id = Request.QueryString["id"];
            if (!String.IsNullOrEmpty(_id))
            {
                foreach (GridViewRow gvr in gvVaccinationEvent.Rows)
                {
                    int id = int.Parse(_id);
                    Label lblId = (Label)gvr.FindControl("lblId");
                    VaccinationEvent ve = VaccinationEvent.GetVaccinationEventById(int.Parse(lblId.Text));

                    if (ve.Id == id)
                        gvr.Visible = true;
                    else
                        gvr.Visible = false;

                }
            }
        }
    }

    protected void gvVaccinationEvent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!this.IsPostBack)
        {
            e.Row.Cells[0].Visible = false;
            VaccinationEvent o = (VaccinationEvent)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtVaccinationDate = (TextBox)e.Row.FindControl("txtVaccinationDate");
               
                CheckBox chkDoneStatus = (CheckBox)e.Row.FindControl("chkStatus");
                DropDownList ddlNonvaccinationReason = (DropDownList)e.Row.FindControl("ddlNonvaccinationReason");
          
                DropDownList ddlVaccineLot = (DropDownList)e.Row.FindControl("ddlVaccineLot");
                ObjectDataSource odsItemLot = (ObjectDataSource)e.Row.FindControl("odsItemLot");
                if (ddlHealthFacility.SelectedIndex != 0)
                {
                    string hfcode = HealthFacility.GetHealthFacilityById(int.Parse(ddlHealthFacility.SelectedValue)).Code;
                    odsItemLot.SelectParameters.Clear();
                    odsItemLot.SelectParameters.Add("itemId", o.Dose.ScheduledVaccination.ItemId.ToString());
                    odsItemLot.SelectParameters.Add("hfId", hfcode);
                    odsItemLot.DataBind();
                    ddlVaccineLot.DataBind();
                    if (ddlVaccineLot.Items.Count > 2)
                        ddlVaccineLot.SelectedIndex = 2;
                    else
                        ddlVaccineLot.SelectedIndex = 0;
                }
                AjaxControlToolkit.CalendarExtender ceVaccinationDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("ceVaccinationDate");
                RegularExpressionValidator revVaccinationDate = (RegularExpressionValidator)e.Row.FindControl("revVaccinationDate");

                ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
                ceVaccinationDate.Format = dateformat.DateFormat;
                revVaccinationDate.ErrorMessage = dateformat.DateFormat;
                revVaccinationDate.ValidationExpression = dateformat.DateExpresion;
                ceVaccinationDate.EndDate = DateTime.Today.Date;
                ceVaccinationDate.StartDate = o.Child.Birthdate;
                txtVaccinationDate.Text = DateTime.Today.ToString(dateformat.DateFormat);

                if (o.VaccinationStatus)
                {
                    chkDoneStatus.Checked = true;
                    ddlVaccineLot.SelectedValue = o.VaccineLotId.ToString();
                    txtVaccinationDate.Text = o.VaccinationDate.ToString(dateformat.DateFormat);
                    ddlNonvaccinationReason.Visible = false;
                }
                else if (!o.VaccinationStatus && o.NonvaccinationReasonId != 0)
                {
                    chkDoneStatus.Checked = false;
                    ddlVaccineLot.SelectedIndex = 0;
                    txtVaccinationDate.Text = o.VaccinationDate.ToString(dateformat.DateFormat);
                    ddlNonvaccinationReason.Visible = true;
                    ddlNonvaccinationReason.SelectedValue = o.NonvaccinationReasonId.ToString();
                }
            }
        }
    }

    protected void ddlVaccineLot_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlVaccineLot = (DropDownList)sender;
        GridViewRow gvVaccinationEvent = (GridViewRow)ddlVaccineLot.NamingContainer;
    }

    protected void chkDoneStatus_OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkDoneStatus = (CheckBox)sender;
        GridViewRow gvVaccinationEvent = (GridViewRow)chkDoneStatus.NamingContainer;
        DropDownList ddlVaccineLot = (DropDownList)gvVaccinationEvent.FindControl("ddlVaccineLot");

        ((DropDownList)gvVaccinationEvent.FindControl("ddlNonvaccinationReason")).Visible = !chkDoneStatus.Checked;
        if (!chkDoneStatus.Checked)
            ddlVaccineLot.SelectedIndex = 0; // ddlVaccineLot.Items.Count - 1;

    }

    protected void ddlHealthFacility_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id = 0;
        string _id = Request.QueryString["appId"];
        if (!String.IsNullOrEmpty(_id))
        {
            int.TryParse(_id, out id);
            //VaccinationEvent o = VaccinationEvent.GetVaccinationEventById(id);
            int hfId = int.Parse(ddlHealthFacility.SelectedValue);
            if (hfId != -1)
            {
                bool isUsed = SystemModule.GetSystemModuleByName("StockManagement").IsUsed;
                if (isUsed)
                {
                    if (gvVaccinationEvent.Rows.Count > 0)
                    {
                        foreach (GridViewRow gvr in gvVaccinationEvent.Rows)
                        {
                            if (gvr.RowType == DataControlRowType.DataRow)
                            {
                                int vid = int.Parse(gvr.Cells[0].Text);
                                VaccinationEvent o = VaccinationEvent.GetVaccinationEventById(vid);
                                DropDownList ddlVaccineLot = (DropDownList)gvr.FindControl("ddlVaccineLot");
                                ObjectDataSource odsItemLot = (ObjectDataSource)gvr.FindControl("odsItemLot");
                                string hfcode = HealthFacility.GetHealthFacilityById(int.Parse(ddlHealthFacility.SelectedValue)).Code;
                                odsItemLot.SelectParameters.Clear();
                                odsItemLot.SelectParameters.Add("itemId", o.Dose.ScheduledVaccination.ItemId.ToString());
                                odsItemLot.SelectParameters.Add("hfId", hfcode);
                                odsItemLot.DataBind();
                                ddlVaccineLot.DataBind();
                                if (ddlVaccineLot.Items.Count > 2)
                                    ddlVaccineLot.SelectedIndex = 2;
                                else
                                    ddlVaccineLot.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }

        }
    }
}
