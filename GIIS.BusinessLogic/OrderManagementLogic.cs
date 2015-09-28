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
using GIIS.BusinessLogic.Exceptions;
//using GIIS.BusinessLogic.Model;
using GIIS.DataLayer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIIS.BusinessLogic
{
    /// <summary>
    /// Order Management Logic
    /// </summary>
    public class OrderManagementLogic
    {

        /// <summary>
        /// Update the order details
        /// </summary>
        /// <param name="order">The order whose details should be updated</param>
        /// <param name="modifiedBy">The identification of the user performing the update</param>
        private void UpdateOrderDetails(TransferOrderHeader order, Int32 modifiedBy)
        {

            // Update the details
            foreach (var dtl in TransferOrderDetail.GetTransferOrderDetailByOrderNum(order.OrderNum))
            {
                dtl.OrderDetailStatus = order.OrderStatus;
                this.UpdateOrderLine(dtl, modifiedBy);
            }

        }

        /// <summary>
        /// Gets the oldest lot in a facility of the specified <paramref name="gtin"/>
        /// </summary>
        /// <param name="facility">The facility in which the oldest lot should be found</param>
        /// <param name="gtin">The GTIN of the item to be found</param>
        /// <returns>The <see cref="T:GIIS.DataLayer.ItemLot"/> with the earliest expiry date</returns>
        public ItemLot GetOldestLot(HealthFacility facility, String gtin)
        {
            var oldestLot = from l in
                                (from v in HealthFacilityBalance.GetHealthFacilityBalanceByHealthFacilityCode(facility.Code)
                                 where gtin == v.Gtin
                                 select new { il = ItemLot.GetItemLotByGtinAndLotNo(gtin, v.LotNumber), hfb = v })
                            where l.il != null
                            orderby l.il.ExpireDate
                            select l;
            var candidateLot = (oldestLot.FirstOrDefault(o=>o.hfb.Balance > 0) ?? oldestLot.FirstOrDefault());

            // Candidate lot is null when there is demand (from a kit item) with no lot available!
            if (candidateLot == null)
            {
                // Is it becaues there is no lot in existence?
                var itemLot = ItemLot.GetItemLotByGtin(gtin);
                if (itemLot == null)
                {
                    itemLot = new ItemLot()
                    {
                        Gtin = gtin,
                        LotNumber = String.Format("N/A-{0}", gtin),
                        Notes = "Automatically inserted by order system",
                        ItemId = ItemManufacturer.GetItemManufacturerByGtin(gtin).ItemId
                    };
                    itemLot.Id = ItemLot.Insert(itemLot);
                }

                // Is there no balance?
                var balanceLot = HealthFacilityBalance.GetHealthFacilityBalanceByLotNumber(itemLot.LotNumber);
                if (balanceLot == null)
                    HealthFacilityBalance.Insert(new HealthFacilityBalance()
                    {
                        Gtin = gtin,
                        HealthFacilityCode = facility.Code,
                        LotNumber = itemLot.LotNumber
                    });
                return itemLot;
            }
            else
                return candidateLot.il;
        }

        /// <summary>
        /// Add an order line to the order defined by <paramref name="order"/>
        /// </summary>
        /// <param name="order">The order to which the line should be added</param>
        /// <param name="gtin">The global trade identification number of the item in the line</param>
        /// <param name="lot">(Optional) The lot number to be used. Note if null is passed in <paramref name="lot"/> then the oldest lot is used first</param>
        /// <param name="qty">The quantity of the item to be added to the order</param>
        /// <param name="uom">(Optional) The base unit of measure. If <paramref name="uom"/> is null then the default UOM for the item described by <paramref name="gtin"/> is used</param>
        /// <returns>The constructed and saved <see cref="T:GIIS.DataLayer.TransferOrderDetail"/></returns>
        /// <remarks>
        /// The add order line function is responsible for adding a new order line to the order detail. This function operates in the following manner: 
        /// <list type="ordered">
        /// <item>
        ///     <description>[Guard Condition] If the order passed into the function IS in the “Packed” or “Shipped” state then throw an invalid state exception (sanity check)</description>
        /// </item>
        /// <item>
        ///     <description>Lookup the item by the GTIN provided in the function call.</description>
        /// </item>
        /// <item>
        ///     <description>[Guard Condition] If the lot number is provided, and the lot number is not a valid lot number for the item provided then throw an error code</description>
        /// </item>
        /// <item>
        ///     <description>If the current status of the order to which the detail is to be attached is “Released” then
        ///         <list type="ordered">
        ///             <item>
        ///                 <description>Instantiate a StockManagementLogic BLL class</description>
        ///             </item>
        ///             <item>Allocate the stock using the Allocate method</item>
        ///         </list>
        ///     </description>
        /// </item>
        /// <item>
        ///     <description>Set the unit of measure, quantity, gtin, etc. fields of the TransferOrderDetail instance to the parameters and fields derived from loaded Item.</description>
        /// </item>
        /// <item>
        ///     <description>Save the order line.</description>
        /// </item>
        ///</list>
        /// </remarks>
        public TransferOrderDetail AddOrderLine(TransferOrderHeader order, String gtin, String lot, Int32 qty, Uom uom, Int32 modifiedBy)
        {
            if (order == null)
                throw new ArgumentNullException("order");
            if (String.IsNullOrEmpty(gtin))
                throw new ArgumentNullException("gtin");
            
            // Sanity check
            if (order.OrderStatus == (int)OrderStatusType.Shipped ||
                order.OrderStatus == (int)OrderStatusType.Cancelled)
                throw new IllegalStateException((OrderStatusType)order.OrderStatus, "TransferOrderHeader", "AddOrderLine");

            // Lookup item by GTIN and optionally lot
            ItemLot item = null;
            if(!String.IsNullOrEmpty(lot))
                item = ItemLot.GetItemLotByGtinAndLotNo(gtin, lot);
            if (item == null)
            {   // not found - We get the item by lot 
                item = ItemLot.GetItemLotByGtin(gtin);
                lot = "*"; // null;
            }

            // Item still null?
            if (item == null)
                throw new InvalidOperationException(String.Format("Cannot locate item with GTIN {0}", gtin));

            // Construct the order detail
            TransferOrderDetail retVal = new TransferOrderDetail()
            {
                ModifiedBy = modifiedBy, 
                ModifiedOn = DateTime.Now,
                OrderCustomItem= false,
                OrderDetailDescription = item.ItemObject.Name,
                OrderDetailStatus = order.OrderStatus,
                OrderGtin = gtin,
                OrderGtinLotnum = lot,
                OrderNum = order.OrderNum,
                OrderQty = qty,
                OrderQtyInBaseUom = qty,
                OrderUom = uom.Name
            };

            // HACK: Overcome lack of transaction processing we have to be careful about how we do this
            ItemTransaction allocateTransaction = null;

            // Current state of order is released? We need to allocate this line item
            if (order.OrderStatus == (int)OrderStatusType.Released)
            {
                StockManagementLogic stockLogic = new StockManagementLogic();
                // We need to release this order ... If the lot was null we'll use the oldest lot number
                if (String.IsNullOrEmpty(lot))
                    item = this.GetOldestLot(order.OrderFacilityFromObject, gtin);
                
                // Allocation of specified lot
                allocateTransaction = stockLogic.Allocate(order.OrderFacilityFromObject, gtin, item.LotNumber, qty, null, modifiedBy);

                // Update order
                retVal.OrderGtinLotnum = item.LotNumber;

            }

            // Save
            retVal.OrderDetailNum = TransferOrderDetail.Insert(retVal);

            // HACK: Update the allocate transaction. This would be cleaned up with a transaction to back out changes (basically do the order detail then allocate)
            if(allocateTransaction != null)
            {
                allocateTransaction.RefId = retVal.OrderNum.ToString();
                allocateTransaction.RefIdNum = retVal.OrderDetailNum;
                ItemTransaction.Update(allocateTransaction);
            }

            return retVal;

        }

        /// <summary>
        /// Cancels an order regardless of state
        /// </summary>
        /// <param name="order">The order to be cancelled</param>
        /// <returns>The cancelled <see cref="T:GIIS.DataLayer.TransferOrderHeader"/></returns>
        /// <remarks>
        /// The cancel order function is used to cancel any order and back-out any stock transactions that have occurred. 
        /// The process for this function is as follows:
        /// <list type="ordered">
        /// <item><description>	[Guard Condition] If the order passed into the function IS NOT in states “Requested”, “Released” or “Packed” then throw an invalid state exception (sanity check)</description></item>
        /// <item><description>	Create an instance of the StockManagementLogic BLL class</description></item>
        /// <item><description>	The function updates the TransferOrderHeader status to “Cancelled”</description></item>
        /// <item><description>	For each item in the TransferOrderDetails for the order:
        ///     <list type="ordered">
        ///         <item><description>Call the UpdateOrderLine function to set the status of the order detail item to “Cancelled”.</description></item>
        ///     </list>
        /// </description></item>
        /// 
        /// <item><description>	Save the TransferOrderHeader</description></item>
        /// </list>
        /// </remarks>
        public TransferOrderHeader CancelOrder(TransferOrderHeader order, Int32 modifiedBy)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            // Sanity check
            if (order.OrderStatus != (int)OrderStatusType.Requested &&
                order.OrderStatus != (int)OrderStatusType.Released &&
                order.OrderStatus != (int)OrderStatusType.Packed)
                throw new IllegalStateException((OrderStatusType)order.OrderStatus, "TransferOrderHeader", "Cancel");

            // Update the header
            order.OrderStatus = (int)OrderStatusType.Cancelled;
            order.ModifiedBy = modifiedBy;
            order.ModifiedOn = DateTime.Now;
            order.RevNum++;
            TransferOrderHeader.Update(order);

            // Update details
            this.UpdateOrderDetails(order, modifiedBy);

            return order;
            
        }

        /// <summary>
        /// Updates the order to packed
        /// </summary>
        /// <param name="order">The order which is the be set in the packed state</param>
        /// <returns>The <see cref="T:GIIS.DataLayer.TransferOrderHeader"/> which has been marked as Packed</returns>
        /// <remarks>
        /// The pack order function is used whenever the caller wishes to update an existing order to the “packed” status. This function does no stock transaction and merely updates the order lines.
        /// The process for this function is as follows:
        /// <list type="ordered">
        /// <item><description>	[Guard Condition] If the order passed into the function IS NOT in state “Released” then throw an invalid state exception (sanity check)</description></item>
        /// <item><description>	The function updates the TransferOrderHeader status to “Packed”.</description></item>
        /// <item><description>	For each item in the TransferOrderDetails for the order:
        ///     <list type="ordered">
        ///         <item><description>a.	Call UpdateOrderLine function to set the status of the order detail item to “Packed”</description></item>
        ///     </list>
        /// </description></item>
        /// <item><description>	Save the TransferOrderHeader</description></item>
        /// </list>
        /// </remarks>
        public TransferOrderHeader PackOrder(TransferOrderHeader order, Int32 modifiedBy)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            // Guard condition
            if (order.OrderStatus != (int)OrderStatusType.Released)
                throw new IllegalStateException((OrderStatusType)order.OrderStatus, "TransferOrderHeader", "PackOrder");

            // Update status
            order.OrderStatus = (int)OrderStatusType.Packed;
            order.ModifiedBy = modifiedBy;
            order.ModifiedOn = DateTime.Now;
            order.RevNum++;
            TransferOrderHeader.Update(order);

            // Update details
            this.UpdateOrderDetails(order, modifiedBy);

            return order;
        }

        /// <summary>
        /// Releases an order 
        /// </summary>
        /// <param name="order">The order which is to be placed in the released state</param>
        /// <returns>The <see cref="T:GIIS.DataLayer.TransferOrderHeader"/> of the released order</returns>
        /// <remarks>
        /// The release order function is used whenever the caller wishes to update an existing order and “release” it. This function allocates stock in the item transaction tables (using the StockManagementLogic class).
        /// The process for this function is as follows:
        /// <list type="ordered">
        /// <item><description>	[Guard Condition] If the order passed into the function IS NOT in state “Requested” then throw an invalid state exception (sanity check)</description></item>
        /// <item><description>	For each item in the TransferOrderDetails for the order:
        ///     <list type="ordered">
        ///         <item><description>Call UpdateOrderLine function to set the status of the order detail item to “Released”</description></item>
        ///     </list>
        /// </description></item>
        /// <item><description>	Save the TransferOrderHeader</description></item>
        /// </list>
        /// </remarks>
        public TransferOrderHeader ReleaseOrder(TransferOrderHeader order, Int32 modifiedBy)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            // Guard condition
            if (order.OrderStatus != (int)OrderStatusType.Requested)
                throw new IllegalStateException((OrderStatusType)order.OrderStatus, "TransferOrderHeader", "ReleaseOrder");

            // Update the order
            order.OrderStatus = (int)OrderStatusType.Released;
            order.ModifiedBy = modifiedBy;
            order.ModifiedOn = DateTime.Now;
            order.RevNum++;
            TransferOrderHeader.Update(order);

            // Update details
            this.UpdateOrderDetails(order, modifiedBy);

            return order;
        }

        /// <summary>
        /// Remove an order line
        /// </summary>
        /// <param name="line">The order line which is to be removed from its parent order</param>
        /// <returns>The <see cref="T:GIIS.DataLayer.TransferOrderDetail"/> representing the line that was removed</returns>
        /// <remarks>
        /// The remove order line function is used to remove an order detail from a transfer order. The process is as follows:
        /// <list type="ordered">
        /// <item><description>	Load the current order line for the database using the order line #</description></item>
        /// <item><description>	Instantiate an instance of <see cref="T:GIIS.BusinessLogic.StockManagementLogic"/>  BLL class.</description></item>
        /// <item><description>	If the status of the current order line is:
        ///     <list type="table">
        ///         <listheader>
        ///             <term>State</term>
        ///             <description>Action</description>
        ///         </listheader>
        ///         <item><term>Released</term><description>Call the <see cref="M:GIIS.BusinessLogic.StockManagementLogic.Allocate(GIIS.DataLayer.HealthFacility, System.String, System.String, System.Int32)"/> method of the <see cref="T:GIIS.BusinessLogic.StockManagementLogic"/> instance to perform the de-allocation of the item.</description></item>
        ///         <item><term>Packed</term><description>Call the Allocate method of the <see cref="T:GIIS.BusinessLogic.StockManagementLogic"/>  instance to perform the de-allocation of the item.</description></item>
        ///         <item><term>Shipped</term><description>Throw an invalid operation exception as shipped orders (and their lines) cannot be edited</description></item>
        ///         <item><term>Cancelled</term><description>Throw an invalid operation exception as cancelled orders (and their lines) cannot be edited</description></item>
        ///     </list>
        /// </description></item>
        /// <item><description>	Delete the order line from the transfer order table.</description></item>
        /// </list>
        /// </remarks>
        public TransferOrderDetail RemoveOrderLine(TransferOrderDetail line, Int32 modifiedBy)
        {
            // Remove order line
            line.OrderQty = 0;
            return this.UpdateOrderLine(line, modifiedBy);
        }

        /// <summary>
        /// Request an order from a mini-drp run
        /// </summary>
        /// <param name="to">The facility which is the target of the drp run</param>
        /// <param name="modifiedBy">The user which is requesting the order from DRP run</param>
        public TransferOrderHeader RequestOrderFromDrp(HealthFacility to, Int32 modifiedBy)
        {
            
            // HACK: Currently this is running form SQL, should be run from DALs
            
            // TODO: Perform the mini-DRP run

            // Get the results of the run for the facility
            string sql = "GET_REPLENISHMENT_ORDER";
            using (var dt = DBManager.ExecuteReaderCommand(sql, System.Data.CommandType.StoredProcedure, new List<NpgsqlParameter>() {
                new NpgsqlParameter("HF_ID_IN", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = to.Code }
            }))
            {
                // Create the order header
                TransferOrderHeader retVal = new TransferOrderHeader()
                {
                    ModifiedBy = modifiedBy,
                    ModifiedOn = DateTime.Now,
                    OrderStatus = (int)OrderStatusType.Requested,
                    OrderFacilityFrom = to.Parent.Code,
                    OrderFacilityTo = to.Code,
                    OrderSchedReplenishDate = DateTime.Now
                };

                List<TransferOrderDetail> dtls = new List<TransferOrderDetail>();

                // Convert the datatable to a streamed reader
                using(var rdr = dt.CreateDataReader())
                {
                    while(rdr.Read())
                    {
                        retVal.OrderSchedReplenishDate = retVal.OrderSchedReplenishDate == default(DateTime) ? Convert.ToDateTime(rdr["plan_date"]) : retVal.OrderSchedReplenishDate;

                        // Create line items
                        // Get the item/lot
                        var itemLot = this.GetOldestLot(retVal.OrderFacilityFromObject, Convert.ToString(rdr["gtin"]));
                        if (itemLot == null)
                            throw new InvalidOperationException("Cannot find item specified in gtin");

                        // Dtl
                        TransferOrderDetail dtl = new TransferOrderDetail()
                        {
                            OrderDetailDescription = Convert.ToString(rdr["name"]),
                            OrderNum = retVal.OrderNum,
                            OrderGtin = itemLot.Gtin,
                            OrderGtinLotnum = itemLot.LotNumber,
                            OrderQty = Convert.ToDouble(rdr["base_replenish_qty"]),
                            OrderQtyInBaseUom = Convert.ToDouble(rdr["base_replenish_qty"]),
                            OrderUom = Convert.ToString(rdr["base_uom"])
                        };
                        dtls.Add(dtl);

                    }
                }   
                
                retVal.OrderNum = TransferOrderHeader.Insert(retVal);
                foreach (var dtl in dtls)
                {
                    dtl.OrderNum = retVal.OrderNum;
                    TransferOrderDetail.Insert(dtl);
                }

                return retVal;
            }

        }

        /// <summary>
        /// Requests that an order be created based on the contents of the DRP table for the specified <paramref name="drpPlanId"/>
        /// </summary>
        /// <param name="facility">The health facility for which to create an order</param>
        /// <returns>The constructed <see cref="T:GIIS.DataLayer.TransferOrderHeader"/> instance which was constructed from the <paramref name="drpPlanId"/></returns>
        /// <remarks>
        /// <list type="ordered">
        /// <item><description>The function will create a new TransferOrderHeader DAL object</description></item>
        /// <item><description>	The function will load a HealthFacility object from the gln and gln_parent identifiers in the DRP table assigning them to the DAO object</description></item>
        /// <item><description>	The function will set the order status to “Requested”</description></item>
        /// <item><description>	The function will set the order date to the plan date</description></item>
        /// <item><description>	The function will save the order header.</description></item>
        /// <item><description>	For each item in the DRP plan the function will create a TransferOrderDetail objects using the AddOrderLine function in the Logic class using the specified quantity, gtin provided.       /// </remarks></description></item>
        /// </list>
        /// </remarks>
        public TransferOrderHeader RequestOrder(Int32[] drpIds, Int32 modifiedBy)
        {

            // HACK: Get this into the DAL
            StringBuilder drpIdsFilter = new StringBuilder();
            foreach (var id in drpIds)
                drpIdsFilter.AppendFormat("{0},", id);
            drpIdsFilter.Remove(drpIdsFilter.Length - 1, 1);

            string sql = String.Format("SELECT drp_plan.*, drp_facility.gln_parent FROM drp_plan INNER JOIN drp_facility USING (gln) WHERE drp_id IN ({0})", drpIdsFilter);
            using (var dt = DBManager.ExecuteReaderCommand(sql, System.Data.CommandType.Text, null))
            {
                // Create the order header
                TransferOrderHeader retVal = new TransferOrderHeader()
                {
                    ModifiedBy = modifiedBy,
                    ModifiedOn = DateTime.Now,
                    OrderStatus = (int)OrderStatusType.Requested
                };

                // Convert the datatable to a streamed reader
                using (var rdr = dt.CreateDataReader())
                {
                    while (rdr.Read())
                    {
                        retVal.OrderSchedReplenishDate = retVal.OrderSchedReplenishDate == default(DateTime) ? Convert.ToDateTime(rdr["plan_date"]) : retVal.OrderSchedReplenishDate;
                        retVal.OrderFacilityFrom = retVal.OrderFacilityFrom ?? Convert.ToString(rdr["gln_parent"]);
                        retVal.OrderFacilityTo = retVal.OrderFacilityTo ?? Convert.ToString(rdr["gln"]);

                        // Create line items
                        // Get the item/lot
                        var itemLot = this.GetOldestLot(retVal.OrderFacilityFromObject, Convert.ToString(rdr["gtin"]));
                        if (itemLot == null)
                            throw new InvalidOperationException("Cannot find item specified in gtin");

                        // Dtl
                        TransferOrderDetail dtl = new TransferOrderDetail()
                        {
                            OrderDetailDescription = itemLot.ItemObject.Name,
                            OrderNum = retVal.OrderNum,
                            OrderGtin = itemLot.Gtin,
                            OrderGtinLotnum = itemLot.LotNumber,
                            OrderQty = Convert.ToDouble(rdr["planned_order_receipt"]),
                            OrderQtyInBaseUom = Convert.ToDouble(rdr["planned_order_receipt"]),
                            OrderUom = ItemManufacturer.GetItemManufacturerByGtin(itemLot.Gtin).BaseUom
                        };
                        dtl.OrderDetailNum = TransferOrderDetail.Insert(dtl);

                    }
                }

                retVal.OrderNum = TransferOrderHeader.Insert(retVal);

                return retVal;

            }

        }

        /// <summary>
        /// Manually requests an order be created from <paramref name="from"/> to <paramref name="to"/> in the Requested state
        /// </summary>
        /// <param name="from">The <see cref="T:GIIS.DataLayer.HealthFacility"/> from which the order is originating (where stock originates)</param>
        /// <param name="to">The <see cref="T:GIIS.DataLayer.HealthFacility"/> to which the order is destined (where stock will end up)</param>
        /// <param name="orderDate">The date that the order is to take place</param>
        /// <returns>A constructed <see cref="T:GIIS.DataLayer.TransferOrderHeader"/> containing the constructed order items</returns>
        /// <remarks>
        /// Callers of this function will need to make subsequent calls to <see cref="M:GIIS.BusinessLogic.OrderManagementLogic.AddOrderLine(GIIS.DataLayer.TransferOrderHeader, System.String, System.String, System.Int32, GIIS.DataLayer.Uom) "/> function
        /// to add order lines.
        /// </remarks>
        public TransferOrderHeader RequestOrder(HealthFacility from, HealthFacility to, DateTime orderDate, Int32 modifiedBy)
        {
            if (from == null)
                throw new ArgumentNullException("from");
            else if (to == null)
                throw new ArgumentNullException("to");
            else if (orderDate == default(DateTime))
                throw new ArgumentException("orderDate");

            // Create the header
            TransferOrderHeader retVal = new TransferOrderHeader()
            {
                ModifiedBy = modifiedBy,
                ModifiedOn = DateTime.Now,
                OrderFacilityFrom = from.Code,
                OrderFacilityTo = to.Code,
                OrderSchedReplenishDate = orderDate,
                OrderStatus = (int)OrderStatusType.Requested,
                RevNum = 0
            };

            // Save
            retVal.OrderNum = TransferOrderHeader.Insert(retVal);

            return retVal;
        }

        /// <summary>
        /// Ships the specified <paramref name="order"/>
        /// </summary>
        /// <param name="order">The order which is to be marked as "shipped"</param>
        /// <returns>The <see cref="T:GIIS.DataLayer.TransferOrderHeader"/> which has been marked as Shipped</returns>
        /// <remarks>
        /// The ship order function is used whenever the caller wishes to update an existing order to the “shipped” status and perform the transfers. 
        ///The process for this function is as follows:
        ///<list type="ordered">
        /// <item><description>	[Guard Condition] If the order passed into the function IS NOT in state “Packed” then throw an invalid state exception (sanity check)</description></item>
        /// <item><description>	The function updates the TransferOrderHeader status to “Shipped”.</description></item>
        /// <item><description>	For each item in the TransferOrderDetails for the order:
        ///     <list type="ordered">
        ///         <item><description>Call <see cref="M:GIIS.BusinessLogic.OrderManagementLogic.UpdateOrderLine(GIIS.DataLayer.TransferOrderDetail)"/>function to set the status of the order detail item to “Shipped”</description></item>    
        /// </list>
        /// </description></item>
        /// <item><description>	Save the TransferOrderHeader</description></item>
        /// </list>
        /// </remarks>
        public TransferOrderHeader ShipOrder(TransferOrderHeader order, Int32 modifiedBy)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            // Guard condition
            if (order.OrderStatus != (int)OrderStatusType.Packed)
                throw new IllegalStateException((OrderStatusType)order.OrderStatus, "TransferOrderHeader", "ShipOrder");

            // Update header
            order.OrderStatus = (int)OrderStatusType.Shipped;
            order.ModifiedBy = modifiedBy;
            order.ModifiedOn = DateTime.Now;
            order.RevNum++;
            TransferOrderHeader.Update(order);

            // Update details
            this.UpdateOrderDetails(order, modifiedBy);

            return order;
        }

        /// <summary>
        /// Updates the specified <paramref name="orderLine"/> based on business rules
        /// </summary>
        /// <param name="orderLine">The <see cref="T:GIIS.DataLayer.TransferOrderDetail"/> line to be updated</param>
        /// <returns>The <see cref="T:GIIS.DataLayer.TransferOrderDetail"/> which was updated</returns>
        /// <remarks>
        /// <list type="ordered">
        /// <item><description>	Load the current order line for the database using the order line #</description></item>
        /// <item><description>	Instantiate an instance of StockManagementLogic BLL class.</description></item>
        /// <item><description>	If the status of the current order line is:
        ///     <list type="table">
        ///         <listHeader>
        ///             <term>State</term>
        ///             <description>Actions</description>
        ///         </listHeader>
        ///         <item>
        ///             <term>Requested</term>
        ///             <description>
        ///                 <list type="ordered">
        ///                     <item><description>[Guard Condition] Ensure the new state is either “Cancelled”, “Requested” or “Released” otherwise throw an invalid state transition exception</description></item>
        ///                     <item><description>Update the quantity and status of the  order detail item</description></item>
        ///                     <item><description>If the new state is “Released” then call the Allocate function of the StockManagementLogic instance to allocate the specified order detail.</description></item>
        ///                     <item><description>Save the order detail</description></item>
        ///                 </list>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Released</term>
        ///             <description>
        ///                 <list type="ordered">
        ///                     <item><description>[Guard Condition] Ensure the new state is either “Cancelled”, “Released” or “Packed” otherwise thrown an invalid state transition exception</description></item>
        ///                     <item><description>If the current state is “Released” then
        ///                         <list type="ordered">
        ///                             <item><description>Calculate the difference in quantity from the “old” record and “new” record</description></item>
        ///                             <item><description>Call the Allocate method of the StockManagementLogic instance to perform the additional allocation/de-allocation.</description></item>
        ///                         </list>
        ///                     </description></item>
        ///                     <item><description>	Update the quantity and status of the order detail item.</description></item>
        ///                     <item><description>If the new state is “Cancelled” then call the Allocate method of the StockManagementLogic instance to perform the de-allocation of the item.</description></item>
        ///                     <item><description>Save the order detail</description></item>
        ///                 </list>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Packed</term>
        ///             <description>
        ///                 <list type="ordered">
        ///                     <item><description>[Guard Condition] Ensure the new state is either “Cancelled”, “Packed” or “Shipped”</description></item>
        ///                     <item><description>Update the quantity and status of the order detail item.</description></item>
        ///                     <item><description>If the new state is “cancelled” then call the Allocate method of the StockManagementLogic instance to perform the de-allocation of the item.</description></item>
        ///                     <item><description>If the new state is “Shipped” then
        ///                         <list type="ordered">
        ///                             <item><description>Call the allocate method of the StockManagementLogic instance to perform the de-allocation of the line item.</description></item>
        ///                             <item><description>Call the Transfer method of the StockManagementLogic instance to perform the transfer transactions between the source and target facilities.</description></item>
        ///                         </list>
        ///                     </description></item>
        ///                     <item><description>Save the order detail</description></item>
        ///                 </list>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Shipped</term>
        ///             <description>Throw an invalid operation exception as shipped orders (and their lines) cannot be edited</description>
        ///         </item>
        ///         <item>
        ///             <term>Cancelled</term>
        ///             <description>Throw an invalid operation exception as cancelled orders (and their lines) cannot be edited</description>
        ///         </item>
        ///     </list>
        /// </description></item>
        /// </list>
        /// </remarks>
        public TransferOrderDetail UpdateOrderLine(TransferOrderDetail orderLine, Int32 modifiedBy)
        {
            if (orderLine == null)
                throw new ArgumentNullException("orderLine");
            else if (orderLine.OrderDetailNum == default(Int32))
                throw new ArgumentException("Order line is not saved", "orderLine");

            // Load the current order line from the database
            TransferOrderDetail currentOrderLine = TransferOrderDetail.GetTransferOrderDetailByOrderDetailNum(orderLine.OrderDetailNum);
            TransferOrderHeader currentOrderHeader = TransferOrderHeader.GetTransferOrderHeaderByOrderNum(orderLine.OrderNum);

            // Can't change the GTIN with this function
            if (orderLine.OrderGtin != currentOrderLine.OrderGtin)
                throw new InvalidOperationException("Cannot change the GTIN with this function. Remove the order line first and add another order-line with the new GTIN");

            // New order lot number is null?  We need to get oldest lot          
            if (String.IsNullOrEmpty(orderLine.OrderGtinLotnum) || orderLine.OrderGtinLotnum == "*")
            {
                ItemLot itemLot = GetOldestLot(currentOrderHeader.OrderFacilityFromObject, orderLine.OrderGtin); //currentOrderLine.OrderGtinLotnum;
                if (itemLot != null)
                    orderLine.OrderGtinLotnum = itemLot.LotNumber;
            }

            StockManagementLogic stockLogic = new StockManagementLogic();

            // Apply rules
            switch((OrderStatusType)currentOrderLine.OrderDetailStatus)
            {
                case OrderStatusType.Requested:
                    // State transitions
                    if (orderLine.OrderDetailStatus != (int)OrderStatusType.Cancelled &&
                        orderLine.OrderDetailStatus != (int)OrderStatusType.Released &&
                        orderLine.OrderDetailStatus != (int)OrderStatusType.Requested)
                        throw new IllegalStateException((OrderStatusType)orderLine.OrderDetailStatus, "TransferOrderDetail", "UpdateOrderLine");

                    // Allocate the data if this is a transition
                    if(orderLine.OrderDetailStatus == (int)OrderStatusType.Released)
                        stockLogic.Allocate(currentOrderHeader.OrderFacilityFromObject, orderLine.OrderGtin, orderLine.OrderGtinLotnum, (int)orderLine.OrderQty, orderLine, modifiedBy);
                    
                    break;
                case OrderStatusType.Released:

                    // Guard conditions
                    if (orderLine.OrderDetailStatus != (int)OrderStatusType.Cancelled &&
                        orderLine.OrderDetailStatus != (int)OrderStatusType.Released &&
                        orderLine.OrderDetailStatus != (int)OrderStatusType.Packed)
                        throw new IllegalStateException((OrderStatusType)orderLine.OrderDetailStatus, "TransferOrderDetail", "UpdateOrderLine");

                    // We need to adjust the allocations?
                    if (currentOrderLine.OrderQty != orderLine.OrderQty)
                        stockLogic.Allocate(currentOrderHeader.OrderFacilityFromObject, orderLine.OrderGtin, orderLine.OrderGtinLotnum, (int)(orderLine.OrderQty - currentOrderLine.OrderQty), orderLine, modifiedBy);

                    // Released -> Cancelled = Deallocate
                    if (orderLine.OrderDetailStatus == (int)OrderStatusType.Cancelled)
                        stockLogic.Allocate(currentOrderHeader.OrderFacilityFromObject, orderLine.OrderGtin, orderLine.OrderGtinLotnum, -(int)orderLine.OrderQty, orderLine, modifiedBy);

                    break;
                case OrderStatusType.Packed:
                    // Guard conditions
                    if (orderLine.OrderDetailStatus != (int)OrderStatusType.Cancelled &&
                        orderLine.OrderDetailStatus != (int)OrderStatusType.Packed &&
                        orderLine.OrderDetailStatus != (int)OrderStatusType.Shipped)
                        throw new IllegalStateException((OrderStatusType)orderLine.OrderDetailStatus, "TransferOrderDetail", "UpdateOrderLine");

                    // We need to adjust the allocations?
                    if (currentOrderLine.OrderQty != orderLine.OrderQty)
                        stockLogic.Allocate(currentOrderHeader.OrderFacilityFromObject, orderLine.OrderGtin, orderLine.OrderGtinLotnum, (int)(orderLine.OrderQty - currentOrderLine.OrderQty), orderLine, modifiedBy);

                    // Packed -> Cancelled = Deallocate
                    if (orderLine.OrderDetailStatus == (int)OrderStatusType.Cancelled)
                        stockLogic.Allocate(currentOrderHeader.OrderFacilityFromObject, orderLine.OrderGtin, orderLine.OrderGtinLotnum, -(int)orderLine.OrderQty, orderLine, modifiedBy);
                    // Packed -> Shipped = Deallocate then Transfer
                    else if(orderLine.OrderDetailStatus == (int)OrderStatusType.Shipped)
                    {
                        stockLogic.Allocate(currentOrderHeader.OrderFacilityFromObject, orderLine.OrderGtin, orderLine.OrderGtinLotnum, -(int)orderLine.OrderQty, orderLine, modifiedBy);
                        stockLogic.Transfer(currentOrderHeader.OrderFacilityFromObject, currentOrderHeader.OrderFacilityToObject, orderLine.OrderGtin, orderLine.OrderGtinLotnum, orderLine, (int)orderLine.OrderQty, modifiedBy);
                    }

                    break;
                case OrderStatusType.Shipped:
                    throw new InvalidOperationException("Shipped orders cannot be modified " + orderLine.OrderDetailNum.ToString());
                case OrderStatusType.Cancelled:
                    throw new InvalidOperationException("Cancelled orders cannot be modified");
            }

            // Update
            orderLine.ModifiedBy = modifiedBy;
            orderLine.ModifiedOn = DateTime.Now;
            TransferOrderDetail.Update(orderLine);

            // Return the order line
            return orderLine;
        }
    }
}
