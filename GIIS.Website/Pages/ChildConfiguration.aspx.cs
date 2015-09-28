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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _Child : System.Web.UI.Page
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
            //int userId = CurrentEnvironment.LoggedUser.Id;
            if ((actionList != null) && actionList.Contains("ViewPersonConfiguration"))
            {
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

                Dictionary<string, string> wtListConfiguration = (Dictionary<string, string>)HttpContext.Current.Cache["ChildConfiguration-dictionary" + language];
                if (wtListConfiguration == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ChildConfiguration");
                    wtListConfiguration = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtListConfiguration.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("ChildConfiguration-dictionary" + language, wtListConfiguration);
                }
                #region Controls
                this.chbSystemId.Text = wtList["ChildSystem"];
                this.chbFirstname1.Text = wtList["ChildFirstname1"];
                this.chbFirstname2.Text = wtList["ChildFirstname2"];
                this.chbLastname1.Text = wtList["ChildLastname1"];
                this.chbLastname2.Text = wtList["ChildLastname2"];
                this.chbBirthdate.Text = wtList["ChildBirthdate"];
                this.chbGender.Text = wtList["ChildGender"];
                this.chbHealthcenterId.Text = wtList["ChildHealthcenter"];
                this.chbBirthplaceId.Text = wtList["ChildBirthplace"];
                this.chbCommunityId.Text = wtList["ChildCommunity"];
                this.chbDomicileId.Text = wtList["ChildDomicile"];
                this.chbStatusId.Text = wtList["ChildStatus"];
                this.chbAddress.Text = wtList["ChildAddress"];
                this.chbPhone.Text = wtList["ChildPhone"];
                this.chbMobile.Text = wtList["ChildMobile"];
                this.chbEmail.Text = wtList["ChildEmail"];
                this.chbMotherId.Text = wtList["ChildMother"];
                this.chbMotherFirstname.Text = wtList["ChildMotherFirstname"];
                this.chbMotherLastname.Text = wtList["ChildMotherLastname"];
                this.chbFatherId.Text = wtList["ChildFather"];
                this.chbFatherFirstname.Text = wtList["ChildFatherFirstname"];
                this.chbFatherLastname.Text = wtList["ChildFatherLastname"];
                this.chbCaretakerId.Text = wtList["ChildCaretaker"];
                this.chbCaretakerFirstname.Text = wtList["ChildCaretakerFirstname"];
                this.chbCaretakerLastname.Text = wtList["ChildCaretakerLastname"];
                this.chbNotes.Text = wtList["ChildNotes"];
                //this.chbIsActive.Text = wtList["ChildIsActive"];

                this.chbMandatorySystemId.Text = wtList["ChildSystem"];
                this.chbMandatoryFirstname1.Text = wtList["ChildFirstname1"];
                this.chbMandatoryFirstname2.Text = wtList["ChildFirstname2"];
                this.chbMandatoryLastname1.Text = wtList["ChildLastname1"];
                this.chbMandatoryLastname2.Text = wtList["ChildLastname2"];
                this.chbMandatoryBirthdate.Text = wtList["ChildBirthdate"];
                this.chbMandatoryGender.Text = wtList["ChildGender"];
                this.chbMandatoryHealthcenterId.Text = wtList["ChildHealthcenter"];
                this.chbMandatoryBirthplaceId.Text = wtList["ChildBirthplace"];
                this.chbMandatoryCommunityId.Text = wtList["ChildCommunity"];
                this.chbMandatoryDomicileId.Text = wtList["ChildDomicile"];
                this.chbMandatoryStatusId.Text = wtList["ChildStatus"];
                this.chbMandatoryAddress.Text = wtList["ChildAddress"];
                this.chbMandatoryPhone.Text = wtList["ChildPhone"];
                this.chbMandatoryMobile.Text = wtList["ChildMobile"];
                this.chbMandatoryEmail.Text = wtList["ChildEmail"];
                this.chbMandatoryMotherId.Text = wtList["ChildMother"];
                this.chbMandatoryMotherFirstname.Text = wtList["ChildMotherFirstname"];
                this.chbMandatoryMotherLastname.Text = wtList["ChildMotherLastname"];
                this.chbMandatoryFatherId.Text = wtList["ChildFather"];
                this.chbMandatoryFatherFirstname.Text = wtList["ChildFatherFirstname"];
                this.chbMandatoryFatherLastname.Text = wtList["ChildFatherLastname"];
                this.chbMandatoryCaretakerId.Text = wtList["ChildCaretaker"];
                this.chbMandatoryCaretakerFirstname.Text = wtList["ChildCaretakerFirstname"];
                this.chbMandatoryCaretakerLastname.Text = wtList["ChildCaretakerLastname"];
                this.chbMandatoryNotes.Text = wtList["ChildNotes"];
                //this.chbMandatoryIsActive.Text = wtList["ChildIsActive"];

                #endregion

                GetControlsValue(this);

                Configuration c1 = Configuration.GetConfigurationByName("IdentificationNo1");
                txtIdentificationNo1.Text = c1.Value;

                Configuration c2 = Configuration.GetConfigurationByName("IdentificationNo2");
                txtIdentificationNo2.Text = c2.Value;

                Configuration c3 = Configuration.GetConfigurationByName("IdentificationNo3");
                txtIdentificationNo3.Text = c3.Value;

                Configuration cc1 = Configuration.GetConfigurationByName("IdentificationValidationExpression1");
                string value = cc1.Value.Replace("$", "").Replace("[0-9]", "9");
                value = value.Replace("\\", "");
                value = value.Replace("[^0-9_|°¬!#%/()?¡¿+{}[]:.,;@ª^*<>=&-]", "X");
                txtValidatorIdentificationNo1.Text = value;

                Configuration cc2 = Configuration.GetConfigurationByName("IdentificationValidationExpression2");
                 value = cc2.Value.Replace("$", "").Replace("[0-9]", "9");
                value = value.Replace("\\", "");
                value = value.Replace("[^0-9_|°¬!#%/()?¡¿+{}[]:.,;@ª^*<>=&-]", "X");
                txtValidatorIdentificationNo2.Text = value;

                Configuration cc3 = Configuration.GetConfigurationByName("IdentificationValidationExpression3");
                value = cc3.Value.Replace("$", "").Replace("[0-9]", "9");
                value = value.Replace("\\", "");
                value = value.Replace("[^0-9_|°¬!#%/()?¡¿+{}[]:.,;@ª^*<>=&-]", "X");
                txtValidatorIdentificationNo3.Text = value;

                lblTitle.Text = wtListConfiguration["ChildConfigurationPageTitle"];
                lblSuccess.Text = wtList["ChildSuccessText"];
                lblError.Text = wtList["ChildErrorText"];
                btnSave.Text = wtListConfiguration["ChildConfigurationSaveButton"];

                TabPanel1.HeaderText = wtListConfiguration["ChildConfigurationVisibleFields"];
                TabPanel2.HeaderText = wtListConfiguration["ChildConfigurationMandatoryFields"];
                TabPanel3.HeaderText = wtListConfiguration["ChildConfigurationExtraFields"];
                chbIdentificationNo1.Text = wtListConfiguration["ChildConfigurationVisible"];
                chbMandatoryIdentificationNo1.Text = wtListConfiguration["ChildConfigurationMandatory"];
                chbIdentificationNo2.Text = wtListConfiguration["ChildConfigurationVisible"];
                chbMandatoryIdentificationNo2.Text = wtListConfiguration["ChildConfigurationMandatory"];
                chbIdentificationNo3.Text = wtListConfiguration["ChildConfigurationVisible"];
                chbMandatoryIdentificationNo3.Text = wtListConfiguration["ChildConfigurationMandatory"];
                lblFieldLabel.Text = wtListConfiguration["ChildConfigurationFieldLabel"];
                lblFieldFormat.Text = wtListConfiguration["ChildConfigurationFieldFormat"];

            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                //int userId = CurrentEnvironment.LoggedUser.Id;
                bool b = SetControlsValue(this);

                Configuration c1 = Configuration.GetConfigurationByName("IdentificationNo1");
                c1.Value = txtIdentificationNo1.Text;
                Configuration.Update(c1);

                Configuration c2 = Configuration.GetConfigurationByName("IdentificationNo2");
                c2.Value = txtIdentificationNo2.Text;
                Configuration.Update(c2);

                Configuration c3 = Configuration.GetConfigurationByName("IdentificationNo3");
                c3.Value = txtIdentificationNo3.Text;
                Configuration.Update(c3);

                Configuration cc1 = Configuration.GetConfigurationByName("IdentificationValidationExpression1");
                cc1.Value = CreateValidator(txtValidatorIdentificationNo1.Text);
                Configuration.Update(cc1);

                Configuration cc2 = Configuration.GetConfigurationByName("IdentificationValidationExpression2");
                cc2.Value = CreateValidator(txtValidatorIdentificationNo2.Text);
                Configuration.Update(cc2);

                Configuration cc3 = Configuration.GetConfigurationByName("IdentificationValidationExpression3");
                cc3.Value = CreateValidator(txtValidatorIdentificationNo3.Text);
                Configuration.Update(cc3);

                if (b)
                {
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

    private bool SetControlsValue(Control parent)
    {
        // Create new stopwatch
        Stopwatch stopwatch = new Stopwatch();

        // Begin timing
        stopwatch.Start();

       

        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
                SetControlsValue(c);
            else
            {
                if (c is CheckBox)
                {
                    CheckBox chb = new CheckBox();
                    chb = (c as CheckBox);

                    string s = chb.ID;

                    PersonConfiguration pc = PersonConfiguration.GetPersonConfigurationByColumnName(s.Replace("chb", ""));
                    PersonConfiguration pcMandatory = PersonConfiguration.GetPersonConfigurationByColumnName(s.Replace("chbMandatory", ""));

                    if (pc != null)
                    {                        
                        pc.IsVisible = chb.Checked;
                        PersonConfiguration.Update(pc);
                    }

                    if (pcMandatory != null)
                    {
                        pcMandatory.IsMandatory = chb.Checked;
                        PersonConfiguration.Update(pcMandatory);
                    }
                }
            }
        }

        // Stop timing
        stopwatch.Stop();

        // Write result
        string sss = stopwatch.Elapsed.ToString();

        return true;
    }

    private void GetControlsValue(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
                GetControlsValue(c);
            else
            {
                if (c is CheckBox)
                {
                    CheckBox chb = new CheckBox();
                    chb = (c as CheckBox);

                    string s = chb.ID;

                    PersonConfiguration pc = PersonConfiguration.GetPersonConfigurationByColumnName(s.Replace("chb", ""));
                    PersonConfiguration pcMandatory = PersonConfiguration.GetPersonConfigurationByColumnName(s.Replace("chbMandatory", ""));

                    if (pc != null)
                    {
                        chb.Checked = pc.IsVisible;
                    }

                    if (pcMandatory != null)
                    {
                        chb.Checked = pcMandatory.IsMandatory;
                    }
                }
            }
        }
    }

    private string CreateValidator(string s)
    {
        char[] arr = s.ToCharArray();

        string result = "";

        foreach (char c in arr)
        {
            if (IsNumber(c))
            {
                result += "[0-9]";
            }
            else if (IsChar(c))
            {
                result += @"[^0-9_\|°¬!#\$%/\()\?¡¿+{}[\]:.\,;@ª^\*<>=&-]";
            }
            else
            {
                result += c;
            }
        }

        return result + "$";
    }

    private bool IsNumber(char c)
    {
        Regex regex = new Regex(@"[0-9]");
        return regex.IsMatch(c.ToString());
    }

    private bool IsChar(char c)
    {
        Regex regex = new Regex(@"[^0-9_\|°¬!#\$%/\()\?¡¿+{}[\]:.\,;@ª^\*<>=&-]");
        return regex.IsMatch(c.ToString());
    }
}