-- DVDMT REPORT

CREATE OR REPLACE FUNCTION dvdmt_report(
    IN month integer,
    IN year integer,
	IN facilityId integer,
	IN regionId integer,
	IN districtId integer)
  RETURNS TABLE(hf_name text, bcg_011 bigint, bcg_12 bigint, opv1_0 bigint, opv1_011 bigint, opv1_12 bigint, opv2_011 bigint, opv2_12 bigint, opv3_011 bigint, opv3_12 bigint, dtp1_011 bigint, dtp1_12 bigint, dtp2_011 bigint, dtp2_12 bigint, dtp3_011 bigint, dtp3_12 bigint, pvc1_011 bigint, pvc1_12 bigint, pvc2_011 bigint, pvc2_12 bigint, pvc3_011 bigint, pvc3_12 bigint, rota1_011 bigint, rota1_12 bigint, rota2_011 bigint, rota2_12 bigint, rota3_011 bigint, rota3_12 bigint, measles1_011 bigint, measles1_12 bigint, measles2_018 bigint, measles2_18 bigint, tt1 bigint, tt2 bigint, tt3 bigint, tt4 bigint, tt5 bigint, bcg_r real, bcg_b real, bcg_s real, bcg_vvm real, bcg_fr real, bcg_exp real, bcg_o real, opv_r real, opv_b real, opv_s real, opv_vvm real, opv_fr real, opv_exp real, opv_o real, dtp_r real, dtp_b real, dtp_s real, dtp_vvm real, dtp_fr real, dtp_exp real, dtp_o real, pvc_r real, pvc_b real, pvc_s real, pvc_vvm real, pvc_fr real, pvc_exp real, pvc_o real, tet_r real, tet_b real, tet_s real, tet_vvm real, tet_fr real, tet_exp real, tet_o real, rot_r real, rot_b real, rot_s real, rot_vvm real, rot_fr real, rot_exp real, rot_o real, msl_r real, msl_b real, msl_s real, msl_vvm real, msl_fr real, msl_exp real, msl_o real) AS
$BODY$
WITH RECURSIVE facility(hfid) AS (
	SELECT "ID" as hfid , "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = COALESCE(facilityId,districtId,regionId)
	UNION
	SELECT "ID", facility.hfid FROM facility, "HEALTH_FACILITY" WHERE "HEALTH_FACILITY"."PARENT_ID" = facility.hfid
	)
