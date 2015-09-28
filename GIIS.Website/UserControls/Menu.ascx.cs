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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.DataLayer;
using System.Diagnostics;

public partial class UserControls_Menu : System.Web.UI.UserControl
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        string language = CurrentEnvironment.Language;
        int languageId = int.Parse(language);
        Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Menu-dictionary" + language];
        if (wtList == null)
        {
            List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Menu");
            wtList = new Dictionary<string, string>();
            foreach (WordTranslate vwt in wordTranslateList)
                wtList.Add(vwt.Code, vwt.Name);
            HttpContext.Current.Cache.Insert("Menu-dictionary" + language, wtList);
        }

        List<string> actionList = null;
        string sessionNameAction = "";
        if (!this.IsPostBack)
        {
            if (CurrentEnvironment.LoggedUser != null)
            {
                sessionNameAction = "__GIS_actionList_" + CurrentEnvironment.LoggedUser.Id;
                actionList = (List<string>)Session[sessionNameAction];
                System.Collections.Generic.ICollection<GIIS.DataLayer.Menu> topCategories = GIIS.DataLayer.Menu.GetTrueParents();
                System.Collections.Generic.ICollection<GIIS.DataLayer.Menu> allowedTopCategories = new List<GIIS.DataLayer.Menu>();
                string a1,a2 = "";
                foreach(GIIS.DataLayer.Menu i in topCategories)
                {
                    a1 = "View" + i.Title.Replace(" ","");
                    a2 = "ViewMenu" + i.Title.Replace(" ", "");
                    if ( actionList.Contains(a1) || actionList.Contains(a2) ) 
                    {
                        allowedTopCategories.Add(i);
                    }
                }
                AddCategories(allowedTopCategories, this.tvMenu.Nodes);
                //AddCategories(topCategories, this.tvMenu.Nodes);

                if (Request.QueryString["menuId"] != null)
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["menuId"].ToString()))
                    {
                        Session["__SelectedMenu"] = Request.QueryString["menuId"].ToString();
                    }
                }

                // Current node should be expanded and bold
                if (Request.QueryString["menuId"] != null)
                    if (!String.IsNullOrEmpty(Request.QueryString["menuId"].ToString()))
                    {
                        TreeNode node = GetNodeForValue(this.tvMenu.Nodes, Request.QueryString["menuId"].ToString());
                        if (node != null)
                        {
                            node.Expand();
                          
                            //node.Selected = false;
                            while (node.Parent != null)
                            {
                                node = node.Parent;
                                node.Expand();
                            }
                            //node.Selected = true;
                            
                        }
                    }
            }
        }
    }

    private void AddCategories(System.Collections.Generic.ICollection<GIIS.DataLayer.Menu> menus, TreeNodeCollection nodes)
    {
        List<string> actionList = null;
        string sessionNameAction = "";
        if ((CurrentEnvironment.LoggedUser != null))
        {
            sessionNameAction = "__GIS_actionList_" + CurrentEnvironment.LoggedUser.Id;
            actionList = (List<string>)Session[sessionNameAction];

            string a1, a2 = "";
            foreach (GIIS.DataLayer.Menu c in menus)
            {
                a1 = "View" + c.Title.Replace(" ", "");
                a2 = "ViewMenu" + c.Title.Replace(" ", "");
                if ( actionList.Contains(a1) || actionList.Contains(a2) )
                {
                    string url = HttpContext.Current.Request.Url.AbsolutePath + string.Format(@"?menuId={0}&text={1}", c.NavigateUrl, c.Title);
                    TreeNode node = new TreeNode(c.Title, c.NavigateUrl, "", url, null);
                    node.SelectAction = TreeNodeSelectAction.Expand;

                    nodes.Add(node);
                    AddCategories(GIIS.DataLayer.Menu.GetTrueChildren(c.Id), node.ChildNodes);
                }
            }
        }
    }
    
    private TreeNode GetNodeForValue(TreeNodeCollection nodes, string value)
    {
        TreeNode selectedNode = null;
        foreach (TreeNode node in nodes)
        {
            if (node.Value == value)
            {
                return node;
            }
            else
            {
                TreeNode n = GetNodeForValue(node.ChildNodes, value);
                if (n != null && selectedNode == null)
                {
                    selectedNode = n;
                    break;
                }
            }
        }
        return selectedNode;
    }

    protected void SetExapandNodes(TreeNodeCollection nodes)
    {
        foreach (TreeNode n in nodes)
        {
            //if (this.SelectedNodes.Exists(x => x.Equals(n.Value)))
            //    n.Expand();
            //if (n.ChildNodes.Count > 0)
            //    this.SetExapandNodes(n.ChildNodes);
        }
    }
}


  