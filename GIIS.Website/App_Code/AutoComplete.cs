using System;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GIIS.DataLayer;
using DataStructures.AutoComplete;



/// <summary>
/// Summary description for AutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 

[System.Web.Script.Services.ScriptService]
public class AutoComplete : System.Web.Services.WebService
{

    public AutoComplete()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }


    [WebMethod]
    public string[] GetCompletionList(string prefixText, int count)
    {

        //-----I defined a parameter instead of passing value directly to 
        //prevent SQL injection--------//
        String query = @"select * from ""ITEM_CATEGORY"" Where LOWER(""NAME"") like '%" + prefixText.ToLower() + "%'";
        //cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");
        DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

        //Then return List of string(txtItems) as result
        List<string> txtItems = new List<string>();
        // String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            txtItems.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row[1].ToString(), row[0].ToString()));
        }

        return txtItems.ToArray();
    }

    //Get HealthCenters
    [WebMethod(EnableSession = true)]
    public string[] GetHealthCenters(string prefixText, int count)
    {
        Trie hfTrie = (Trie)Context.Cache["HFLTrie"];
        if (hfTrie == null)
        {
            String query = @"select ""ID"", ""NAME"" from ""HEALTH_FACILITY"" Where ""LEAF"" = false AND ""IS_ACTIVE"" = true "; // ""NAME"" ilike '" + prefixText.ToLower() + "%'";

            //cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");
            HealthFacility hf = HealthFacility.GetHealthFacilityById(CurrentEnvironment.LoggedUser.HealthFacilityId);

            if (!hf.TopLevel)
            {
                string s = HealthFacility.GetAllChildsForOneHealthFacility(hf.Id);
                query += String.Format(@" AND ""ID"" in ({0})", s);
            }

            DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

            hfTrie = new Trie();
            foreach (DataRow row in dt.Rows)
            {
                hfTrie.Add(row[1].ToString() + "," + row[0].ToString());
            }
            Context.Cache["HFLTrie"] = hfTrie;
        }


        //Then return List of string(txtItems) as result
        List<string> txtItems = new List<string>();
        // String dbValues;
        txtItems = hfTrie.GetCompletionList(prefixText, count);
        List<string> list = new List<string>();
        for (int i = 0; i < txtItems.Count; i++)
        {
            int indx = txtItems[i].IndexOf(",");
            string f = txtItems[i].Substring(0, indx);
            string l = txtItems[i].Substring(indx + 1);
            list.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(f, l));
        }
        return list.ToArray();
    }
    [WebMethod]
    public string[] GetAllHealthCenters(string prefixText, int count)
    {
        Trie hfTrie = (Trie)Context.Cache["HFTrie"];
        if (hfTrie == null)
        {
            String query = @"select ""ID"", ""NAME"" from ""HEALTH_FACILITY"" Where ""IS_ACTIVE"" = true "; // ""NAME"" ilike '" + prefixText.ToLower() + "%'";

            //cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");

            DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

            hfTrie = new Trie();
            foreach (DataRow row in dt.Rows)
            {
                hfTrie.Add(row[1].ToString() + "," + row[0].ToString());
            }
            Context.Cache["HFTrie"] = hfTrie;
        }


        //Then return List of string(txtItems) as result
        List<string> txtItems = new List<string>();
        // String dbValues;
        txtItems = hfTrie.GetCompletionList(prefixText, count);
        List<string> list = new List<string>();
        for (int i = 0; i < txtItems.Count; i++)
        {
            int indx = txtItems[i].IndexOf(",");
            string f = txtItems[i].Substring(0, indx);
            string l = txtItems[i].Substring(indx + 1);
            list.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(f, l));
        }
        return list.ToArray();
    }
    [WebMethod]
    public string[] GetVaccineStores(string prefixText, int count)
    {

        Trie hfTrie = (Trie)Context.Cache["VSTrie"];
        if (hfTrie == null)
        {
            String query = @"select ""ID"", ""NAME"" from ""HEALTH_FACILITY"" Where ""VACCINE_STORE"" = true AND ""IS_ACTIVE"" = true "; // ""NAME"" ilike '" + prefixText.ToLower() + "%'";

            //cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");

            DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

            hfTrie = new Trie();
            foreach (DataRow row in dt.Rows)
            {
                hfTrie.Add(row[1].ToString() + "," + row[0].ToString());
            }
            Context.Cache["VSTrie"] = hfTrie;
        }


        //Then return List of string(txtItems) as result
        List<string> txtItems = new List<string>();
        // String dbValues;
        txtItems = hfTrie.GetCompletionList(prefixText, count);
        List<string> list = new List<string>();
        for (int i = 0; i < txtItems.Count; i++)
        {
            int indx = txtItems[i].IndexOf(",");
            string f = txtItems[i].Substring(0, indx);
            string l = txtItems[i].Substring(indx + 1);
            list.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(f, l));
        }
        return list.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetSubHealthCenters(string prefixText, int count)
    {

        //-----I defined a parameter instead of passing value directly to 
        //prevent SQL injection--------//
        Trie hfTrie = (Trie)Context.Cache["SubVSTrie"];
        if (hfTrie == null)
        {
            String query = @"select ""ID"", ""NAME"" from ""HEALTH_FACILITY"" Where ""VACCINE_STORE"" = true AND ""IS_ACTIVE"" = true "; // ""NAME"" ilike '" + prefixText.ToLower() + "%'";

            //cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");
            HealthFacility hf = HealthFacility.GetHealthFacilityById(CurrentEnvironment.LoggedUser.HealthFacilityId);

            if (!hf.TopLevel)
            {
                string s = HealthFacility.GetAllChildsForOneHealthFacility(hf.Id);
                query += String.Format(@" AND ""ID"" in ({0})", s);
            }

            DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

            hfTrie = new Trie();
            foreach (DataRow row in dt.Rows)
            {
                hfTrie.Add(row[1].ToString() + "," + row[0].ToString());
            }
            Context.Cache["SubVSTrie"] = hfTrie;
        }


        //Then return List of string(txtItems) as result
        List<string> txtItems = new List<string>();
        // String dbValues;
        txtItems = hfTrie.GetCompletionList(prefixText, count);
        List<string> list = new List<string>();
        for (int i = 0; i < txtItems.Count; i++)
        {
            int indx = txtItems[i].IndexOf(",");
            string f = txtItems[i].Substring(0, indx);
            string l = txtItems[i].Substring(indx + 1);
            list.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(f, l));
        }
        //foreach (DataRow row in dt.Rows)
        //{
        //    txtItems.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row[1].ToString(), row[0].ToString()));
        //}

        return list.ToArray();
    }

    [WebMethod]
    public string[] GetVaccinationPoints(string prefixText, int count)
    {

        Trie hfTrie = (Trie)Context.Cache["VPTrie"];
        if (hfTrie == null)
        {
            String query = @"select ""ID"", ""NAME"" from ""HEALTH_FACILITY"" Where ""VACCINATION_POINT"" = true AND ""IS_ACTIVE"" = true "; // ""NAME"" ilike '" + prefixText.ToLower() + "%'";

            //cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");

            DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

            hfTrie = new Trie();
            foreach (DataRow row in dt.Rows)
            {
                hfTrie.Add(row[1].ToString() + "," + row[0].ToString());
            }
            Context.Cache["VPTrie"] = hfTrie;
        }


        //Then return List of string(txtItems) as result
        List<string> txtItems = new List<string>();
        // String dbValues;
        txtItems = hfTrie.GetCompletionList(prefixText, count);
        List<string> list = new List<string>();
        for (int i = 0; i < txtItems.Count; i++)
        {
            int indx = txtItems[i].IndexOf(",");
            string f = txtItems[i].Substring(0, indx);
            string l = txtItems[i].Substring(indx + 1);
            list.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(f, l));
        }
        return list.ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string[] GetSubVaccinationPoints(string prefixText, int count)
    {

        //-----I defined a parameter instead of passing value directly to 
        //prevent SQL injection--------//
        Trie hfTrie = (Trie)Context.Cache["SubVPTrie"];
        if (hfTrie == null)
        {
            String query = @"select ""ID"", ""NAME"" from ""HEALTH_FACILITY"" Where ""VACCINATION_POINT"" = true AND ""IS_ACTIVE"" = true "; // ""NAME"" ilike '" + prefixText.ToLower() + "%'";

            //cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");
            HealthFacility hf = HealthFacility.GetHealthFacilityById(CurrentEnvironment.LoggedUser.HealthFacilityId);

            if (!hf.TopLevel)
            {
                string s = HealthFacility.GetAllChildsForOneHealthFacility(hf.Id);
                query += String.Format(@" AND ""ID"" in ({0})", s);
            }

            DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

            hfTrie = new Trie();
            foreach (DataRow row in dt.Rows)
            {
                hfTrie.Add(row[1].ToString() + "," + row[0].ToString());
            }
            Context.Cache["SubVPTrie"] = hfTrie;
        }


        //Then return List of string(txtItems) as result
        List<string> txtItems = new List<string>();
        // String dbValues;
        txtItems = hfTrie.GetCompletionList(prefixText, count);
        List<string> list = new List<string>();
        for (int i = 0; i < txtItems.Count; i++)
        {
            int indx = txtItems[i].IndexOf(",");
            string f = txtItems[i].Substring(0, indx);
            string l = txtItems[i].Substring(indx + 1);
            list.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(f, l));
        }
        //foreach (DataRow row in dt.Rows)
        //{
        //    txtItems.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row[1].ToString(), row[0].ToString()));
        //}

        return list.ToArray();
    }

    //Get Places
    [WebMethod]
    public string[] GetPlaces(string prefixText, int count)
    {
        Trie hfTrie = (Trie)Context.Cache["PlaceTrie"];
        if (hfTrie == null)
        {
            String query = @"Select p1.""ID"", p1.""NAME"" from ""PLACE"" p1  where p1.""LEAF"" = FALSE AND p1.""IS_ACTIVE"" = true "; // and p1.""NAME"" ilike '" + prefixText.ToLower() + "%'";

            //cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");

            DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

            hfTrie = new Trie();
            foreach (DataRow row in dt.Rows)
            {
                hfTrie.Add(row[1].ToString() + "," + row[0].ToString());
            }
            Context.Cache["PlaceTrie"] = hfTrie;
        }


        //Then return List of string(txtItems) as result
        List<string> txtItems = new List<string>();
        // String dbValues;
        txtItems = hfTrie.GetCompletionList(prefixText, count);
        List<string> list = new List<string>();
        for (int i = 0; i < txtItems.Count; i++)
        {
            int indx = txtItems[i].IndexOf(",");
            string f = txtItems[i].Substring(0, indx);
            string l = txtItems[i].Substring(indx + 1);
            list.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(f, l));
        }
        return list.ToArray();
    }

    [WebMethod]
    public string[] GetLeafPlaces(string prefixText, int count)
    {
        Trie hfTrie = (Trie)Context.Cache["PlaceLTrie"];
        if (hfTrie == null)
        {
            String query = @"Select p1.""ID"", p1.""NAME"" || '-' || p2.""NAME""  from ""PLACE"" p1 join ""PLACE"" p2 on p1.""PARENT_ID"" = p2.""ID"" where p1.""LEAF"" = true AND p1.""IS_ACTIVE"" = true "; // and p1.""NAME"" ilike '" + prefixText.ToLower() + "%'";

            //cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");

            DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

            hfTrie = new Trie();
            foreach (DataRow row in dt.Rows)
            {
                hfTrie.Add(row[1].ToString() + "," + row[0].ToString());
            }
            Context.Cache["PlaceLTrie"] = hfTrie;
        }


        //Then return List of string(txtItems) as result
        List<string> txtItems = new List<string>();
        // String dbValues;
        txtItems = hfTrie.GetCompletionList(prefixText, count);
        List<string> list = new List<string>();
        for (int i = 0; i < txtItems.Count; i++)
        {
            int indx = txtItems[i].IndexOf(",");
            string f = txtItems[i].Substring(0, indx);
            string l = txtItems[i].Substring(indx + 1);
            list.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(f, l));
        }
        return list.ToArray();

    }
    [WebMethod]
    public string[] GetCommunities(string prefixText, int count)
    {
        Trie hfTrie = (Trie)Context.Cache["CommTrie"];
        if (hfTrie == null)
        {
            String query = @"select ""ID"", ""NAME"" from ""COMMUNITY"" Where ""IS_ACTIVE"" = true"; // AND LOWER(""NAME"") like '%" + prefixText.ToLower() + "%'";

            //cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");

            DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

            hfTrie = new Trie();
            foreach (DataRow row in dt.Rows)
            {
                hfTrie.Add(row[1].ToString() + "," + row[0].ToString());
            }
            Context.Cache["CommTrie"] = hfTrie;
        }


        //Then return List of string(txtItems) as result
        List<string> txtItems = new List<string>();
        // String dbValues;
        txtItems = hfTrie.GetCompletionList(prefixText, count);
        List<string> list = new List<string>();
        for (int i = 0; i < txtItems.Count; i++)
        {
            int indx = txtItems[i].IndexOf(",");
            string f = txtItems[i].Substring(0, indx);
            string l = txtItems[i].Substring(indx + 1);
            list.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(f, l));
        }
        return list.ToArray();

       
    }
    //Get HealthCenters below the health center of the current user except national level
    [WebMethod(EnableSession = true)]
    public string[] GetHealthCentersforUser(string prefixText, int count)
    {

        Trie hfTrie = (Trie)Context.Cache["SubHFTrie"];
        if (hfTrie == null)
        {
            String query = @"select ""ID"", ""NAME"" from ""HEALTH_FACILITY"" Where ""IS_ACTIVE"" = true "; // ""NAME"" ilike '" + prefixText.ToLower() + "%'";

            //cmd.Parameters.AddWithValue("@myParameter", "%" + prefixText + "%");
            HealthFacility hf = HealthFacility.GetHealthFacilityById(CurrentEnvironment.LoggedUser.HealthFacilityId);

            if (!hf.TopLevel)
            {
                string s = HealthFacility.GetAllChildsForOneHealthFacility(hf.Id);
                query += String.Format(@" AND ""ID"" in ({0})", s);
            }

            DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

            hfTrie = new Trie();
            foreach (DataRow row in dt.Rows)
            {
                hfTrie.Add(row[1].ToString() + "," + row[0].ToString());
            }
            Context.Cache["SubHFTrie"] = hfTrie;
        }


        //Then return List of string(txtItems) as result
        List<string> txtItems = new List<string>();
        // String dbValues;
        txtItems = hfTrie.GetCompletionList(prefixText, count);
        List<string> list = new List<string>();
        for (int i = 0; i < txtItems.Count; i++)
        {
            int indx = txtItems[i].IndexOf(",");
            string f = txtItems[i].Substring(0, indx);
            string l = txtItems[i].Substring(indx + 1);
            list.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(f, l));
        }
        return list.ToArray();
    }
}