SELECT h."NAME",
--BCG
(SELECT dvdmt_dose_under_months(61,$1,$2,h."ID",330)),
(SELECT dvdmt_dose_over_months(61,$1,$2,h."ID",330)),
--OPV0
(SELECT dvdmt_dose_under_months(62,$1,$2,h."ID",1)),
--OPV1
(SELECT dvdmt_dose_under_months(63,$1,$2,h."ID",330)),
(SELECT dvdmt_dose_over_months(63,$1,$2,h."ID",330)),
--OPV2
(SELECT dvdmt_dose_under_months(64,$1,$2,h."ID",330)),
(SELECT dvdmt_dose_over_months(64,$1,$2,h."ID",330)),
--OPV3
(SELECT dvdmt_dose_under_months(65,$1,$2,h."ID",330)),
(SELECT dvdmt_dose_over_months(65,$1,$2,h."ID",330)),
--DTP1
(SELECT dvdmt_dose_under_months(66,$1,$2,h."ID",330)),
(SELECT dvdmt_dose_over_months(66,$1,$2,h."ID",330)),
--DTP2
(SELECT dvdmt_dose_under_months(67,$1,$2,h."ID",330)),
(SELECT dvdmt_dose_over_months(67,$1,$2,h."ID",330)),
--DTP3
(SELECT dvdmt_dose_under_months(68,$1,$2,h."ID",330)),
(SELECT dvdmt_dose_over_months(68,$1,$2,h."ID",330)),
--PVC1
(SELECT dvdmt_dose_under_months(74,$1,$2,h."ID",330)),
(SELECT dvdmt_dose_over_months(74,$1,$2,h."ID",330)),
--PVC2
(SELECT dvdmt_dose_under_months(75,$1,$2,h."ID",330)),
(SELECT dvdmt_dose_over_months(75,$1,$2,h."ID",330)),
--PVC3
(SELECT dvdmt_dose_under_months(76,$1,$2,h."ID",330)),
(SELECT dvdmt_dose_over_months(76,$1,$2,h."ID",330)),
--ROTA1
(SELECT dvdmt_dose_under_months(69,$1,$2,h."ID",330)),
(SELECT dvdmt_dose_over_months(69,$1,$2,h."ID",330)),
--ROTA2
(SELECT dvdmt_dose_under_months(70,$1,$2,h."ID",330)),
(SELECT dvdmt_dose_over_months(70,$1,$2,h."ID",330)),
--ROTA3
(SELECT dvdmt_dose_under_months(71,$1,$2,h."ID",330)),
(SELECT dvdmt_dose_over_months(71,$1,$2,h."ID",330)),
--MSL1
(SELECT dvdmt_dose_under_months(77,$1,$2,h."ID",330)),
(SELECT dvdmt_dose_over_months(77,$1,$2,h."ID",330)),
--MSL2
(SELECT dvdmt_dose_under_months(77,$1,$2,h."ID",540)),
(SELECT dvdmt_dose_over_months(77,$1,$2,h."ID",540)),
--TT1
(SELECT dvdmt_dose_under_months((SELECT "ID" FROM "DOSE" WHERE "FULLNAME" = 'TT1'),$1,$2,h."ID",540)),
(SELECT dvdmt_dose_under_months((SELECT "ID" FROM "DOSE" WHERE "FULLNAME" = 'TT2'),$1,$2,h."ID",540)),
(SELECT dvdmt_dose_under_months((SELECT "ID" FROM "DOSE" WHERE "FULLNAME" = 'TT3'),$1,$2,h."ID",540)),
(SELECT dvdmt_dose_under_months((SELECT "ID" FROM "DOSE" WHERE "FULLNAME" = 'TT4'),$1,$2,h."ID",540)),
(SELECT dvdmt_dose_under_months((SELECT "ID" FROM "DOSE" WHERE "FULLNAME" = 'TT5'),$1,$2,h."ID",540)),

--------------
--STOCK-------
--------------
--ADJUSTMENTS
--25 EXPIRED
--32 VVM
--99 FROZEN

--BCG
(SELECT dvdmt_item_received(42,h."CODE")),
(SELECT dvdmt_item_balance(42,h."CODE")),
(SELECT dvdmt_item_stock(42,h."CODE")),
(SELECT dvdmt_item_adjustment(42,h."CODE",32)),
(SELECT dvdmt_item_adjustment(42,h."CODE",99)),
(SELECT dvdmt_item_adjustment(42,h."CODE",25)),
(SELECT dvdmt_item_adjustment(42,h."CODE",24)),

--OPV
(SELECT dvdmt_item_received(44,h."CODE")),
(SELECT dvdmt_item_balance(44,h."CODE")),
(SELECT dvdmt_item_stock(44,h."CODE")),
(SELECT dvdmt_item_adjustment(44,h."CODE",32)),
(SELECT dvdmt_item_adjustment(44,h."CODE",99)),
(SELECT dvdmt_item_adjustment(44,h."CODE",25)),
(SELECT dvdmt_item_adjustment(44,h."CODE",24)),

--DTP
(SELECT dvdmt_item_received(45,h."CODE")),
(SELECT dvdmt_item_balance(45,h."CODE")),
(SELECT dvdmt_item_stock(45,h."CODE")),
(SELECT dvdmt_item_adjustment(45,h."CODE",32)),
(SELECT dvdmt_item_adjustment(45,h."CODE",99)),
(SELECT dvdmt_item_adjustment(45,h."CODE",25)),
(SELECT dvdmt_item_adjustment(45,h."CODE",24)),

