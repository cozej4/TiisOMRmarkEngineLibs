-- Function: child_vaccinations(integer)

-- DROP FUNCTION child_vaccinations(integer);

CREATE OR REPLACE FUNCTION child_vaccinations(IN childid integer, IN antigenId integer, IN dobfrom text, IN dobto text)
  RETURNS TABLE(id integer, system_id text, firstname text, secondname text, lastname text, dob date, gender text, vaccine text, v_date date, v_status boolean, reason text, age text) AS
$BODY$
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
	WHERE c."ID" = childId
	AND d."ID" = antigenId
	AND c."BIRTHDATE" BETWEEN dobfrom::DATE AND dobto::DATE
$BODY$
  LANGUAGE sql VOLATILE
  COST 100
  ROWS 1000;

DELETE FROM "REPORT_PARAMETERS" WHERE "REPORT_ID" = 6;

INSERT INTO "REPORT_PARAMETERS" VALUES(6,'ChildId','Child',NULL,TRUE,3);
INSERT INTO "REPORT_PARAMETERS" VALUES(6,'AntigenId','Antigen',NULL,TRUE,5);
INSERT INTO "REPORT_PARAMETERS" VALUES(6,'dobfrom','Birthday From',NULL,TRUE,1);
INSERT INTO "REPORT_PARAMETERS" VALUES(6,'dobto','Birthday To',NULL,TRUE,1);