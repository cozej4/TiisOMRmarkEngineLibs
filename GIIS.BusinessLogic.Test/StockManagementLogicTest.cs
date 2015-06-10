using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GIIS.DataLayer;

namespace GIIS.BusinessLogic.Test
{
    /// <summary>
    /// Tests for the <see cref="T:GIIS.BusinessLogic.StockManagementLogic"/> class.
    /// </summary>
    [TestClass]
    public class StockManagementLogicTest
    {

        /// <summary>
        /// Class initialization
        /// </summary>
        [ClassInitialize]
        public void Initialize()
        {
            TestData.InitializeTestDataset();
        }

        /// <summary>
        /// Test Allocations
        /// </summary>
        [TestMethod]
        public void AllocationTest()
        {
            StockManagementLogic stockLogic = new StockManagementLogic();
            // Assertion of initial stock balances
            HealthFacilityBalance balance = HealthFacilityBalance.GetHealthFacilityBalanceByHealthFacilityCode("HF888").Find(o => o.Gtin == TestData.GTIN_UNDER_TEST);
            int preBalance = (int)balance.Allocated;
            stockLogic.Allocate(HealthFacility.GetHealthFacilityByCode("HF888"), TestData.GTIN_UNDER_TEST, balance.LotNumber, 100, null, 1);
            // Get the new balance 
            balance = HealthFacilityBalance.GetHealthFacilityBalanceByHealthFacilityCode("HF888").Find(o => o.Gtin == TestData.GTIN_UNDER_TEST);
            Assert.AreEqual(preBalance + 100, balance.Allocated);
        }

        /// <summary>
        /// Test Transfer
        /// </summary>
        [TestMethod]
        public void TransferTest()
        {
            StockManagementLogic stockLogic = new StockManagementLogic();
            // Assertion of initial stock balances
            HealthFacilityBalance balanceHf888 = HealthFacilityBalance.GetHealthFacilityBalanceByHealthFacilityCode("HF888").Find(o => o.Gtin == TestData.GTIN_UNDER_TEST),
                balanceHf999 = HealthFacilityBalance.GetHealthFacilityBalanceByHealthFacilityCode("HF999").Find(o => o.Gtin == TestData.GTIN_UNDER_TEST);

            int preBalance888 = (int)balanceHf888.Balance,
                preBalance999 = (int)balanceHf999.Balance;

            stockLogic.Transfer(HealthFacility.GetHealthFacilityByCode("HF888"), HealthFacility.GetHealthFacilityByCode("HF999"), TestData.GTIN_UNDER_TEST, balanceHf888.LotNumber, null, 100, 1);
            // Get the new balance 
            balanceHf888 = HealthFacilityBalance.GetHealthFacilityBalanceByHealthFacilityCode("HF888").Find(o => o.Gtin == TestData.GTIN_UNDER_TEST);
            balanceHf999 = HealthFacilityBalance.GetHealthFacilityBalanceByHealthFacilityCode("HF999").Find(o => o.Gtin == TestData.GTIN_UNDER_TEST);

            Assert.AreEqual(preBalance888 - 100, balanceHf888.Balance);
            Assert.AreEqual(preBalance999 + 100, balanceHf999.Balance);
        }

    }
}
