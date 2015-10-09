-- Function: defaulters_list(integer)

-- DROP FUNCTION defaulters_list(integer);

CREATE OR REPLACE FUNCTION defaulters_list(IN facilityId integer, IN villageId integer, IN doseId integer)
  RETURNS TABLE(id integer, system_id text, firstname text, secondname text, lastname text, dob date, gender text, vaccine text, vaccination_date date, contact text) AS
$BODY$
 WITH RECURSIVE facility(hfid) AS (
	SELECT "ID" as hfid, "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = facilityId
	UNION
	SELECT "ID", facility.hfid FROM facility, "HEALTH_FACILITY" WHERE "HEALTH_FACILITY"."PARENT_ID" = facility.hfid
)
SELECT
c."ID",
c."SYSTEM_ID",
c."FIRSTNAME1",
c."FIRSTNAME2",
c."LASTNAME1",
c."BIRTHDATE",
CASE c."GENDER" WHEN true THEN 'Male' ELSE 'Female' END as "GENDER",
d."FULLNAME" as "VACCCINE",
v."VACCINATION_DATE",
c."PHONE" as "CONTACT"
FROM "CHILD" c
	JOIN "VACCINATION_EVENT" v ON c."ID" = v."CHILD_ID"
	JOIN "DOSE" d on v."DOSE_ID" = d."ID"
	JOIN "AGE_DEFINITIONS" a ON d."AGE_DEFINITION_ID" = a."ID"
	WHERE c."HEALTHCENTER_ID" IN (SELECT hfid FROM facility)
	AND v."IS_ACTIVE" = true
	AND v."VACCINATION_STATUS" = false
	AND v."VACCINATION_DATE" <= current_date
	AND d."ID" = doseId
	AND c."DOMICILE_ID" = villageId
 $BODY$
  LANGUAGE sql VOLATILE
  COST 100
  ROWS 1000;
  
DELETE FROM "REPORT_PARAMETERS" WHERE "REPORT_ID" = 4;
  
INSERT INTO "REPORT_PARAMETERS" VALUES(4,'Facility','Facility',NULL,TRUE,2);
INSERT INTO "REPORT_PARAMETERS" VALUES(4,'Dose','Antigen',NULL,TRUE,5);
INSERT INTO "REPORT_PARAMETERS" VALUES(4,'Village','Village',NULL,TRUE,11);