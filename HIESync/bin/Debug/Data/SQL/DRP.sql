delete from drp_sched_demand;
delete from drp_sched_replenish;
delete from drp_plan;

drop view GTIN_view cascade;  
create view GTIN_view as select
  a."GTIN" as GTIN, 
  b."NAME" as GTIN_description,  
  b."ITEM_CATEGORY_ID" as GTIN_type,
  a."BASE_UOM" as base_UOM,
  a."ALT_1_UOM" as pack_UOM_1,
  a."ALT_1_QTY_PER" as base_UOM_per_pack_1,
  a."ALT_2_UOM" as pack_UOM_2,
  a."ALT_2_QTY_PER" as base_UOM_per_pack_2,
  a."STORAGE_SPACE" as litres_storage_per_base_UOM,
  1 as planning_lead_time_days,
  a."GTIN_PARENT" as GTIN_parent,
  a."BASE_UOM_CHILD_PER_BASE_UOM_PARENT" as base_UOM_child_per_base_UOM_parent,
  0 as GTIN_lowcode
from "ITEM_MANUFACTURER" a, "ITEM" b where
  a."ITEM_ID" = b."ID" and
  a."IS_ACTIVE" = true;

delete from drp_GTIN;
insert into drp_GTIN (
  gtin,
  gtin_description,
  gtin_type,
  base_uom,
  pack_uom_1,
  base_uom_per_pack_1,
  pack_uom_2,
  base_uom_per_pack_2,
  litres_storage_per_base_uom,
  planning_lead_time_days,
  gtin_parent,
  base_uom_child_per_base_uom_parent,
  gtin_lowcode)
select    
  gtin,
  gtin_description,
  gtin_type,
  base_uom,
  pack_uom_1,
  base_uom_per_pack_1,
  pack_uom_2,
  base_uom_per_pack_2,
  litres_storage_per_base_uom,
  planning_lead_time_days,
  gtin_parent,
  base_uom_child_per_base_uom_parent,
  gtin_lowcode
from GTIN_view;

update drp_GTIN set GTIN_lowcode = 0;
update drp_GTIN set GTIN_lowcode = 1 where gtin_parent in (select b.gtin from drp_GTIN b where b.GTIN_lowcode = 0);
update drp_GTIN set GTIN_lowcode = 2 where gtin_parent in (select b.gtin from drp_GTIN b where b.GTIN_lowcode = 1);
update drp_GTIN set GTIN_lowcode = 3 where gtin_parent in (select b.gtin from drp_GTIN b where b.GTIN_lowcode = 2);
update drp_GTIN set GTIN_lowcode = 4 where gtin_parent in (select b.gtin from drp_GTIN b where b.GTIN_lowcode = 3);
update drp_GTIN set GTIN_lowcode = 5 where gtin_parent in (select b.gtin from drp_GTIN b where b.GTIN_lowcode = 4);

delete from drp_facility;  
insert into drp_facility (
  gln,
  gln_description,
  gln_type,
  gln_capacity_litres,
  gln_parent,
  planning_transfer_time_days,
  planning_replenish_cycle_days,
  planning_next_replenishment,
  gln_lowcode)
select  
  a."CODE",
  a."NAME",
  a."TYPE_ID",
  a."COLD_STORAGE_CAPACITY",
  b."CODE",
  1,
  28,
  date_trunc('month', current_date),
  0
from "HEALTH_FACILITY" a left outer join  "HEALTH_FACILITY" b on (
  a."PARENT_ID" = b."ID" and
  a."IS_ACTIVE" = true and 
  b."IS_ACTIVE" = true);

update drp_facility set GLN_lowcode = 0;
update drp_facility set GLN_lowcode = 1 where gln_parent in (select b.gln from drp_facility b where b.gln_lowcode = 0);  
update drp_facility set GLN_lowcode = 2 where gln_parent in (select b.gln from drp_facility b where b.gln_lowcode = 1);  
update drp_facility set GLN_lowcode = 3 where gln_parent in (select b.gln from drp_facility b where b.gln_lowcode = 2);  
update drp_facility set GLN_lowcode = 4 where gln_parent in (select b.gln from drp_facility b where b.gln_lowcode = 3);  
update drp_facility set GLN_lowcode = 5 where gln_parent in (select b.gln from drp_facility b where b.gln_lowcode = 4);  
update drp_facility set GLN_lowcode = 6 where gln_parent in (select b.gln from drp_facility b where b.gln_lowcode = 5);  
update drp_facility set GLN_lowcode = 7 where gln_parent in (select b.gln from drp_facility b where b.gln_lowcode = 6);  

-- 21 days of iz days per month
DROP VIEW AVG_RATE;
CREATE VIEW AVG_RATE AS SELECT
  A."GTIN" AS GTIN,
  A."HEALTH_FACILITY_CODE" AS GLN,
  SUM(A."TRANSACTION_QTY_IN_BASE_UOM") / 28 AS AVG_USAGE
FROM "ITEM_TRANSACTION" A, DRP_GTIN_GLN_ATTRIBUTES B WHERE   
  A."TRANSACTION_TYPE_ID" = 5 AND
  A."GTIN" = B.GTIN AND
  A."HEALTH_FACILITY_CODE" = B.GLN AND
  A."TRANSACTION_DATE" <= B.AS_OF_DATE - 28
