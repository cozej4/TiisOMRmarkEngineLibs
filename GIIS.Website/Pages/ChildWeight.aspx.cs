using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;


public partial class _ChildWeight : System.Web.UI.Page
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

            if (CurrentEnvironment.LoggedUser != null) //(actionList.Contains("ViewChildWeight") && 
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ChildWeight");
                Dictionary<string, string> wtList = new Dictionary<string, string>();
                foreach (WordTranslate vwt in wordTranslateList)
                    wtList.Add(vwt.Code, vwt.Name);

                //controls
                this.lbChild.Text = wtList["ChildWeightChild"];
                this.lblWeight.Text = wtList["ChildWeightWeight"];
                this.lblDate.Text = wtList["ChildWeightDate"];

                ////grid header text
                gvChildWeight.Columns[1].HeaderText = wtList["ChildWeightChild"];
                gvChildWeight.Columns[3].HeaderText = wtList["ChildWeightWeight"];
                gvChildWeight.Columns[2].HeaderText = wtList["ChildWeightDate"];
                gvChildWeight.Columns[4].HeaderText = wtList["ChildWeightNotes"];

                ////actions
                //this.btnAdd.Visible = actionList.Contains("AddChildWeight");
                this.btnEdit.Visible = actionList.Contains("EditChildWeight");
                
                //validators
                rfvWeight.ErrorMessage = wtList["ChildWeightMandatory"];

                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    HttpContext.Current.Session["_childId"] = id;
                    Child curChild = Child.GetChildById(id);
                    lnkChild.Text = curChild.Name;
                    lnkChild.PostBackUrl = "Child.aspx?id=" + curChild.Id;
                    lblChildBirthdate.Text = curChild.Birthdate.ToString("dd-MMM-yyyy");
                    lblDate.Text = DateTime.Today.Date.ToString("dd-MMM-yyyy");

                    ChildWeight cw = ChildWeight.GetChildWeightByChildIdAndDate(id, DateTime.Today.Date);
                    if (cw != null)
                    {
                        txtWeight.Text = cw.Weight.ToString();
                        btnAdd.Visible = false;
                        btnEdit.Visible = false;
                        txtWeight.Enabled = false;
                        if (cw.Date == DateTime.Today.Date)
                        {
                            txtWeight.Enabled = true;
                            btnEdit.Visible = true;
                        }
                        gvChildWeight.DataBind();
                        btnSupplement.Visible = true;
                    }
                    else
                    {
                        gvChildWeight.DataBind();
                        btnEdit.Visible = false;
                        btnAdd.Visible = true;
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

                ChildWeight o = new ChildWeight();
                o.ChildId = id;//int.Parse(HttpContext.Current.Session["_childId"].ToString());
                o.Date = DateTime.Today.Date;
                o.Weight = Helper.ConvertToDecimal(txtWeight.Text);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                int i = ChildWeight.Insert(o);

                if (i > 0)
                {
                    lblWeightMessage.Visible = true;
                    string str = GetWeightMessage(o);
                    string message = "";
                    switch (str)
                    {
                        case "SD<=-3":
                            message = "This child is SIGNIFICANTLY underweight for their age- nutritional counselling and further interventions should be considered.";
                            break;
                        case "SD<=-2":
                            message = "This child’s weight is too low for their age. Consider nutritional counselling for the caregiver.";
                            break;
                        case "SD>=2":
                            message = "This child’s weight is too high for their age. Consider nutritional counselling for the caregiver.";
                            break;
                        case "SD>=3":
                            message = "This child’s weight is too high for their age. Consider nutritional counselling for the caregiver.";
                            break;
                        case "":
                            lblWeightMessage.Visible = false;
                            break;

                    }

                    lblWeightMessage.Text = message;

                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvChildWeight.DataBind();
                    btnSupplement.Visible = true;
                    //ClearControls(this);
                    btnAdd.Visible = false;
                    btnEdit.Visible = true;
                }
                else
                {
                    lblWeightMessage.Visible = false;

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

    protected string GetWeightMessage(ChildWeight o)
    {
        Child child = Child.GetChildById(o.ChildId);
        int days = o.Date.Subtract(child.Birthdate).Days;
        string gender = child.Gender ? "M" : "F";
        Weight w = Weight.GetWeight(days, gender);
        string message = "";
        if (o.Weight <= w.SD3neg)
            message = "SD<=-3";
        else if (o.Weight <= w.SD2neg)
            message = "SD<=-2";
        else if (o.Weight >= w.SD3)
            message = "SD>=3";
        else if (o.Weight >= w.SD2)
            message = "SD>=2";
        return message;
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

                ChildWeight o = ChildWeight.GetChildWeightByChildIdAndDate(id, DateTime.Today.Date);

                o.Weight = Helper.ConvertToDecimal(txtWeight.Text);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                int i = ChildWeight.Update(o);

                if (i > 0)
                {
                    lblWeightMessage.Visible = true;
                    string str = GetWeightMessage(o);
                    string message = "";
                    switch (str)
                    {
                        case "SD<=-3":
                            message = "This child is SIGNIFICANTLY underweight for their age- nutritional counselling and further interventions should be considered.";
                            break;
                        case "SD<=-2":
                            message = "This child’s weight is too low for their age. Consider nutritional counselling for the caregiver.";
                            break;
                        case "SD>=2":
                            message = "This child’s weight is too high for their age. Consider nutritional counselling for the caregiver.";
                            break;
                        case "SD>=3":
                            message = "This child’s weight is too high for their age. Consider nutritional counselling for the caregiver.";
                            break;
                        case "":
                            lblWeightMessage.Visible = false;
                            break;

                    }

                    lblWeightMessage.Text = message;


                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvChildWeight.DataBind();
                    btnSupplement.Visible = true;
                }
                else
                {
                    lblWeightMessage.Visible = false;

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

    protected void gvChildWeight_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvChildWeight.PageIndex = e.NewPageIndex;
    }

    protected void btnSupplement_Click(object sender, EventArgs e)
    {
        int Cid = 0;
        string _id = Request.QueryString["id"];
        if (String.IsNullOrEmpty(_id))
        {
            if (HttpContext.Current.Session["_childId"] != null)
            {
                Cid = (int)HttpContext.Current.Session["_childId"];

            }
        }
        else
        {
            int.TryParse(_id, out Cid);
        }
        string url = string.Format("ChildSupplements.aspx?id={0}", Cid);
        Response.Redirect(url, false);
        Context.ApplicationInstance.CompleteRequest();
        HttpContext.Current.Session["_childId"] = null;
    }
}
