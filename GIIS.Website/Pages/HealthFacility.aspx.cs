using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;


public partial class _HealthFacility : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewHealthFacility") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["HealthFacility-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "HealthFacility");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("HealthFacility-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["HealthFacilityName"];
                this.lblCode.Text = wtList["HealthFacilityCode"];
                this.lblParentId.Text = wtList["HealthFacilityParent"];
                this.lblTopLevel.Text = wtList["HealthFacilityTopLevel"];
                this.lblLeaf.Text = wtList["HealthFacilityLeaf"];
                this.lblNotes.Text = wtList["HealthFacilityNotes"];
                this.lblIsActive.Text = wtList["HealthFacilityIsActive"];
                this.lblVaccinationPoint.Text = wtList["HealthFacilityVaccinationPoint"];
                this.lblAddress.Text = wtList["HealthFacilityAddress"];
                this.lblContact.Text = wtList["HealthFacilityContact"];
                this.lblType.Text = wtList["HealthFacilityType"];
                this.lblOwnership.Text = wtList["HealthFacilityOwnership"];
                this.lblColdStorageCapacity.Text = wtList["HealthFacilityColdStorageCap"];
                this.rblIsActive.Items[0].Text = wtList["HealthFacilityYes"];
                this.rblIsActive.Items[1].Text = wtList["HealthFacilityNo"];
                this.rblLeaf.Items[0].Text = wtList["HealthFacilityYes"];
                this.rblLeaf.Items[1].Text = wtList["HealthFacilityNo"];
                this.rblTopLevel.Items[0].Text = wtList["HealthFacilityYes"];
                this.rblTopLevel.Items[1].Text = wtList["HealthFacilityNo"];
                this.lblVaccineStore.Text = wtList["HealthFacilityVaccineStore"];
                this.rblVaccineStore.Items[0].Text = wtList["HealthFacilityYes"];
                this.rblVaccineStore.Items[1].Text = wtList["HealthFacilityNo"];
               
                //grid header text
                gvHealthFacility.Columns[1].HeaderText = wtList["HealthFacilityName"];
                gvHealthFacility.Columns[2].HeaderText = wtList["HealthFacilityCode"];
                gvHealthFacility.Columns[3].HeaderText = wtList["HealthFacilityParent"];
                gvHealthFacility.Columns[4].HeaderText = wtList["HealthFacilityTopLevel"];
                gvHealthFacility.Columns[5].HeaderText = wtList["HealthFacilityLeaf"];
                gvHealthFacility.Columns[6].HeaderText = wtList["HealthFacilityVaccinationPoint"];
                gvHealthFacility.Columns[7].HeaderText = wtList["HealthFacilityVaccineStore"];
                gvHealthFacility.Columns[9].HeaderText = wtList["HealthFacilityIsActive"];
                gvHealthFacility.Columns[14].HeaderText = wtList["HealthFacilityAddUserButton"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddHealthFacility");
                this.btnEdit.Visible = actionList.Contains("EditHealthFacility");
                this.btnRemove.Visible = actionList.Contains("RemoveHealthFacility");
                //this.btnAddUser.Visible = actionList.Contains("HealthFacilityAddUser");

                //buttons
                this.btnAdd.Text = wtList["HealthFacilityAddButton"];
                this.btnEdit.Text = wtList["HealthFacilityEditButton"];
                this.btnRemove.Text = wtList["HealthFacilityRemoveButton"];
              //this.btnAddUser.Text = wtList["HealthFacilityAddUserButton"];

                //Page Title
                this.lblTitle.Text = wtList["HealthFacilityPageTitle"];

                //message
                this.lblSuccess.Text = wtList["HealthFacilitySuccessText"];
                this.lblWarning.Text = wtList["HealthFacilityWarningText"];
                this.lblError.Text = wtList["HealthFacilityErrorText"];
                if ((String)HttpContext.Current.Session["_successHealthfacility"] == "1")
                    lblSuccess.Visible = true;
                else
                    lblSuccess.Visible = false;

                HttpContext.Current.Session["_successHealthfacility"] = "0";

                //validators
                cvHealthFacility.ErrorMessage = wtList["HealthFacilityMandatory"];
                revHealthFacility.Text = wtList["HealthFacilityParentValidator"];
                revName.ErrorMessage = wtList["HealthFacilityNameValidator"];
               // revCode.ErrorMessage = wtList["HealthFacilityCodeValidator"];
                revCapacity.ErrorMessage = wtList["HealthFacilityCapacityValidator"];
                revPhone.ErrorMessage = wtList["HealthFacilityContactValidator"];
                cvLevels.ErrorMessage = wtList["HealthFacilityLevelValidator"];

                //Selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    HealthFacility o = HealthFacility.GetHealthFacilityById(id);
                    txtName.Text = o.Name;
                    txtCode.Text = o.Code;
                    if (o.Parent != null)
                        txtParentId.SelectedItemText = o.Parent.Name.ToString();
                    txtNotes.Text = o.Notes;
                    rblTopLevel.SelectedValue = o.TopLevel.ToString();
                    rblLeaf.SelectedValue = o.Leaf.ToString();
                    rblVaccinationPoint.SelectedValue = o.VaccinationPoint.ToString();
                    rblVaccineStore.SelectedValue = o.VaccineStore.ToString();
                    rblIsActive.SelectedValue = o.IsActive.ToString();
                    txtAddress.Text = o.Address;
                    txtContact.Text = o.Contact;
                    txtColdStorageCapacity.Text = o.ColdStorageCapacity.ToString();
                    if (o.TypeId != null)
                    ddlType.SelectedValue = o.TypeId.ToString();
                    ddlOwnership.SelectedValue = o.Ownership.ToString();

                    gridview_Databind(id);
                    healthCenterId = o.ParentId.ToString();
                    btnAdd.Visible = false;
                   // btnAddUser.Visible = false;
                    //HttpContext.Current.Session["_lastHealthfacility"] = id;
                }
                else
                {
                    if (HttpContext.Current.Session["_lastHealthfacility"] != null)
                    {
                        int i = (int)HttpContext.Current.Session["_lastHealthfacility"];
                        gridview_Databind(i);
                       // btnAddUser.Visible = true;
                        btnEdit.Visible = false;
                        btnRemove.Visible = false;
                        HttpContext.Current.Session["_lastHealthfacility"] = null;
                      //  int max = HealthFacility.GetHealthFacilityMaxId();
                      //  int lcode = int.Parse(HealthFacility.GetHealthFacilityById(max).Code);
                      //  lcode += 1;
                      //string code = lcode.ToString();
                      //  switch (code.ToString().Length)
                      //  {
                      //      case 1:
                      //          code = "0000" + code;
                      //          break;
                      //      case 2:
                      //          code = "000" + code;
                      //          break;
                      //      case 3:
                      //          code = "00" + code;
                      //          break;
                      //      case 4:
                      //          code = "0" + code;
                      //          break;
                      //  }
                        //txtCode.Text = code;   
                    }
                    else
                    {
                        btnEdit.Visible = false;
                        btnRemove.Visible = false;
                       // btnAddUser.Visible = false;

                        //new code

                        //int max = HealthFacility.GetHealthFacilityMaxId();
                        //int lcode = int.Parse(HealthFacility.GetHealthFacilityById(max).Code);
                        //lcode += 1;
                        //string code = lcode.ToString();
                        //switch (code.ToString().Length)
                        //{
                        //    case 1:
                        //        code = "0000" + code;
                        //        break;
                        //    case 2:
                        //        code = "000" + code;
                        //        break;
                        //    case 3:
                        //        code = "00" + code;
                        //        break;
                        //    case 4:
                        //        code = "0" + code;
                        //        break;
                        //}
                        //txtCode.Text = code;   
                    }
                }

                if (HealthFacility.ExistsTopLevel() > 0)
                    rblTopLevel.Enabled = false;

            }
            else
            {
                Response.Redirect("Default.aspx", false);
            }
        }
    }

    protected void gridview_Databind(int id)
    {
        odsHealthFacility.SelectParameters.Clear();
        odsHealthFacility.SelectParameters.Add("i", id.ToString());
        odsHealthFacility.DataBind();
    }

    protected void cvRadioButtons_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if ((rblTopLevel.Items[0].Selected == true) && (rblLeaf.Items[0].Selected == true))
        {
            args.IsValid = false;
        }
    }

    protected void HealthCenters_ValueSelected(object sender, System.EventArgs e)
    {
        healthCenterId = txtParentId.SelectedItemID.ToString();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                revHealthFacility.Visible = false;
                if (nameExists(txtName.Text.Replace("'", @"''")))
                    return;
                if (codeExists(txtCode.Text.Trim()))
                    return;

                HealthFacility o = new HealthFacility();
                if (healthCenterId == null || (HealthFacility.GetHealthFacilityById(int.Parse(healthCenterId)) == null))
                {
                    revHealthFacility.Visible = true;
                    return;
                }

                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text;
                o.ParentId = int.Parse(healthCenterId);
                o.TopLevel = bool.Parse(rblTopLevel.SelectedValue);
                o.Leaf = bool.Parse(rblLeaf.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                o.VaccinationPoint = bool.Parse(rblVaccinationPoint.SelectedValue);
                o.VaccineStore = bool.Parse(rblVaccineStore.SelectedValue);
                o.Address = txtAddress.Text.Replace("'", @"''");
                o.Contact = txtContact.Text;
                double cs = 0;
                double.TryParse(txtColdStorageCapacity.Text, out cs);
                o.ColdStorageCapacity = cs;
                o.TypeId = int.Parse(ddlType.SelectedValue);
                o.Ownership = int.Parse(ddlOwnership.SelectedValue);

                int i = HealthFacility.Insert(o);
                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gridview_Databind(i);
                    ClearControls(this);
                    revHealthFacility.Visible = false;
                    HttpContext.Current.Session["_successHealthfacility"] = "1";
                    HttpContext.Current.Session["_lastHealthfacility"] = i;

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

        Response.Redirect(Request.RawUrl, false);
    }
    
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int id = -1;
                string _id = Request.QueryString["id"].ToString();
                int.TryParse(_id, out id);
                int userId = CurrentEnvironment.LoggedUser.Id;
                revHealthFacility.Visible = false;

                HealthFacility o = HealthFacility.GetHealthFacilityById(id);

                if (healthCenterId == null)
                {
                    revHealthFacility.Visible = true;
                    return;
                }
                if (o.ParentId != int.Parse(healthCenterId))
                {
                    if (HealthFacility.GetHealthFacilityById(int.Parse(healthCenterId)) == null)
                    {
                        revHealthFacility.Visible = true;
                        return;
                    }
                }
                if (nameExists(txtName.Text.Replace("'", @"''")) && (o.Name != txtName.Text.Trim()))
                    return;
                if (codeExists(txtCode.Text.Trim()) && (o.Code != txtCode.Text.Trim()))
                    return;
                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text;
                o.ParentId = int.Parse(healthCenterId);
                o.TopLevel = bool.Parse(rblTopLevel.SelectedValue);
                o.Leaf = bool.Parse(rblLeaf.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                o.VaccinationPoint = bool.Parse(rblVaccinationPoint.SelectedValue);
                o.Address = txtAddress.Text.Replace("'", @"''");
                o.VaccineStore = bool.Parse(rblVaccineStore.SelectedValue);
                o.Contact = txtContact.Text;
                o.ColdStorageCapacity = double.Parse(txtColdStorageCapacity.Text);
                o.TypeId = int.Parse(ddlType.SelectedValue);
                o.Ownership = int.Parse(ddlOwnership.SelectedValue);
                if (id == 1)
                    o.IsActive = true;

                int i = HealthFacility.Update(o);
                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gridview_Databind(id);
                    // ClearControls(this);
                    revHealthFacility.Visible = false;
                    HttpContext.Current.Session["_successHealthfacility"] = "1";
                    //  HttpContext.Current.Session["_lastHealthfacility"] = id;
                    Response.Redirect(Request.RawUrl, false);
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
    
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            int id = -1;
            string _id = Request.QueryString["id"].ToString();
            int.TryParse(_id, out id);
            int userId = CurrentEnvironment.LoggedUser.Id;
            int i = 0;
            if (id != 1)
                i = HealthFacility.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gvHealthFacility.DataBind();
                ClearControls(this);
                HttpContext.Current.Session["_successHealthfacility"] = "1";
                Response.Redirect(Request.RawUrl, false);
            }
            else
            {
                lblSuccess.Visible = false;
                lblWarning.Visible = false;
                lblError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }

    protected void gvHealthFacility_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHealthFacility.PageIndex = e.NewPageIndex;
    }

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
                //if (c is CheckBox)
                //    (c as CheckBox).Checked = false;
                if (c is DropDownList)
                    (c as DropDownList).SelectedIndex = 0;
            }
        }
    }
    protected void gvHealthFacility_Databound(object sender, System.EventArgs e)
    {
        
    }
    protected bool nameExists(string name)
    {
        if (HealthFacility.GetHealthFacilityByName(name) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
    protected bool codeExists(string code)
    {
        if (HealthFacility.GetHealthFacilityByCode(code) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
    //protected void btnAddNew_Click(object sender, EventArgs e)
    //{
    //    // Response.Write("<script>window.open('User.aspx','_blank','mywindow','width=320,height=480,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no,resizable=yes');</script>");
    //    // Response.Redirect("User.aspx", true);
    //    StringBuilder sb = new StringBuilder();
    //    sb.Append("<script>");
    //    sb.Append("window.open('User.aspx', 'mywin', 'left=100,top=30,width=960,height=600,toolbar=0,resizable=0');");
    //    sb.Append("</script>");

    //    //Page.RegisterStartupScript("test", sb.ToString());
    //    ClientScript.RegisterStartupScript(this.GetType(), "test", sb.ToString());
    //}
}