GROUP BY A."GTIN", A."HEALTH_FACILITY_CODE";   
  
-- case when hf type is a sdp use alt_1_qty_per else if divo then alt_2_qty_per
delete from drp_gtin_gln_attributes;
insert into drp_gtin_gln_attributes
  (gtin,
  gln,
  as_of_date,
  replenish_pack_qty,
  safety_stock_qty,
  planning_daily_demand,
  on_hand_balance,
  allocated_balance,
  on_hold_balance)
select
  a."GTIN",
  a."HEALTH_FACILITY_CODE",
  max(b."TRANSACTION_DATE"),
  D."ALT_1_QTY_PER",
  C."SAFETY_STOCK",
  C."AVG_DAILY_DEMAND_RATE",
  sum(A."BALANCE"),
  sum(A."ALLOCATED"),
  0
from "HEALTH_FACILITY_BALANCE" a, "ITEM_TRANSACTION" b, "GTIN_HF_STOCK_POLICY" C, "ITEM_MANUFACTURER" D where
  a."GTIN" = B."GTIN" AND
  A."HEALTH_FACILITY_CODE" = B."HEALTH_FACILITY_CODE" AND
  A."GTIN" = C."GTIN" AND
  A."HEALTH_FACILITY_CODE" = C."HEALTH_FACILITY_CODE" AND
  A."GTIN" = D."GTIN"
  -- and 
GROUP BY
  a."GTIN",
  a."HEALTH_FACILITY_CODE",
  D."ALT_1_QTY_PER",
  C."SAFETY_STOCK",
  C."AVG_DAILY_DEMAND_RATE"; 
  
update drp_gtin_gln_attributes set planning_daily_demand =
  (select avg_usage from avg_rate a where 
    a.gtin = drp_gtin_gln_attributes.gtin and
    a.gln = drp_gtin_gln_attributes.gln);

update drp_gtin_gln_attributes set planning_daily_demand = coalesce (planning_daily_demand, 1);   

delete from drp_plan;

--drop view drp_periods cascade;
drop table drp_periods cascade;

create table drp_periods as select 
  1 as period,
  gln, gln_parent, gln_lowcode, planning_transfer_time_days as LT,
  planning_next_replenishment as drp_date,
  planning_replenish_cycle_days as days
from drp_facility union select 
  2 as period,
  gln, gln_parent, gln_lowcode, planning_transfer_time_days as LT,
  planning_next_replenishment + 1 * planning_replenish_cycle_days as drp_date,
  planning_replenish_cycle_days as days
from drp_facility union select 
  3 as period,
  gln, gln_parent, gln_lowcode, planning_transfer_time_days as LT,
  planning_next_replenishment + 2 * planning_replenish_cycle_days as drp_date,
  planning_replenish_cycle_days as days
from drp_facility union select 
  4 as period,
  gln, gln_parent, gln_lowcode, planning_transfer_time_days as LT,
  planning_next_replenishment + 3 * planning_replenish_cycle_days as drp_date,
  planning_replenish_cycle_days as days  
from drp_facility union select 
  5 as period,
  gln, gln_parent, gln_lowcode, planning_transfer_time_days as LT,
  planning_next_replenishment + 4 * planning_replenish_cycle_days as drp_date,
  planning_replenish_cycle_days as days  
from drp_facility union select 
  6 as period,
  gln, gln_parent, gln_lowcode, planning_transfer_time_days as LT,
  planning_next_replenishment + 5 * planning_replenish_cycle_days as drp_date,
  planning_replenish_cycle_days as days  
from drp_facility ; 


delete from drp_sched_demand;
insert into drp_sched_demand
  (gtin,
  gln,
  doc_id,
  sched_demand_date,
  drp_date,
  period,
  sched_demand_qty)
select
  a."ORDER_GTIN",    
  B."ORDER_FACILITY_FROM",
  B."ORDER_NUM",
  B."ORDER_SCHED_REPLENISH_DATE" - C.lt,
  c.drp_date,
  c.period,
  sum(a."ORDER_QTY_IN_BASE_UOM")
from "TRANSFER_ORDER_DETAIL" A, "TRANSFER_ORDER_HEADER" B, drp_periods c  where
  a."ORDER_NUM" = b."ORDER_NUM" and
  b."ORDER_FACILITY_FROM" = c.gln_parent and 
  b."ORDER_FACILITY_TO" = c.gln and
  (b."ORDER_STATUS" = 1 or b."ORDER_STATUS" = 2) and
  b."ORDER_FACILITY_TO" = c.gln and
  ((B."ORDER_SCHED_REPLENISH_DATE" - C.lt >= c.drp_date and
  B."ORDER_SCHED_REPLENISH_DATE" - C.lt < c.drp_date + c.days) or
  (B."ORDER_SCHED_REPLENISH_DATE" - C.lt < c.drp_date and c.period = 1))
group by   
  a."ORDER_GTIN",    
  B."ORDER_FACILITY_FROM",
  B."ORDER_NUM",
  B."ORDER_SCHED_REPLENISH_DATE" - C.lt,
  c.drp_date,
  c.period;
  