--PCV
(SELECT dvdmt_item_received(60,h."CODE")),
(SELECT dvdmt_item_balance(60,h."CODE")),
(SELECT dvdmt_item_stock(60,h."CODE")),
(SELECT dvdmt_item_adjustment(60,h."CODE",32)),
(SELECT dvdmt_item_adjustment(60,h."CODE",99)),
(SELECT dvdmt_item_adjustment(60,h."CODE",25)),
(SELECT dvdmt_item_adjustment(60,h."CODE",24)),

--TETANUS
(SELECT dvdmt_item_received(59,h."CODE")),
(SELECT dvdmt_item_balance(59,h."CODE")),
(SELECT dvdmt_item_stock(59,h."CODE")),
(SELECT dvdmt_item_adjustment(59,h."CODE",32)),
(SELECT dvdmt_item_adjustment(59,h."CODE",99)),
(SELECT dvdmt_item_adjustment(59,h."CODE",25)),
(SELECT dvdmt_item_adjustment(59,h."CODE",24)),

--ROTA
(SELECT dvdmt_item_received(48,h."CODE")),
(SELECT dvdmt_item_balance(48,h."CODE")),
(SELECT dvdmt_item_stock(48,h."CODE")),
(SELECT dvdmt_item_adjustment(48,h."CODE",32)),
(SELECT dvdmt_item_adjustment(48,h."CODE",99)),
(SELECT dvdmt_item_adjustment(48,h."CODE",25)),
(SELECT dvdmt_item_adjustment(48,h."CODE",24)),

--MEASLES
(SELECT dvdmt_item_received(61,h."CODE")),
(SELECT dvdmt_item_balance(61,h."CODE")),
(SELECT dvdmt_item_stock(61,h."CODE")),
(SELECT dvdmt_item_adjustment(61,h."CODE",32)),
(SELECT dvdmt_item_adjustment(61,h."CODE",99)),
(SELECT dvdmt_item_adjustment(61,h."CODE",25)),
(SELECT dvdmt_item_adjustment(61,h."CODE",24))

FROM "HEALTH_FACILITY" h
JOIN "VACCINATION_EVENT" v ON h."ID" = v."HEALTH_FACILITY_ID"
WHERE h."ID" IN (SELECT hfid FROM facility)
GROUP BY h."NAME", h."ID"
ORDER BY h."NAME";
$BODY$
  LANGUAGE sql VOLATILE;

-- CROSSTAB FOR VACCINATION DATES
CREATE VIEW child_vaccination_date_crosstab AS
SELECT * FROM crosstab(
	'SELECT c."ID", c."LASTNAME1", c."FIRSTNAME1", c."HEALTHCENTER_ID", d."FULLNAME", CASE WHEN v."VACCINATION_STATUS" THEN v."VACCINATION_DATE" ELSE NULL END AS vacc_date FROM "VACCINATION_EVENT" v INNER JOIN "CHILD" c ON (v."CHILD_ID" = c."ID") INNER JOIN "DOSE" d ON (v."DOSE_ID" = d."ID") ORDER BY 1, d."FULLNAME"',
	'SELECT "FULLNAME" FROM "DOSE" ORDER BY "FULLNAME"'
)
	as (
		"ID" INT, 
		"LASTNAME1" TEXT, 
		"FIRSTNAME1" TEXT, 
		"HEALTHCENTER_ID" INT,
		"BCG" DATE,
		"DTP-HepB-Hib 1" DATE,
		"DTP-HepB-Hib 2" DATE,
		"DTP-HepB-Hib 3" DATE,
		"Measles 1" DATE,
		"Measles 2" DATE,
		"Measles Rubella 1" DATE,
		"Measles Rubella 2" DATE,
		"OPV 1" DATE,
		"OPV 2" DATE,
		"OPV 3" DATE,
		"OPV0" DATE,
		"PCV 1" DATE,
		"PCV 2" DATE,
		"PCV 3" DATE,
		"Rota 1" DATE,
		"Rota 2" DATE
	);
	
ALTER TABLE "WEIGHT_TALLY" ADD EBF INT;
ALTER TABLE "WEIGHT_TALLY" ADD RF INT;


