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

public partial class _TransferOrderHeader : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewTransferOrderHeader") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["TransferOrderHeader-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "TransferOrderHeader");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("TransferOrderHeader-dictionary" + language, wtList);
                }

                //this.lblOrderNum.Text = wtList["TransferOrderHeaderOrderNum"];
                this.lblOrderSchedReplenishDate.Text = wtList["TransferOrderHeaderOrderSchedReplenishDate"];
                this.lblOrderFacilityFrom.Text = wtList["TransferOrderHeaderOrderFacilityFrom"];
                this.lblOrderFacilityTo.Text = wtList["TransferOrderHeaderOrderFacilityTo"];
                //this.lblOrderStatus.Text = wtList["TransferOrderHeaderOrderIsActive"];
                //this.lblOrderCarrier.Text = wtList["TransferOrderHeaderOrderCarrier"];
                //this.lblRevNum.Text = wtList["TransferOrderHeaderRevNum"];

                HealthFacility currentHealhFacility = CurrentEnvironment.LoggedUser.HealthFacility;

                List<HealthFacility> healthFacilityListFrom = new List<HealthFacility>();
                if (currentHealhFacility.Parent != null)
                    healthFacilityListFrom.Add(currentHealhFacility.Parent);
                healthFacilityListFrom.Add(currentHealhFacility);

                ddlHealthFacilityFrom.DataSource = healthFacilityListFrom;
                ddlHealthFacilityFrom.DataBind();

                

                ddlHealthFacilityFrom_SelectedIndexChanged(new object(), new EventArgs());



                if (currentHealhFacility.TypeId == 2)
                {
                    ddlHealthFacilityFrom.SelectedValue = currentHealhFacility.Code;
                    ddlHealthFacilityFrom_SelectedIndexChanged(ddlHealthFacilityFrom, new EventArgs());
                }

                if (currentHealhFacility.TypeId == 3)
                {
                    ddlHealthFacilityFrom.SelectedValue = currentHealhFacility.Parent.Code;
                    ddlHealthFacilityTo.SelectedValue = currentHealhFacility.Code;
                    ddlHealthFacilityTo.Enabled = false;
                }

                ceOrderSchedReplenishDate.Format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revOrderSchedReplenishDate.ErrorMessage = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revOrderSchedReplenishDate.ValidationExpression = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateExpresion;

                ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
                txtOrderSchedReplenishDate.Text = DateTime.Today.ToString(dateformat.DateFormat.ToString());

                this.btnAdd.Visible = actionList.Contains("AddTransferOrderHeader");
                this.btnAdd.Text = wtList["TransferOrderHeaderAddButton"];

                this.btnAddDetails.Visible = false;// actionList.Contains("AddDetailsTransferOrderHeader");
                this.btnAddDetails.Text = wtList["TransferOrderHeaderAddDetailsButton"];
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

                TransferOrderHeader o = new TransferOrderHeader();

                DateTime date = DateTime.ParseExact(txtOrderSchedReplenishDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                o.OrderSchedReplenishDate = date;
                o.OrderFacilityFrom = ddlHealthFacilityFrom.SelectedValue;
                o.OrderFacilityTo = ddlHealthFacilityTo.SelectedValue;
                o.OrderStatus = 0;//Requested
                o.OrderCarrier = txtOrderCarrier.Text;
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                o.RevNum = 0;

                int i = TransferOrderHeader.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    //gridview_Databind(i);
                   // ClearControls(this);
                    HttpContext.Current.Session["_successTransferOrderHeader"] = "1";
                    HttpContext.Current.Session["_lastTransferOrderHeader"] = i;

                    this.btnAdd.Visible = false;
                    this.btnAddDetails.Visible = true;
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

    protected void ddlHealthFacilityFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (CurrentEnvironment.LoggedUser != null)
        {
            HealthFacility currentHealhFacility = CurrentEnvironment.LoggedUser.HealthFacility;

            if (currentHealhFacility != null)
            {
                //load health facility from
                List<HealthFacility> healthFacilityListTo = HealthFacility.GetHealthFacilityListByTypeId(currentHealhFacility.TypeId);
                List<HealthFacility> healthFacilityListTo1 = HealthFacility.GetHealthFacilityByParentId(CurrentEnvironment.LoggedUser.HealthFacilityId);

                healthFacilityListTo.AddRange(healthFacilityListTo1);

                HealthFacility tmp = null;
                foreach (HealthFacility hf in healthFacilityListTo)
                {
                    if (hf.Code == ddlHealthFacilityFrom.SelectedValue)
                        tmp = hf;
                }

                if (tmp != null)
                    healthFacilityListTo.Remove(tmp);

                ddlHealthFacilityTo.DataSource = healthFacilityListTo;
               

                //apply disable logic on health facility to
                if (currentHealhFacility.Parent != null)
                {
                    if (ddlHealthFacilityFrom.SelectedValue == currentHealhFacility.Parent.Code)
                    {
                        ddlHealthFacilityTo.SelectedValue = currentHealhFacility.Code;
                        ddlHealthFacilityTo.Enabled = false;
                    }
                    else
                    {
                        ddlHealthFacilityTo.Enabled = true;
                    }
                }
                ddlHealthFacilityTo.DataBind();
            }
        }

    }

    protected void ValidateOrderSchedReplenishDate(object sender, ServerValidateEventArgs e)
    {
        ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));

        DateTime date = DateTime.ParseExact(txtOrderSchedReplenishDate.Text, dateformat.DateFormat, CultureInfo.InvariantCulture);

        e.IsValid = date >= DateTime.Today;
    }

    protected void btnAddDetails_Click(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["_lastTransferOrderHeader"] != null)
        {
            string orderNumber = HttpContext.Current.Session["_lastTransferOrderHeader"].ToString();
            Response.Redirect(string.Format("TransferOrderDetail.aspx?orderNum={0}", orderNumber));
        }
    }
}