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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIIS.BusinessLogic
{
    /// <summary>
    /// Stock management business logic
    /// </summary>
    public class StockManagementLogic
    {

        /// <summary>
        /// Gets the current health facility balance for <paramref name="facility"/> of item with <paramref name="gtin"/> or creates if necessary
        /// </summary>
        /// <param name="facility">The facility to query for current balance</param>
        /// <param name="gtin">The GTIN of the item to retrieve the balance for</param>
        /// <param name="lot">The lot number to retrieve the balance</param>
        /// <returns></returns>
        private HealthFacilityBalance GetCurrentBalance(HealthFacility facility, String gtin, String lot)
        {

            if (facility == null)
                throw new ArgumentNullException("facility");
            else if (String.IsNullOrEmpty(gtin))
                throw new ArgumentNullException("gtin");
            else if (String.IsNullOrEmpty(lot))
                throw new ArgumentNullException("lot");

            // Current balance
            HealthFacilityBalance balance = HealthFacilityBalance.GetHealthFacilityBalanceByHealthFacilityCode(facility.Code).Find(o => o.Gtin == gtin && o.LotNumber == lot);
            if (balance == null)
            {
                balance = new HealthFacilityBalance()
                {
                    Balance = 0,
                    Gtin = gtin,
                    HealthFacilityCode = facility.Code,
                    LotNumber = lot
                };
                HealthFacilityBalance.Insert(balance);
            }
            return balance;
        }

        /// <summary>
        /// Make an adjustment
        /// </summary>
        /// <param name="facility">The facility in which the adjustment is being made</param>
        /// <param name="gtin">The GTIN of the stock item being adjusted</param>
        /// <param name="lot">The lot number of the stock being adjusted</param>
        /// <param name="qty">The amount of the adjustment</param>
        /// <param name="reason">The reason for the adjustment</param>
        /// <returns>The <see cref="T:GIIS.DataLayer.ItemTransaction"/> representing the transaction created</returns>
        /// <remarks>This method updates the ItemTransaction and HealthFacility Balances</remarks>
        public ItemTransaction Adjust(HealthFacility facility, String gtin, String lot, Int32 qty, AdjustmentReason reason, Int32 modifiedBy, DateTime date)
        {

            if (facility == null)
                throw new ArgumentNullException("facility");
            else if (String.IsNullOrEmpty(gtin))
                throw new ArgumentNullException("gtin");

            // Adjustment type
            TransactionType adjustmentType = TransactionType.GetTransactionTypeList().FirstOrDefault(o => o.Name == "Adjustment");
            if (adjustmentType == null)
                throw new InvalidOperationException("Cannot find transaction type 'Adjustment'");

            var balance = this.GetCurrentBalance(facility, gtin, lot);

            // Sanity check, is there a balance of this? 
            //if (qty < 0 && -qty > balance.Balance)
            //    throw new InvalidOperationException("Quantity is a negative adjustment on a balance which exceeds the current balance");
             int quantity;
             if (reason.Positive)
                 quantity = qty;
             else
                 quantity = -qty;
            // Create the item transaction
            ItemTransaction retVal = new ItemTransaction()
            {
                ModifiedBy = modifiedBy,
                AdjustmentId = reason.Id,
                Gtin = gtin,
                GtinLot = lot,
                HealthFacilityCode = facility.Code,
                ModifiedOn = DateTime.Now,
                TransactionDate = date,
                TransactionQtyInBaseUom = quantity,
                TransactionTypeId = adjustmentType.Id
            };

            // Adjust the balance
            // An adjustment reason can be positive - results in increased balance
            if (reason.Id != 99)
            {
                if (reason.Positive)
                    balance.Balance += qty;
                else
                {
                    balance.Balance -= qty;
                    balance.Wasted += qty;
                }
            }
            // Adjust the balances of other reasons
            //if(adjustmentType != null)
            //{
            //    switch(adjustmentType.Name)
            //    {
            //        case "Adjustments":
            //            balance.Wasted += -qty;
            //            break;
            //    }
            //}

            // Save the balances
            HealthFacilityBalance.Update(balance);
            int i = ItemTransaction.Insert(retVal);

            return ItemTransaction.GetItemTransactionById(i);
        }

        /// <summary>
        /// Allocate stock
        /// </summary>
        /// <param name="facility">The facility in which to allocate stock</param>
        /// <param name="gtin">The GTIN of the stock to be allocated</param>
        /// <param name="lot">The lot number of stock to be allocated</param>
        /// <param name="qty">The amount of stock to be allocated</param>
        /// <returns>The <see cref="T:GIIS.DataLayer.ItemTransaction"/> representing the allocation</returns>
        public ItemTransaction Allocate(HealthFacility facility, String gtin, String lot, Int32 qty, TransferOrderDetail orderLine, Int32 modifiedBy)
        {

            // Sanity check
            if (facility == null)
                throw new ArgumentNullException("facility");
            else if (String.IsNullOrEmpty(gtin))
                throw new ArgumentNullException("gtin");
            else if (String.IsNullOrEmpty(lot))
                throw new ArgumentNullException("lot");

            // Adjustment type
            TransactionType allocationType = TransactionType.GetTransactionTypeList().FirstOrDefault(o => o.Name == "Allocation");
            if (allocationType == null)
                throw new InvalidOperationException("Cannot find transaction type 'Allocation'");

            // Get current balance and ensure we have enough to do the balance
            var balance = this.GetCurrentBalance(facility, gtin, lot);
            
            //if (qty > 0 && balance.Balance < qty)
            //    throw new InvalidOperationException(String.Format("Cannot de-allocate more stock than is available (Request: {0},  Available:{1}, GTIN: {2})", qty, balance, ItemLot.GetItemLotByGtin(gtin).ItemObject.Name));
            //else if (qty < 0 && balance.Allocated < -qty)
            //    throw new InvalidOperationException("Cannot de-allocate more stock than is currently allocated");

            // Create an item transaction
            ItemTransaction retVal = new ItemTransaction()
            {
                Gtin = gtin,
                GtinLot = lot,
                HealthFacilityCode = facility.Code,
                ModifiedBy = modifiedBy,
                ModifiedOn = DateTime.Now,
                RefId = orderLine != null ? orderLine.OrderNum.ToString() : null,
                RefIdNum = orderLine != null ? orderLine.OrderDetailNum : 0,
                TransactionDate = DateTime.Now,
                TransactionQtyInBaseUom = qty,
                TransactionTypeId = allocationType.Id
            };

            // Update the balance
            //balance.Balance -= qty;
            balance.Allocated += qty;

            // Save
            HealthFacilityBalance.Update(balance);
            retVal.Id = ItemTransaction.Insert(retVal);

            return retVal;
        }

        /// <summary>
        /// Transfers stock from facility <paramref name="from"/> to facility <paramref name="to"/>
        /// </summary>
        /// <param name="from">The facility from which stock is being transferred</param>
        /// <param name="to">The facility to which stock is being transferred</param>
        /// <param name="gtin">The GTIN of the stock being transfered</param>
        /// <param name="lot">The lot of the stock being transferred</param>
        /// <param name="orderDetail">The order detail which informed this transfer</param>
        /// <param name="qty">The quantity of stock to be transferred</param>
        /// <returns>The <see cref="T:GIIS.DataLayer.ItemTransaction"/> representing the transfers </returns>
        public List<ItemTransaction> Transfer(HealthFacility from, HealthFacility to, String gtin, String lot, TransferOrderDetail orderDetail, Int32 qty, Int32 modifiedBy)
        {
            // Validate parameters
            if (from == null)
                throw new ArgumentNullException("from");
            else if (to == null)
                throw new ArgumentNullException("to");
            else if (String.IsNullOrEmpty(gtin))
                throw new ArgumentNullException("gtin");
            else if (String.IsNullOrEmpty(lot))
                throw new ArgumentNullException("lot");

            // Transfer type
            TransactionType transferType = TransactionType.GetTransactionTypeList().FirstOrDefault(o => o.Name == "Transfer");
            if (transferType == null)
                throw new InvalidOperationException("Cannot find transaction type 'Transfer'");

            // Lookup the current balances in both facilities
            HealthFacilityBalance fromBalance = this.GetCurrentBalance(from, gtin, lot),
                toBalance = this.GetCurrentBalance(to, gtin, lot);

            // Validate the fromBalance has sufficient quantity to transfer
            //if (fromBalance.Balance < qty)
            //    throw new InvalidOperationException("Insufficient quantity in the 'from' facility to perform transfer");

            // Two ItemTransactions
            ItemTransaction fromTransaction = new ItemTransaction()
            {
                Gtin = gtin,
                GtinLot = lot,
                HealthFacilityCode = from.Code,
                ModifiedBy = modifiedBy,
                ModifiedOn = DateTime.Now,
                RefId = orderDetail != null ? orderDetail.OrderNum.ToString() : null,
                RefIdNum = orderDetail != null ? orderDetail.OrderDetailNum : 0,
                TransactionDate = DateTime.Now,
                TransactionQtyInBaseUom = -qty, // transfer out
                TransactionTypeId = transferType.Id
            },
            toTrasnaction = new ItemTransaction()
            {
                Gtin = gtin,
                GtinLot = lot,
                HealthFacilityCode = to.Code,
                ModifiedBy = modifiedBy,
                ModifiedOn = DateTime.Now,
                RefId = orderDetail != null ? orderDetail.OrderNum.ToString() : null,
                RefIdNum = orderDetail != null ? orderDetail.OrderDetailNum : 0,
                TransactionDate = DateTime.Now,
                TransactionQtyInBaseUom = qty, // transfer in
                TransactionTypeId = transferType.Id
            };

            // Update balances 
            fromBalance.Balance -= qty;
            toBalance.Balance += qty;
            toBalance.Received += qty;
            fromBalance.Distributed += qty;

            // Save
            HealthFacilityBalance.Update(fromBalance);
            HealthFacilityBalance.Update(toBalance);
            fromTransaction.Id = ItemTransaction.Insert(fromTransaction);
            toTrasnaction.Id = ItemTransaction.Insert(toTrasnaction);

            // Return 
            return new List<ItemTransaction>() { fromTransaction, toTrasnaction };
        }

        /// <summary>
        /// Performs stock transaction for a vaccination event
        /// </summary>
        /// <param name="facility">The facility in which the vaccination event occurred</param>
        /// <param name="vaccination">The vaccination event</param>
        /// <returns>The <see cref="T:GIIS.DataLayer.ItemTransaction"/> representing the transfer</returns>
        public ItemTransaction Vaccinate(HealthFacility facility, VaccinationEvent vaccination)
        {
            
            List<ItemLot> lots = new List<ItemLot>();

            // Verify 
            if (facility == null)
                throw new ArgumentNullException("facility");
            else if (vaccination == null)
                throw new ArgumentNullException("vaccination");
            else if (String.IsNullOrEmpty(vaccination.VaccineLot))
            {
                ItemManufacturer gtin = ItemManufacturer.GetItemManufacturerByItem(vaccination.Dose.ScheduledVaccination.ItemId);
                OrderManagementLogic oml = new OrderManagementLogic();
                lots.Add(oml.GetOldestLot(facility, gtin.Gtin));
            }

            //throw new ArgumentException("Vaccination event missing lot#", "vaccination");

            // Item lot that was used
            if (lots.Count == 0)
                lots.Add(ItemLot.GetItemLotById(vaccination.VaccineLotId));
                //lots.Add(ItemLot.GetItemLotByLotNumber(vaccination.VaccineLot));
            if (lots.Count == 0)
                return null;

            
            // throw new InvalidOperationException("Cannot find the specified ItemLot relation (GTIN)");

            // Transaction type
            TransactionType vaccinationType = TransactionType.GetTransactionTypeList().FirstOrDefault(o => o.Name == "Vaccination");
            if (vaccinationType == null)
                throw new InvalidOperationException("Cannot find transaction type 'Vaccination'");

            // Return value
            ItemTransaction retVal = null;
            
            // Iterate through lots
            for (int i = 0; i < lots.Count; i++)
            {
                var lot = lots[i];

                // Get balance
                HealthFacilityBalance balance = this.GetCurrentBalance(facility, lot.Gtin, lot.LotNumber);
                //if (balance.Balance < 1)
                //    throw new InvalidOperationException("Insufficient doses to register vaccination event");

                // Create a transaction
#if XAMARIN
#else
#endif
                ItemTransaction itl = new ItemTransaction()
                {
                    Gtin = lot.Gtin,
                    GtinLot = lot.LotNumber,
                    HealthFacilityCode = facility.Code,
                    ModifiedBy = vaccination.ModifiedBy,
                    ModifiedOn = vaccination.ModifiedOn,
                    TransactionDate = vaccination.ModifiedOn,
                    TransactionQtyInBaseUom = -1,
                    TransactionTypeId = vaccinationType.Id
                };
                if (String.IsNullOrEmpty(vaccination.VaccineLot))
                {
                    vaccination.VaccineLotId = lot.Id;
                    VaccinationEvent.Update(vaccination);
                }

                // Update facility balances
                balance.Balance -= 1;
                balance.Used += 1;

                // Now save
                HealthFacilityBalance.Update(balance);
                itl.Id = ItemTransaction.Insert(itl);

                if (retVal == null)
                    retVal = itl;

                // Linked or kitted items
                var lotGtinItem = ItemManufacturer.GetItemManufacturerByGtin(lot.Gtin);
                foreach (var im in lotGtinItem.KitItems)
                    lots.Add(new OrderManagementLogic().GetOldestLot(facility, im.Gtin));

            }

            return retVal;
        }

        /// <summary>
        /// Performs a stock count for the specified <paramref name="gtin"/> at <paramref name="facility"/>
        /// </summary>
        /// <param name="facility">The facility in which the count occurred</param>
        /// <param name="gtin">The GTIN of the item being counted</param>
        /// <param name="lot">The lot number of the item being counted</param>
        /// <param name="count">The count of items</param>
        /// <returns>The <see cref="T:GIIS.DataLayer.ItemTransaction"/> representing the count</returns>
        public ItemTransaction StockCount(HealthFacility facility, String gtin, String lot, UInt32 count, Int32 modifiedBy, DateTime date)
        {

            // Validate
            if (facility == null)
                throw new ArgumentNullException("facility");
            else if (String.IsNullOrEmpty(gtin))
                throw new ArgumentNullException("gtin");
            else if (String.IsNullOrEmpty(lot))
                throw new ArgumentNullException("lot");

            // Stock Count 
            TransactionType stockCountType = TransactionType.GetTransactionTypeList().FirstOrDefault(o => o.Name == "Stock Count");
            if (stockCountType == null)
                throw new InvalidOperationException("Cannot find transaction type 'Stock Count'");

            // Balance
            var balance = this.GetCurrentBalance(facility, gtin, lot);

            // Now we want to create transaction for count operation
            ItemTransaction retVal = new ItemTransaction()
            {
                Gtin = gtin,
                GtinLot = lot,
                HealthFacilityCode = facility.Code,
                ModifiedBy = modifiedBy,
                ModifiedOn = DateTime.Now,
                TransactionDate = date,
                TransactionQtyInBaseUom = count,
                TransactionTypeId = stockCountType.Id
            };

            // Overwrite the values
            int qty = (int)(balance.Balance - count);
            if (qty > 0)
                Adjust(facility, gtin, lot, qty, AdjustmentReason.GetAdjustmentReasonById(99), modifiedBy, date);
            balance.StockCount = balance.Balance = count;

            // Save
            HealthFacilityBalance.Update(balance);
            int i = ItemTransaction.Insert(retVal);

            return ItemTransaction.GetItemTransactionById(i);
        }

        /// <summary>
        /// Performs rollback of stock transaction for a vaccination event
        /// </summary>
        /// <param name="facility">The healthfacility where the vaccination event happened</param>
        /// <param name="vaccineLotId">The vaccine lot that was used in the vaccination event</param>
        public int ClearBalance(HealthFacility facility, int vaccineLotId)
        {

            if (vaccineLotId > 0)
            {
                ItemLot il = ItemLot.GetItemLotById(vaccineLotId);
                HealthFacilityBalance hb = HealthFacilityBalance.GetHealthFacilityBalance(facility.Code, il.Gtin, il.LotNumber);
                if (hb != null)
                {
                    hb.Used -= 1;
                    hb.Balance += 1;
                    int i = HealthFacilityBalance.Update(hb);
                    return i;
                }
            }
            return -1;
        }
    }
}
