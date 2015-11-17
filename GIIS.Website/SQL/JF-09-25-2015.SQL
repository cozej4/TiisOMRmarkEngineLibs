-- ADD ACTION ID TO THE PARAMETER TABLE
ALTER TABLE "REPORT_PARAMETERS" ADD "ORDER" INTEGER NOT NULL DEFAULT 0;
ALTER TABLE "REPORT_PARAMETER_INPUT_TYPE" ALTER COLUMN "COMPLETE_SOURCE" TYPE TEXT;
ALTER TABLE "REPORT_PARAMETERS" ADD "ACTION_ID" INTEGER NOT NULL DEFAULT 482;
ALTER TABLE "REPORT_PARAMETERS" ALTER COLUMN "ACTION_ID" DROP DEFAULT;
ALTER TABLE "REPORT_PARAMETER_INPUT_TYPE" ADD "DEFAULT" VARCHAR(128); 

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
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (2, 'FACILITY', 'select', 'SELECT "ID","NAME" FROM (WITH RECURSIVE facility(hfid) AS ( SELECT "ID" as hfid, "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = @FacilityId UNION SELECT "ID", facility.hfid FROM facility, "HEALTH_FACILITY" WHERE "HEALTH_FACILITY"."PARENT_ID" = facility.hfid) SELECT "ID", "NAME", "NAME" as SORT FROM "HEALTH_FACILITY" WHERE "ID" IN (SELECT hfid FROM facility) UNION SELECT NULL, ''All'', ''AAAAAAAAA'') AS H ORDER BY sort ', '1', 'System.Int32', 'SELECT @FacilityId');
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (3, 'CHILD', 'select', 'SELECT "ID", "LASTNAME1" || ',' || "FIRSTNAME1" FROM "CHILD" WHERE "HEALTHCENTER_ID" IN (WITH RECURSIVE facility(hfid) AS ( SELECT "ID" as hfid, "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = @FacilityId UNION SELECT "ID", facility.hfid FROM facility, "HEALTH_FACILITY" WHERE "HEALTH_FACILITY"."PARENT_ID" = facility.hfid) SELECT HFID FROM FACILITY)', '1', 'System.Int32', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (4, 'HIDDEN', 'input', 'SELECT @FacilityId', 'hidden', 'System.Int32', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (5, 'DOSE', 'select', 'SELECT "ID","FULLNAME" FROM "DOSE" ORDER BY "FULLNAME"', '1', 'System.Int32', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (6, 'MONTH', 'select', 'SELECT I, TO_CHAR(TO_TIMESTAMP(I::TEXT, ''MM''),''MON'') FROM generate_series(1,12) I', '1', 'System.Int32', 'SELECT EXTRACT(MONTH FROM CURRENT_TIMESTAMP)' );
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (7, 'YEAR', 'select', 'SELECT DISTINCT EXTRACT(YEAR FROM "MODIFIED_ON"), EXTRACT(YEAR FROM "MODIFIED_ON") FROM "CHILD" UNION SELECT MAX(EXTRACT(YEAR FROM "MODIFIED_ON")) + 1, MAX(EXTRACT(YEAR FROM "MODIFIED_ON")) + 1 FROM "CHILD"', '1', 'System.Int32', 'SELECT EXTRACT(YEAR FROM CURRENT_TIMESTAMP)');
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (8, 'STATUS', 'select', 'SELECT I, CASE WHEN I = 1 THEN ''REQUESTED'' WHEN I = 2 THEN ''RELEASED'' WHEN I = 3 THEN ''PACKED'' WHEN I = 4 THEN ''SHIPPED'' WHEN I = 5 THEN ''CANCELLED'' END AS STATUS FROM GENERATE_SERIES(1,5) I', '1', 'System.Int32', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (9, 'FACILITY_GLN', 'select', 'SELECT "CODE","NAME" FROM (WITH RECURSIVE facility(hfid) AS ( SELECT "ID" as hfid, "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = @FacilityId UNION SELECT "ID", facility.hfid FROM facility, "HEALTH_FACILITY" WHERE "HEALTH_FACILITY"."PARENT_ID" = facility.hfid) SELECT "CODE", "NAME", "NAME" as SORT FROM "HEALTH_FACILITY" WHERE "ID" IN (SELECT hfid FROM facility) UNION SELECT NULL, ''All'', ''AAAAAAAAA'') AS H ORDER BY sort ', '1', 'System.Int32', 'SELECT @FacilityId');
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (10, 'MYFACILITY_GLN', 'input', 'SELECT "CODE" FROM "HEALTH_FACILITY" WHERE "ID" = @FacilityId', 'input', 'System.String', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (11, 'VILLAGE', 'select', 'SELECT "ID","NAME" FROM (SELECT p."ID",p."NAME", p."NAME" AS s FROM "PLACE" p WHERE "HEALTH_FACILITY_ID" IN (WITH RECURSIVE facility(hfid) AS ( SELECT "ID" as hfid, "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = @FacilityId UNION SELECT "ID", facility.hfid FROM facility, "HEALTH_FACILITY" WHERE "HEALTH_FACILITY"."PARENT_ID" = facility.hfid) SELECT HFID FROM facility) UNION SELECT NULL, ''All'', ''AAA'') H ORDER BY s', '1', 'System.Int32', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (12, 'BIRTHPLC', 'select', 'SELECT "ID","NAME" FROM (SELECT p."ID",p."NAME", p."NAME" AS s FROM "BIRTHPLACE" p UNION SELECT NULL, ''All'', ''AAA'') H ORDER BY s', '1', 'System.Int32', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (13, 'DISTRICT', 'select', 'SELECT "ID","NAME" FROM (SELECT "ID", "NAME", "NAME" AS S FROM "HEALTH_FACILITY" WHERE "TYPE_ID" = 2 UNION SELECT NULL, ''All'', ''AAAAA'') Q','1','System.Int32', null);
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

-- PARAMETERS
INSERT INTO "REPORT_PARAMETERS" SELECT 2, 'villageId', 'Villiage Name', 'Identifies the village which should be filtered', true, 11, "ID", 2 FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 2, 'dobFrom', 'DOB (From)', 'Identifies the lower bound of the date of birth filter', true, 1, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 2, 'dobTo', 'DOB (To)', 'Identifies the upper bound of the date of birth filter', true, 1, "ID", 5 FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
--INSERT INTO "REPORT_PARAMETERS" SELECT 2, 'birthFacility', 'Birth Facility', 'Identifies the type of birth facility', true, 12, "ID" FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 2, 'healthFacility', 'Health Facility', 'Identifies the current facility for the child', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 2, 'districtId', 'District', 'Identifies the district (nb: facilities are not filtered when this value changes)', true, 13, "ID", 1 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 2, 'regionId', 'Region', 'Identifies the region (nb: district is not filtered when this value changes)', true, 13, "ID", 0 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';