delete from drp_sched_replenish;
insert into drp_sched_replenish
  (gtin,
  gln,
  from_gln,
  doc_id,
  sched_replenish_date,
  drp_date,
  period,
  sched_replenish_qty)
select
  a."ORDER_GTIN",    
  B."ORDER_FACILITY_TO",
  B."ORDER_FACILITY_FROM",  
  B."ORDER_NUM",
  B."ORDER_SCHED_REPLENISH_DATE",
  c.drp_date,
  c.period,
  sum(a."ORDER_QTY_IN_BASE_UOM")
from "TRANSFER_ORDER_DETAIL" A, "TRANSFER_ORDER_HEADER" B, drp_periods c  where
  a."ORDER_NUM" = b."ORDER_NUM" and
  b."ORDER_FACILITY_FROM" = c.gln_parent and 
  b."ORDER_FACILITY_TO" = c.gln and
  (b."ORDER_STATUS" = 1 or b."ORDER_STATUS" = 2) and
  b."ORDER_FACILITY_TO" = c.gln and
  ((B."ORDER_SCHED_REPLENISH_DATE" >= c.drp_date and
  B."ORDER_SCHED_REPLENISH_DATE"  < c.drp_date + c.days) or
  (B."ORDER_SCHED_REPLENISH_DATE"  < c.drp_date and c.period = 1))
group by   
  a."ORDER_GTIN",    
  B."ORDER_FACILITY_FROM",
  B."ORDER_NUM",
  B."ORDER_SCHED_REPLENISH_DATE" ,
  c.drp_date,
  c.period;  
  
drop view drp_demand_1_3_0;
create view drp_demand_1_3_0 as select
  a.period,
  a.gln_lowcode,
  b.gtin,
  c.gtin_lowcode,
  a.gln,
  a.LT,
  a.gln_parent,
  a.drp_date,
  b.on_hand_balance  as start_on_hand,
  (b.planning_daily_demand * a.days) as period_demand
from drp_periods a, drp_gtin_gln_attributes b, drp_gtin c where
  a.period = 1 and
  a.gln = b.gln and
  a.gln_lowcode = 3 and
  b.gtin = c.gtin and
  c.gtin_lowcode = 0;

delete from drp_plan;  
insert into drp_plan
  (gtin,
  gln,
  plan_date,
  estimated_on_hand,
  planned_demand,
  sched_demand,
  sched_replenish,
  sched_on_hand,
  planned_order_release_date,
  planned_order_receipt,
  planned_on_hand,
  period,
  gln_lowcode,
  gtin_lowcode,
  regen_time)
select 
  a.gtin,
  a.gln,
  a.drp_date,
  a.start_on_hand,
  a.period_demand,
  coalesce(sum(b.sched_demand_qty),0),
  coalesce(sum(c.sched_replenish_qty),0),
  a.start_on_hand - a.period_demand,
  a.drp_date - a.LT,
  0,
  a.start_on_hand - a.period_demand,
  1, 3, 0,
  current_timestamp
from drp_demand_1_3_0 a 
left outer join drp_sched_demand b on (
  a.gtin = b.gtin and 
  a.gln = b.gln and
  b.period = 1)
left outer join drp_sched_replenish c on (
  a.gtin = c.gtin and 
  a.gln = c.gln and
  c.period = 1)
group by
  a.gtin,
  a.gln,
  a.drp_date,
  a.start_on_hand,
  a.period_demand,  
  a.start_on_hand - a.period_demand,
  a.drp_date - a.LT;

update drp_plan set sched_on_hand = estimated_on_hand - planned_demand - sched_demand + sched_replenish
  where period = 1 and gln_lowcode = 3 and gtin_lowcode = 0;  

update drp_plan set planned_order_receipt =
(select 
  ceiling (-1 * (sched_on_hand - b.safety_stock_qty) / b.replenish_pack_qty) * b.replenish_pack_qty
from drp_gtin_gln_attributes b where 
  drp_plan.gtin = b.gtin and 
  drp_plan.gln = b.gln and sched_on_hand < b.safety_stock_qty)
where  period = 1 and gln_lowcode = 3 and gtin_lowcode = 0;
  
update drp_plan set planned_order_receipt = coalesce (planned_order_receipt, 0)
  where period = 1 and gln_lowcode = 3 and gtin_lowcode = 0;  

update drp_plan set planned_on_hand = sched_on_hand + planned_order_receipt
  where period = 1 and gln_lowcode = 3 and gtin_lowcode = 0; 



drop view drp_demand_1_3_0;
create view drp_demand_2_3_0 as select
  a.period,
  a.gln_lowcode,
  b.gtin,
  c.gtin_lowcode,
  a.gln,
  a.LT,
  a.gln_parent,
  a.drp_date,
  d.sched_on_hand as start_on_hand,
  (b.planning_daily_demand * a.days) as period_demand
from drp_periods a, drp_gtin_gln_attributes b, drp_gtin c, drp_plan d where
  a.period = 2 and
  a.gln = b.gln and
  a.gln_lowcode = 3 and
  b.gtin = c.gtin and
  c.gtin_lowcode = 0 and
  a.gln = d.gln and
  b.gtin = d.gtin and
  d.period = 1;
  
