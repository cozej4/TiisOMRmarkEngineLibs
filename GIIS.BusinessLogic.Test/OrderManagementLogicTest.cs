using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GIIS.DataLayer;
using System.Linq;

namespace GIIS.BusinessLogic.Test
{
    /// <summary>
    /// Order management logic test
    /// </summary>
    [TestClass]
    public class OrderManagementLogicTest
    {

        /// <summary>
        /// Initialize the tests
        /// </summary>
        [ClassInitialize]
       public static void Initialize(TestContext foo)
        {
          //  TestData.InitializeTestDataset();
        }

        /// <summary>
        /// Complete order workflow - From Manually Created Order
        /// </summary>
        [TestMethod]
        public void SuccessfulCompleteOrderManagementFlowTest()
        {

            try
            {
                // Order logic
                OrderManagementLogic orderLogic = new OrderManagementLogic();


                HealthFacility from = HealthFacility.GetHealthFacilityByCode("HF888");
                HealthFacility to = HealthFacility.GetHealthFacilityByCode("HF999");
                DateTime orderDate = DateTime.Now;

                // Step 1 : Create an Order
                var orderHeader = orderLogic.RequestOrder(from, to, orderDate, 1);
                
                // Assert that Step 1 actually creates the Order Header
                Assert.IsNotNull(orderHeader.OrderNum);
                Assert.AreEqual(from.Id, orderHeader.OrderFacilityFromObject.Id);
                Assert.AreEqual(to.Id, orderHeader.OrderFacilityToObject.Id);
                Assert.AreEqual(orderDate, orderHeader.OrderSchedReplenishDate);
                Assert.AreEqual((int)OrderStatusType.Requested, orderHeader.OrderStatus);

                // Step 2 : Create order line items
                var orderLine = orderLogic.AddOrderLine(orderHeader, TestData.GTIN_UNDER_TEST, null, 100, Uom.GetUomById(1), 1);
                // Assert line item was created with most recent lot #
                Assert.AreEqual(orderHeader.OrderNum, orderLine.OrderNum);
                Assert.AreEqual(TestData.GTIN_UNDER_TEST, orderLine.OrderGtin);
                Assert.AreEqual(TestData.GTIN_LOT_USE_FIRST, orderLine.OrderGtinLotnum);
                Assert.AreEqual((int)OrderStatusType.Requested, orderLine.OrderDetailStatus);

                // Step 3 : Release the order
                orderHeader = orderLogic.ReleaseOrder(orderHeader, 1);
                
                // Assert header is updated
                Assert.AreEqual(from.Id, orderHeader.OrderFacilityFromObject.Id);
                Assert.AreEqual(to.Id, orderHeader.OrderFacilityToObject.Id);
                Assert.AreEqual((int)OrderStatusType.Released, orderHeader.OrderStatus);

                // Assert lines are updated
                orderLine = TransferOrderDetail.GetTransferOrderDetailByOrderNum(orderHeader.OrderNum)[0];
                Assert.AreEqual((int)OrderStatusType.Released, orderHeader.OrderStatus);

                // Assert that there was a stock transaction for allocation
                var txAllocate = TransactionType.GetTransactionTypeList().First(o => o.Name == "Allocation");
                Assert.IsTrue(ItemTransaction.GetItemTransactionList().Exists(o => o.HealthFacilityCode == "HF888" && 
                    o.Gtin == TestData.GTIN_UNDER_TEST && 
                    o.GtinLot == TestData.GTIN_LOT_USE_FIRST && 
                    o.TransactionTypeId == txAllocate.Id && 
                    o.RefId == orderHeader.OrderNum.ToString() && 
                    o.RefIdNum == orderLine.OrderDetailNum && 
                    o.TransactionQtyInBaseUom == 100), "No allocation could be found");

                // Step 4: Pack the order
                orderHeader = orderLogic.PackOrder(orderHeader, 1);
                Assert.AreEqual(from.Id, orderHeader.OrderFacilityFromObject.Id);
                Assert.AreEqual(to.Id, orderHeader.OrderFacilityToObject.Id);
                Assert.AreEqual((int)OrderStatusType.Packed, orderHeader.OrderStatus);

                // Assert lines are updated
                orderLine = TransferOrderDetail.GetTransferOrderDetailByOrderNum(orderHeader.OrderNum)[0];
                Assert.AreEqual((int)OrderStatusType.Packed, orderHeader.OrderStatus);

                // Step 5: Ship the order
                orderHeader = orderLogic.ShipOrder(orderHeader, 1);
                Assert.AreEqual(from.Id, orderHeader.OrderFacilityFromObject.Id);
                Assert.AreEqual(to.Id, orderHeader.OrderFacilityToObject.Id);
                Assert.AreEqual((int)OrderStatusType.Shipped, orderHeader.OrderStatus);

                // Assert lines are updated
                orderLine = TransferOrderDetail.GetTransferOrderDetailByOrderNum(orderHeader.OrderNum)[0];
                Assert.AreEqual((int)OrderStatusType.Shipped, orderHeader.OrderStatus);

                // Assert allocations are made
                var txTrasnfer = TransactionType.GetTransactionTypeList().First(o => o.Name == "Transfer");
                Assert.IsTrue(ItemTransaction.GetItemTransactionList().Exists(o => o.HealthFacilityCode == "HF888" &&
                    o.Gtin == TestData.GTIN_UNDER_TEST &&
                    o.GtinLot == TestData.GTIN_LOT_USE_FIRST && 
                    o.TransactionTypeId == txAllocate.Id && 
                    o.RefId == orderHeader.OrderNum.ToString() && 
                    o.RefIdNum == orderLine.OrderDetailNum && 
                    o.TransactionQtyInBaseUom == -100), "No de-allocation could be found");
                Assert.IsTrue(ItemTransaction.GetItemTransactionList().Exists(o => o.HealthFacilityCode == "HF888" &&
                    o.Gtin == TestData.GTIN_UNDER_TEST &&
                    o.GtinLot == TestData.GTIN_LOT_USE_FIRST && 
                    o.TransactionTypeId == txTrasnfer.Id && 
                    o.RefId == orderHeader.OrderNum.ToString() && 
                    o.RefIdNum == orderLine.OrderDetailNum && 
                    o.TransactionQtyInBaseUom == -100), "No transfer out could be found");
                Assert.IsTrue(ItemTransaction.GetItemTransactionList().Exists(o => o.HealthFacilityCode == "HF999"
                    && o.Gtin == TestData.GTIN_UNDER_TEST &&
                    o.GtinLot == TestData.GTIN_LOT_USE_FIRST && 
                    o.TransactionTypeId == txTrasnfer.Id && 
                    o.RefId == orderHeader.OrderNum.ToString() && 
                    o.RefIdNum == orderLine.OrderDetailNum && 
                    o.TransactionQtyInBaseUom == 100), "No transfer in could be found");
            }
            catch(Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
    }
}
