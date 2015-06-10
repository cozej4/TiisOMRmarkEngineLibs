using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.DataLayer;

public partial class Pages_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
        string str = Request.ServerVariables.Get("SCRIPT_NAME");
        int index = str.LastIndexOf("/");
        string page = str.Substring(index + 1);
        string language = CurrentEnvironment.Language;
        int languageId = int.Parse(language);

        int i = Help.CheckExists(page, languageId);
        if (i > 0)
            helpTips.Text = Help.GetHelpByPageAndLanguage(page, languageId).HelpText;
        else
            helpTips.Text = "Help Tips <br/><br/><br/><br/>";

        if (CurrentEnvironment.LoggedUser == null)
            Response.Redirect("../Default.aspx", false);

       // ddlLanguage.SelectedValue = CurrentEnvironment.Language;

        if (!this.Page.IsPostBack)
        {
            Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Menu-dictionary" + language];
            if (wtList == null)
            {
                List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Menu");
                wtList = new Dictionary<string, string>();
                foreach (WordTranslate vwt in wordTranslateList)
                    wtList.Add(vwt.Code, vwt.Name);
                HttpContext.Current.Cache.Insert("Menu-dictionary" + language, wtList);
            }

            #region Controls
            //controls            
            this.mChild.InnerText = wtList["Child"];
            this.mImmunization.InnerText = wtList["Immunization"];
            this.mReports.InnerText = wtList["Reports"];
            this.mVaccineManagement.InnerText = wtList["VaccineManagement"];
            this.mStock.InnerText = wtList["Stock"];
            this.mCustomization.InnerText = wtList["Customization"];
            this.mConfiguration.InnerText = wtList["Configuration"];
            string tText = null;
            this.weighTallyLink.InnerText = wtList.TryGetValue("WeighTally", out tText) ? tText : "Weigh Tally";
            this.mRivoReceipt.InnerText = wtList.TryGetValue("RivoReceipt", out tText) ? tText : "RIVO Receipt";
            this.mForms.InnerText = wtList.TryGetValue("Forms", out tText) ? tText : "Forms";

            this.mSearchChildren.InnerText = wtList["SearchChildren"];
            this.mRegisterChild.InnerText = wtList["RegisterChild"];
            this.mFindDuplications.InnerText = wtList["FindDuplications"];

            this.mMonthlyPlan.InnerText = wtList["MonthlyPlan"];
            this.mImmunizationCard.InnerText = wtList["ImmunizationCard"];
            this.mRegister.InnerText = wtList["Register"];
            this.mHealthFacilityCohort.InnerText = wtList.TryGetValue("HealthFacilityCohort", out tText) ? tText : "Population";

            this.mOrder.InnerText = wtList["TransferOrders"];
            this.mMakeanOrder.InnerText = wtList["MakeanOrder"];
            this.mIncomingOrders.InnerText = wtList["IncomingOrders"];

            this.mCurrentStockByLot.InnerText = wtList["CurrentStockByLot"];
            this.mStockCount.InnerText = wtList["StockCount"];
            this.mAdjustment.InnerText = wtList["Adjustment"];
            this.mGtinHFStockPolicy.InnerText = wtList["GtinHFStockPolicy"];
            this.mItemManufacturer.InnerText = wtList["ItemManufacturer"];
            this.mItemLot.InnerText = wtList["ItemLot"];

            this.mHealthFacilityType.InnerText = wtList["HealthFacilityType"];
            this.mHealthFacilities.InnerText = wtList["HealthFacilities"];
            this.mPlaces.InnerText = wtList["Places"];
            this.mBirthplace.InnerText = wtList.TryGetValue("Birthplace", out tText) ? tText : "Birthplaces";
            this.mCommunities.InnerText = wtList["Communities"];

            this.mItems.InnerText = wtList["Items"];
            this.mManufacturers.InnerText = wtList["Manufacturers"];
            this.mAdjustmentReasons.InnerText = wtList["AdjustmentReasons"];
            this.mUom.InnerText = wtList["Uom"];

            this.mUsers.InnerText = wtList["Users"];
            this.mRoles.InnerText = wtList["Roles"];
            this.mAssignActionstoRole.InnerText = wtList["AssignActionstoRole"];

            this.mScheduledVaccinations.InnerText = wtList["ScheduledVaccinations"];
            this.mAgeDefinitions.InnerText = wtList["AgeDefinitions"];
            this.mDoses.InnerText = wtList["Doses"];
            this.mNonVaccinationReasons.InnerText = wtList["NonVaccinationReasons"];

            this.mGlobalConfigurations.InnerText = wtList["GlobalConfigurations"];
            this.mChildConfiguration.InnerText = wtList["ChildConfiguration"];
            this.mHelp.InnerText = wtList["Help"];
            this.mTranslation.InnerText = wtList["Translation"];
            this.mLanguages.InnerText = wtList["Languages"];
            this.mItemCategories.InnerText = wtList["ItemCategories"];
            this.mSystemModules.InnerText = wtList["SystemModules"];

            //this.mDispatches.InnerText = wtList["Dispatches"];
            //this.mArrival.InnerText = wtList["Arrival"];
            //this.mMakeanOrder.InnerText = wtList["MakeanOrder"];
            //this.mIncomingOrders.InnerText = wtList["IncomingOrders"];
            //this.mOutgoingOrders.InnerText = wtList["OutgoingOrders"];
            //this.mIncomingDispatches.InnerText = wtList["IncomingDispatches"];
            // this.mMakeDispatch.InnerText = wtList["MakeaDispatch"];
            //this.mOutgoingDispatches.InnerText = wtList["OutgoingDispatches"];

            //this.mCurrentStock.InnerText = wtList["CurrentStock"];
            //this.mCurrentStockByLot.InnerText = wtList["CurrentStockByLot"];
            //this.mStockCount.InnerText = wtList["StockCount"];
            //this.mAdjustment.InnerText = wtList["Adjustment"];
            //this.mOpenStock.InnerText = wtList["OpenStock"];
            //this.mMinMaxStock.InnerText = wtList["MinMaxStock"];

            // this.mUserManagement.InnerText = wtList["UserManagement"];
            // this.mAssignRoletoUser.InnerText = wtList["AssignRoletoUser"];
            #endregion

            List<string> actions = null;
            string sessionNameAction = "";
            if (CurrentEnvironment.LoggedUser != null)
            {
                sessionNameAction = "__GIS_actionList_" + CurrentEnvironment.LoggedUser.Id;
                actions = (List<string>)Session[sessionNameAction];
            }

            if (CurrentEnvironment.LoggedUser != null && (actions == null || actions.Count== 0))
            {
                actions = new List<string>();

                List<Actions> actionList = Actions.GetActionsListByUserId(CurrentEnvironment.LoggedUser.Id);
                foreach (Actions action in actionList)
                {
                    actions.Add(action.Name);
                }
            }

            Session[sessionNameAction] = actions;

            if (actions != null && actions.Count >= 1)
            {
                #region Menus
                // visible menus
                this.aChild.Visible = actions.Contains("ViewMenuChild");
                this.aImmunization.Visible = actions.Contains("ViewMenuImmunization");
                this.aReports.Visible = actions.Contains("ViewMenuReports");
                this.aVaccineManagement.Visible = actions.Contains("ViewMenuVaccinationSchedule");
                this.aStock.Visible = actions.Contains("ViewMenuStock");
                this.aCustomization.Visible = actions.Contains("ViewMenuSetup");
                this.aConfiguration.Visible = actions.Contains("ViewMenuConfiguration");

                this.aSearchChildren.Visible = actions.Contains("ViewMenuSearchChildren");
                this.aRegisterChild.Visible = actions.Contains("ViewMenuRegisterChild");
                this.aFindDuplications.Visible = actions.Contains("ViewMenuFindDuplications");

                this.aMonthlyPlan.Visible = actions.Contains("ViewMenuMonthlyPlan");
                this.aImmunizationCard.Visible = actions.Contains("ViewMenuImmunizationCard");
                this.aRegister.Visible = actions.Contains("ViewMenuRegister");
                this.aHealthFacilityCohort.Visible = actions.Contains("ViewMenuHealthFacilityCohort");

                this.aOrder.Visible = actions.Contains("ViewMenuOrders");
                this.aMakeanOrder.Visible = actions.Contains("ViewMenuNewTransferOrder");
                this.aIncomingOrders.Visible = actions.Contains("ViewMenuTransferOrders");

                this.aStock.Visible = actions.Contains("ViewMenuStock");
                this.aCurrentStockByLot.Visible = actions.Contains("ViewMenuCurrentStock");
                this.aStockCount.Visible = actions.Contains("ViewMenuStockCount");
                this.aAdjustment.Visible = actions.Contains("ViewMenuMakeAdjustment");
                this.aGtinHFStockPolicy.Visible = actions.Contains("ViewMenuGtinHFStockPolicy");
                this.aItemManufacturer.Visible = actions.Contains("ViewMenuItemManufacturer");
                this.aItemLot.Visible = actions.Contains("ViewMenuItemLot");

                this.aScheduledVaccinations.Visible = actions.Contains("ViewMenuScheduledVaccinations");
                this.aAgeDefinitions.Visible = actions.Contains("ViewMenuAgeDefinitions");
                this.aDoses.Visible = actions.Contains("ViewMenuDoses");
                this.aNonVaccinationReasons.Visible = actions.Contains("ViewMenuNonVaccinationReasons");
                this.aRivoReceipt.Visible = actions.Contains("RivoReceipts");
                //this.aStockBalanceReports.Visible = actions.Contains("ViewMenuStockBalanceReports");
                //this.aRunningBalance.Visible = actions.Contains("ViewMenuRunningBalance");
                //this.aItemInHealthFacility.Visible = actions.Contains("ViewMenuItemInHealthFacilities");
                //this.aItemLotInHealthFacility.Visible = actions.Contains("ViewMenuItemLotInHealthFacilities");
                //this.aStockCountList.Visible = actions.Contains("ViewMenuViewStockCounts");
                //this.aAdjustmentsList.Visible = actions.Contains("ViewMenuViewAdjustments");

                //this.aImmunizationReports.Visible = actions.Contains("ViewMenuImmunizationReports");
                //this.aCohortCoverageReport.Visible = actions.Contains("ViewMenuCohortCoverageReport");
                //this.aStockReports.Visible = actions.Contains("ViewMenuStockReports");
                //this.aClosedVialWastage.Visible = actions.Contains("ViewMenuClosedVialWastage");
                //this.aItemLotsCloseToExpiry.Visible = actions.Contains("ViewMenuItemLotsCloseToExpiry");
                //this.aLotTracking.Visible = actions.Contains("ViewMenuLotTracking");
                //this.aConsumption.Visible = actions.Contains("ViewMenuConsumption");

                // HACK: JF : Need a formal permission for this
                this.weighTallyLink.Visible = actions.Contains("ViewMenuHealthFacilities");

                this.aHealthFacilityType.Visible = actions.Contains("ViewMenuHealthFacilityType");
                this.aHealthFacilities.Visible = actions.Contains("ViewMenuHealthFacilities");
                this.aPlaces.Visible = actions.Contains("ViewMenuPlaces");
                this.aCommunities.Visible = actions.Contains("ViewMenuCommunities");
                this.aItems.Visible = actions.Contains("ViewMenuItems");
                this.aManufacturers.Visible = actions.Contains("ViewMenuManufacturers");
                this.aAdjustmentReasons.Visible = actions.Contains("ViewMenuAdjustmentReasons");
                this.aUom.Visible = actions.Contains("ViewUom");
                this.aBirthPlace.Visible = actions.Contains("ViewMenuPlaces"); //actions.Contains("ViewMenuBirthplace");

                // this.aUserManagement.Visible = actions.Contains("ViewMenuUserManagement");
                this.aUsers.Visible = actions.Contains("ViewMenuUsers");
                this.aRoles.Visible = actions.Contains("ViewMenuRoles");
                // this.aAssignRoletoUser.Visible = actions.Contains("ViewMenuAssignRoletoUser");
                this.aAssignActionstoRole.Visible = actions.Contains("ViewMenuAssignActionstoRole");

                this.aGlobalConfigurations.Visible = actions.Contains("ViewMenuGlobalConfigurations");
                this.aChildConfiguration.Visible = false;// actions.Contains("ViewMenuChildConfiguration");
                this.aItemCategories.Visible = actions.Contains("ViewMenuItemCategories");
                this.aHelp.Visible = actions.Contains("ViewMenuHelp");
                this.aTranslation.Visible = actions.Contains("ViewMenuTranslation");
                this.aLanguages.Visible = actions.Contains("ViewMenuLanguages");
                this.aSystemModules.Visible = false; // actions.Contains("ViewMenuSystemModules");

                #endregion
            }
        }
        else
        {
        }
        }
    }
  
}
