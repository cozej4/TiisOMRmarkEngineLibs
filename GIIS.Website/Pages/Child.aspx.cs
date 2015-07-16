using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class _Child : System.Web.UI.Page
{
    static String birthplaceId;
    static String healthFacilityId;
    static String communityId;
    static String domicileId;
    static bool contRegistration = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        #region Validator

        string tmp = "";
        StringBuilder sb = new StringBuilder();

        //sb.AppendLine("<script type=\"text/javaScript\">");
        sb.AppendLine("function cvChild_Validate(sender, args) {");

        List<PersonConfiguration> pcList1 = PersonConfiguration.GetPersonConfigurationList();

        foreach (PersonConfiguration pc in pcList1)
            if (pc.IsMandatory)
            {
                if (pc.ColumnName.Contains("Health"))
                {
                    tmp += "(document.getElementById('ctl00_ContentPlaceHolder1_txtHealthcenterId_SBox').value == '') || ";
                    continue;
                }
                if (pc.ColumnName.Contains("Birthplace"))
                {
                    tmp += "(document.getElementById('ctl00_ContentPlaceHolder1_ddlBirthplace').value == '-1') || ";
                    continue;
                }
                if (pc.ColumnName.Contains("Community"))
                {
                    tmp += "(document.getElementById('ctl00_ContentPlaceHolder1_txtCommunityId_CBox').value == '') || ";
                    continue;
                }
                if (pc.ColumnName.Contains("Domicile"))
                {
                    tmp += "(document.getElementById('ctl00_ContentPlaceHolder1_txtDomicileId_DBox').value == '') || ";
                    continue;
                }
                if (!pc.ColumnName.Contains("Status") && !pc.ColumnName.Contains("Gender") && !pc.ColumnName.Contains("System"))
                    tmp += "(document.getElementById('ctl00_ContentPlaceHolder1_txt" + pc.ColumnName + "').value == '') || ";
            }
        sb.AppendLine("var text = " + tmp.Substring(0, tmp.Length - 3) + ";");//remove the last ||
        sb.AppendLine("if (text) {");
        sb.AppendLine("args.IsValid = false;");
        sb.AppendLine("return;");
        sb.AppendLine("}");
        sb.AppendLine("args.IsValid = true;");
        sb.AppendLine("}");
        //sb.AppendLine("</script>");

        Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), sb.ToString(), true);

        #endregion

        if (!this.Page.IsPostBack)
        {
            Page.DataBind();

            List<string> actionList = null;
            string sessionNameAction = "";
            if (CurrentEnvironment.LoggedUser != null)
            {
                sessionNameAction = "__GIS_actionList_" + CurrentEnvironment.LoggedUser.Id;
                actionList = (List<string>)Session[sessionNameAction];
            }

            if ((actionList != null) && actionList.Contains("ViewChild") && (CurrentEnvironment.LoggedUser != null))
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
                        Control rbl = FindMyControl(this, "rbl" + pc.ColumnName);
                        Control tr = FindMyControl(this, "tr" + pc.ColumnName);

                        if (lbl != null)
                            lbl.Visible = false;

                        if (txt != null)
                            txt.Visible = false;

                        if (rbl != null)
                            rbl.Visible = false;

                        if (tr != null)
                            tr.Visible = false;

                        //for (int i = 1; i <= gvChild.Columns.Count; i++)
                        //{
                        //    if (gvChild.Columns[i].HeaderText == pc.ColumnName)
                        //    {
                        //        gvChild.Columns[i].Visible = false;
                        //        break;
                        //    }
                        //}
                    }

                    Control span = FindMyControl(this, "span" + pc.ColumnName);
                    if (span != null)
                    {
                        span.Visible = pc.IsMandatory;
                    }
                }

                string id1 = Configuration.GetConfigurationByName("IdentificationNo1").Value;
                string id2 = Configuration.GetConfigurationByName("IdentificationNo2").Value;
                string id3 = Configuration.GetConfigurationByName("IdentificationNo3").Value;
                string revid1 = Configuration.GetConfigurationByName("IdentificationValidationExpression1").Value;
                string revid2 = Configuration.GetConfigurationByName("IdentificationValidationExpression2").Value;
                string revid3 = Configuration.GetConfigurationByName("IdentificationValidationExpression3").Value;

                #endregion

                #region Controls
                //controls
                this.lblBarcodeID.Text = wtList["BarcodeId"];
                this.lblTempId.Text = wtList["TempId"];
                this.lblSystemId.Text = wtList["ChildSystem"];
                this.lblFirstname1.Text = wtList["ChildFirstname1"];
                this.lblFirstname2.Text = wtList["ChildFirstname2"];
                this.lblLastname1.Text = wtList["ChildLastname1"];
                //this.lblLastname2.Text = wtList["ChildLastname2"];
                this.lblBirthdate.Text = wtList["ChildBirthdate"];
                this.lblGender.Text = wtList["ChildGender"];
                this.lblHealthcenterId.Text = wtList["ChildHealthcenter"];
                this.lblBirthplaceId.Text = wtList["ChildBirthplace"];
                this.lblCommunityId.Text = wtList["ChildCommunity"];
                this.lblDomicileId.Text = wtList["ChildDomicile"];
                this.lblStatusId.Text = wtList["ChildStatus"];
                this.lblAddress.Text = wtList["ChildAddress"];
                this.lblPhone.Text = wtList["ChildPhone"];
                //this.lblMobile.Text = wtList["ChildMobile"];
                //this.lblEmail.Text = wtList["ChildEmail"];
                //this.lblMotherId.Text = wtList["ChildMother"];
                this.lblMotherFirstname.Text = wtList["ChildMotherFirstname"];
                this.lblMotherLastname.Text = wtList["ChildMotherLastname"];
                //this.lblFatherId.Text = wtList["ChildFather"];
                //this.lblFatherFirstname.Text = wtList["ChildFatherFirstname"];
                //this.lblFatherLastname.Text = wtList["ChildFatherLastname"];
                //this.lblCaretakerId.Text = wtList["ChildCaretaker"];
                //this.lblCaretakerFirstname.Text = wtList["ChildCaretakerFirstname"];
                //this.lblCaretakerLastname.Text = wtList["ChildCaretakerLastname"];
                this.lblNotes.Text = wtList["ChildNotes"];
                //this.lblIsActive.Text = wtList["ChildIsActive"];
                this.lblIdentificationNo1.Text = id1;
                this.lblIdentificationNo2.Text = id2;
                this.lblIdentificationNo3.Text = id3;
                this.rblGender.Items[0].Text = wtList["ChildMale"];
                this.rblGender.Items[1].Text = wtList["ChildFemale"];
                #endregion

                //Page Title
                this.lblTitle.Text = wtList["ChildPageTitle"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddChild");
                this.btnEdit.Visible = actionList.Contains("EditChild");
                //this.btnRemove.Visible = actionList.Contains("RemoveChild");
                this.btnAddNew.Visible = actionList.Contains("AddNewChild");
                this.btnImmunizationCard.Visible = actionList.Contains("ViewImmunizationCard");

                //buttons
                this.btnAdd.Text = wtList["ChildAddButton"];
                this.btnEdit.Text = wtList["ChildEditButton"];
                //this.btnRemove.Text = wtList["ChildRemoveButton"];
                this.btnAddNew.Text = wtList["ChildAddNewButton"];
                this.btnImmunizationCard.Text = wtList["ChildImmunizationCardButton"];
                
                //message
                this.lblSuccess.Text = wtList["ChildSuccessText"];
                this.lblWarning.Text = wtList["ChildWarningText"];
                this.lblError.Text = wtList["ChildErrorText"];
                this.lblWarningBarcode.Text = wtList["ChildWarningBarcode"];

                //validators
                ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));

                ceBirthdate.Format = dateformat.DateFormat;
                revBirthdate.ErrorMessage = dateformat.DateFormat;
                revBirthdate.ValidationExpression = dateformat.DateExpresion;
                cvChild.ErrorMessage = wtList["ChildMandatory"];
                //revEmail.ErrorMessage = wtList["ChildEmailValidator"];
                revPhone.ErrorMessage = wtList["ChildPhoneValidator"];
                //revMobile.ErrorMessage = wtList["ChildMobileValidator"];
                revFirstname1.ErrorMessage = wtList["ChildFirstnameValidator"];
                //revFirstname2.ErrorMessage = wtList["ChildFirstnameValidator"];
                revLastname1.ErrorMessage = wtList["ChildLastnameValidator"];
                //revLastname2.ErrorMessage = wtList["ChildLastnameValidator"];
                cvBirthdate.ErrorMessage = wtList["ChildBirthdateValidator"];

                string value = revid1.Replace("$", "").Replace("[0-9]", "9");
                value = value.Replace("\\", "");
                value = value.Replace("[^0-9_|°¬!#%/()?¡¿+{}[]:.,;@ª^*<>=&-]", "X");
                revIdentification1.ErrorMessage = value;

                value = revid2.Replace("$", "").Replace("[0-9]", "9");
                value = value.Replace("\\", "");
                value = value.Replace("[^0-9_|°¬!#%/()?¡¿+{}[]:.,;@ª^*<>=&-]", "X");
                revIdentification2.ErrorMessage = value;

                value = revid3.Replace("$", "").Replace("[0-9]", "9");
                value = value.Replace("\\", "");
                value = value.Replace("[^0-9_|°¬!#%/()?¡¿+{}[]:.,;@ª^*<>=&-]", "X");
                revIdentification3.ErrorMessage = value;

                revIdentification1.ValidationExpression = revid1;
                revIdentification2.ValidationExpression = revid2;
                revIdentification3.ValidationExpression = revid3;
                ceBirthdate.EndDate = DateTime.Today.Date;
                //selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    lblSystemId.Visible = true;
                    txtSystemId.Visible = true;
                    Child o = Child.GetChildById(id);
                    txtSystemId.Text = o.SystemId;
                    txtFirstname1.Text = o.Firstname1;
                    txtFirstname2.Text = o.Firstname2;
                    txtLastname1.Text = o.Lastname1;
                    // txtLastname2.Text = o.Lastname2;
                    txtBirthdate.Text = o.Birthdate.ToString(dateformat.DateFormat.ToString());
                    rblGender.Items[0].Selected = o.Gender;
                    rblGender.Items[1].Selected = !o.Gender;
                    txtHealthcenterId.SelectedItemText = o.Healthcenter.Name.ToString();
                    healthFacilityId = o.HealthcenterId.ToString();
                    ddlStatus.SelectedValue = o.StatusId.ToString();
                    txtIdentificationNo1.Text = o.IdentificationNo1;
                    txtIdentificationNo2.Text = o.IdentificationNo2;
                    txtIdentificationNo3.Text = o.IdentificationNo3;
                    txtBarcodeId.Text = o.BarcodeId;
                    if (string.IsNullOrEmpty(o.BarcodeId))
                    {
                        if (!string.IsNullOrEmpty(o.TempId))
                        {
                            lblTempId.Visible = true;
                            txtTempId.Visible = true;
                            txtTempId.Text = o.TempId;
                        }
                    }
                    if (o.BirthplaceId > 0)
                    {
                        birthplaceId = o.BirthplaceId.ToString();
                        ddlBirthplace.SelectedValue = birthplaceId;
                    }
                    if (o.CommunityId > 0)
                    {
                        txtCommunityId.SelectedItemText = o.Community.Name.ToString();
                        communityId = o.CommunityId.ToString();
                    }
                    if (o.DomicileId != 0)
                    {
                        txtDomicileId.SelectedItemText = o.Domicile.Name.ToString();
                        domicileId = o.DomicileId.ToString();
                    }
                    txtAddress.Text = o.Address;
                    txtPhone.Text = o.Phone;
                    //txtMobile.Text = o.Mobile;
                    //txtEmail.Text = o.Email;
                    //txtMotherId.Text = o.MotherId;
                    txtMotherFirstname.Text = o.MotherFirstname;
                    txtMotherLastname.Text = o.MotherLastname;
                    //txtFatherId.Text = o.FatherId;
                    //txtFatherFirstname.Text = o.FatherFirstname;
                    //txtFatherLastname.Text = o.FatherLastname;
                    //txtCaretakerId.Text = o.CaretakerId;
                    //txtCaretakerFirstname.Text = o.CaretakerFirstname;
                    //txtCaretakerLastname.Text = o.CaretakerLastname;
                    txtNotes.Text = o.Notes;
                    odsVaccinationEvent.SelectParameters.Clear();
                    odsVaccinationEvent.SelectParameters.Add("childId", id.ToString());

                    int vcount = VaccinationAppointment.GetVaccinationAppointmentsByChildNotModified(o.Id).Count;
                    int count = VaccinationAppointment.GetVaccinationAppointmentsByChild(o.Id).Count;
                    if (vcount != count)
                        txtBirthdate.Enabled = false;

                    btnAdd.Visible = false;
                    btnAddNew.Visible = false;
                    btnImmunizationCard.Visible = true;
                    btnWeight.Visible = true;
                    btnAefi.Visible = true;
                }
                else
                {
                    //lblSystemId.Visible = false;
                    //txtSystemId.Visible = false;
                    lblTempId.Visible = false;
                    txtTempId.Visible = false;
                    btnEdit.Visible = false;
                    //btnRemove.Visible = false;
                    btnAddNew.Visible = false;
                    btnImmunizationCard.Visible = false;

                }
                if ((String)HttpContext.Current.Session["_successChild"] == "1")
                {
                    lblSuccess.Visible = true;
                    btnAddNew.Visible = true;
                    btnEdit.Visible = true;
                    //btnRemove.Visible = true;
                    btnImmunizationCard.Visible = true;
                }
                else if ((String)HttpContext.Current.Session["_successChild"] == "2")
                    lblSuccess.Visible = true;
                else
                {
                    lblSuccess.Visible = false;
                    btnAddNew.Visible = false;
                    //btnImmunizationCard.Visible = true;
                }

                HttpContext.Current.Session["_successChild"] = "0";
            }
            else
            {

                Response.Redirect("Default.aspx");
            }
        }
    }

    //protected void Places_ValueSelected(object sender, System.EventArgs e)
    //{
    //    birthplaceId = txtBirthplaceId.SelectedItemID.ToString();
    //}
    protected void Domicile_ValueSelected(object sender, System.EventArgs e)
    {
        domicileId = txtDomicileId.SelectedItemID.ToString();
    }
    protected void Communities_ValueSelected(object sender, System.EventArgs e)
    {
        communityId = txtCommunityId.SelectedItemID.ToString();
    }
    protected void HealthCenters_ValueSelected(object sender, System.EventArgs e)
    {
        healthFacilityId = txtHealthcenterId.SelectedItemID.ToString();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;

                birthplaceId = ddlBirthplace.SelectedValue;

                if (birthplaceId == null || Birthplace.GetBirthplaceById(int.Parse(birthplaceId)) == null)
                {
                    if (spanBirthplaceId.Visible == true)
                    {
                        revBirthplace.Visible = true;
                        return;
                    }
                }
                if (domicileId == null || Place.GetPlaceById(int.Parse(domicileId)) == null)
                {
                    if (spanDomicileId.Visible == true)
                    {
                        revDomicile.Visible = true;
                        return;
                    }
                }
                if (communityId == null || Community.GetCommunityById(int.Parse(communityId)) == null)
                {
                    if (spanCommunityId.Visible == true)
                    {
                        revCommunity.Visible = true;
                        return;
                    }
                }
                if (healthFacilityId == null || HealthFacility.GetHealthFacilityById(int.Parse(healthFacilityId)) == null)
                {
                    revHealthcenter.Visible = true;
                    return;
                }

                Child o = new Child();

                o.Firstname1 = txtFirstname1.Text.Trim();
                o.Firstname2 = txtFirstname2.Text.Trim();
                o.Lastname1 = txtLastname1.Text.Trim();
                // o.Lastname2 = txtLastname2.Text.Replace("'", @"''");
                DateTime date = DateTime.ParseExact(txtBirthdate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                o.Birthdate = date;
                o.Gender = bool.Parse(rblGender.SelectedValue);
                if (childExists(o.Lastname1, o.Gender, o.Birthdate) && (!contRegistration))
                    return;

                if (healthFacilityId != null && healthFacilityId != "0")
                {
                    o.HealthcenterId = int.Parse(healthFacilityId);
                }
                if (birthplaceId != null && birthplaceId != "0" && birthplaceId!= "-1")
                {
                    o.BirthplaceId = int.Parse(birthplaceId);
                }
                if (communityId != null && communityId != "0")
                {
                    o.CommunityId = int.Parse(communityId);
                }
                if (domicileId != null && domicileId != "0")
                {
                    o.DomicileId = int.Parse(domicileId);
                }

                o.StatusId = int.Parse(ddlStatus.SelectedValue);
                if (!String.IsNullOrEmpty(txtAddress.Text))
                    o.Address = txtAddress.Text;
                if (!String.IsNullOrEmpty(txtPhone.Text))
                    o.Phone = txtPhone.Text;
                //  o.Mobile = txtMobile.Text;
                //o.Email = txtEmail.Text.Replace("'", @"''");
                //  o.MotherId = txtMotherId.Text.Replace("'", @"''");
                if (!String.IsNullOrEmpty(txtMotherFirstname.Text))
                    o.MotherFirstname = txtMotherFirstname.Text;
                if (!String.IsNullOrEmpty(txtMotherLastname.Text))
                    o.MotherLastname = txtMotherLastname.Text;
                //o.FatherId = txtFatherId.Text.Replace("'", @"''");
                //o.FatherFirstname = txtFatherFirstname.Text.Replace("'", @"''");
                //o.FatherLastname = txtFatherLastname.Text.Replace("'", @"''");
                //o.CaretakerId = txtCaretakerId.Text.Replace("'", @"''");
                //o.CaretakerFirstname = txtCaretakerFirstname.Text.Replace("'", @"''");
                //o.CaretakerLastname = txtCaretakerLastname.Text.Replace("'", @"''");
                o.Notes = txtNotes.Text;
                o.IsActive = true; // bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                if (!String.IsNullOrEmpty(txtIdentificationNo1.Text))
                    o.IdentificationNo1 = txtIdentificationNo1.Text;
                if (!String.IsNullOrEmpty(txtIdentificationNo2.Text))
                    o.IdentificationNo2 = txtIdentificationNo2.Text;
                if (!String.IsNullOrEmpty(txtIdentificationNo3.Text))
                    o.IdentificationNo3 = txtIdentificationNo3.Text;
                if (!String.IsNullOrEmpty(txtBarcodeId.Text))
                {
                    o.BarcodeId = txtBarcodeId.Text.Trim();
                    if (Child.GetChildByBarcode(o.BarcodeId) != null)
                    {
                        lblWarningBarcode.Visible = true;
                        return;
                    }
                }
                if (!String.IsNullOrEmpty(txtTempId.Text))
                    o.TempId = txtTempId.Text.Trim();

                if (o.IdentificationNo1 != String.Empty && o.IdentificationNo1 != null)
                {
                    if (IdentificationNo1Exists(o.IdentificationNo1))
                        return;
                }
                if (o.IdentificationNo2 != String.Empty && o.IdentificationNo2 != null)
                {
                    if (IdentificationNo2Exists(o.IdentificationNo2))
                        return;
                }
                if (o.IdentificationNo3 != String.Empty && o.IdentificationNo3 != null)
                {
                    if (IdentificationNo3Exists(o.IdentificationNo3))
                        return;
                }

                o.SystemId = DateTime.Now.ToString("yyMMddhhmmss");
                int i = Child.Insert(o);

                if (i > 0)
                {

                    //add appointments
                    int j = VaccinationAppointment.InsertVaccinationsForChild(i, userId);
                    if (j > 0)
                    {
                        odsVaccinationEvent.SelectParameters.Clear();
                        odsVaccinationEvent.SelectParameters.Add("childId", i.ToString());

                        lblSuccess.Visible = true;
                        lblWarning.Visible = false;
                        lblError.Visible = false;
                        btnAddNew.Visible = true;
                        btnAdd.Visible = false;
                        btnImmunizationCard.Visible = false;
                        btnEdit.Visible = true;
                        //btnRemove.Visible = true;
                        lblWarningBarcode.Visible = false;
                        lnbContinue.Visible = false;

                        HttpContext.Current.Session["_successChild"] = "1";
                        HttpContext.Current.Session["_lastChildId"] = i;
                    }
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = false;
                    lblError.Visible = true;
                    btnAddNew.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
            btnAddNew.Visible = false;
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int id = -1;
                string _id = Request.QueryString["id"];
                if (String.IsNullOrEmpty(_id))
                {
                    if (HttpContext.Current.Session["_lastChildId"] != null)
                        id = (int)HttpContext.Current.Session["_lastChildId"];
                }
                else
                    int.TryParse(_id, out id);
                int userId = CurrentEnvironment.LoggedUser.Id;

                birthplaceId = ddlBirthplace.SelectedValue;
                if (birthplaceId == null || Birthplace.GetBirthplaceById(int.Parse(birthplaceId)) == null)
                {
                    if (spanBirthplaceId.Visible == true)
                    {
                        revBirthplace.Visible = true;
                        return;
                    }
                }
                if (domicileId == null || Place.GetPlaceById(int.Parse(domicileId)) == null)
                {
                    if (spanDomicileId.Visible == true)
                    {
                        revDomicile.Visible = true;
                        return;
                    }
                }
                if (communityId == null || Community.GetCommunityById(int.Parse(communityId)) == null)
                {
                    if (spanCommunityId.Visible == true)
                    {
                        revCommunity.Visible = true;
                        return;
                    }
                }
                if (healthFacilityId == null || HealthFacility.GetHealthFacilityById(int.Parse(healthFacilityId)) == null)
                {
                    revHealthcenter.Visible = true;
                    return;
                }

                Child o = Child.GetChildById(id);

                o.Firstname1 = txtFirstname1.Text.Trim();
                o.Firstname2 = txtFirstname2.Text.Trim();
                o.Lastname1 = txtLastname1.Text.Trim();
                //o.Lastname2 = txtLastname2.Text.Replace("'", @"''");

                DateTime date = DateTime.ParseExact(txtBirthdate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                int datediff = Int32.MaxValue;
                if (o.Birthdate != date)
                {
                    if (o.Birthdate.Year != date.Year)
                        datediff = date.Subtract(o.Birthdate).Days;
                    else
                        datediff = date.Subtract(o.Birthdate).Days;
                }
                o.Birthdate = date;

                o.Gender = bool.Parse(rblGender.SelectedValue);

                int healthcenter = 0;
                if (o.HealthcenterId != int.Parse(healthFacilityId))
                {
                    healthcenter = int.Parse(healthFacilityId);
                }
                o.HealthcenterId = int.Parse(healthFacilityId);

                if (birthplaceId != null && birthplaceId != "0" && birthplaceId != "-1" )
                {
                    o.BirthplaceId = int.Parse(birthplaceId);
                }
                else
                    o.BirthplaceId = null;
                if (communityId != null && communityId != "0")
                {
                    o.CommunityId = int.Parse(communityId);
                }
                else
                    o.CommunityId = null;
                if (domicileId != null && domicileId != "0")
                {
                    o.DomicileId = int.Parse(domicileId);
                }
                else
                    o.DomicileId = null;
                o.StatusId = int.Parse(ddlStatus.SelectedValue);
                bool appstatus = true;
                if (o.StatusId != 1) //== 2 || o.StatusId == 3 || o.StatusId == 4 )
                    appstatus = false;
                if (!String.IsNullOrEmpty(txtAddress.Text))
                    o.Address = txtAddress.Text;
                if (!String.IsNullOrEmpty(txtPhone.Text))
                    o.Phone = txtPhone.Text;
                //  o.Mobile = txtMobile.Text;
                //o.Email = txtEmail.Text.Replace("'", @"''");
                //  o.MotherId = txtMotherId.Text.Replace("'", @"''");
                if (!String.IsNullOrEmpty(txtMotherFirstname.Text))
                    o.MotherFirstname = txtMotherFirstname.Text;
                if (!String.IsNullOrEmpty(txtMotherLastname.Text))
                    o.MotherLastname = txtMotherLastname.Text;
                //o.FatherId = txtFatherId.Text.Replace("'", @"''");
                //o.FatherFirstname = txtFatherFirstname.Text.Replace("'", @"''");
                //o.FatherLastname = txtFatherLastname.Text.Replace("'", @"''");
                //o.CaretakerId = txtCaretakerId.Text.Replace("'", @"''");
                //o.CaretakerFirstname = txtCaretakerFirstname.Text.Replace("'", @"''");
                //o.CaretakerLastname = txtCaretakerLastname.Text.Replace("'", @"''");
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = true; // bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                if (!String.IsNullOrEmpty(txtIdentificationNo1.Text))
                    o.IdentificationNo1 = txtIdentificationNo1.Text;
                if (!String.IsNullOrEmpty(txtIdentificationNo2.Text))
                    o.IdentificationNo2 = txtIdentificationNo2.Text;
                if (!String.IsNullOrEmpty(txtIdentificationNo3.Text))
                    o.IdentificationNo3 = txtIdentificationNo3.Text;
                if (!String.IsNullOrEmpty(txtBarcodeId.Text))
                {
                    //if (!o.BarcodeId.Equals(txtBarcodeId.Text.Trim())
                    //{
                    //    // This child already has a barcode are you sure you want to assign another one
                    //}
                    o.BarcodeId = txtBarcodeId.Text.Trim();

                    if (Child.GetChildByBarcode(o.BarcodeId) != null && Child.GetChildByBarcode(o.BarcodeId).Id != id)
                    {
                        lblWarningBarcode.Visible = true;
                        return;
                    }
                }
                if (!String.IsNullOrEmpty(txtTempId.Text))
                    o.TempId = txtTempId.Text.Trim();

                int i = Child.Update(o);

                if (i > 0)
                {
                    List<VaccinationAppointment> applist = VaccinationAppointment.GetVaccinationAppointmentsByChildNotModified(id);
                    List<VaccinationAppointment> applistall = VaccinationAppointment.GetVaccinationAppointmentsByChild(id);
                    if (!appstatus)
                    {
                        foreach (VaccinationAppointment app in applist)
                            VaccinationAppointment.Update(appstatus, app.Id);
                    }

                    if (healthcenter != 0)
                    {
                        foreach (VaccinationAppointment app in applist)
                        {
                            VaccinationAppointment.Update(o.HealthcenterId, app.Id);
                            VaccinationEvent.Update(app.Id, o.HealthcenterId);
                        }

                    }
                    if (datediff != Int32.MaxValue)
                    {
                        bool done = false;
                        foreach (VaccinationAppointment app in applistall)
                        {
                            VaccinationEvent ve = VaccinationEvent.GetVaccinationEventByAppointmentId(app.Id)[0];
                            if (ve.VaccinationStatus || ve.NonvaccinationReasonId != 0)
                            {
                                done = true;
                                break;
                            }
                        }

                        foreach (VaccinationAppointment app in applist)
                        {
                            if (done)
                                break;
                            VaccinationAppointment.Update(app.ScheduledDate.AddDays(datediff), app.Id);
                            VaccinationEvent.Update(app.Id, app.ScheduledDate.AddDays(datediff));
                        }


                    }
                    HttpContext.Current.Session["_successUpdateChild"] = "1";

                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
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

    //protected void btnRemove_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int id = -1;
    //        string _id = Request.QueryString["id"];
    //        if (String.IsNullOrEmpty(_id))
    //        {
    //            if (HttpContext.Current.Session["_lastChildId"] != null)
    //                id = (int)HttpContext.Current.Session["_lastChildId"];
    //        }
    //        else
    //            int.TryParse(_id, out id);
    //        int userId = CurrentEnvironment.LoggedUser.Id;

    //        VaccinationEvent.DeleteByChild(id);
    //        VaccinationAppointment.DeleteByChild(id);
    //        int i = Child.Delete(id);

    //        if (i > 0)
    //        {
    //            HttpContext.Current.Session["_successChild"] = "2";
    //            lblSuccess.Visible = true;
    //            lblWarning.Visible = false;
    //            lblError.Visible = false;
    //            ClearControls(this);
    //            gvVaccinationEvent.Visible = false;
    //            btnEdit.Visible = false;
    //            btnRemove.Visible = false;
    //            btnAddNew.Visible = false;
    //            btnAdd.Visible = true;
    //            btnImmunizationCard.Visible = false;
    //            Response.Redirect("Child.aspx", false);
    //        }
    //        else
    //        {
    //            lblSuccess.Visible = false;
    //            lblWarning.Visible = true;
    //            lblError.Visible = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblSuccess.Visible = false;
    //        lblWarning.Visible = false;
    //        lblError.Visible = true;
    //    }
    //}

    private void ClearControls(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
                ClearControls(c);
            else
            {
                if (c is TextBox)
                    (c as TextBox).Text = "";
                if (c is DropDownList)
                    (c as DropDownList).SelectedIndex = 0;
            }
        }
    }

    private static Control FindMyControl(Control Root, string Id)
    {

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
    protected bool childExists(string lastname, bool gender, DateTime birthdate)
    {
        if (Child.GetPersonByLastnameBirthdate(lastname, birthdate) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lnbContinue.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
    protected bool IdentificationNo1Exists(string id)
    {
        if (Child.GetPersonIdentification1(id) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
    protected bool IdentificationNo2Exists(string id)
    {
        if (Child.GetPersonIdentification2(id) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
    protected bool IdentificationNo3Exists(string id)
    {
        if (Child.GetPersonIdentification3(id) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
    protected void lnbContinue_Click(object sender, EventArgs e)
    {
        contRegistration = true;
        btnAdd_Click(sender, e);
    }

    protected void gvVaccinationEvent_DataBound(object sender, EventArgs e)
    {
        if (gvVaccinationEvent.Rows.Count > 0)
        {

            foreach (GridViewRow gvr in gvVaccinationEvent.Rows)
            {
                gvr.Cells[0].Visible = false;
            }
            gvVaccinationEvent.HeaderRow.Cells[0].Visible = false;
            //int id = 0;
            //DateTime sdate; 
            //foreach (GridViewRow gvr in gvVaccinationEvent.Rows)
            //{
            //    int appid = int.Parse(gvr.Cells[0].Text);
            //    if (id == appid)

            //}
        }
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        lblSuccess.Visible = false;
        HttpContext.Current.Session["_successChild"] = "0";
        Response.Redirect(Request.RawUrl, false);
    }
    protected void btnImmunizationCard_Click(object sender, EventArgs e)
    {
        int Cid = 0;
        string _id = Request.QueryString["id"];
        if (String.IsNullOrEmpty(_id))
        {
            if (HttpContext.Current.Session["_lastChildId"] != null)
                Cid = (int)HttpContext.Current.Session["_lastChildId"];
        }
        else
            int.TryParse(_id, out Cid);

        string url = string.Format("ViewImmunizationCard.aspx?id={0}", Cid);
        Response.Redirect(url, false);
        Context.ApplicationInstance.CompleteRequest();
        HttpContext.Current.Session["_lastChildId"] = null;
    }

    protected void ValidateBirthdate(object sender, ServerValidateEventArgs e)
    {
        ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));

        DateTime date = DateTime.ParseExact(txtBirthdate.Text, dateformat.DateFormat, CultureInfo.InvariantCulture);

        e.IsValid = date <= DateTime.Today;
    }
    //protected void ValidateBirthdate(object sender, ServerValidateEventArgs e)
    //{
    //    ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
    //    DateTimeFormatInfo dateFormatProvider = new DateTimeFormatInfo();
    //    dateFormatProvider.ShortDatePattern = "dd/MM/yyyy";
    //    DateTime date = DateTime.Parse(txtBirthdate.Text, dateFormatProvider);

    //    e.IsValid = date <= DateTime.Today;
    //}

    protected void btnWeight_Click(object sender, EventArgs e)
    {
        int Cid = 0;
        string _id = Request.QueryString["id"];
        if (String.IsNullOrEmpty(_id))
        {
            if (HttpContext.Current.Session["_lastChildId"] != null)
            {
                Cid = (int)HttpContext.Current.Session["_lastChildId"];

            }
        }
        else
        {
            int.TryParse(_id, out Cid);
        }
        string url = string.Format("ChildWeight.aspx?id={0}", Cid);
        Response.Redirect(url, false);
        Context.ApplicationInstance.CompleteRequest();
        HttpContext.Current.Session["_lastChildId"] = null;
    }
    protected void btnAefi_Click(object sender, EventArgs e)
    {
        int Cid = 0;
        string _id = Request.QueryString["id"];
        if (String.IsNullOrEmpty(_id))
        {
            if (HttpContext.Current.Session["_lastChildId"] != null)
            {
                Cid = (int)HttpContext.Current.Session["_lastChildId"];

            }
        }
        else
        {
            int.TryParse(_id, out Cid);
        }
        string url = string.Format("ChildAEFI.aspx?id={0}", Cid);
        Response.Redirect(url, false);
        Context.ApplicationInstance.CompleteRequest();
        HttpContext.Current.Session["_lastChildId"] = null;
    }
   
}