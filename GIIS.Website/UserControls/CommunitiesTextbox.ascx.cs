using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

public partial class UserControls_CommunitiesTextbox : System.Web.UI.UserControl
{
    private string _AutoCompleteExtender = "";
    private Unit _BoxWidth;
    private string _SelectedItemID = "";
    private bool _OnClickSubmit = false;
    private bool _OnChangedSubmit = false;
    private ServiceMethods _ServiceMethod = ServiceMethods.Community;
    private bool _HighlightResults = false;
    private string _WaterMarkText = "";
    private string _SelectedItemText = "";

    private bool _ClearAfterSelect = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!(IsPostBack))
        {
            CBox.Width = (Unit)BoxWidth;
            SetServiceMethod();
            _SelectedItemID = "";
            _SelectedItemText = "";

            if (HighlightResults)
            {
                autoComplete2.OnClientPopulated = "ClientPopulated";
            }


            string HiddenCID = HiddenControlID.ClientID;
            string script = "function ControlClickSubmit_" + autoComplete2.ClientID + "(source, e) " + "   { " + "    var node; " + " var value = e.get_value(); " + " if (value) node = e.get_item(); " + "       else " + " { " + "   value = e.get_item().parentNode._value; " + "   node = e.get_item().parentNode; " + " } " + " var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml; " + " source.get_element().value = text; " + "     document.getElementById('" + HiddenCID + "').value = value; " + "     var btn = document.getElementById('" + subBtn.ClientID + "'); " + "     var bx = document.getElementById('" + CBox.ClientID + "'); " + "     bx.value = text; " + "     btn.click();}";

            autoComplete2.OnClientItemSelected = "ControlClickSubmit_" + autoComplete2.ClientID;
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "ControlClickSubmit_" + autoComplete2.ClientID, script, true);

        }
    }
    protected void SetServiceMethod()
    {
        switch (ServiceMethod)
        {
            case ServiceMethods.Community:
                autoComplete2.ServiceMethod = "GetCommunities";
                break;
          
        }
    }
    public void SetValue(string value)
    {
        autoComplete2.ContextKey = value;
        CBox.Text = value;
    }
    public event ValueSelectedEventHandler ValueSelected;
    public delegate void ValueSelectedEventHandler(object sender, System.EventArgs e);


    protected void SelectAction(object sender, System.EventArgs e)
    {
        SelectedItemID = HiddenControlID.Value;
        SelectedItemText = CBox.Text;

        if (OnClickSubmit)
        {
            if (ValueSelected != null)
            {
                ValueSelected(sender, e);
            }
        }

        if (ClearAfterSelect)
        {
            CBox.Text = "";
        }

    }

    [Bindable(true)]
    [Browsable(true)]
    public Unit BoxWidth
    {
        get { return _BoxWidth; }
        set { _BoxWidth = value; }
    }


    [Bindable(true)]
    [Browsable(true)]
    public string OnSelected
    {
        get { return _AutoCompleteExtender; }
        set { _AutoCompleteExtender = value; }
    }


    [Bindable(true)]
    [Browsable(true)]
    public string SelectedItemID
    {
        get { return _SelectedItemID; }
        set { _SelectedItemID = value; }
    }

    [Bindable(true)]
    [Browsable(true)]
    public string SelectedItemText
    {
        get { return _SelectedItemText; }
        set
        {
            _SelectedItemText = value;
            autoComplete2.ContextKey = value;
            CBox.Text = value;
        }

    }

    [Bindable(true)]
    [Browsable(true)]
    public bool OnClickSubmit
    {
        get { return _OnClickSubmit; }
        set { _OnClickSubmit = value; }
    }

    [Bindable(true)]
    [Browsable(true)]
    public bool OnChangedSubmit
    {
        get { return _OnChangedSubmit; }
        set { _OnChangedSubmit = value; }
    }

    [Bindable(true)]
    [Browsable(true)]
    public ServiceMethods ServiceMethod
    {
        get { return _ServiceMethod; }
        set { _ServiceMethod = value; }
    }

    [Bindable(true)]
    [Browsable(true)]
    public bool HighlightResults
    {
        get { return _HighlightResults; }
        set { _HighlightResults = value; }
    }


    [Bindable(true)]
    [Browsable(true)]
    public string WaterMarktext
    {
        get { return _WaterMarkText; }
        set { _WaterMarkText = value; }
    }


    [Bindable(true)]
    [Browsable(true)]
    public bool ClearAfterSelect
    {
        get { return _ClearAfterSelect; }
        set { _ClearAfterSelect = value; }
    }


    public enum ServiceMethods
    {
        Community = 1
      
    }
}