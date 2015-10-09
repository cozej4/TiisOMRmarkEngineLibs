-- Function: coverage_report(integer)

-- DROP FUNCTION coverage_report(integer);

CREATE OR REPLACE FUNCTION coverage_report(IN doseid integer, IN facilityId integer, IN dobfrom text, IN dobto text)
  RETURNS TABLE(month double precision, dose_id integer, dose_name text, vaccinated bigint, total bigint) AS
$BODY$
SELECT * FROM (WITH RECURSIVE facility(hfid) AS (
	SELECT "ID" as hfid , "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = facilityid
	UNION
	SELECT "ID", facility.hfid FROM facility, "HEALTH_FACILITY" WHERE "HEALTH_FACILITY"."PARENT_ID" = facility.hfid
)
Select V.m,V.dose_id,D."FULLNAME",
	(SELECT COUNT(*) from "VACCINATION_EVENT" v2
		JOIN "CHILD" C ON v2."CHILD_ID" = C."ID"
		WHERE (EXTRACT (MONTH FROM v2."VACCINATION_DATE") <= V.m)
		AND c."HEALTHCENTER_ID" IN (SELECT hfid FROM facility)
		AND C."BIRTHDATE" BETWEEN dobfrom::DATE AND dobto::DATE
		AND (v2."VACCINATION_STATUS" = TRUE)
		AND (v2."IS_ACTIVE" = TRUE)
		AND (v2."DOSE_ID" = V.dose_id)) as vaccinated,

	(SELECT COUNT(*) from "VACCINATION_EVENT" v2
		JOIN "CHILD" C ON v2."CHILD_ID" = C."ID"
		WHERE (EXTRACT (MONTH FROM v2."VACCINATION_DATE") <= V.m)
		AND c."HEALTHCENTER_ID" IN (SELECT hfid FROM facility)
		AND C."BIRTHDATE" BETWEEN dobfrom::DATE AND dobto::DATE
		AND (v2."DOSE_ID" = V.dose_id)) as total
FROM
	(SELECT EXTRACT (MONTH FROM "VACCINATION_DATE") m,
		"DOSE_ID" dose_id
		FROM "VACCINATION_EVENT") V
JOIN "DOSE" D ON V.dose_id = D."ID"
WHERE V.dose_id = doseid
GROUP BY V.m,D."FULLNAME",V.dose_id
ORDER BY V.m,D."FULLNAME"
) as asd
$BODY$
  LANGUAGE sql VOLATILE
  COST 100
  ROWS 1000;

DELETE FROM "REPORT_PARAMETERS" WHERE "REPORT_ID" = 7;
INSERT INTO "REPORT_PARAMETERS" VALUES(7,'DoseId','Antigen',NULL,TRUE,5);
INSERT INTO "REPORT_PARAMETERS" VALUES(7,'FacilityId','Facility',NULL,TRUE,2);
INSERT INTO "REPORT_PARAMETERS" VALUES(7,'dobfrom','Birthday From',NULL,TRUE,1);
INSERT INTO "REPORT_PARAMETERS" VALUES(7,'dobto','Birthday To',NULL,TRUE,1);
  