-- Function: vaccines_administered(text, integer, text)

-- DROP FUNCTION vaccines_administered(text, integer, text);

CREATE OR REPLACE FUNCTION healthfacilityconsumption(
    IN fromdate text,
    IN parentid integer,
    IN todate text)
  RETURNS TABLE(name text, usage bigint, month_no double precision, month text, fullname text, parent_name text) AS
$BODY$
	SELECT
	h."NAME",
	COUNT(d."ID") as "USAGE",
	v."MONTH" as "MONTH_NO",
	TO_CHAR(TO_TIMESTAMP(v."MONTH"::CHAR, 'MM'), 'MON') AS "MONTH",
	d."FULLNAME",
	p."NAME" as "PARENT_NAME"
	FROM (SELECT *, EXTRACT(MONTH FROM "VACCINATION_DATE") AS "MONTH" FROM "VACCINATION_EVENT") v
	JOIN "HEALTH_FACILITY" h ON v."HEALTH_FACILITY_ID" = h."ID"
	JOIN "DOSE" d ON v."DOSE_ID" = d."ID"
	INNER JOIN "HEALTH_FACILITY" p ON (h."PARENT_ID" = p."ID")
	AND v."VACCINATION_STATUS" = true
	AND v."VACCINATION_DATE" BETWEEN fromdate::DATE AND todate::DATE
	AND p."ID" = parentid
	GROUP BY h."NAME", d."ID", p."NAME", v."MONTH"
	ORDER BY h."NAME", v."MONTH", d."FULLNAME"
$BODY$
  LANGUAGE sql VOLATILE
  COST 100
  ROWS 1000;

DELETE FROM "REPORT_PARAMETERS" WHERE "REPORT_ID" = 5;
  
INSERT INTO "REPORT_PARAMETERS" VALUES(5,'Facility','Facility',NULL,TRUE,2);
INSERT INTO "REPORT_PARAMETERS" VALUES(5,'fromDate','Date From',NULL,TRUE,1);
INSERT INTO "REPORT_PARAMETERS" VALUES(5,'toDate','Date To',NULL,TRUE,1);