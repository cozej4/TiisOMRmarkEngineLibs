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


public partial class _ChildSupplements : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewChildSupplements") && (CurrentEnvironment.LoggedUser != null)) 
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ChildSupplements");
                Dictionary<string, string> wtList = new Dictionary<string, string>();
                foreach (WordTranslate vwt in wordTranslateList)
                    wtList.Add(vwt.Code, vwt.Name);

                //controls
                //this.lblChildId.Text = wtList["ChildSupplementsChild"];
                this.lblVitA.Text = wtList["ChildSupplementsVita"];
                this.lblMebendezol.Text = wtList["ChildSupplementsMebendezol"];
                this.lblDate.Text = wtList["ChildSupplementsDate"];

                //grid header text
                //gvChildSupplements.Columns[1].HeaderText = wtList["ChildSupplementsChild"];
                gvChildSupplements.Columns[3].HeaderText = wtList["ChildSupplementsVita"];
                gvChildSupplements.Columns[4].HeaderText = wtList["ChildSupplementsMebendezol"];
                gvChildSupplements.Columns[2].HeaderText = wtList["ChildSupplementsDate"];

                //actions
                this.btnAdd.Visible =  actionList.Contains("AddChildSupplements");
                //this.btnEdit.Visible = actionList.Contains("EditChildSupplements");

                //validators
                //rfvChildId.ErrorMessage = wtList["ChildSupplementsMandatory"];
                //ceDate.Format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                //revDate.ErrorMessage = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                //revDate.ValidationExpression = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateExpresion;

                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    HttpContext.Current.Session["_childId"] = id;
                    
                    lblVitADate.Text = DateTime.Today.Date.ToString("dd-MMM-yyyy");
                    lblMebendezolDate.Text = DateTime.Today.Date.ToString("dd-MMM-yyyy");

                    ChildSupplements su = ChildSupplements.GetChildSupplementsByChild(id);
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
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
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
                if (c is DropDownList)
                    (c as DropDownList).SelectedIndex = 0;
            }
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;

                int id = -1;
                string _id = Request.QueryString["id"].ToString();
                int.TryParse(_id, out id);
                int i;

                ChildSupplements su = ChildSupplements.GetChildSupplementsByChild(id);
                if (su != null)
                {
                    su.Vita = chkVitA.Checked;
                    su.Mebendezol = chkMebendezol.Checked;
                    su.Date = DateTime.Now;
                    su.ModifiedOn = DateTime.Now;
                    su.ModifiedBy = userId;

                    i = ChildSupplements.Update(su);
                }
                else
                {
                    su = new ChildSupplements();
                    su.ChildId = id;
                    su.Vita = chkVitA.Checked;
                    su.Mebendezol = chkMebendezol.Checked;
                    su.Date = DateTime.Now;
                    su.ModifiedOn = DateTime.Now;
                    su.ModifiedBy = userId;

                    i = ChildSupplements.Insert(su);
                }

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvChildSupplements.DataBind();
                    gvChildSupplements.Visible = true;
                    //ClearControls(this);
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


    //protected void btnEdit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Page.IsValid)
    //        {
    //            int id = -1;
    //            string _id = Request.QueryString["id"].ToString();
    //            int.TryParse(_id, out id);
    //            int userId = CurrentEnvironment.LoggedUser.Id;

    //            ChildSupplements o = ChildSupplements.GetChildSupplementsById(id);

    //            o.Vita = chkVitA.Checked;
    //            o.Mebendezol = chkMebendezol.Checked;
    //            o.Date = DateTime.Now;
    //            o.ModifiedOn = DateTime.Now;
    //            o.ModifiedBy = userId;

    //            int i = ChildSupplements.Update(o, id);

    //            if (i > 0)
    //            {
    //                lblSuccess.Visible = true;
    //                lblWarning.Visible = false;
    //                lblError.Visible = false;
    //                gvChildSupplements.DataBind();
    //            }
    //            else
    //            {
    //                lblSuccess.Visible = false;
    //                lblWarning.Visible = false;
    //                lblError.Visible = true;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblSuccess.Visible = false;
    //        lblWarning.Visible = false;
    //        lblError.Visible = true;
    //    }
    //}

    protected void gvChildSupplements_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvChildSupplements.PageIndex = e.NewPageIndex;
    }
}
