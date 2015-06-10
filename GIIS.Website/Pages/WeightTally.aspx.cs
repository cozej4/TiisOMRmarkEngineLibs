using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Web.Configuration;
using System.Data.SqlClient;
using Npgsql;
using System.IO;
public partial class Pages_WeightTally : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        // Validation
        if (CurrentEnvironment.LoggedUser == null)
            Response.Redirect("Default.aspx");
        status.Visible = Page.IsPostBack;
        
        if (!Page.IsPostBack)
        {
            List<int> months = new List<int>();
            for (int i = 1; i <= 12; i++) months.Add(i);

            MonthDropdown.DataSource = months;
            MonthDropdown.DataBind();
            MonthDropdown.SelectedIndex = (DateTime.Now.Month - 1);

            Dictionary<string, int> facilities = new Dictionary<string, int>();
            var hf = HealthFacility.GetHealthFacilityById(CurrentEnvironment.LoggedUser.HealthFacilityId);
            facilities.Add(hf.Name, hf.Id); 
            foreach (HealthFacility h in HealthFacility.GetHealthFacilityByParentId(CurrentEnvironment.LoggedUser.HealthFacilityId).OrderBy(x => x.Name))
            {
                facilities.Add(h.Name, h.Id);
            }


            HealthFacilityDropdown.DataSource = facilities;
            HealthFacilityDropdown.DataTextField = "key";
            HealthFacilityDropdown.DataValueField = "value";
            HealthFacilityDropdown.DataBind();
        }
        else
        {

            try
            {
                if (DocumentUpload.HasFile)
                {

                    Stream stream = DocumentUpload.FileContent;
                    StreamReader reader = new StreamReader(stream);
                    List<String[]> lines = new List<string[]>();
                    while (!reader.EndOfStream)
                    {
                        String[] line = reader.ReadLine().Split(',').Skip(1).ToArray();
                        lines.Add(line);
                    }
                    foreach (string[] s in lines)
                    {
                        Debug.WriteLine(String.Join(" ", s));
                    }

                    string connString = WebConfigurationManager.ConnectionStrings["GiisConnectionString"].ConnectionString;
                    using (NpgsqlConnection con = new NpgsqlConnection(connString))
                    {
                            con.Open();
                            int hf = int.Parse(HealthFacilityDropdown.SelectedValue);
                            DateTime date = new DateTime(DateTime.Now.Year, MonthDropdown.SelectedIndex + 1, DateTime.Now.Day);
                            for (int i = 1; i < lines.Count; i++)
                            {
                                using (NpgsqlCommand command = new NpgsqlCommand("insert_new_weight", con) { CommandType = System.Data.CommandType.StoredProcedure })
                                {

                                    command.Parameters.Add(new NpgsqlParameter("hf", hf));
                                command.Parameters.Add(new NpgsqlParameter("date", date));
                                command.Parameters.Add(new NpgsqlParameter("age_group", lines[i][0]));
                                    command.Parameters.Add(new NpgsqlParameter("m_gt_80", lines[i][1] == String.Empty ? 0 : int.Parse(lines[i][1])));
                                    command.Parameters.Add(new NpgsqlParameter("m_60_80", lines[i][2] == String.Empty ? 0 : int.Parse(lines[i][2])));
                                    command.Parameters.Add(new NpgsqlParameter("m_lt_60", lines[i][3] == String.Empty ? 0 : int.Parse(lines[i][3])));
                                    command.Parameters.Add(new NpgsqlParameter("f_gt_80", lines[i][4] == String.Empty ? 0 : int.Parse(lines[i][4])));
                                    command.Parameters.Add(new NpgsqlParameter("f_60_80", lines[i][5] == String.Empty ? 0 : int.Parse(lines[i][5])));
                                    command.Parameters.Add(new NpgsqlParameter("f_lt_60", lines[i][6] == String.Empty ? 0 : int.Parse(lines[i][6])));
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();
                            }

                        }
                        status.Text = "Weight tally sheet submitted";
                        status.CssClass = "label label-success";
                    }
                }
                else
                {
                    status.Text = "No file selected";
                    status.CssClass = "label label-danger";
                    return;
                }
            }
            catch(Exception ex)
            {
                status.Text = ex.Message;
                status.CssClass = "label label-danger";

            }
        }
        Debug.WriteLine("page loaded");
    }
}