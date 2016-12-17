DROP VIEW facility_stock_balance;

CREATE OR REPLACE VIEW facility_stock_balance AS
SELECT ct.*, stk.qty as last_month_stock_count FROM crosstab(
	'SELECT id::TEXT||name||itm_name||date, id, name, itm_name, date, reason, qty FROM dvdmt_facility_stock ORDER BY 1, itm_name',
	'SELECT "NAME" FROM "TRANSACTION_TYPE" UNION SELECT "NAME" FROM "ADJUSTMENT_REASON" ORDER BY "NAME"'
)
	as ct (
	row_name TEXT,
	id REAL,
	name TEXT,
	itm_name TEXT,
	date TEXT,
	"Adjustment" REAL,
"Allocation" REAL,
"Arrival" REAL,
"Chanjo imepewa maternity" REAL,
"Chanjo Kuganda" REAL,
"Chanjo Kutoka Wilayani" REAL,
"Dispatch" REAL,
"Imetolewa kimakosa" REAL,
"Kuisha muda wa matumizi" REAL,
"Kutoka kituo kingine" REAL,
"Kwenda kituo kingine" REAL,
"Mabadiliko ya VVM" REAL,
"Receipt" REAL,
"Stock Count" REAL,
"Transfer" REAL,
"Vaccination" REAL,
"Ziliingizwa chache kimakosa" REAL,
"Ziliingizwa zaidi kimakosa" REAL,
"Zilizobaki kwa campaign" REAL,
"Zimemwagwa" REAL,
"Zimepotea" REAL,
"Zimetoka Wilayani" REAL,
"Zimevunjika" REAL


	) 
LEFT JOIN dvdmt_facility_stock stk ON (ct.id = stk.id AND stk.reason = 'Stock Count' AND stk.itm_name = ct.itm_name AND to_char((to_timestamp(ct.date, 'MON-YYYY') - '1 MONTH'::INTERVAL), 'MON-YYYY') = stk.date);


