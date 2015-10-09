-- Function: non_vaccinations_report(integer)

-- DROP FUNCTION non_vaccinations_report(integer);

CREATE OR REPLACE FUNCTION non_vaccinations_report(IN parentid integer, IN dobfrom text, IN dobto text, IN doseId integer)
  RETURNS TABLE(id integer, system_id text, firstname text, secondname text, lastname text, dob date, gender text, vaccine text, v_date date, v_status boolean, reason text, age text) AS
$BODY$
WITH RECURSIVE facility(hfid) AS (
	SELECT "ID" as hfid, "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = parentid
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
v."VACCINATION_STATUS",
n."NAME" as "REASON",
a."NAME" as "AT_AGE"
FROM "CHILD" c
	JOIN "VACCINATION_EVENT" v ON c."ID" = v."CHILD_ID"
	JOIN "DOSE" d on v."DOSE_ID" = d."ID"
	JOIN "AGE_DEFINITIONS" a ON d."AGE_DEFINITION_ID" = a."ID"
	LEFT JOIN "NONVACCINATION_REASON" n on v."NONVACCINATION_REASON_ID" = n."ID"
	WHERE v."IS_ACTIVE" = true
	AND v."VACCINATION_DATE" <= current_date
	AND v."HEALTH_FACILITY_ID" IN (SELECT hfid FROM facility)
	AND c."BIRTHDATE" BETWEEN dobfrom::DATE AND dobto::DATE
	and v."DOSE_ID" = doseid
$BODY$
  LANGUAGE sql VOLATILE
  COST 100
  ROWS 1000;


DELETE FROM "REPORT_PARAMETERS" WHERE "REPORT_ID" = 1;

INSERT INTO "REPORT_PARAMETERS" VALUES(1,'Facility','Facility',NULL,TRUE,2);
INSERT INTO "REPORT_PARAMETERS" VALUES(1,'Dose','Antigen',NULL,TRUE,5);
INSERT INTO "REPORT_PARAMETERS" VALUES(1,'dobfrom','Birthday From',NULL,TRUE,1);
INSERT INTO "REPORT_PARAMETERS" VALUES(1,'dobto','Birthday To',NULL,TRUE,1);
