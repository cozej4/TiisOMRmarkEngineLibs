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
using GIIS.DataLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

public partial class Pages_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrentEnvironment.LoggedUser == null)
            Response.Redirect("../Default.aspx", false);
        else if (!Page.IsPostBack)
        {
            GetStockStatusinDosesChart();

            int hfid = CurrentEnvironment.LoggedUser.HealthFacilityId;

            DataTable originalData = new HealthFacilityBalance().GetCoverageChart(hfid);

             DataTable dt = Pivot(originalData, "Name", "Month", "Percentage");

            GetVaccinationCoverageChart(dt);
        }
       
    }

    private void GetStockStatusinDosesChart()
    {
        // this set the datasource
        int hfid = CurrentEnvironment.LoggedUser.HealthFacilityId;
        this.Chart1.DataSource = new HealthFacilityBalance().GetChartData(hfid);
        

        // clear all the (possible) existing series
        this.Chart1.Series.Clear();

        // add the hours series
        var hoursSeries = this.Chart1.Series.Add("Balance");
        hoursSeries.XValueMember = "NAME";
        hoursSeries.YValueMembers = "BALANCE";
        hoursSeries.ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;

        // add the percentages series
        var percSeries = this.Chart1.Series.Add("Safety Stock");
        percSeries.XValueMember = "NAME";
        percSeries.YValueMembers = "SAFETY_STOCK";
        percSeries.ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;

        Chart1.Legends.Add(new System.Web.UI.DataVisualization.Charting.Legend());
        Chart1.Titles.Add("Stock Status (in Doses)");

        Chart1.Series[0].IsValueShownAsLabel = true;
        Chart1.Series[1].IsValueShownAsLabel = true;
        Chart1.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 12;
        

        Chart1.Legends["Legend1"].LegendStyle = LegendStyle.Table;
        Chart1.Legends["Legend1"].Docking = Docking.Bottom;
        Chart1.Legends["Legend1"].Alignment = System.Drawing.StringAlignment.Center;

        Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
        Chart1.ChartAreas[0].AxisX.Interval = 1;
        
        //Chart1.ChartAreas[0].Area3DStyle.Enable3D = true;
    }

    private void GetVaccinationCoverageChart(DataTable initialDataSource)
    {
        DateTime currentDate = DateTime.Today;

        for (int i = 1; i < initialDataSource.Columns.Count; i++)
        {
            Series series = new Series();
            foreach (DataRow dr in initialDataSource.Rows)
            {
                //float y = (float)dr[i];
                series.Points.AddXY(dr["Name"].ToString(), dr[i]);
                series.Name = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.AddMonths(-4+i).Month);
                series.IsValueShownAsLabel = true;
            }
            Chart2.Series.Add(series);
        }

        Chart2.Legends.Add(new System.Web.UI.DataVisualization.Charting.Legend());
        Chart2.Titles.Add("Vaccination Coverage");


        Chart2.Legends["Legend1"].LegendStyle = LegendStyle.Table;
        Chart2.Legends["Legend1"].Docking = Docking.Bottom;
        Chart2.Legends["Legend1"].Alignment = System.Drawing.StringAlignment.Center;
    }

    public DataTable Pivot(
            DataTable src,
            string VerticalColumnName,
            string HorizontalColumnName,
            string ValueColumnName)
    {

        DataTable dst = new DataTable();
        if (src == null || src.Rows.Count == 0)
            return dst;

        // find all distinct names for column and row
        ArrayList ColumnValues = new ArrayList();
        ArrayList RowValues = new ArrayList();
        foreach (DataRow dr in src.Rows)
        {
            // find all column values
            object column = dr[VerticalColumnName];
            if (!ColumnValues.Contains(column))
                ColumnValues.Add(column);

            //find all row values
            object row = dr[HorizontalColumnName];
            if (!RowValues.Contains(row))
                RowValues.Add(row);
        }

        ColumnValues.Sort();
        RowValues.Sort();

        //create columns
        dst = new DataTable();
        dst.Columns.Add(VerticalColumnName, src.Columns[VerticalColumnName].DataType);
        Type t = src.Columns[ValueColumnName].DataType;
        foreach (object ColumnNameInRow in RowValues)
        {
            dst.Columns.Add(ColumnNameInRow.ToString(), t);
        }

        //create destination rows
        foreach (object RowName in ColumnValues)
        {
            DataRow NewRow = dst.NewRow();
            NewRow[VerticalColumnName] = RowName.ToString();
            dst.Rows.Add(NewRow);
        }

        //fill out pivot table
        foreach (DataRow drSource in src.Rows)
        {
            object key = drSource[VerticalColumnName];
            string ColumnNameInRow = Convert.ToString(drSource[HorizontalColumnName]);
            int index = ColumnValues.IndexOf(key);
            dst.Rows[index][ColumnNameInRow] = sum(dst.Rows[index][ColumnNameInRow], drSource[ValueColumnName]);
        }

        return dst;
    }

    dynamic sum(dynamic a, dynamic b)
    {
        if (a is DBNull && b is DBNull)
            return DBNull.Value;
        else if (a is DBNull && !(b is DBNull))
            return b;
        else if (!(a is DBNull) && b is DBNull)
            return a;
        else
            return a + b;
    }
}