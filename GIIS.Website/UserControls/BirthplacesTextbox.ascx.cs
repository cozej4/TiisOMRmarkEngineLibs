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
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

public partial class UserControls_BirthplacesTextbox : System.Web.UI.UserControl
{
    private string _AutoCompleteExtender = "";
    private Unit _BoxWidth;
    private string _SelectedItemID = "";
    private bool _OnClickSubmit = false;
    private bool _OnChangedSubmit = false;
    private ServiceMethods _ServiceMethod = ServiceMethods.BirthPlace;
    private bool _HighlightResults = false;
    private string _WaterMarkText = "";
    private string _SelectedItemText = "";

    private bool _ClearAfterSelect = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!(IsPostBack))
        {
            BBox.Width = (Unit)BoxWidth;
            SetServiceMethod();
            _SelectedItemID = "";
            _SelectedItemText = "";

            if (HighlightResults)
            {
                autoComplete3.OnClientPopulated = "ClientPopulated";
            }


            string HiddenCID = HiddenControlID.ClientID;
            string script = "function ControlClickSubmit_" + autoComplete3.ClientID + "(source, e) " + "   { " + "    var node; " + " var value = e.get_value(); " + " if (value) node = e.get_item(); " + "       else " + " { " + "   value = e.get_item().parentNode._value; " + "   node = e.get_item().parentNode; " + " } " + " var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml; " + " source.get_element().value = text; " + "     document.getElementById('" + HiddenCID + "').value = value; " + "     var btn = document.getElementById('" + subBtn.ClientID + "'); " + "     var bx = document.getElementById('" + BBox.ClientID + "'); " + "     bx.value = text; " + "     btn.click();}";

            autoComplete3.OnClientItemSelected = "ControlClickSubmit_" + autoComplete3.ClientID;
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "ControlClickSubmit_" + autoComplete3.ClientID, script, true);

        }
    }
    protected void SetServiceMethod()
    {
        switch (ServiceMethod)
        {
            case ServiceMethods.BirthPlace:
                autoComplete3.ServiceMethod = "GetLeafPlaces";
                break;
            case ServiceMethods.ParentPlaces:
                autoComplete3.ServiceMethod = "GetPlaces";
                break;

        }
    }
    public void SetValue(string value)
    {
        autoComplete3.ContextKey = value;
        BBox.Text = value;
    }
    public event ValueSelectedEventHandler ValueSelected;
    public delegate void ValueSelectedEventHandler(object sender, System.EventArgs e);


    protected void SelectAction(object sender, System.EventArgs e)
    {
        SelectedItemID = HiddenControlID.Value;
        SelectedItemText = BBox.Text;

        if (OnClickSubmit)
        {
            if (ValueSelected != null)
            {
                ValueSelected(sender, e);
            }
        }

        if (ClearAfterSelect)
        {
            BBox.Text = "";
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
            autoComplete3.ContextKey = value;
            BBox.Text = value;
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
        BirthPlace = 1,
        ParentPlaces = 2
    }
}