CREATE OR REPLACE FUNCTION insert_new_weight(
    hf_in integer,
    date_in date,
    age_group_in integer,
    m_gt_80_in integer,
    m_60_80_in integer,
    m_lt_60_in integer,
	m_rf_in integer,
	m_ebf_in integer,
    f_gt_80_in integer,
    f_60_80_in integer,
    f_lt_60_in integer,
	f_rf_in integer,
	f_ebf_in integer
) RETURNS void AS
$BODY$
BEGIN
	INSERT INTO "WEIGHT_TALLY" 
		(HEALTHFACILITY_ID, ENTRY_DATE, OVERWEIGHT, NORMALWEIGHT, UNDERWEIGHT, AGE_ID, GENDER, EBF, RF)
		VALUES
		(HF_IN, DATE_IN, M_GT_80_IN, M_60_80_IN, M_LT_60_IN, AGE_GROUP_IN, 'M', M_EBF_IN, M_RF_IN);
	INSERT INTO "WEIGHT_TALLY" 
		(HEALTHFACILITY_ID, ENTRY_DATE, OVERWEIGHT, NORMALWEIGHT, UNDERWEIGHT, AGE_ID, GENDER, EBF, RF)
		VALUES
		(HF_IN, DATE_IN, F_GT_80_IN, F_60_80_IN, F_LT_60_IN, AGE_GROUP_IN,'F', F_EBF_IN, F_RF_IN);
	RETURN;		 
END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION insert_new_weight(integer, date, character varying, integer, integer, integer, integer, integer, integer)
  OWNER TO postgres;

INSERT INTO "DOSE" ("SCHEDULED_VACCINATION_ID", "AGE_DEFINITION_ID", "DOSE_NUMBER", "FULLNAME", "IS_ACTIVE", "MODIFIED_ON", "MODIFIED_BY") VALUES (41, 31, 1, 'TT1', FALSE, CURRENT_TIMESTAMP, 1);

-- ADD ACTION ID TO THE PARAMETER TABLE
ALTER TABLE "REPORT_PARAMETER_INPUT_TYPE" ALTER COLUMN "COMPLETE_SOURCE" TYPE TEXT;
ALTER TABLE "REPORT_PARAMETERS" ADD "ACTION_ID" INTEGER NOT NULL DEFAULT 482;
ALTER TABLE "REPORT_PARAMETERS" ALTER COLUMN "ACTION_ID" DROP DEFAULT;
ALTER TABLE "REPORT_PARAMETER_INPUT_TYPE" ADD "DEFAULT" VARCHAR(128); 
ALTER TABLE "REPORT_PARAMETERS" ADD "ORDER" INTEGER NOT NULL DEFAULT 0;

INSERT INTO "ACTIONS" ("NAME", "NOTES") VALUES ('VaccinatorReportParms', 'Vaccinator Report Parameters');
INSERT INTO "ACTIONS" ("NAME", "NOTES") VALUES ('DistrictReportParms', 'District Report Parameters');
INSERT INTO "ACTIONS" ("NAME", "NOTES") VALUES ('RegionalReportParms', 'Regional Report Parameters');
INSERT INTO "ACTIONS" ("NAME", "NOTES") VALUES ('NationalReportParms', 'National Report Parameters');

INSERT INTO "ROLE_ACTION" ("ROLE_ID", "ACTION_ID", "IS_ACTIVE") SELECT A."ID", B."ID", TRUE FROM "ACTIONS" B, "ROLE" A WHERE A."NAME" = 'Vaccinator' AND B."NAME" = 'VaccinatorReportParms';
INSERT INTO "ROLE_ACTION" ("ROLE_ID", "ACTION_ID", "IS_ACTIVE") SELECT A."ID", B."ID", TRUE FROM "ACTIONS" B, "ROLE" A WHERE A."NAME" = 'Middle Level Officer' AND B."NAME" IN('VaccinatorReportParms', 'DistrictReportParms');
INSERT INTO "ROLE_ACTION" ("ROLE_ID", "ACTION_ID", "IS_ACTIVE") SELECT A."ID", B."ID", TRUE FROM "ACTIONS" B, "ROLE" A WHERE A."NAME" = 'National Level Officer' AND B."NAME" IN('VaccinatorReportParms', 'DistrictReportParms', 'RegionalReportParms', 'NationalReportParms');
INSERT INTO "ROLE_ACTION" ("ROLE_ID", "ACTION_ID", "IS_ACTIVE") SELECT A."ID", B."ID", TRUE FROM "ACTIONS" B, "ROLE" A WHERE A."NAME" = 'Administrator' AND B."NAME" IN('VaccinatorReportParms', 'DistrictReportParms', 'RegionalReportParms', 'NationalReportParms');