insert into drp_plan
  (gtin,
  gln,
  plan_date,
  estimated_on_hand,
  planned_demand,
  sched_demand,
  sched_replenish,
  sched_on_hand,
  planned_order_release_date,
  planned_order_receipt,
  planned_on_hand,
  period,
  gln_lowcode,
  gtin_lowcode,
  regen_time)
select 
  a.gtin,
  a.gln,
  a.drp_date,
  a.start_on_hand,
  a.period_demand,
  0,
  0,
  a.start_on_hand - a.period_demand,
  a.drp_date - a.LT,
  0,
  a.start_on_hand - a.period_demand,
  2, 3, 0,
  current_timestamp
from drp_demand_2_3_0 a;


update drp_plan set
  sched_demand = 
  (select sum(sched_demand_qty) from drp_sched_demand a where 
  a.period = 2 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 2 and gln_lowcode = 3 and gtin_lowcode = 0  ;
  
update drp_plan set
  sched_replenish =
   (select sum(sched_replenish_qty) from drp_sched_replenish a where 
   a.period = 2 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 2 and gln_lowcode = 3 and gtin_lowcode = 0   ;   
   
  
update drp_plan set sched_demand = coalesce (sched_demand, 0) 
  where period = 2 and gln_lowcode = 3 and gtin_lowcode = 0;
update drp_plan set sched_replenish = coalesce (sched_replenish,0) 
  where period = 2 and gln_lowcode = 3 and gtin_lowcode = 0;
   

update drp_plan set sched_on_hand = estimated_on_hand - planned_demand - sched_demand + sched_replenish
  where period = 2 and gln_lowcode = 3 and gtin_lowcode = 0;  

update drp_plan set planned_order_receipt =
(select 
  ceiling (-1 * (sched_on_hand - b.safety_stock_qty) / b.replenish_pack_qty) * b.replenish_pack_qty
from drp_gtin_gln_attributes b where 
  drp_plan.gtin = b.gtin and 
  drp_plan.gln = b.gln and sched_on_hand < b.safety_stock_qty)
where  period = 2 and gln_lowcode = 3 and gtin_lowcode = 0;
  
update drp_plan set planned_order_receipt = coalesce (planned_order_receipt, 0)
  where period = 2 and gln_lowcode = 3 and gtin_lowcode = 0;  

update drp_plan set planned_on_hand = sched_on_hand + planned_order_receipt
  where period = 2 and gln_lowcode = 3 and gtin_lowcode = 0; 



drop view drp_demand_2_3_0;
create view drp_demand_3_3_0 as select
  a.period,
  a.gln_lowcode,
  b.gtin,
  c.gtin_lowcode,
  a.gln,
  a.LT,
  a.gln_parent,
  a.drp_date,
  d.sched_on_hand as start_on_hand,
  (b.planning_daily_demand * a.days) as period_demand
from drp_periods a, drp_gtin_gln_attributes b, drp_gtin c, drp_plan d where
  a.period = 3 and
  a.gln = b.gln and
  a.gln_lowcode = 3 and
  b.gtin = c.gtin and
  c.gtin_lowcode = 0 and
  a.gln = d.gln and
  b.gtin = d.gtin and
  d.period = 2;
  
insert into drp_plan
  (gtin,
  gln,
  plan_date,
  estimated_on_hand,
  planned_demand,
  sched_demand,
  sched_replenish,
  sched_on_hand,
  planned_order_release_date,
  planned_order_receipt,
  planned_on_hand,
  period,
  gln_lowcode,
  gtin_lowcode,
  regen_time)
select 
  a.gtin,
  a.gln,
  a.drp_date,
  a.start_on_hand,
  a.period_demand,
  0,
  0,
  a.start_on_hand - a.period_demand,
  a.drp_date - a.LT,
  0,
  a.start_on_hand - a.period_demand,
  3, 3, 0,
  current_timestamp
from drp_demand_3_3_0 a;


update drp_plan set
  sched_demand = 
  (select sum(sched_demand_qty) from drp_sched_demand a where 
  a.period = 3 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 3 and gln_lowcode = 3 and gtin_lowcode = 0  ;
  
update drp_plan set
  sched_replenish =
   (select sum(sched_replenish_qty) from drp_sched_replenish a where 
   a.period = 3 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 3 and gln_lowcode = 3 and gtin_lowcode = 0   ;   
   
  
update drp_plan set sched_demand = coalesce (sched_demand, 0) 
  where period = 3 and gln_lowcode = 3 and gtin_lowcode = 0;
update drp_plan set sched_replenish = coalesce (sched_replenish,0) 
  where period = 3 and gln_lowcode = 3 and gtin_lowcode = 0;
   

update drp_plan set sched_on_hand = estimated_on_hand - planned_demand - sched_demand + sched_replenish
  where period = 3 and gln_lowcode = 3 and gtin_lowcode = 0;  

update drp_plan set planned_order_receipt =
(select 
  ceiling (-1 * (sched_on_hand - b.safety_stock_qty) / b.replenish_pack_qty) * b.replenish_pack_qty
from drp_gtin_gln_attributes b where 
  drp_plan.gtin = b.gtin and 
  drp_plan.gln = b.gln and sched_on_hand < b.safety_stock_qty)
where  period = 3 and gln_lowcode = 3 and gtin_lowcode = 0;
  
update drp_plan set planned_order_receipt = coalesce (planned_order_receipt, 0)
  where period = 3 and gln_lowcode = 3 and gtin_lowcode = 0;  

update drp_plan set planned_on_hand = sched_on_hand + planned_order_receipt
  where period = 3 and gln_lowcode = 3 and gtin_lowcode = 0; 



drop view drp_demand_3_3_0;
create view drp_demand_1_3_1 as select
  a.period,
  a.gln_lowcode,
  b.gtin,
  c.gtin_lowcode,
  a.gln,
  a.LT,
  a.gln_parent,
  a.drp_date,
  b.on_hand_balance  as start_on_hand,
  (c.base_uom_child_per_base_uom_parent * d.planned_demand) as period_demand
from drp_periods a, drp_gtin_gln_attributes b, drp_gtin c, drp_plan d, drp_gtin_gln_attributes e where
  a.period = 1 and
  a.gln = b.gln and
  a.gln_lowcode = 3 and
  b.gtin = c.gtin and
  c.gtin_lowcode = 1 and
  a.gln = d.gln and
  c.gtin_parent = d.gtin and
  d.period = 1 and 
  d.gtin = e.gtin and
  d.gln = e.gln;
  
insert into drp_plan
  (gtin,
  gln,
  plan_date,
  estimated_on_hand,
  planned_demand,
  sched_demand,
  sched_replenish,
  sched_on_hand,
  planned_order_release_date,
  planned_order_receipt,
  planned_on_hand,
  period,
  gln_lowcode,
  gtin_lowcode,
  regen_time)
select 
  a.gtin,
  a.gln,
  a.drp_date,
  a.start_on_hand,
  a.period_demand,
  0,
  0,
  a.start_on_hand - a.period_demand,
  a.drp_date - a.LT,
  0,
  a.start_on_hand - a.period_demand,
  1, 3, 1,
  current_timestamp
from drp_demand_1_3_1 a;


update drp_plan set
  sched_demand = 
  (select sum(sched_demand_qty) from drp_sched_demand a where 
  a.period = 1 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 1 and gln_lowcode = 3 and gtin_lowcode = 1  ;
  
update drp_plan set
  sched_replenish =
   (select sum(sched_replenish_qty) from drp_sched_replenish a where 
   a.period = 1 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 1 and gln_lowcode = 3 and gtin_lowcode = 1   ;   
   
  
update drp_plan set sched_demand = coalesce (sched_demand, 0) 
  where period = 1 and gln_lowcode = 3 and gtin_lowcode = 1;
update drp_plan set sched_replenish = coalesce (sched_replenish,0) 
  where period = 1 and gln_lowcode = 3 and gtin_lowcode = 1;
   

update drp_plan set sched_on_hand = estimated_on_hand - planned_demand - sched_demand + sched_replenish
  where period = 1 and gln_lowcode = 3 and gtin_lowcode = 1;  

update drp_plan set planned_order_receipt =
(select 
  ceiling (-1 * (sched_on_hand - b.safety_stock_qty) / b.replenish_pack_qty) * b.replenish_pack_qty
from drp_gtin_gln_attributes b where 
  drp_plan.gtin = b.gtin and 
  drp_plan.gln = b.gln and sched_on_hand < b.safety_stock_qty)
where  period = 1 and gln_lowcode = 3 and gtin_lowcode = 1;
  
update drp_plan set planned_order_receipt = coalesce (planned_order_receipt, 0)
  where period = 1 and gln_lowcode = 3 and gtin_lowcode = 1;  

update drp_plan set planned_on_hand = sched_on_hand + planned_order_receipt
  where period = 1 and gln_lowcode = 3 and gtin_lowcode = 1; 
  


drop view drp_demand_1_3_1;
create view drp_demand_2_3_1 as select
  a.period,
  a.gln_lowcode,
  b.gtin,
  c.gtin_lowcode,
  a.gln,
  a.LT,
  a.gln_parent,
  a.drp_date,
  e.sched_on_hand as start_on_hand,
  (c.base_uom_child_per_base_uom_parent * d.planned_demand) as period_demand
from drp_periods a, drp_gtin_gln_attributes b, drp_gtin c, drp_plan d, drp_plan e where
  a.period = 2 and
  a.gln = b.gln and
  a.gln_lowcode = 3 and
  b.gtin = c.gtin and
  c.gtin_lowcode = 1 and
  a.gln = d.gln and
  c.gtin_parent = d.gtin and
  d.period = 2 and 
  c.gtin = e.gtin and
  d.gln = e.gln and
  e.period = 1;
  
insert into drp_plan
  (gtin,
  gln,
  plan_date,
  estimated_on_hand,
  planned_demand,
  sched_demand,
  sched_replenish,
  sched_on_hand,
  planned_order_release_date,
  planned_order_receipt,
  planned_on_hand,
  period,
  gln_lowcode,
  gtin_lowcode,
  regen_time)
select 
  a.gtin,
  a.gln,
  a.drp_date,
  a.start_on_hand,
  a.period_demand,
  0,
  0,
  a.start_on_hand - a.period_demand,
  a.drp_date - a.LT,
  0,
  a.start_on_hand - a.period_demand,
  2, 3, 1,
  current_timestamp
from drp_demand_2_3_1 a;


update drp_plan set
  sched_demand = 
  (select sum(sched_demand_qty) from drp_sched_demand a where 
  a.period = 2 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 2 and gln_lowcode = 3 and gtin_lowcode = 1  ;
  
update drp_plan set
  sched_replenish =
   (select sum(sched_replenish_qty) from drp_sched_replenish a  where 
   a.period = 2 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 2 and gln_lowcode = 3 and gtin_lowcode = 1   ;   
   
  
update drp_plan set sched_demand = coalesce (sched_demand, 0) 
  where period = 2 and gln_lowcode = 3 and gtin_lowcode = 1;
update drp_plan set sched_replenish = coalesce (sched_replenish,0) 
  where period = 2 and gln_lowcode = 3 and gtin_lowcode = 1;
   

update drp_plan set sched_on_hand = estimated_on_hand - planned_demand - sched_demand + sched_replenish
  where period = 2 and gln_lowcode = 3 and gtin_lowcode = 1;  

update drp_plan set planned_order_receipt =
(select 
  ceiling (-1 * (sched_on_hand - b.safety_stock_qty) / b.replenish_pack_qty) * b.replenish_pack_qty
from drp_gtin_gln_attributes b where 
  drp_plan.gtin = b.gtin and 
  drp_plan.gln = b.gln and sched_on_hand < b.safety_stock_qty)
where  period = 2 and gln_lowcode = 3 and gtin_lowcode = 1;
  
update drp_plan set planned_order_receipt = coalesce (planned_order_receipt, 0)
  where period = 2 and gln_lowcode = 3 and gtin_lowcode = 1;  

update drp_plan set planned_on_hand = sched_on_hand + planned_order_receipt
  where period = 2 and gln_lowcode = 3 and gtin_lowcode = 1;   
  


drop view drp_demand_2_3_1;
create view drp_demand_3_3_1 as select
  a.period,
  a.gln_lowcode,
  b.gtin,
  c.gtin_lowcode,
  a.gln,
  a.LT,
  a.gln_parent,
  a.drp_date,
  e.sched_on_hand as start_on_hand,
  (c.base_uom_child_per_base_uom_parent * d.planned_demand) as period_demand
from drp_periods a, drp_gtin_gln_attributes b, drp_gtin c, drp_plan d, drp_plan e where
  a.period = 3 and
  a.gln = b.gln and
  a.gln_lowcode = 3 and
  b.gtin = c.gtin and
  c.gtin_lowcode = 1 and
  a.gln = d.gln and
  c.gtin_parent = d.gtin and
  d.period = 3 and 
  c.gtin = e.gtin and
  d.gln = e.gln and
  e.period = 2;
  
insert into drp_plan
  (gtin,
  gln,
  plan_date,
  estimated_on_hand,
  planned_demand,
  sched_demand,
  sched_replenish,
  sched_on_hand,
  planned_order_release_date,
  planned_order_receipt,
  planned_on_hand,
  period,
  gln_lowcode,
  gtin_lowcode,
  regen_time)
select 
  a.gtin,
  a.gln,
  a.drp_date,
  a.start_on_hand,
  a.period_demand,
  0,
  0,
  a.start_on_hand - a.period_demand,
  a.drp_date - a.LT,
  0,
  a.start_on_hand - a.period_demand,
  3, 3, 1,
  current_timestamp
from drp_demand_3_3_1 a;


update drp_plan set
  sched_demand = 
  (select sum(sched_demand_qty) from drp_sched_demand a  where 
  a.period = 3 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 3 and gln_lowcode = 3 and gtin_lowcode = 1  ;
  
update drp_plan set
  sched_replenish =
   (select sum(sched_replenish_qty) from drp_sched_replenish a  where 
   a.period=3 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 3 and gln_lowcode = 3 and gtin_lowcode = 1   ;   
   
  
update drp_plan set sched_demand = coalesce (sched_demand, 0) 
  where period = 3 and gln_lowcode = 3 and gtin_lowcode = 1;
update drp_plan set sched_replenish = coalesce (sched_replenish,0) 
  where period = 3 and gln_lowcode = 3 and gtin_lowcode = 1;
   

update drp_plan set sched_on_hand = estimated_on_hand - planned_demand - sched_demand + sched_replenish
  where period = 3 and gln_lowcode = 3 and gtin_lowcode = 1;  

update drp_plan set planned_order_receipt =
(select 
  ceiling (-1 * (sched_on_hand - b.safety_stock_qty) / b.replenish_pack_qty) * b.replenish_pack_qty
from drp_gtin_gln_attributes b where 
  drp_plan.gtin = b.gtin and 
  drp_plan.gln = b.gln and sched_on_hand < b.safety_stock_qty)
where  period = 3 and gln_lowcode = 3 and gtin_lowcode = 1;
  
update drp_plan set planned_order_receipt = coalesce (planned_order_receipt, 0)
  where period = 3 and gln_lowcode = 3 and gtin_lowcode = 1;  

update drp_plan set planned_on_hand = sched_on_hand + planned_order_receipt
  where period = 3 and gln_lowcode = 3 and gtin_lowcode = 1;     
  


drop view drp_demand_3_3_1;
drop view drp_demand_1_2_a;
create view drp_demand_1_2_a as select
  a.period,
  a.gln_lowcode,
  b.gtin,
  d.gtin_lowcode,
  a.gln,
  a.LT,
  a.gln_parent,
  d.planned_order_release_date as drp_date,
  b.on_hand_balance as start_on_hand,
  sum(d.planned_order_receipt) as period_demand
from drp_periods a, drp_gtin_gln_attributes b, drp_periods c, drp_plan d where
  a.period = 1 and
  a.gln = b.gln and
  a.gln_lowcode = 2 and
  b.gtin = d.gtin and
  a.gln = c.gln_parent and
  c.gln_lowcode = 3 and
  c.gln = d.gln and
  d.period = 1 and
  d.gln_lowcode = 3
group by  
  a.period,
  a.gln_lowcode,
  b.gtin,
  d.gtin_lowcode,
  a.gln,
  a.LT,
  a.gln_parent,
  d.planned_order_release_date,
  b.on_hand_balance;
  
insert into drp_plan
  (gtin,
  gln,
  plan_date,
  estimated_on_hand,
  planned_demand,
  sched_demand,
  sched_replenish,
  sched_on_hand,
  planned_order_release_date,
  planned_order_receipt,
  planned_on_hand,
  period,
  gln_lowcode,
  gtin_lowcode,
  regen_time)
select 
  a.gtin,
  a.gln,
  a.drp_date,
  a.start_on_hand,
  a.period_demand,
  0,
  0,
  a.start_on_hand - a.period_demand,
  a.drp_date - a.LT,
  0,
  a.start_on_hand - a.period_demand,
  1, 2, a.gtin_lowcode,
  current_timestamp
from drp_demand_1_2_a a;


update drp_plan set
  sched_demand = 
  (select sum(sched_demand_qty) from drp_sched_demand a  where 
  a.period = 1 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 1 and gln_lowcode = 2  ;
  
update drp_plan set
  sched_replenish =
   (select sum(sched_replenish_qty) from drp_sched_replenish a  where 
   a.period = 1 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 1 and gln_lowcode = 2 ;   
   
  
update drp_plan set sched_demand = coalesce (sched_demand, 0) 
  where period = 1 and gln_lowcode = 2;
update drp_plan set sched_replenish = coalesce (sched_replenish,0) 
  where period = 1 and gln_lowcode = 2;
   

update drp_plan set sched_on_hand = estimated_on_hand - planned_demand - sched_demand + sched_replenish
  where period = 1 and gln_lowcode = 2;  

update drp_plan set planned_order_receipt =
(select 
  ceiling (-1 * (sched_on_hand - b.safety_stock_qty) / b.replenish_pack_qty) * b.replenish_pack_qty
from drp_gtin_gln_attributes b where 
  drp_plan.gtin = b.gtin and 
  drp_plan.gln = b.gln and sched_on_hand < b.safety_stock_qty)
where  period = 1 and gln_lowcode = 2;
  
update drp_plan set planned_order_receipt = coalesce (planned_order_receipt, 0)
  where period = 1 and gln_lowcode = 2;  

update drp_plan set planned_on_hand = sched_on_hand + planned_order_receipt
  where period = 1 and gln_lowcode = 2;   
  



drop view drp_demand_1_2_a;
create view drp_demand_2_2_a as select
  a.period,
  a.gln_lowcode,
  b.gtin,
  d.gtin_lowcode,
  a.gln,
  a.LT,
  a.gln_parent,
  d.planned_order_release_date as drp_date,
  e.sched_on_hand as start_on_hand,
  sum(d.planned_order_receipt) as period_demand
from drp_periods a, drp_gtin_gln_attributes b, drp_periods c, drp_plan d, drp_plan e where
  a.period = 2 and
  a.gln = b.gln and
  a.gln_lowcode = 2 and
  b.gtin = d.gtin and
  a.gln = c.gln_parent and
  c.gln_lowcode = 3 and
  c.gln = d.gln and
  d.period = 2 and
  d.gln_lowcode = 3 and
  e.period = 1 and
  e.gtin = b.gtin and
  e.gln = a.gln
group by  
  a.period,
  a.gln_lowcode,
  b.gtin,
  d.gtin_lowcode,
  a.gln,
  a.LT,
  a.gln_parent,
  d.planned_order_release_date,
  e.sched_on_hand;
  
insert into drp_plan
  (gtin,
  gln,
  plan_date,
  estimated_on_hand,
  planned_demand,
  sched_demand,
  sched_replenish,
  sched_on_hand,
  planned_order_release_date,
  planned_order_receipt,
  planned_on_hand,
  period,
  gln_lowcode,
  gtin_lowcode,
  regen_time)
select 
  a.gtin,
  a.gln,
  a.drp_date,
  a.start_on_hand,
  a.period_demand,
  0,
  0,
  a.start_on_hand - a.period_demand,
  a.drp_date - a.LT,
  0,
  a.start_on_hand - a.period_demand,
  2, 2, a.gtin_lowcode,
  current_timestamp
from drp_demand_2_2_a a;


update drp_plan set
  sched_demand = 
  (select sum(sched_demand_qty) from drp_sched_demand a where 
  a.period = 2 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 2 and gln_lowcode = 2  ;
  
update drp_plan set
  sched_replenish =
   (select sum(sched_replenish_qty) from drp_sched_replenish a  where 
   a.period = 2 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 2 and gln_lowcode = 2 ;   
   
  
update drp_plan set sched_demand = coalesce (sched_demand, 0) 
  where period = 2 and gln_lowcode = 2;
update drp_plan set sched_replenish = coalesce (sched_replenish,0) 
  where period = 2 and gln_lowcode = 2;
   

update drp_plan set sched_on_hand = estimated_on_hand - planned_demand - sched_demand + sched_replenish
  where period = 2 and gln_lowcode = 2;  

update drp_plan set planned_order_receipt =
(select 
  ceiling (-1 * (sched_on_hand - b.safety_stock_qty) / b.replenish_pack_qty) * b.replenish_pack_qty
from drp_gtin_gln_attributes b where 
  drp_plan.gtin = b.gtin and 
  drp_plan.gln = b.gln and sched_on_hand < b.safety_stock_qty)
where  period = 2 and gln_lowcode = 2;
  
update drp_plan set planned_order_receipt = coalesce (planned_order_receipt, 0)
  where period = 2 and gln_lowcode = 2;  

update drp_plan set planned_on_hand = sched_on_hand + planned_order_receipt
  where period = 2 and gln_lowcode = 2;     
  



drop view drp_demand_2_2_a;
create view drp_demand_3_2_a as select
  a.period,
  a.gln_lowcode,
  b.gtin,
  d.gtin_lowcode,
  a.gln,
  a.LT,
  a.gln_parent,
  d.planned_order_release_date as drp_date,
  e.sched_on_hand as start_on_hand,
  sum(d.planned_order_receipt) as period_demand
from drp_periods a, drp_gtin_gln_attributes b, drp_periods c, drp_plan d, drp_plan e where
  a.period = 3 and
  a.gln = b.gln and
  a.gln_lowcode = 2 and
  b.gtin = d.gtin and
  a.gln = c.gln_parent and
  c.gln_lowcode = 3 and
  c.gln = d.gln and
  d.period = 3 and
  d.gln_lowcode = 3 and
  e.period = 2 and
  e.gtin = b.gtin and
  e.gln = a.gln
group by  
  a.period,
  a.gln_lowcode,
  b.gtin,
  d.gtin_lowcode,
  a.gln,
  a.LT,
  a.gln_parent,
  d.planned_order_release_date,
  e.sched_on_hand;
  
insert into drp_plan
  (gtin,
  gln,
  plan_date,
  estimated_on_hand,
  planned_demand,
  sched_demand,
  sched_replenish,
  sched_on_hand,
  planned_order_release_date,
  planned_order_receipt,
  planned_on_hand,
  period,
  gln_lowcode,
  gtin_lowcode,
  regen_time)
select 
  a.gtin,
  a.gln,
  a.drp_date,
  a.start_on_hand,
  a.period_demand,
  0,
  0,
  a.start_on_hand - a.period_demand,
  a.drp_date - a.LT,
  0,
  a.start_on_hand - a.period_demand,
  3, 2, a.gtin_lowcode,
  current_timestamp
from drp_demand_3_2_a a;


update drp_plan set
  sched_demand = 
  (select sum(sched_demand_qty) from drp_sched_demand a where 
  a.period = 3 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 3 and gln_lowcode = 2  ;
  
update drp_plan set
  sched_replenish =
   (select sum(sched_replenish_qty) from drp_sched_replenish a  where 
   a.period = 3 and a.gtin = drp_plan.gtin and a.gln = drp_plan.gln)
where period = 3 and gln_lowcode = 2 ;   
   
  
update drp_plan set sched_demand = coalesce (sched_demand, 0) 
  where period = 3 and gln_lowcode = 2;
update drp_plan set sched_replenish = coalesce (sched_replenish,0) 
  where period = 3 and gln_lowcode = 2;
   

update drp_plan set sched_on_hand = estimated_on_hand - planned_demand - sched_demand + sched_replenish
  where period = 3 and gln_lowcode = 2;  

update drp_plan set planned_order_receipt =
(select 
  ceiling (-1 * (sched_on_hand - b.safety_stock_qty) / b.replenish_pack_qty) * b.replenish_pack_qty
from drp_gtin_gln_attributes b where 
  drp_plan.gtin = b.gtin and 
  drp_plan.gln = b.gln and sched_on_hand < b.safety_stock_qty)
where  period = 3 and gln_lowcode = 2;
  
update drp_plan set planned_order_receipt = coalesce (planned_order_receipt, 0)
  where period = 3 and gln_lowcode = 2;  

update drp_plan set planned_on_hand = sched_on_hand + planned_order_receipt
  where period = 3 and gln_lowcode = 2;       

