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
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class _Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            List<string> actionList = null;
            string sessionNameAction = "";
            if (CurrentEnvironment.LoggedUser != null)
            {
                sessionNameAction = "__GIS_actionList_" + CurrentEnvironment.LoggedUser.Id;
                actionList = (List<string>)Session[sessionNameAction];
            }

            if (CurrentEnvironment.LoggedUser != null)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Reports-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Reports");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Reports-dictionary" + language, wtList);
                }

                //controls
                this.lblTitle.Text = wtList["ReportsPageTitle"];

                //this.lblImmunizationReports.Text = wtList["ReportsImmunization"];
                //this.mCohortCoverageReport.InnerText = wtList["ReportsCohortCoverage"];
                //this.mActivityReport.InnerText = wtList["ReportsActivityReport"];

                //this.lblStockReports.Text = wtList["ReportsStock"];

                //this.mRunningBalance.InnerText = wtList["ReportsRunningBalance"];
                //this.mItemInHealthFacility.InnerText = wtList["ReportsItemInHealthFacilities"];
                //this.mItemLotInHealthFacility.InnerText = wtList["ReportsItemLotInHealthFacilities"];
                //this.mStockCountList.InnerText = wtList["ReportsViewStockCounts"];
                //this.mAdjustmentsList.InnerText = wtList["ReportsViewAdjustments"];

                //this.mClosedVialWastage.InnerText = wtList["ReportsClosedVialWastage"];
                //this.mItemLotsCloseToExpiry.InnerText = wtList["ReportsItemLotsCloseToExpiry"];
                //this.mLotTracking.InnerText = wtList["ReportsLotTracking"];
                //this.mConsumption.InnerText = wtList["ReportsConsumption"];


                //visible 
                //this.lblImmunizationReports.Visible = actionList.Contains("ViewMenuImmunizationReports");
                //this.aCohortCoverageReport.Visible = actionList.Contains("ViewMenuCohortCoverageReport");
                //this.aActivityReport.Visible = actionList.Contains("ViewMenuActivityReport");

                //this.lblStockReports.Visible = actionList.Contains("ViewMenuStockReports");

                //this.aRunningBalance.Visible = actionList.Contains("ViewMenuRunningBalance");
                //this.aItemInHealthFacility.Visible = actionList.Contains("ViewMenuItemInHealthFacilities");
                //this.aItemLotInHealthFacility.Visible = actionList.Contains("ViewMenuItemLotInHealthFacilities");
                //this.aStockCountList.Visible = actionList.Contains("ViewMenuViewStockCounts");
                //this.aAdjustmentsList.Visible = actionList.Contains("ViewMenuViewAdjustments");

                //this.aClosedVialWastage.Visible = actionList.Contains("ViewMenuClosedVialWastage");
                //this.aItemLotsCloseToExpiry.Visible = actionList.Contains("ViewMenuItemLotsCloseToExpiry");
                //this.aLotTracking.Visible = actionList.Contains("ViewMenuLotTracking");
                //this.aConsumption.Visible = actionList.Contains("ViewMenuConsumption");


                // Populate 
                // HACK: Should be done with DAL but under time crunches
                StringBuilder myRoles = new StringBuilder();
                actionList.ForEach(a=>myRoles.AppendFormat("'{0}',", a));
                myRoles.Remove(myRoles.Length - 1, 1);
                string reportQuery = "SELECT \"REPORT\".*, \"GROUP_NAME\" FROM \"REPORT\" INNER JOIN \"REPORT_GROUP\" ON (\"REPORT\".\"GROUP_ID\" = \"REPORT_GROUP\".\"ID\") WHERE \"ACTION_ID\" IN (SELECT \"ID\" FROM \"ACTIONS\" WHERE \"NAME\" IN (" + myRoles.ToString() + ")) AND \"REPORT_TYPE\" = 'R' ORDER BY \"REPORT_GROUP\".\"GROUP_NAME\", \"REPORT\".\"REPORT_NAME\"";
                using(var dt = DBManager.ExecuteReaderCommand(reportQuery, System.Data.CommandType.Text, null))
                {
                    using(var rdr = dt.CreateDataReader())
                    {
                        string grpHeader = string.Empty;
                        HtmlGenericControl currentGroup = null;

                        while(rdr.Read())
                        {
                            // Group header
                            if(rdr["GROUP_NAME"].ToString() != grpHeader)
                            {
                                grpHeader = rdr["GROUP_NAME"].ToString();
                                var liGroup = new HtmlGenericControl("li");
                                liGroup.Controls.Add(new HtmlGenericControl("h4") { InnerText = grpHeader });
                                currentGroup = new HtmlGenericControl("ul");
                                liGroup.Controls.Add(currentGroup);
                                ulReports.Controls.Add(liGroup);
                            }

                            // Link to the report
                            var li = new HtmlGenericControl("li");
                            currentGroup.Controls.Add(li);
                            li.Controls.Add(new Label() { Text = rdr["REPORT_NAME"].ToString() });
                            li.Controls.Add(new Label() { Text = "(" });
                            li.Controls.Add(new HyperLink()
                                {
                                    Target = "_blank",
                                    Text = "PDF",
                                    NavigateUrl = "~/Pages/RunReport.aspx?reportId=" + rdr["ID"].ToString() + "&format=pdf"
                                }
                            );
                            li.Controls.Add(new Label() { Text = "|" });
                            li.Controls.Add(new HyperLink()
                            {
                                Target = "_blank",
                                Text = "XLS",
                                NavigateUrl = "~/Pages/RunReport.aspx?reportId=" + rdr["ID"].ToString() + "&format=xls"
                            }
                            );
                            li.Controls.Add(new Label() { Text = ")" });


                        }
                    }
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }


   
}