-- TRUNCATE THE REPORTS/PARAMETERS TABLE AS THIS IS AN UPDATE
TRUNCATE TABLE "REPORT" CASCADE;
TRUNCATE TABLE "REPORT_PARAMETER_INPUT_TYPE" CASCADE;

-- PARAMETERS AS DEFINED IN THE SPREADSHEET
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (1, 'DATE', 'input', NULL, 'text', 'System.DateTime', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (2, 'FACILITY', 'select', 'SELECT "ID","NAME" FROM (WITH RECURSIVE facility(hfid) AS ( SELECT "ID" as hfid, "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = @FacilityId UNION SELECT "ID", facility.hfid FROM facility, "HEALTH_FACILITY" WHERE "HEALTH_FACILITY"."PARENT_ID" = facility.hfid) SELECT "ID", "NAME", "NAME" as SORT FROM "HEALTH_FACILITY" WHERE "ID" IN (SELECT hfid FROM facility) AND "LEAF" = TRUE UNION SELECT NULL, ''All'', ''AAAAAAAAA'') AS H ORDER BY sort ', '1', 'System.Int32', 'SELECT @FacilityId');
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (3, 'CHILD', 'select', 'SELECT "ID", "LASTNAME1" || '','' || "FIRSTNAME1" FROM "CHILD" WHERE "HEALTHCENTER_ID" IN (WITH RECURSIVE facility(hfid) AS ( SELECT "ID" as hfid, "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = @FacilityId UNION SELECT "ID", facility.hfid FROM facility, "HEALTH_FACILITY" WHERE "HEALTH_FACILITY"."PARENT_ID" = facility.hfid) SELECT HFID FROM FACILITY)', '1', 'System.Int32', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (4, 'HIDDEN', 'input', 'SELECT @FacilityId', 'hidden', 'System.Int32', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (5, 'DOSE', 'select', 'SELECT "ID","FULLNAME" FROM "DOSE" ORDER BY "FULLNAME"', '1', 'System.Int32', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (6, 'MONTH', 'select', 'SELECT I, TO_CHAR(TO_TIMESTAMP(I::TEXT, ''MM''),''MON'') FROM generate_series(1,12) I', '1', 'System.Int32', 'SELECT EXTRACT(MONTH FROM CURRENT_TIMESTAMP)' );
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (7, 'YEAR', 'select', 'SELECT DISTINCT EXTRACT(YEAR FROM "MODIFIED_ON"), EXTRACT(YEAR FROM "MODIFIED_ON") FROM "CHILD" WHERE EXTRACT(YEAR FROM "MODIFIED_ON") > 1990 UNION SELECT MAX(EXTRACT(YEAR FROM "MODIFIED_ON")) + 1, MAX(EXTRACT(YEAR FROM "MODIFIED_ON")) + 1 FROM "CHILD"', '1', 'System.Int32', 'SELECT EXTRACT(YEAR FROM CURRENT_TIMESTAMP)');
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (8, 'STATUS', 'select', 'SELECT I, CASE WHEN I = 1 THEN ''REQUESTED'' WHEN I = 2 THEN ''RELEASED'' WHEN I = 3 THEN ''PACKED'' WHEN I = 4 THEN ''SHIPPED'' WHEN I = 5 THEN ''CANCELLED'' END AS STATUS FROM GENERATE_SERIES(1,5) I', '1', 'System.Int32', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (9, 'FACILITY_GLN', 'select', 'SELECT "CODE","NAME" FROM (WITH RECURSIVE facility(hfid) AS ( SELECT "ID" as hfid, "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = @FacilityId UNION SELECT "ID", facility.hfid FROM facility, "HEALTH_FACILITY" WHERE "HEALTH_FACILITY"."PARENT_ID" = facility.hfid) SELECT "CODE", "NAME", "NAME" as SORT FROM "HEALTH_FACILITY" WHERE "ID" IN (SELECT hfid FROM facility) UNION SELECT NULL, ''All'', ''AAAAAAAAA'') AS H ORDER BY sort ', '1', 'System.Int32', 'SELECT @FacilityId');
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (10, 'MYFACILITY_GLN', 'input', 'SELECT "CODE" FROM "HEALTH_FACILITY" WHERE "ID" = @FacilityId', 'input', 'System.String', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (11, 'VILLAGE', 'select', 'SELECT "ID","NAME" FROM (SELECT p."ID",p."NAME", p."NAME" AS s FROM "PLACE" p WHERE "HEALTH_FACILITY_ID" IN (WITH RECURSIVE facility(hfid) AS ( SELECT "ID" as hfid, "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = @FacilityId UNION SELECT "ID", facility.hfid FROM facility, "HEALTH_FACILITY" WHERE "HEALTH_FACILITY"."PARENT_ID" = facility.hfid) SELECT HFID FROM facility) UNION SELECT NULL, ''All'', ''AAA'') H ORDER BY s', '1', 'System.Int32', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (12, 'BIRTHPLC', 'select', 'SELECT "ID","NAME" FROM (SELECT p."ID",p."NAME", p."NAME" AS s FROM "BIRTHPLACE" p UNION SELECT NULL, ''All'', ''AAA'') H ORDER BY s', '1', 'System.Int32', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (13, 'DISTRICT', 'select', 'SELECT "ID","NAME" FROM (SELECT "ID", "NAME", "NAME" AS S FROM "HEALTH_FACILITY" WHERE "TYPE_ID" = 2 UNION SELECT NULL, ''All'', ''AAAAA'') Q','1','System.Int32', 'SELECT CASE WHEN "TYPE_ID" = 2 THEN "ID" ELSE "PARENT_ID" END AS "ID" FROM "HEALTH_FACILITY" WHERE "ID" = @FacilityId');
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (14, 'REGION', 'select', 'SELECT "ID","NAME" FROM (SELECT "ID", "NAME", "NAME" AS S FROM "HEALTH_FACILITY" WHERE "TYPE_ID" = 1 UNION SELECT NULL, ''All'', ''AAAAA'') Q','1','System.Int32', null);

-- REPORTS
INSERT INTO "REPORT" VALUES (1, 'Adverse_Effects', 'Non-Vaccinations', 482, 2, 'A list of non-vaccination events due to adverse reactions, refusal, etc.', 'R');
INSERT INTO "REPORT" VALUES (2, 'All_Children', 'All Children', 482, 2, 'A list of all children by facility, district, etc.', 'R');
INSERT INTO "REPORT" VALUES (4, 'Defaulter_Report', 'Defaulters', 482, 2, NULL, 'R');
INSERT INTO "REPORT" VALUES (6, 'Child_Vaccinations', 'Child Vaccinations', 482, 2, NULL, 'R');

-- MONTHLY REPORTS
INSERT INTO "REPORT" VALUES (8, 'DVDMT_Report', 'DVDMT Worksheet', 481, 3, NULL, 'R');
INSERT INTO "REPORT" VALUES (14, 'drp_divo_overview', 'Forecasted Consumption', 481, 3, NULL, 'R');
INSERT INTO "REPORT" VALUES (15, 'Facility_Coverage', 'Facility Coverage/Performance', 481, 3, NULL, 'R');

-- VACCINATIONS
INSERT INTO "REPORT" VALUES (5, 'Vaccines_Administered', 'Health Facility Consumption', 481, 1, NULL, 'R');
INSERT INTO "REPORT" VALUES (9, 'Facility_Balance_Report', 'Facility Balances', 481, 1, NULL, 'R');
INSERT INTO "REPORT" VALUES (13, 'Pack_Order_Report', 'Facility Pick/Pack Report', 481, 1, NULL, 'R');

-- FORMS
INSERT INTO "REPORT" VALUES (11, 'Vaccine_Report_Barcode', 'Monthly Register', 481, 4, NULL, 'F');
INSERT INTO "REPORT" VALUES (12, 'Vaccine_Report_Temp', 'Monthly Register (Temp Id)', 481, 4, NULL, 'F');
INSERT INTO "REPORT" VALUES (16, 'New_Vaccination_Register', 'Monthly Register (v9)', 481, 4, 'Prints a list of children due for vaccines at a particular facility', 'F');

-- PARAMETERS
INSERT INTO "REPORT_PARAMETERS" SELECT 2, 'villageId', 'Villiage Name', 'Identifies the village which should be filtered', true, 11, "ID", 2 FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 2, 'dobFrom', 'DOB (From)', 'Identifies the lower bound of the date of birth filter', true, 1, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 2, 'dobTo', 'DOB (To)', 'Identifies the upper bound of the date of birth filter', true, 1, "ID", 5 FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
--INSERT INTO "REPORT_PARAMETERS" SELECT 2, 'birthFacility', 'Birth Facility', 'Identifies the type of birth facility', true, 12, "ID" FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 2, 'healthFacility', 'Health Facility', 'Identifies the current facility for the child', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 2, 'districtId', 'District', 'Identifies the district (nb: facilities are not filtered when this value changes)', true, 13, "ID", 1 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 2, 'regionId', 'Region', 'Identifies the region (nb: district is not filtered when this value changes)', true, 14, "ID", 0 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';

INSERT INTO "REPORT_PARAMETERS" SELECT 6, 'dobFrom', 'DOB (From)', 'Identifies the lower bound of the date of birth filter', true, 1, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 6, 'dobTo', 'DOB (To)', 'Identifies the upper bound of the date of birth filter', true, 1, "ID", 5 FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 6, 'facilityId', 'Health Facility', 'Identifies the current facility for the child', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 6, 'districtId', 'District', 'Identifies the district (nb: facilities are not filtered when this value changes)', true, 13, "ID", 1 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 6, 'regionId', 'Region', 'Identifies the region (nb: district is not filtered when this value changes)', true, 14, "ID", 0 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';

INSERT INTO "REPORT_PARAMETERS" SELECT 8, 'Month', 'Month', 'Identifies the month which should be generated for the DVDMT', true, 6, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 8, 'Year', 'Year', 'Identifies the year which should be generated for the DVDMT', true, 7, "ID", 5 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 8, 'healthFacility', 'Health Facility', 'Identifies the current facility for the child', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 8, 'districtId', 'District', 'Identifies the district (nb: facilities are not filtered when this value changes)', true, 13, "ID", 1 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 8, 'regionId', 'Region', 'Identifies the region (nb: district is not filtered when this value changes)', true, 14, "ID", 0 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';


INSERT INTO "REPORT_PARAMETERS" SELECT 11, 'Month', 'Month', 'Identifies the month which should be generated for the DVDMT', true, 6, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 11, 'Year', 'Year', 'Identifies the year which should be generated for the DVDMT', true, 7, "ID", 5 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 11, 'healthFacility', 'Health Facility', 'Identifies the current facility for the child', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';

INSERT INTO "REPORT_PARAMETERS" SELECT 12, 'Month', 'Month', 'Identifies the month which should be generated for the DVDMT', true, 6, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 12, 'Year', 'Year', 'Identifies the year which should be generated for the DVDMT', true, 7, "ID", 5 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 12, 'healthFacility', 'Health Facility', 'Identifies the current facility for the child', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';

INSERT INTO "REPORT_PARAMETERS" SELECT 16, 'month', 'Month', 'Identifies the month which should be generated for the DVDMT', true, 6, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 16, 'year', 'Year', 'Identifies the year which should be generated for the DVDMT', true, 7, "ID", 5 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 16, 'facilityId', 'Health Facility', 'Identifies the current facility for the child', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
