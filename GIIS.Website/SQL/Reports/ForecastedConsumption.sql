-- Function: drp_divo_overview(text, text, text)

-- DROP FUNCTION drp_divo_overview(text, text, text);

CREATE OR REPLACE FUNCTION drp_divo_overview(
    IN facilityId integer,
    IN doseId integer,
    IN fromdate text,
    IN todate text)
  RETURNS TABLE(gtin text, gln text, plan_date date, estimated_on_hand double precision,
   planned_demand double precision, sched_demand double precision,
    sched_replenish double precision, sched_on_hand double precision,
     planned_order_release_date date, planned_order_receipt double precision,
      planned_on_hand double precision, regen_time timestamp without time zone,
       drp_id numeric, period integer, gtin_lowcode integer, gln_lowcode integer,
        gtin_description text, to_gln text, to_facility text, from_gln text, from_facility text) AS
$BODY$
	WITH RECURSIVE facility(hfid) AS (
		SELECT "ID" as hfid, "HEALTH_FACILITY"."CODE", "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = facilityId
		UNION
		SELECT "ID", "HEALTH_FACILITY"."CODE", facility.hfid FROM facility, "HEALTH_FACILITY" WHERE "HEALTH_FACILITY"."PARENT_ID" = facility.hfid
	)
	select * from drp_plan_summary where from_gln in (SELECT "CODE" FROM facility) and
	plan_date between fromdate::DATE and todate::DATE
$BODY$
  LANGUAGE sql VOLATILE
  COST 100
  ROWS 1000;

DELETE FROM "REPORT_PARAMETERS" WHERE "REPORT_ID" = 14;
  
INSERT INTO "REPORT_PARAMETERS" VALUES(14,'Facility','Facility',NULL,TRUE,2);
INSERT INTO "REPORT_PARAMETERS" VALUES(14,'Dose','Antigen',NULL,TRUE,5);
INSERT INTO "REPORT_PARAMETERS" VALUES(14,'fromDate','Date From',NULL,TRUE,1);
INSERT INTO "REPORT_PARAMETERS" VALUES(14,'toDate','Date To',NULL,TRUE,1);