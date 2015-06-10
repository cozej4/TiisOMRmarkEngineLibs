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
using GIIS.BusinessLogic;
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
                        String[] line = reader.ReadLine().Split(',').ToArray();
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

                            for (int i = 1; i < lines.Count; i++)
                            {
                                using (NpgsqlCommand command = new NpgsqlCommand("insert_stock_tally", con) { CommandType = System.Data.CommandType.StoredProcedure })
                                {

                                    // First: Is this the right sheet?
                                    if (!lines[i][0].StartsWith("BIDSTS"))
                                        throw new InvalidOperationException("Invalid CSV uploaded");

                                    int month = Int32.Parse(lines[i][2]);
                                    string gender = lines[i][1];
                                    for (int c = 3; c < lines[i].Length; c++)
                                    {

                                        var dose = Dose.GetDoseByFullname(lines[0][c]);
                                        var count = Int32.Parse(String.IsNullOrEmpty(lines[i][c]) ? "0" : lines[i][c]);
                                        if(dose == null) throw new InvalidOperationException(String.Format("Dose {0} is not a valid dose", lines[0][c]));
                                        command.Parameters.Add(new NpgsqlParameter("hf", hf));
                                        command.Parameters.Add(new NpgsqlParameter("month", month));
                                        command.Parameters.Add(new NpgsqlParameter("gender", gender.Substring(0,1)));
                                        command.Parameters.Add(new NpgsqlParameter("dose", dose.Id));
                                        command.Parameters.Add(new NpgsqlParameter("value", count));
                                        command.ExecuteNonQuery();
                                        
                                        // Hack: Drive down stock
                                        StockManagementLogic logic = new StockManagementLogic();
                                        for(int cd = 0; cd < count; cd++)
                                            logic.Vaccinate(HealthFacility.GetHealthFacilityById(hf), new VaccinationEvent() {
                                                DoseId = dose.Id
                                            });
                                        command.Parameters.Clear();
                                    }
                            }

                        }
                        status.Text = "Stock tally sheet submitted";
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