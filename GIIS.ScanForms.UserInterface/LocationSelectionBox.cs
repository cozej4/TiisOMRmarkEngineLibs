using GIIS.DataLayer;
using OmrMarkEngine.Template.Scripting.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIIS.ScanForms.UserInterface
{
    public partial class LocationSelectionBox : Form
    {
        public LocationSelectionBox()
        {
            InitializeComponent();
            PopulateLocationNode(null);
        }

        public int FacilityId { get { return (int)trvLocation.SelectedNode.Tag; } }

        public bool Remember { get { return this.checkBox1.Checked; } }
        /// <summary>
        /// Populate the location node
        /// </summary>
        public void PopulateLocationNode(TreeNode parent)
        {
            RestUtil restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));

            var parentId = 0;
            if (parent != null)
                parentId = (int)parent.Tag;
            else
            {
                if (String.IsNullOrEmpty(restUtil.GetCurrentUserName)) // Login
                    try
                    {
                        restUtil.Get<User>("UserManagement.svc/GetUser", new KeyValuePair<string, object>("username", ""), new KeyValuePair<String, Object>("password", ""));
                    }
                    catch { }

                var userInfo = restUtil.Get<User>("UserManagement.svc/GetUserInfo", new KeyValuePair<string, object>("username", restUtil.GetCurrentUserName));
                parentId = userInfo.HealthFacilityId;
            }
            var facilities = restUtil.Get<HealthFacility[]>("HealthFacilityManagement.svc/GetHealthFacilityByParentId", new KeyValuePair<string, object>("parentId", parentId));
            
            if (parent == null)
            {
                foreach (var hf in facilities)
                {
                    var tn = new TreeNode()
                    {
                        Tag = hf.Id,
                        Text = hf.Name
                    };
                    if (!hf.Leaf)
                        tn.Nodes.Add(String.Empty);
                    trvLocation.Nodes.Add(tn);
                }
            }
            else
            {
                parent.Nodes.Clear();
                foreach (var hf in facilities)
                {
                    var tn = new TreeNode()
                    {
                        Tag = hf.Id,
                        Text = hf.Name
                    };
                    if(!hf.Leaf)
                        tn.Nodes.Add(String.Empty);
                    parent.Nodes.Add(tn);
                }
            }
        }

        private void trvLocation_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            this.PopulateLocationNode(e.Node);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
