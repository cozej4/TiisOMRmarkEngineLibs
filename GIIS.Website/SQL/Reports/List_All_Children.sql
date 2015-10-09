-- Function: list_all_children(integer)

-- DROP FUNCTION list_all_children(integer);

CREATE OR REPLACE FUNCTION list_all_children(IN facilityid integer, IN dobfrom text, IN dobto text)
  RETURNS TABLE(id integer, system_id text, firstname1 text, firstname2 text, lastname1 text, lastname2 text, dob date, gender boolean, healthcenter_id integer, birthplace_id integer, community_id integer, domicile_id integer, status_id integer, address text, phone text, mobile text, email text, mother_id text, mother_f text, mother_l text, father_id text, father_f text, father_l text, caretaker_id text, caretaker_f text, caretaker_l text, notes text, is_active boolean, modified_on timestamp without time zone, modified_by integer, id_1 text, id_2 text, id_3 text, barcode text, temp_id text, facility text, village text, gender_human text) AS
$BODY$
SELECT * FROM (WITH RECURSIVE facility(hfid) AS (
	SELECT "ID" as hfid , "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = facilityid
	UNION
	SELECT "ID", facility.hfid FROM facility, "HEALTH_FACILITY" WHERE "HEALTH_FACILITY"."PARENT_ID" = facility.hfid
)
SELECT c.*, h."NAME" AS "FACILITY_NAME", p."NAME" AS "VILLAGE",
CASE c."GENDER" WHEN true THEN 'Male' ELSE 'Female' END as "GENDER_HUMAN"
FROM "CHILD" c INNER JOIN "HEALTH_FACILITY" h ON (c."HEALTHCENTER_ID" = h."ID")
INNER JOIN "PLACE" p ON (c."DOMICILE_ID" = p."ID")
WHERE c."HEALTHCENTER_ID" IN (SELECT hfid FROM facility)
AND c."BIRTHDATE" BETWEEN dobfrom::DATE AND dobto::DATE
ORDER BY "VILLAGE") AS FOO

$BODY$
  LANGUAGE sql VOLATILE
  COST 100
  ROWS 1000;
  
DELETE FROM "REPORT_PARAMETERS" WHERE "REPORT_ID" = 2;
INSERT INTO "REPORT_PARAMETERS" VALUES(2,'facilityId','Facility',NULL,TRUE,2);
INSERT INTO "REPORT_PARAMETERS" VALUES(2,'dobfrom','Birthday From',NULL,TRUE,1);
INSERT INTO "REPORT_PARAMETERS" VALUES(2,'dobto','Birthday To',NULL,TRUE,1);