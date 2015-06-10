using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_OrderAlert : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrentEnvironment.LoggedUser != null)
        {
            int healthFacilityId = CurrentEnvironment.LoggedUser.HealthFacilityId;

            List<Order> orderList = Order.GetOrderListByHealthFacilityId(healthFacilityId);
            //gvOrder.DataSource = orderList;
            //gvOrder.DataBind();

            // lblCount.Text = "New Orders: " + Order.GetCountOrderListByHealthFacilityId(healthFacilityId);

            Literal l = new Literal();
            l.Text = "<ul>";

            string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;

            foreach (Order o in orderList)
            {
               // l.Text += string.Format("<li><a href='ViewOrder.aspx?id={0}' target='_blank' >{1} - {2}</a></li>", o.Id, o.Sender.Name, o.OrderDate.ToString(dateformat));
            }

            l.Text += "</ul>";
            pnlOrders.Controls.Add(l);
        }
        else
            pnlOrders.Visible = false;
    }
}