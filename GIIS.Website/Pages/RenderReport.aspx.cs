using GIIS.DataLayer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_RenderReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // TOTAL HAX ...
        String jasperUrl = String.Empty;
        StringBuilder myRoles = new StringBuilder();
        var sessionNameAction = "__GIS_actionList_" + CurrentEnvironment.LoggedUser.Id;
        var actionList = (List<string>)Session[sessionNameAction];
        actionList.ForEach(a => myRoles.AppendFormat("'{0}',", a));
        myRoles.Remove(myRoles.Length - 1, 1);
        String reportName = null;
        // Step 1 - Get report data
        string command = "SELECT * FROM \"REPORT\" WHERE \"ID\" = @Id AND \"ACTION_ID\" IN (SELECT \"ID\" FROM \"ACTIONS\" WHERE \"NAME\" IN (" + myRoles.ToString() + "))";
        using (var dt = DBManager.ExecuteReaderCommand(command, System.Data.CommandType.Text, new List<Npgsql.NpgsqlParameter>() {
            new NpgsqlParameter("Id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = Request.QueryString["reportId"] }
        }))
        {
            using (var rdr = dt.CreateDataReader())
            {
                if (rdr.Read())
                {
                    reportName = rdr["JASPER_ID"].ToString();
                    jasperUrl = String.Format("{0}/{1}.{2}?j_username={3}&j_password={4}&", ConfigurationManager.AppSettings["JasperServer"], rdr["JASPER_ID"],
                        Request.QueryString["format"] ?? "html", ConfigurationManager.AppSettings["JasperUser"], ConfigurationManager.AppSettings["JasperPassword"]);
                }
                else
                {
                    Response.Redirect("/Default.aspx");
                    return;
                }
            }
        }

        // Build url
        foreach (var kvp in Request.QueryString.AllKeys)
            jasperUrl += String.Format("{0}={1}&", kvp, Request.QueryString[kvp]);

        try
        {
            HttpWebRequest httpRequest = WebRequest.Create(jasperUrl) as HttpWebRequest;
            httpRequest.ServerCertificateValidationCallback = (o, c, h, s) => true;
            var response = httpRequest.GetResponse();
            Response.ContentType = response.ContentType;
            Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.{1}", reportName, Request.QueryString["format"] ?? "html" ));

            Response.ClearContent();
            Stream reportStream = response.GetResponseStream();
            int br = 0;
            byte[] buffer = new byte[1024];
            while (br < response.ContentLength)
            {
                int bufferBytes = reportStream.Read(buffer, 0, 1024);
                br += bufferBytes;
                Response.BinaryWrite(buffer.Take(bufferBytes).ToArray());
            }

        }
        catch (Exception ex)
        {
            Response.ContentType = "text/plain";
            Response.Write("Error running report: \r\n");
            Response.Write(ex.ToString());
            Response.Write("\r\nJasper URL:" + jasperUrl);
        }
    }
}