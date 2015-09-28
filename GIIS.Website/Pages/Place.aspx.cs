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


public partial class _Place : System.Web.UI.Page
{
    static String placeId;
    static String healthFacilityId;
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

            if ((actionList != null) && actionList.Contains("ViewPlace") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Place-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Place");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Place-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["PlaceName"];
                this.lblParentId.Text = wtList["PlaceParent"];
                this.lblLeaf.Text = wtList["PlaceLeaf"];
                this.lblNotes.Text = wtList["PlaceNotes"];
                this.lblIsActive.Text = wtList["PlaceIsActive"];
                this.rblIsActive.Items[0].Text = wtList["PlaceYes"];
                this.rblIsActive.Items[1].Text = wtList["PlaceNo"];
                this.rblLeaf.Items[0].Text = wtList["PlaceYes"];
                this.rblLeaf.Items[1].Text = wtList["PlaceNo"];
                this.lblHealthFacility.Text = wtList["PlaceHealthFacility"];

                //grid header text
                gvPlace.Columns[1].HeaderText = wtList["PlaceName"];
                gvPlace.Columns[3].HeaderText = wtList["PlaceParent"];
                gvPlace.Columns[4].HeaderText = wtList["PlaceLeaf"];
                gvPlace.Columns[5].HeaderText = wtList["PlaceNotes"];
                gvPlace.Columns[6].HeaderText = wtList["PlaceIsActive"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddPlace");
                this.btnEdit.Visible = actionList.Contains("EditPlace");
                this.btnRemove.Visible = actionList.Contains("RemovePlace");

                //buttons
                this.btnAdd.Text = wtList["PlaceAddButton"];
                this.btnEdit.Text = wtList["PlaceEditButton"];
                this.btnRemove.Text = wtList["PlaceRemoveButton"];

                //Page Title
                this.lblTitle.Text = wtList["PlacePageTitle"];

                //message
                this.lblSuccess.Text = wtList["PlaceSuccessText"];
                this.lblWarning.Text = wtList["PlaceWarningText"];
                this.lblError.Text = wtList["PlaceErrorText"];
                if ((String)HttpContext.Current.Session["_successPlace"] == "1")
                    lblSuccess.Visible = true;
                else
                    lblSuccess.Visible = false;

                HttpContext.Current.Session["_successPlace"] = "0";

                //validators

                revPlace.Text = wtList["PlaceParentValidator"];
                cvPlace.ErrorMessage = wtList["PlaceMandatory"];
                revName.Text = wtList["PlaceNameValidator"];

                //selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    GIIS.DataLayer.Place o = GIIS.DataLayer.Place.GetPlaceById(id);
                    txtName.Text = o.Name;
                    txtCode.Text = o.Code;
                    if (o.Parent != null)
                        txtParentId.SelectedItemText = o.Parent.Name.ToString();
                    rblLeaf.SelectedValue = o.Leaf.ToString();
                    if (o.Leaf)
                    {
                        lblHealthFacility.Visible = true;
                        ddlHealthFacility.Visible = true;
                        string where = string.Empty;
                        if (!(CurrentEnvironment.LoggedUser.HealthFacilityId == 1))
                        {
                            string s = HealthFacility.GetAllChildsForOneHealthFacility(CurrentEnvironment.LoggedUser.HealthFacilityId); //true
                            where = string.Format(@"AND ""ID"" = {1} OR ""ID"" in ( {0})", s, CurrentEnvironment.LoggedUser.HealthFacilityId);
                        }
                        odsHealthF.SelectParameters.Clear();
                        odsHealthF.SelectParameters.Add("ids", where);
                        odsHealthF.DataBind();
                        ddlHealthFacility.SelectedValue = o.HealthFacilityId.ToString();
                        //txtHealthcenterId.SelectedItemText = o.HealthFacility.Name.ToString();
                    }
                    txtNotes.Text = o.Notes;
                    rblIsActive.SelectedValue = o.IsActive.ToString();
                    gridview_Databind(id);
                    placeId = o.ParentId.ToString();
                    btnAdd.Visible = false;
                }
                else
                {
                    //string s = HealthFacility.GetAllChildsForOneHealthFacility(CurrentEnvironment.LoggedUser.HealthFacilityId);
                    //odsHealthF.SelectParameters.Clear();
                    //odsHealthF.SelectParameters.Add("ids", s);
                    //odsHealthF.DataBind();
                    btnEdit.Visible = false;
                    btnRemove.Visible = false;
                    lblIsActive.Visible = false;
                    rblIsActive.Visible = false;
                }
                int i = 0;
                if (HttpContext.Current.Session["_lastPlace"] != null)
                {
                    i = (int)HttpContext.Current.Session["_lastPlace"];
                    gridview_Databind(i);
                    HttpContext.Current.Session["_lastPlace"] = null;
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    protected void gridview_Databind(int id)
    {
        odsPlace.SelectParameters.Clear();
        odsPlace.SelectParameters.Add("i", id.ToString());
        odsPlace.DataBind();
    }

    protected void Places_ValueSelected(object sender, System.EventArgs e)
    {
        placeId = txtParentId.SelectedItemID.ToString();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;

                revPlace.Visible = false;

                Place o = new Place();
                if (placeId == null)
                {
                    revPlace.Visible = true;
                    return;
                }
                if (Place.GetPlaceById(int.Parse(placeId)) == null)
                {
                    revPlace.Visible = true;
                    return;
                }
                if (!correct())
                    return;

                if (nameExists(txtName.Text.Trim(), int.Parse(placeId)))
                    return;
                o.Name = txtName.Text.Trim();
                o.Code = txtCode.Text;
                o.ParentId = int.Parse(placeId);
                if (rblLeaf.SelectedValue == "")
                {
                    o.Leaf = false;
                }
                else
                {
                    o.Leaf = bool.Parse(rblLeaf.SelectedValue);
                }
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                if (ddlHealthFacility.SelectedIndex > 0)
                    o.HealthFacilityId = int.Parse(ddlHealthFacility.SelectedValue);
                //if (healthFacilityId != null && (HealthFacility.GetHealthFacilityById(int.Parse(healthFacilityId)) != null))
                //    o.HealthFacilityId = int.Parse(healthFacilityId);
                int i = Place.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gridview_Databind(i);
                    ClearControls(this);
                    revPlace.Visible = false;
                    HttpContext.Current.Session["_successPlace"] = "1";
                    HttpContext.Current.Session["_lastPlace"] = i;
                    Response.Redirect(Request.RawUrl);
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
                revPlace.Visible = false;

                Place o = Place.GetPlaceById(id);

                if (placeId == null)
                {
                    revPlace.Visible = true;
                    return;
                }
                if (Place.GetPlaceById(int.Parse(placeId)) == null)
                {
                    revPlace.Visible = true;
                    return;
                }
                if (!correct())
                    return;
                if (nameExists(txtName.Text.Trim(), int.Parse(placeId)) && (o.Name != txtName.Text.Trim()))
                    return;

                o.Name = txtName.Text.Trim();
                o.Code = txtCode.Text;
                o.ParentId = int.Parse(placeId);
                o.Leaf = bool.Parse(rblLeaf.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                if (ddlHealthFacility.SelectedIndex > 0)
                    o.HealthFacilityId = int.Parse(ddlHealthFacility.SelectedValue);
                else
                    o.HealthFacilityId = null;
                //if (healthFacilityId != null && (HealthFacility.GetHealthFacilityById(int.Parse(healthFacilityId)) != null))
                //    o.HealthFacilityId = int.Parse(healthFacilityId);
                if (id == 1)
                    o.IsActive = true;
                int i = Place.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    lblInfo.Visible = false;
                   // gridview_Databind(i);
                    ClearControls(this);
                    revPlace.Visible = false;
                    HttpContext.Current.Session["_successPlace"] = "1";
                    HttpContext.Current.Session["_lastPlace"] = id;
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = false ;
                    lblError.Visible = true;
                    lblInfo.Visible = false;
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
                i = Place.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                lblInfo.Visible = false;
               // gridview_Databind(id);
                ClearControls(this);
                revPlace.Visible = false;
                HttpContext.Current.Session["_successPlace"] = "1";
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                lblSuccess.Visible = false;
                lblWarning.Visible = false;
                lblError.Visible = true;
                lblInfo.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
            lblInfo.Visible = false;
        }
    }

    protected void gvPlace_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPlace.PageIndex = e.NewPageIndex;
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

                //if (c is DropDownList)
                //    (c as DropDownList).SelectedIndex = 0;
            }
        }
    }
    protected bool nameExists(string name, int parent)
    {
        if (Place.GetPlaceByName(name, parent) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }

    protected bool correct()
    {
        bool result = true;
        if (rblLeaf.SelectedIndex == 0 && ddlHealthFacility.SelectedIndex == 0)
            result = false;
        if (rblLeaf.SelectedIndex == 1 && ddlHealthFacility.SelectedIndex > 0)
            result = false;
        if (!result)
        {
            lblSuccess.Visible = false;
            gvPlace.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = false;
            lblInfo.Visible = true;
        }
        return result;
    }
    protected void HealthCenters_ValueSelected(object sender, System.EventArgs e)
    {
        //healthFacilityId = txtHealthcenterId.SelectedItemID.ToString();
    }
    protected void rblLeaf_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblLeaf.SelectedIndex == 0)
        {
            ddlHealthFacility.Visible = true;
            lblHealthFacility.Visible = true;

            string where = string.Empty;
            if (!(CurrentEnvironment.LoggedUser.HealthFacilityId == 1))
            {
                string s = HealthFacility.GetAllChildsForOneHealthFacility(CurrentEnvironment.LoggedUser.HealthFacilityId, true);
                where = string.Format(@"AND ""ID"" in ( {0})", s);
            }
            odsHealthF.SelectParameters.Clear();
            odsHealthF.SelectParameters.Add("ids", where);
            odsHealthF.DataBind();
        }
        else
        {
            ddlHealthFacility.Visible = false;
            lblHealthFacility.Visible = false;
            if (ddlHealthFacility.Items.Count > 0)
                ddlHealthFacility.SelectedIndex = 0;
        }
    }
}