CREATE OR REPLACE VIEW facility_coverage AS
WITH population AS (
         SELECT "CHILD"."CODE",
            "CHILD"."DOB_YEAR",
            "CHILD"."DOB_MONTH",
            count(*) AS population
           FROM ( SELECT "CHILD_1"."ID",
                    "CHILD_1"."SYSTEM_ID",
                    "CHILD_1"."FIRSTNAME1",
                    "CHILD_1"."FIRSTNAME2",
                    "CHILD_1"."LASTNAME1",
                    "CHILD_1"."LASTNAME2",
                    "CHILD_1"."BIRTHDATE",
                    "CHILD_1"."GENDER",
                    "CHILD_1"."HEALTHCENTER_ID",
                    "CHILD_1"."BIRTHPLACE_ID",
                    "CHILD_1"."COMMUNITY_ID",
                    "CHILD_1"."DOMICILE_ID",
                    "CHILD_1"."STATUS_ID",
                    "CHILD_1"."ADDRESS",
                    "CHILD_1"."PHONE",
                    "CHILD_1"."MOBILE",
                    "CHILD_1"."EMAIL",
                    "CHILD_1"."MOTHER_FIRSTNAME",
                    "CHILD_1"."MOTHER_LASTNAME",
                    "CHILD_1"."FATHER_FIRSTNAME",
                    "CHILD_1"."FATHER_LASTNAME",
                    "CHILD_1"."CARETAKER_FIRSTNAME",
                    "CHILD_1"."CARETAKER_LASTNAME",
                    "CHILD_1"."NOTES",
                    "CHILD_1"."IS_ACTIVE",
                    "CHILD_1"."MODIFIED_ON",
                    "CHILD_1"."MODIFIED_BY",
                    "CHILD_1"."BARCODE_ID",
                    "CHILD_1"."TEMP_ID",
                    "HEALTH_FACILITY"."ID",
                    "HEALTH_FACILITY"."NAME",
                    "HEALTH_FACILITY"."CODE",
                    "HEALTH_FACILITY"."PARENT_ID",
                    "HEALTH_FACILITY"."TOP_LEVEL",
                    "HEALTH_FACILITY"."LEAF",
                    "HEALTH_FACILITY"."NOTES",
                    "HEALTH_FACILITY"."IS_ACTIVE",
                    "HEALTH_FACILITY"."MODIFIED_ON",
                    "HEALTH_FACILITY"."MODIFIED_BY",
                    "HEALTH_FACILITY"."VACCINATION_POINT",
                    "HEALTH_FACILITY"."ADDRESS",
                    "HEALTH_FACILITY"."VACCINE_STORE",
                    "HEALTH_FACILITY"."CONTACT",
                    "HEALTH_FACILITY"."COLD_STORAGE_CAPACITY",
                    "HEALTH_FACILITY"."TYPE_ID",
                    "HEALTH_FACILITY"."OWNERSHIP",
                    "HEALTH_FACILITY"."LOWCODE",
                    date_part('year'::text, "CHILD_1"."BIRTHDATE") AS "DOB_YEAR",
                    date_part('month'::text, "CHILD_1"."BIRTHDATE") AS "DOB_MONTH"
                   FROM "CHILD" "CHILD_1"
                     LEFT JOIN "HEALTH_FACILITY" ON "CHILD_1"."HEALTHCENTER_ID" = "HEALTH_FACILITY"."ID") "CHILD"("ID", "SYSTEM_ID", "FIRSTNAME1", "FIRSTNAME2", "LASTNAME1", "LASTNAME2", "BIRTHDATE", "GENDER", "HEALTHCENTER_ID", "BIRTHPLACE_ID", "COMMUNITY_ID", "DOMICILE_ID", "STATUS_ID", "ADDRESS", "PHONE", "MOBILE", "EMAIL", "MOTHER_FIRSTNAME", "MOTHER_LASTNAME", "FATHER_FIRSTNAME", "FATHER_LASTNAME", "CARETAKER_FIRSTNAME", "CARETAKER_LASTNAME", "NOTES", "IS_ACTIVE", "MODIFIED_ON", "MODIFIED_BY", "BARCODE_ID", "TEMP_ID", "ID_1", "NAME", "CODE", "PARENT_ID", "TOP_LEVEL", "LEAF", "NOTES_1", "IS_ACTIVE_1", "MODIFIED_ON_1", "MODIFIED_BY_1", "VACCINATION_POINT", "ADDRESS_1", "VACCINE_STORE", "CONTACT", "COLD_STORAGE_CAPACITY", "TYPE_ID", "OWNERSHIP", "LOWCODE", "DOB_YEAR", "DOB_MONTH")
          GROUP BY "CHILD"."CODE", "CHILD"."DOB_MONTH", "CHILD"."DOB_YEAR"
        ),vaccinations AS (
         SELECT "HEALTH_FACILITY"."ID" AS "HF_ID",
		 "DOSE"."ID" AS "DOSE_ID",
	    "HEALTH_FACILITY"."NAME" AS "HF_NAME",
		"SCHEDULED_VACCINATION"."ID" AS "ANTIGEN_ID",
            "SCHEDULED_VACCINATION"."NAME" AS "ANTIGEN",
            "DOSE"."DOSE_NUMBER",
            ("CHILD"."HEALTHCENTER_ID" = "VACCINATION_EVENT"."HEALTH_FACILITY_ID") AS in_catchment,
            date_part('year'::text, "VACCINATION_EVENT"."VACCINATION_DATE") AS "VACC_YEAR",
            date_part('month'::text, "VACCINATION_EVENT"."VACCINATION_DATE") AS "VACC_MONTH",
            "DOSE"."FULLNAME",
            "VACCINATION_EVENT"."VACCINATION_STATUS",
            "VACCINATION_EVENT"."NONVACCINATION_REASON_ID",
            "HEALTH_FACILITY_COHORT_DATA"."COHORT",
            "HEALTH_FACILITY"."PARENT_ID" AS parent_id
           FROM "VACCINATION_EVENT"
             JOIN "DOSE" ON "VACCINATION_EVENT"."DOSE_ID" = "DOSE"."ID"
             JOIN "SCHEDULED_VACCINATION" ON "DOSE"."SCHEDULED_VACCINATION_ID" = "SCHEDULED_VACCINATION"."ID"
             JOIN "ITEM" ON "ITEM"."ID" = "SCHEDULED_VACCINATION"."ITEM_ID"
             JOIN "HEALTH_FACILITY" ON "VACCINATION_EVENT"."HEALTH_FACILITY_ID" = "HEALTH_FACILITY"."ID"
             JOIN "CHILD" ON "VACCINATION_EVENT"."CHILD_ID" = "CHILD"."ID"
             LEFT JOIN "HEALTH_FACILITY_COHORT_DATA" ON "HEALTH_FACILITY"."ID" = "HEALTH_FACILITY_COHORT_DATA"."HEALTH_FACILITY_ID" AND date_part('year'::text, "VACCINATION_EVENT"."VACCINATION_DATE") = "HEALTH_FACILITY_COHORT_DATA"."YEAR"::double precision
          WHERE "VACCINATION_EVENT"."IS_ACTIVE" = true
        ), coverage AS (
	SELECT 
		vaccinations."FULLNAME" AS "DOSE_NAME",
			vaccinations."DOSE_ID",
		    vaccinations."DOSE_NUMBER",
		    vaccinations."HF_ID",
		    vaccinations."HF_NAME",
		    vaccinations."ANTIGEN_ID",
		    vaccinations.parent_id,
		    vaccinations."ANTIGEN",
		    vaccinations."VACC_MONTH" + vaccinations."VACC_YEAR" * 12 AS sort,
		    UPPER(to_char(to_timestamp (vaccinations."VACC_MONTH"::TEXT, 'MM'), 'TMmon')) || '-' || vaccinations."VACC_YEAR" AS period,
		    vaccinations."VACC_YEAR" as year,
		    vaccinations."VACC_MONTH" as month,
		    SUM(CASE WHEN vaccinations.in_catchment AND vaccinations."VACCINATION_STATUS" THEN 1 ELSE 0 END) as GIVEN_IN_CATCHMENT,
		    SUM(CASE WHEN NOT(vaccinations.in_catchment) AND vaccinations."VACCINATION_STATUS" THEN 1 ELSE 0 END) as GIVEN_OUT_CATCHMENT,
		    count(*) AS target,
		    max(vaccinations."COHORT") / 12 AS cohort ,
		    sum(
			CASE
			    WHEN vaccinations."VACCINATION_STATUS" = true THEN 1
			    ELSE 0
			END) AS given,
		    sum(
			CASE
			    WHEN vaccinations."VACCINATION_STATUS" = false AND vaccinations."NONVACCINATION_REASON_ID" <> 0 THEN 1
			    ELSE 0
			END) AS skipped
		   FROM vaccinations
		  GROUP BY vaccinations."DOSE_ID", vaccinations."FULLNAME", vaccinations."ANTIGEN", vaccinations."ANTIGEN_ID", vaccinations."DOSE_NUMBER", vaccinations."HF_ID", vaccinations."HF_NAME", vaccinations."VACC_YEAR", vaccinations."VACC_MONTH",
		  vaccinations.parent_id
	)
	SELECT 
	*, (coverage.given::FLOAT / coverage.target) AS coverage_pct, CASE WHEN coverage.cohort = 0 THEN -1 ELSE (coverage.given::FLOAT / coverage.cohort::FLOAT) END AS admin_coverage_pct 
	FROM coverage;
