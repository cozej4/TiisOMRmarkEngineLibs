using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIIS.BusinessLogic.Test
{
    /// <summary>
    /// Initializes the database with test data
    /// </summary>
    public static class TestData
    {

        public const String GTIN_UNDER_TEST = "12345";
        public const String GTIN_LOT_USE_FIRST = "XXY-12345";
        public const String GTIN_LOT_USE_LAST = "XXZ-12345";
        public const String GTIN_LOT_LOW_BAL = "XXX-12345";

        public static int VACCINE_CATEGORY_ID = 999;
        public static int OPV_ITEM_ID = 999;
        public static int DOSE_UOM_ID = 999;
        public static int PHARMACO_MANUFACTURER_ID = 999;

        /// <summary>
        /// Initialize the test data
        /// </summary>
        public static void InitializeTestDataset()
        {
            /// Transaction type 999 = ID
            var transactionTypes = TransactionType.GetTransactionTypeList();
            if (!transactionTypes.Exists(o => o.Name == "Allocation"))
                TransactionType.Insert(new TransactionType()
                {
                    Name = "Allocation"
                });
            if (!transactionTypes.Exists(o => o.Name == "Transfer"))
                TransactionType.Insert(new TransactionType()
                {
                    Name = "Transfer"
                });


            // GTIN 12345 - PHARMACO OPV
            if (ItemManufacturer.GetItemManufacturerByGtin(GTIN_UNDER_TEST) == null)
            {
                // Item Category 999 = Vaccine
                if (ItemCategory.GetItemCategoryById(VACCINE_CATEGORY_ID) == null)
                    VACCINE_CATEGORY_ID = ItemCategory.Insert(new ItemCategory()
                    {
                        Id = VACCINE_CATEGORY_ID,
                        Code = "VACCINE",
                        IsActive = true,
                        ModifiedBy = 1,
                        ModifiedOn = DateTime.Now,
                        Name = "Vaccine"
                    });

                // Item 999 - OPV
                if (Item.GetItemById(OPV_ITEM_ID) == null)
                    OPV_ITEM_ID = Item.Insert(new Item()
                    {
                        Code = "OPV",
                        EntryDate = DateTime.Now,
                        Id = OPV_ITEM_ID,
                        ItemCategoryId = VACCINE_CATEGORY_ID,
                        IsActive = true,
                        ModifiedBy = 1,
                        ModifiedOn = DateTime.Now,
                        Name = "OPV"
                    });

                // Unit of Measure
                if (Uom.GetUomById(DOSE_UOM_ID) == null)
                    DOSE_UOM_ID = Uom.Insert(new Uom()
                    {
                        Id = DOSE_UOM_ID,
                        Name = "DOSE"
                    });

                // Manufacturer 999 - PHARMACO
                if (Manufacturer.GetManufacturerById(PHARMACO_MANUFACTURER_ID) == null)
                    PHARMACO_MANUFACTURER_ID = Manufacturer.Insert(new Manufacturer()
                    {
                        Id = PHARMACO_MANUFACTURER_ID,
                        Code = "PHX",
                        IsActive = true,
                        ModifiedBy = 1,
                        ModifiedOn = DateTime.Now,
                        Name = "PHARMACO INC."
                    });

                ItemManufacturer.Insert(new ItemManufacturer()
                {
                    Alt1QtyPer = 20,
                    BaseUom = "DOSE",
                    BaseUomChildPerBaseUomParent = 10,
                    Gtin = GTIN_UNDER_TEST,
                    IsActive = true,
                    ItemId = OPV_ITEM_ID,
                    ManufacturerId = PHARMACO_MANUFACTURER_ID,
                    ModifiedBy = 1,
                    ModifiedOn = DateTime.Now,
                    Price = 9.99,
                    StorageSpace = 1,
                    Alt1Uom = "DOSE",
                    Alt2QtyPer = 30,
                    Alt2Uom = "DOSE"
                });

            }

            if (ItemLot.GetItemLotByGtin(GTIN_UNDER_TEST) == null)
            {

                // Item 999 - OPV
                if (Item.GetItemById(OPV_ITEM_ID) == null)
                    OPV_ITEM_ID = Item.Insert(new Item()
                    {
                        Code = "OPV",
                        EntryDate = DateTime.Now,
                        Id = OPV_ITEM_ID,
                        ItemCategoryId = VACCINE_CATEGORY_ID,
                        IsActive = true,
                        ModifiedBy = 1,
                        ModifiedOn = DateTime.Now,
                        Name = "OPV"
                    });

                ItemLot.Insert(new ItemLot()
                {
                    ExpireDate = DateTime.Now.AddDays(10),
                    Gtin = GTIN_UNDER_TEST,
                    ItemId = OPV_ITEM_ID,
                    LotNumber = GTIN_LOT_USE_FIRST
                });

                // Item Lot 2 - Will be more stock and expires later
                ItemLot.Insert(new ItemLot()
                {
                    ExpireDate = DateTime.Now.AddDays(40),
                    Gtin = GTIN_UNDER_TEST,
                    ItemId = OPV_ITEM_ID,
                    LotNumber = GTIN_LOT_USE_LAST
                });

                // Item Lot 3 - Will trigger low stock 
                ItemLot.Insert(new ItemLot()
                {
                    ExpireDate = DateTime.Now.AddDays(80),
                    Gtin = GTIN_UNDER_TEST,
                    ItemId = OPV_ITEM_ID,
                    LotNumber = GTIN_LOT_LOW_BAL
                });
            }

            // Type 3 = DISTRICT
            if (HealthFacilityType.GetHealthFacilityTypeById(6) == null)
                HealthFacilityType.Insert(new HealthFacilityType()
                {
                    Id = 3,
                    Code = "DISTRICT",
                    IsActive = true,
                    ModifiedBy = 1,
                    ModifiedOn = DateTime.Now,
                    Name = "DISTRICT LEVEL"
                });

            // Type 6 = SDP
            if (HealthFacilityType.GetHealthFacilityTypeById(6) == null)
                HealthFacilityType.Insert(new HealthFacilityType()
                {
                    Id = 6,
                    Code = "SDP",
                    IsActive = true,
                    ModifiedBy = 1,
                    ModifiedOn = DateTime.Now,
                    Name = "SDP"
                });

            // HF888 = DISTRICT
            if (HealthFacility.GetHealthFacilityByCode("HF888") == null)
                HealthFacility.Insert(new HealthFacility()
                {
                    Id = 888,
                    Code = "HF888",
                    ColdStorageCapacity = 1000,
                    IsActive = true,
                    Leaf = false,
                    ParentId = HealthFacility.GetHealthFacilityByParentId(0)[0].Id,
                    TopLevel = false,
                    TypeId = 3,
                    VaccinationPoint = false,
                    VaccineStore = true,
                    ModifiedBy = 1,
                    ModifiedOn = DateTime.Now
                });

            // GIVE HEALTH FACILITY SOME BALANCE
            var hf888Balances = HealthFacilityBalance.GetHealthFacilityBalanceByHealthFacilityCode("HF888");
            HealthFacilityBalance useFirst = hf888Balances.Find(o => o.Gtin == GTIN_UNDER_TEST && o.LotNumber == GTIN_LOT_USE_FIRST),
                useLast = hf888Balances.Find(o => o.Gtin == GTIN_UNDER_TEST && o.LotNumber == GTIN_LOT_USE_LAST),
                lowStock = hf888Balances.Find(o => o.Gtin == GTIN_UNDER_TEST && o.LotNumber == GTIN_LOT_LOW_BAL);
            if (useFirst == null)
            {
                useFirst = new HealthFacilityBalance()
                {
                    Allocated = 0,
                    Balance = 500,
                    Distributed = 0,
                    Gtin = GTIN_UNDER_TEST,
                    HealthFacilityCode = "HF888",
                    LotNumber = GTIN_LOT_USE_FIRST,
                    Received = 0,
                    StockCount = 500,
                    Used = 0,
                    Wasted = 0
                };
                hf888Balances.Add(useFirst);
                HealthFacilityBalance.Insert(useFirst);
            }
            else
            {
                useFirst.Balance = 500;
                useFirst.StockCount = 500;
                HealthFacilityBalance.Update(useFirst);
            }

            if (useLast == null)
            {
                useLast = new HealthFacilityBalance()
                {
                    Allocated = 0,
                    Balance = 1000,
                    Distributed = 0,
                    Gtin = GTIN_UNDER_TEST,
                    HealthFacilityCode = "HF888",
                    LotNumber = GTIN_LOT_USE_LAST,
                    Received = 0,
                    StockCount = 1000,
                    Used = 0,
                    Wasted = 0
                };
                hf888Balances.Add(useLast);
                HealthFacilityBalance.Insert(useLast);
            }
            else
            {
                useLast.Balance = 1000;
                useLast.StockCount = 1000;
                HealthFacilityBalance.Update(useLast);
            }

            if (lowStock == null)
            {
                lowStock = new HealthFacilityBalance()
                {
                    Allocated = 0,
                    Balance = 10,
                    Distributed = 0,
                    Gtin = GTIN_UNDER_TEST,
                    HealthFacilityCode = "HF888",
                    LotNumber = GTIN_LOT_LOW_BAL,
                    Received = 0,
                    StockCount = 10,
                    Used = 0,
                    Wasted = 0
                };
                hf888Balances.Add(lowStock);
                HealthFacilityBalance.Insert(lowStock);
            }
            else
            {
                useLast.Balance = 10;
                useLast.StockCount = 10;
                HealthFacilityBalance.Update(lowStock);
            }

            // HF999 = SDP
            if (HealthFacility.GetHealthFacilityByCode("HF999") == null)
                HealthFacility.Insert(new HealthFacility()
                {
                    Id = 999,
                    Code = "HF999",
                    ColdStorageCapacity = 100,
                    IsActive = true,
                    Leaf = true,
                    ParentId = HealthFacility.GetHealthFacilityByCode("HF888").Id,
                    TopLevel = false,
                    TypeId = 6,
                    VaccinationPoint = true,
                    VaccineStore = true,
                    ModifiedOn = DateTime.Now,
                    ModifiedBy = 1
                });

        }
    }
}
