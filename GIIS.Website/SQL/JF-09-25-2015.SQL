-- DVDMT REPORT

CREATE OR REPLACE VIEW dvdmt_facility_dose_age_group_given
AS 
	WITH facility_dose_age AS (
	SELECT f."ID" AS id, f."NAME" AS name, v."VACCINATION_DATE" vaccine_date, d."FULLNAME" AS dose_name, d."ID" as dose_id, (v."VACCINATION_DATE"::TIMESTAMP - c."BIRTHDATE"::TIMESTAMP)::INTERVAL AS age_at_vaccination,
		v."VACCINATION_STATUS" as given
		FROM "VACCINATION_EVENT" v 
		INNER JOIN "HEALTH_FACILITY" f ON (v."HEALTH_FACILITY_ID" = f."ID") 
		INNER JOIN "DOSE" d ON (v."DOSE_ID" = d."ID")
		INNER JOIN "CHILD" c ON (c."ID" = v."CHILD_ID")
		ORDER BY 1, d."FULLNAME" 
	), facility_dose_age_group AS(
	SELECT id, name, dose_name, dose_id, to_char(vaccine_date, 'MON-YYYY') vaccine_date,
		CASE WHEN age_at_vaccination = '0 DAYS' THEN '0'
		WHEN age_at_vaccination < '12 months'::INTERVAL THEN '0-11'
		WHEN age_at_vaccination < '18 months'::INTERVAL THEN '12-17'
		ELSE '18+' END AS age_group, given FROM facility_dose_age 
	) SELECT id, name, dose_id, dose_name, vaccine_date, age_group, SUM(CASE WHEN given THEN 1 ELSE 0 END) AS given FROM facility_dose_age_group
	GROUP BY id, name, dose_id, dose_name, vaccine_date, age_group
	ORDER BY name;


CREATE OR REPLACE VIEW dvdmt_facility_dose_age_group_crosstab AS
SELECT * FROM crosstab(
	'SELECT id::TEXT||vaccine_date, id, name, vaccine_date, dose_name || ''-'' || age_group, given FROM  dvdmt_facility_dose_age_group_given ORDER BY 1, dose_name',
	'SELECT "FULLNAME" || ''-'' || ag FROM "DOSE" CROSS JOIN unnest(ARRAY[''0'',''0-11'',''12-17'',''18+'']) AS ag ORDER BY "FULLNAME"'
)
	as (
		row_name TEXT,
		id INT, 
		name TEXT, 
		vaccine_date TEXT,
		"BCG-0" INT,
		"BCG-0-11" INT,
		"BCG-12-17" INT,
		"BCG-18+" INT,
		"DTP-HepB-Hib 1-0" INT,
		"DTP-HepB-Hib 1-0-11" INT,
		"DTP-HepB-Hib 1-12-17" INT,
		"DTP-HepB-Hib 1-18+" INT,
		"DTP-HepB-Hib 2-0" INT,
		"DTP-HepB-Hib 2-0-11" INT,
		"DTP-HepB-Hib 2-12-17" INT,
		"DTP-HepB-Hib 2-18+" INT,
		"DTP-HepB-Hib 3-0" INT,
		"DTP-HepB-Hib 3-0-11" INT,
		"DTP-HepB-Hib 3-12-17" INT,
		"DTP-HepB-Hib 3-18+" INT,
		"Measles 1-0" INT,
		"Measles 1-0-11" INT,
		"Measles 1-12-17" INT,
		"Measles 1-18+" INT,
		"Measles 2-0" INT,
		"Measles 2-0-11" INT,
		"Measles 2-12-17" INT,
		"Measles 2-18+" INT,
		"Measles Rubella 1-0" INT,
		"Measles Rubella 1-0-11" INT,
		"Measles Rubella 1-12-17" INT,
		"Measles Rubella 1-18+" INT,
		"Measles Rubella 2-0" INT,
		"Measles Rubella 2-0-11" INT,
		"Measles Rubella 2-12-17" INT,
		"Measles Rubella 2-18+" INT,
		"OPV0-0" INT,
		"OPV0-0-11" INT,
		"OPV0-12-17" INT,
		"OPV0-18+" INT,
		"OPV 1-0" INT,
		"OPV 1-0-11" INT,
		"OPV 1-12-17" INT,
		"OPV 1-18+" INT,
		"OPV 2-0" INT,
		"OPV 2-0-11" INT,
		"OPV 2-12-17" INT,
		"OPV 2-18+" INT,
		"OPV 3-0" INT,
		"OPV 3-0-11" INT,
		"OPV 3-12-17" INT,
		"OPV 3-18+" INT,
		"PCV 1-0" INT,
		"PCV 1-0-11" INT,
		"PCV 1-12-17" INT,
		"PCV 1-18+" INT,
		"PCV 2-0" INT,
		"PCV 2-0-11" INT,
		"PCV 2-12-17" INT,
		"PCV 2-18+" INT,
		"PCV 3-0" INT,
		"PCV 3-0-11" INT,
		"PCV 3-12-17" INT,
		"PCV 3-18+" INT,
		"Rota 1-0" INT,
		"Rota 1-0-11" INT,
		"Rota 1-12-17" INT,
		"Rota 1-18+" INT,
		"Rota 2-0" INT,
		"Rota 2-0-11" INT,
		"Rota 2-12-17" INT,
		"Rota 2-18+" INT,
		"TT1-0" INT,
		"TT1-0-11" INT,
		"TT1-12-17" INT,
		"TT1-18+" INT,
		"TT2-0" INT,
		"TT2-0-11" INT,
		"TT2-12-17" INT,
		"TT2-18+" INT,
		"TT3-0" INT,
		"TT3-0-11" INT,
		"TT3-12-17" INT,
		"TT3-18+" INT,
		"TT4-0" INT,
		"TT4-0-11" INT,
		"TT4-12-17" INT,
		"TT4-18+" INT,
		"TT5-0" INT,
		"TT5-0-11" INT,
		"TT5-12-17" INT,
		"TT5-18+" INT
	);
	
CREATE OR REPLACE VIEW dvdmt_facility_stock_source AS
SELECT id, gtin, lot, expiry, adjustment_reason, transaction_name, name, date, itm_name, sum(qty) as qty FROM 
(
WITH item_transaction_dtl AS (
SELECT 
	to_char("TRANSACTION_DATE", 'MON-YYYY') as date, 
	"HEALTH_FACILITY_CODE" as hf_code, 
	it."GTIN" as gtin, 
	"GTIN_LOT" as lot, 
	il."EXPIRE_DATE" as expiry,
	tt."NAME" as transaction_name, 
	ar."NAME" as adjustment_reason, 
	"TRANSACTION_QTY_IN_BASE_UOM" AS qty
	FROM "ITEM_TRANSACTION" it 
		INNER JOIN "TRANSACTION_TYPE" tt ON (it."TRANSACTION_TYPE_ID" = tt."ID") 
		INNER JOIN "ITEM_LOT" il ON (il."GTIN" = it."GTIN" AND it."GTIN_LOT" = il."LOT_NUMBER")
		LEFT JOIN "ADJUSTMENT_REASON" ar ON (ar."ID" = it."ADJUSTMENT_ID")
	
), item_transaction_sum AS
( SELECT date, hf_code, gtin, lot, expiry, transaction_name, adjustment_reason, SUM(qty) qty FROM item_transaction_dtl
	GROUP BY date, hf_code, gtin, lot, expiry, transaction_name, adjustment_reason)
SELECT hf."ID" id, hf."NAME" AS name, date, i."NAME" itm_name, adjustment_reason, transaction_name, qty, gtin, lot, expiry 
	FROM item_transaction_sum its INNER JOIN "HEALTH_FACILITY" hf ON (its.hf_code = hf."CODE")
		INNER JOIN "ITEM_MANUFACTURER" im ON (its.gtin = im."GTIN") 	
		INNER JOIN "ITEM" i ON (i."ID" = im."ITEM_ID")
		WHERE i."NAME" IN ('BCG', 'OPV', 'Measles', 'Measles Rubella', 'PCV-13','ROTA','Tetanus', 'DTP-HepB-Hib')
) AS internal GROUP BY id, name, gtin, lot, expiry, adjustment_reason, transaction_name, date, itm_name
		;

CREATE OR REPLACE VIEW dvdmt_facility_stock AS
SELECT id, name, date, itm_name, CASE WHEN transaction_name = 'Adjustment' AND adjustment_reason <> 'Stock Count' THEN adjustment_reason ELSE transaction_name END AS reason, sum(qty) as qty FROM dvdmt_facility_stock_source
GROUP BY id, name, date, itm_name, reason;

CREATE OR REPLACE VIEW dvdmt_stock_column_names AS
SELECT DISTINCT
	"ITEM"."NAME" || CASE WHEN "TRANSACTION_TYPE"."ID" = 4 AND "ADJUSTMENT_REASON"."ID" <> 99 THEN "ADJUSTMENT_REASON"."NAME" WHEN "TRANSACTION_TYPE"."ID" = 4 THEN 'Adjustment' ELSE "TRANSACTION_TYPE"."NAME" END AS name 
	FROM "ITEM" CROSS JOIN "TRANSACTION_TYPE"
	CROSS JOIN "ADJUSTMENT_REASON" 
	WHERE "ITEM"."NAME" IN ('BCG', 'OPV', 'Measles', 'Measles Rubella', 'PCV-13','ROTA','Tetanus', 'DTP-HepB-Hib') 
	AND "ADJUSTMENT_REASON"."ID" IN (25, 24, 32, 99) AND "TRANSACTION_TYPE"."ID" IN (1,2,3,4,5);

DROP VIEW dvdmt_facility_stock_crosstab CASCADE;
CREATE OR REPLACE VIEW dvdmt_facility_stock_crosstab AS
SELECT * FROM crosstab(
	'SELECT id::TEXT||name||date, id, name, date, itm_name || reason, qty FROM dvdmt_facility_stock ORDER BY 1, itm_name',
	'SELECT name FROM dvdmt_stock_column_names ORDER BY name'
)
	as (
	row_name TEXT,
	id INT,
	name TEXT,
	date TEXT,
	"BCGAdjustment" INT,
	"BCGArrival" INT,
	"BCGDispatch" INT,
	"BCGKuisha muda wa matumizi" INT,
	"BCGMabadiliko ya VVM" INT,
	"BCGStock Count" INT,
	"BCGVaccination" INT,
	"BCGZimevunjika" INT,
	"DTP-HepB-HibAdjustment" INT,
	"DTP-HepB-HibArrival" INT,
	"DTP-HepB-HibDispatch" INT,
	"DTP-HepB-HibKuisha muda wa matumizi" INT,
	"DTP-HepB-HibMabadiliko ya VVM" INT,
	"DTP-HepB-HibStock Count" INT,
	"DTP-HepB-HibVaccination" INT,
	"DTP-HepB-HibZimevunjika" INT,
	"MeaslesAdjustment" INT,
	"MeaslesArrival" INT,
	"MeaslesDispatch" INT,
	"MeaslesKuisha muda wa matumizi" INT,
	"MeaslesMabadiliko ya VVM" INT,
	"Measles RubellaAdjustment" INT,
	"Measles RubellaArrival" INT,
	"Measles RubellaDispatch" INT,
	"Measles RubellaKuisha muda wa matumizi" INT,
	"Measles RubellaMabadiliko ya VVM" INT,
	"Measles RubellaStock Count" INT,
	"Measles RubellaVaccination" INT,
	"Measles RubellaZimevunjika" INT,
	"MeaslesStock Count" INT,
	"MeaslesVaccination" INT,
	"MeaslesZimevunjika" INT,
	"OPVAdjustment" INT,
	"OPVArrival" INT,
	"OPVDispatch" INT,
	"OPVKuisha muda wa matumizi" INT,
	"OPVMabadiliko ya VVM" INT,
	"OPVStock Count" INT,
	"OPVVaccination" INT,
	"OPVZimevunjika" INT,
	"PCV-13Adjustment" INT,
	"PCV-13Arrival" INT,
	"PCV-13Dispatch" INT,
	"PCV-13Kuisha muda wa matumizi" INT,
	"PCV-13Mabadiliko ya VVM" INT,
	"PCV-13Stock Count" INT,
	"PCV-13Vaccination" INT,
	"PCV-13Zimevunjika" INT,
	"ROTAAdjustment" INT,
	"ROTAArrival" INT,
	"ROTADispatch" INT,
	"ROTAKuisha muda wa matumizi" INT,
	"ROTAMabadiliko ya VVM" INT,
	"ROTAStock Count" INT,
	"ROTAVaccination" INT,
	"ROTAZimevunjika" INT,
	"TetanusAdjustment" INT,
	"TetanusArrival" INT,
	"TetanusDispatch" INT,
	"TetanusKuisha muda wa matumizi" INT,
	"TetanusMabadiliko ya VVM" INT,
	"TetanusStock Count" INT,
	"TetanusVaccination" INT,
	"TetanusZimevunjika" INT
	);



CREATE OR REPLACE  VIEW dvdmt_facility_dose_stock_tally AS
WITH facility_stock_tally AS(
	SELECT 
		hf."ID" AS id, 
		hf."NAME" as name, 
		d."FULLNAME" as fullname,
		st."COUNT" as count, 
		CASE WHEN "MONTH_NO" <= date_part('month', st."ENTRY_DATE") THEN 
			date_part('year', st."ENTRY_DATE") || '-' || to_char(to_timestamp(st."MONTH_NO"::TEXT, 'MM'), 'TMMON')
		ELSE 
			date_part('year', st."ENTRY_DATE") - 1 || '-' || to_char(to_timestamp(st."MONTH_NO"::TEXT, 'MM'), 'TMMON')
		END AS date 
	 FROM "STOCK_TALLY" st 
		INNER JOIN "DOSE" d ON (st."DOSE_ID" = d."ID") 
		INNER JOIN "HEALTH_FACILITY" hf ON (hf."ID" = st."HEALTH_FACILITY_ID")
) SELECT id, name, fullname, date, sum(count) as sum FROM facility_stock_tally
	GROUP BY id, name, fullname, date;

--select * from dvdmt_facility_dose_stock_tally;
CREATE OR REPLACE VIEW dvdmt_facility_dose_stock_crosstab AS
SELECT * FROM crosstab(
	'SELECT id::TEXT||name||date, id, name, date, fullname, sum FROM dvdmt_facility_dose_stock_tally ORDER BY 1, fullname',
	'SELECT "FULLNAME" FROM "DOSE" ORDER BY "FULLNAME"'
)
	as (
		row_name TEXT,
		id INT, 
		name TEXT, 
		date TEXT, 
		"BCG" INT,
		"DTP-HepB-Hib 1" INT,
		"DTP-HepB-Hib 2" INT,
		"DTP-HepB-Hib 3" INT,
		"Measles 1" INT,
		"Measles 2" INT,
		"Measles Rubella 1" INT,
		"Measles Rubella 2" INT,
		"OPV0" INT,
		"OPV 1" INT,
		"OPV 2" INT,
		"OPV 3" INT,
		"PCV 1" INT,
		"PCV 2" INT,
		"PCV 3" INT,
		"Rota 1" INT,
		"Rota 2" INT,
		"TT1" INT,
		"TT2" INT,
		"TT3" INT,
		"TT4" INT,
		"TT5" INT
	);


CREATE OR REPLACE VIEW dvdmt_report AS
 WITH dvdmt_facility_appt AS (
	SELECT "SCHEDULED_FACILITY_ID" id, "AEFI" aefi, "OUTREACH" outreach, to_char("SCHEDULED_DATE", 'MON-YYYY') AS date FROM "VACCINATION_APPOINTMENT"
), dvdmt_facility_appt_sum AS (
	SELECT id, date, SUM(CASE WHEN aefi = true THEN 1 ELSE 0 END) AS sum_aefi,  SUM(CASE WHEN outreach = true THEN 1 ELSE 0 END) AS sum_outreach,  SUM(CASE WHEN outreach = true THEN 0 ELSE 1 END) AS sum_fixed FROM dvdmt_facility_appt
	GROUP BY id, date
) SELECT hf."ID", hf."NAME", 
	COALESCE(doses_ct.vaccine_date, stock_ct.date, aefi_ct.date, tally_ct.date) AS date, 
	COALESCE(doses_ct."BCG-0",0) + COALESCE(doses_ct."BCG-0-11",0) + COALESCE(tally_ct."BCG",0) as BCG_0_11,
	COALESCE(doses_ct."BCG-12-17",0) + COALESCE(doses_ct."BCG-18+",0) + COALESCE(tally_ct."BCG",0) as BCG_12_plus,
	COALESCE(doses_ct."OPV0-0",0) + COALESCE(doses_ct."OPV0-0-11",0) + COALESCE(tally_ct."OPV0",0) as OPV0,
	COALESCE(doses_ct."OPV 1-0",0) + COALESCE(doses_ct."OPV 1-0-11",0) + COALESCE(tally_ct."OPV 1",0) as OPV1_0_11,
	COALESCE(doses_ct."OPV 1-12-17",0) + COALESCE(doses_ct."OPV 1-18+",0) as OPV1_12_plus,
	COALESCE(doses_ct."OPV 2-0",0) + COALESCE(doses_ct."OPV 2-0-11",0) + COALESCE(tally_ct."OPV 2",0) as OPV2_0_11,
	COALESCE(doses_ct."OPV 2-12-17",0) + COALESCE(doses_ct."OPV 2-18+",0) as OPV2_12_plus,
	COALESCE(doses_ct."OPV 3-0",0) + COALESCE(doses_ct."OPV 3-0-11",0) + COALESCE(tally_ct."OPV 3",0) as OPV3_0_11,
	COALESCE(doses_ct."OPV 3-12-17",0) + COALESCE(doses_ct."OPV 3-18+",0) as OPV3_12_plus,

	COALESCE(doses_ct."PCV 1-0",0) + COALESCE(doses_ct."PCV 1-0-11",0) + COALESCE(tally_ct."PCV 1",0) as PCV1_0_11,
	COALESCE(doses_ct."PCV 1-12-17",0) + COALESCE(doses_ct."PCV 1-18+",0) as PCV1_12_plus,
	COALESCE(doses_ct."PCV 2-0",0) + COALESCE(doses_ct."PCV 2-0-11",0) + COALESCE(tally_ct."PCV 2",0) as PCV2_0_11,
	COALESCE(doses_ct."PCV 2-12-17",0) + COALESCE(doses_ct."PCV 2-18+",0) as PCV2_12_plus,
	COALESCE(doses_ct."PCV 3-0",0) + COALESCE(doses_ct."PCV 3-0-11",0) + COALESCE(tally_ct."PCV 3",0) as PCV3_0_11,
	COALESCE(doses_ct."PCV 3-12-17",0) + COALESCE(doses_ct."PCV 3-18+",0) as PCV3_12_plus,

	COALESCE(doses_ct."Measles 1-0",0) + COALESCE(doses_ct."Measles 1-0-11",0) + COALESCE(tally_ct."Measles 1",0) + 
	COALESCE(doses_ct."Measles Rubella 1-0",0) + COALESCE(doses_ct."Measles Rubella 1-0-11",0) + COALESCE(tally_ct."Measles Rubella 1",0) as MEASLES1_0_11,
	COALESCE(doses_ct."Measles 1-12-17",0) + COALESCE(doses_ct."Measles 1-18+",0) + 
	COALESCE(doses_ct."Measles Rubella 1-12-17",0) + COALESCE(doses_ct."Measles Rubella 1-18+",0) as MEASLES1_12_plus,


	COALESCE(doses_ct."Measles 2-12-17",0) + COALESCE(doses_ct."Measles 2-0",0) + COALESCE(doses_ct."Measles 2-0-11",0) + COALESCE(tally_ct."Measles 2",0) + 
	COALESCE(doses_ct."Measles Rubella 2-12-17",0) + COALESCE(doses_ct."Measles Rubella 1-0",0) + COALESCE(doses_ct."Measles Rubella 2-0-11",0) + COALESCE(tally_ct."Measles Rubella 2",0) as MEASLES2_0_17,
	COALESCE(doses_ct."Measles 2-18+",0) + COALESCE(doses_ct."Measles Rubella 2-18+",0) as MEASLES2_18_plus,


	COALESCE(doses_ct."Rota 1-0",0) + COALESCE(doses_ct."Rota 1-0-11",0) + COALESCE(tally_ct."Rota 1",0) as Rota1_0_11,
	COALESCE(doses_ct."Rota 1-12-17",0) + COALESCE(doses_ct."Rota 1-18+",0) as Rota1_12_plus,
	COALESCE(doses_ct."Rota 2-0",0) + COALESCE(doses_ct."Rota 2-0-11",0) + COALESCE(tally_ct."Rota 2",0) as Rota2_0_11,
	COALESCE(doses_ct."Rota 2-12-17",0) + COALESCE(doses_ct."Rota 2-18+",0) as Rota2_12_plus,

	COALESCE(doses_ct."DTP-HepB-Hib 1-0",0) + COALESCE(doses_ct."DTP-HepB-Hib 1-0-11",0) + COALESCE(tally_ct."DTP-HepB-Hib 1",0) as DTP1_0_11,
	COALESCE(doses_ct."DTP-HepB-Hib 1-12-17",0) + COALESCE(doses_ct."DTP-HepB-Hib 1-18+",0) as DTP1_12_plus,
	COALESCE(doses_ct."DTP-HepB-Hib 2-0",0) + COALESCE(doses_ct."DTP-HepB-Hib 2-0-11",0) + COALESCE(tally_ct."DTP-HepB-Hib 2",0) as DTP2_0_11,
	COALESCE(doses_ct."DTP-HepB-Hib 2-12-17",0) + COALESCE(doses_ct."DTP-HepB-Hib 2-18+",0) as DTP2_12_plus,
	COALESCE(doses_ct."DTP-HepB-Hib 3-0",0) + COALESCE(doses_ct."DTP-HepB-Hib 3-0-11",0) + COALESCE(tally_ct."DTP-HepB-Hib 3",0) as DTP3_0_11,
	COALESCE(doses_ct."DTP-HepB-Hib 3-12-17",0) + COALESCE(doses_ct."DTP-HepB-Hib 3-18+",0) as DTP3_12_plus,

	COALESCE(doses_ct."TT1-0",0) + COALESCE(doses_ct."TT1-0-11",0) + COALESCE(doses_ct."TT1-12-17",0) + COALESCE(doses_ct."TT1-18+",0) as TT1_other,
	COALESCE(doses_ct."TT2-0",0) + COALESCE(doses_ct."TT2-0-11",0) + COALESCE(doses_ct."TT2-12-17",0) + COALESCE(doses_ct."TT2-18+",0) as TT2_other,
	COALESCE(doses_ct."TT3-0",0) + COALESCE(doses_ct."TT3-0-11",0) + COALESCE(doses_ct."TT3-12-17",0) + COALESCE(doses_ct."TT3-18+",0) as TT3_other,
	COALESCE(doses_ct."TT4-0",0) + COALESCE(doses_ct."TT4-0-11",0) + COALESCE(doses_ct."TT4-12-17",0) + COALESCE(doses_ct."TT4-18+",0) as TT4_other,
	COALESCE(doses_ct."TT5-0",0) + COALESCE(doses_ct."TT5-0-11",0) + COALESCE(doses_ct."TT5-12-17",0) + COALESCE(doses_ct."TT5-18+",0) as TT5_other,

	COALESCE(tally_ct."TT1",0) as TT1_tally,
	COALESCE(tally_ct."TT2",0) as TT2_tally,
	COALESCE(tally_ct."TT3",0) as TT3_tally,
	COALESCE(tally_ct."TT4",0) as TT4_tally,
	COALESCE(tally_ct."TT5",0) as TT5_tally,
	aefi_ct.sum_aefi AS aefi,
	aefi_ct.sum_outreach AS outreach,
	aefi_ct.sum_fixed AS fixed,
	"BCGArrival" AS bcg_in,
	"BCGDispatch" AS bcg_out,
	"BCGKuisha muda wa matumizi" AS bcg_expired,
	"BCGMabadiliko ya VVM" AS bcg_vvm,
	"BCGStock Count" AS bcg_count,
	"BCGZimevunjika" AS bcg_damaged,
	"BCGVaccination" AS bcg_used,
	"DTP-HepB-HibArrival" AS dtp_in,
	"DTP-HepB-HibDispatch" AS dtp_out,
	"DTP-HepB-HibKuisha muda wa matumizi" AS dtp_expired,
	"DTP-HepB-HibMabadiliko ya VVM" AS dtp_vvm,
	"DTP-HepB-HibStock Count" AS dtp_count,
	"DTP-HepB-HibZimevunjika" AS dtp_damaged,
	"DTP-HepB-HibVaccination" AS dtp_used,
	COALESCE("Measles RubellaArrival",0) + COALESCE("MeaslesArrival",0) AS measles_in,
	COALESCE("Measles RubellaDispatch",0) + COALESCE("MeaslesDispatch",0) AS measles_out,
	COALESCE("Measles RubellaKuisha muda wa matumizi",0) + COALESCE("MeaslesKuisha muda wa matumizi",0) AS measles_expired,
	COALESCE("Measles RubellaMabadiliko ya VVM",0) + COALESCE("MeaslesMabadiliko ya VVM",0) AS measles_vvm,
	COALESCE("Measles RubellaStock Count",0) + COALESCE("MeaslesStock Count",0) AS measles_count,
	COALESCE("Measles RubellaZimevunjika",0) + COALESCE("MeaslesZimevunjika",0) AS measles_damaged,
	COALESCE("Measles RubellaVaccination",0) + COALESCE("MeaslesVaccination",0) AS measles_vaccination,
	"OPVArrival" AS opv_in,
	"OPVDispatch" AS opv_out,
	"OPVKuisha muda wa matumizi" AS opv_expired,
	"OPVMabadiliko ya VVM" AS opv_vvm,
	"OPVStock Count" AS opv_count,
	"OPVZimevunjika" AS opv_damaged,
	"OPVVaccination" AS opv_vaccination,
	"PCV-13Arrival" AS pcv_in,
	"PCV-13Dispatch" AS pcv_out,
	"PCV-13Kuisha muda wa matumizi" AS pcv_expired,
	"PCV-13Mabadiliko ya VVM" AS pcv_vvm,
	"PCV-13Stock Count" AS pcv_stock,
	"PCV-13Zimevunjika" AS pcv_damaged,
	"PCV-13Vaccination" AS pcv_used,
	"ROTAArrival" AS rota_in,
	"ROTADispatch" AS rota_out,
	"ROTAKuisha muda wa matumizi" AS rota_expired,
	"ROTAMabadiliko ya VVM" AS rota_vvm,
	"ROTAStock Count" AS rota_count,
	"ROTAZimevunjika" AS rota_damaged,
	"ROTAVaccination" AS rota_used,
	"TetanusArrival" AS tt_in,
	"TetanusDispatch" AS tt_out,
	"TetanusKuisha muda wa matumizi" AS tt_expired,
	"TetanusMabadiliko ya VVM" AS tt_vvm,
	"TetanusStock Count" AS tt_count,
	"TetanusZimevunjika" AS tt_damaged,
	"TetanusVaccination" AS tt_used
	
FROM 
	"HEALTH_FACILITY" hf 
	LEFT JOIN dvdmt_facility_dose_age_group_crosstab doses_ct ON (doses_ct.id = hf."ID")
	LEFT JOIN dvdmt_facility_stock_crosstab stock_ct ON (stock_ct.id = hf."ID" AND stock_ct.date = COALESCE(doses_ct.vaccine_date, stock_ct.date))
	LEFT JOIN dvdmt_facility_appt_sum aefi_ct ON (aefi_ct.id = hf."ID" AND aefi_ct.date = COALESCE(doses_ct.vaccine_date, stock_ct.date, aefi_ct.date))
	LEFT JOIN dvdmt_facility_dose_stock_crosstab tally_ct ON (hf."ID" = tally_ct.id  AND tally_ct.date = COALESCE(doses_ct.vaccine_date, stock_ct.date, aefi_ct.date, tally_ct.date));


-- FACILITY COVERAGESCREATE OR REPLACE VIEW facility_coverage AS 
CREATE INDEX "VACCINATION_EVENT_HEALTH_FACILITY" ON "VACCINATION_EVENT"("HEALTH_FACILITY_ID");
CREATE INDEX "VACCINATION_EVENT_HEALTH_DOSE" ON "VACCINATION_EVENT"("DOSE_ID");
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
                    "CHILD_1"."MOTHER_ID",
                    "CHILD_1"."MOTHER_FIRSTNAME",
                    "CHILD_1"."MOTHER_LASTNAME",
                    "CHILD_1"."FATHER_ID",
                    "CHILD_1"."FATHER_FIRSTNAME",
                    "CHILD_1"."FATHER_LASTNAME",
                    "CHILD_1"."CARETAKER_ID",
                    "CHILD_1"."CARETAKER_FIRSTNAME",
                    "CHILD_1"."CARETAKER_LASTNAME",
                    "CHILD_1"."NOTES",
                    "CHILD_1"."IS_ACTIVE",
                    "CHILD_1"."MODIFIED_ON",
                    "CHILD_1"."MODIFIED_BY",
                    "CHILD_1"."IDENTIFICATION_NO1",
                    "CHILD_1"."IDENTIFICATION_NO2",
                    "CHILD_1"."IDENTIFICATION_NO3",
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
                     LEFT JOIN "HEALTH_FACILITY" ON "CHILD_1"."HEALTHCENTER_ID" = "HEALTH_FACILITY"."ID") "CHILD"("ID", "SYSTEM_ID", "FIRSTNAME1", "FIRSTNAME2", "LASTNAME1", "LASTNAME2", "BIRTHDATE", "GENDER", "HEALTHCENTER_ID", "BIRTHPLACE_ID", "COMMUNITY_ID", "DOMICILE_ID", "STATUS_ID", "ADDRESS", "PHONE", "MOBILE", "EMAIL", "MOTHER_ID", "MOTHER_FIRSTNAME", "MOTHER_LASTNAME", "FATHER_ID", "FATHER_FIRSTNAME", "FATHER_LASTNAME", "CARETAKER_ID", "CARETAKER_FIRSTNAME", "CARETAKER_LASTNAME", "NOTES", "IS_ACTIVE", "MODIFIED_ON", "MODIFIED_BY", "IDENTIFICATION_NO1", "IDENTIFICATION_NO2", "IDENTIFICATION_NO3", "BARCODE_ID", "TEMP_ID", "ID_1", "NAME", "CODE", "PARENT_ID", "TOP_LEVEL", "LEAF", "NOTES_1", "IS_ACTIVE_1", "MODIFIED_ON_1", "MODIFIED_BY_1", "VACCINATION_POINT", "ADDRESS_1", "VACCINE_STORE", "CONTACT", "COLD_STORAGE_CAPACITY", "TYPE_ID", "OWNERSHIP", "LOWCODE", "DOB_YEAR", "DOB_MONTH")
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
             JOIN "HEALTH_FACILITY_COHORT_DATA" ON "HEALTH_FACILITY"."ID" = "HEALTH_FACILITY_COHORT_DATA"."HEALTH_FACILITY_ID" AND date_part('year'::text, "VACCINATION_EVENT"."VACCINATION_DATE") = "HEALTH_FACILITY_COHORT_DATA"."YEAR"::double precision
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

CREATE OR REPLACE VIEW facility_stock_balance AS
SELECT ct.*, stk.qty as last_month_stock_count FROM crosstab(
	'SELECT id::TEXT||name||itm_name||date, id, name, itm_name, date, reason, qty FROM dvdmt_facility_stock ORDER BY 1, itm_name',
	'SELECT "NAME" FROM "TRANSACTION_TYPE" UNION SELECT "NAME" FROM "ADJUSTMENT_REASON" ORDER BY "NAME"'
)
	as ct (
	row_name TEXT,
	id INT,
	name TEXT,
	itm_name TEXT,
	date TEXT,
	"Adjustment" INT,
	"Allocation" INT,
	"Arrival" INT,
"Chanjo Kuganda" INT,
	"Dispatch" INT,
	"Kuisha muda wa matumizi" INT,
	"Kutoka kituo kingine" INT,
	"Kwenda kituo kingine" INT,
	"Mabadiliko ya VVM" INT,
	"Receipt" INT,
	"Stock Count" INT,
	"Transfer" INT,
	"Vaccination" INT,
	"Ziliingizwa chache kimakosa" INT,
	"Ziliingizwa zaidi kimakosa" INT,
	"Zilizobaki kwa campaign" INT,
	"Zimepotea" INT,
	"Zimevunjika" INT

	) 
LEFT JOIN dvdmt_facility_stock stk ON (ct.id = stk.id AND stk.reason = 'Stock Count' AND stk.itm_name = ct.itm_name AND to_char((to_timestamp(ct.date, 'MON-YYYY') - '1 MONTH'::INTERVAL), 'MON-YYYY') = stk.date);
/****
 ( NOT NEEDED )
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
  */

-- VACCINATION SUMMARY 
CREATE OR REPLACE VIEW child_vaccination_summary AS
SELECT hfid, report_period,
	SUM(CASE WHEN gender THEN 1 ELSE 0 END) AS male_total,
	SUM(CASE WHEN NOT gender THEN 1 ELSE 0 END) AS female_total,
	COUNT(gender) AS total,
	SUM(CASE WHEN gender AND vaccinated THEN 1 ELSE 0 END) AS male_vaccinated,
	SUM(CASE WHEN NOT gender AND vaccinated THEN 1 ELSE 0 END) AS female_vaccinated,
	SUM(CASE WHEN vaccinated THEN 1 ELSE 0 END) AS total_vaccinated,
	SUM(CASE WHEN gender AND NOT vaccinated THEN 1 ELSE 0 END) AS male_nonvaccinated,
	SUM(CASE WHEN NOT gender AND NOT vaccinated THEN 1 ELSE 0 END) AS female_nonvaccinated,
	SUM(CASE WHEN NOT vaccinated THEN 1 ELSE 0 END) AS total_nonvaccinated,
	SUM(CASE WHEN gender AND outreach THEN 1 ELSE 0 END) AS male_outreach,
	SUM(CASE WHEN NOT gender AND outreach THEN 1 ELSE 0 END) AS female_outreach,
	SUM(CASE WHEN outreach THEN 1 ELSE 0 END) AS total_outreach,
	SUM(CASE WHEN gender AND in_catchment THEN 1 ELSE 0 END) AS male_catchment,
	SUM(CASE WHEN NOT gender AND in_catchment THEN 1 ELSE 0 END) AS female_catchment,
	SUM(CASE WHEN in_catchment THEN 1 ELSE 0 END) AS total_catchment,
	SUM(CASE WHEN gender AND NOT in_catchment THEN 1 ELSE 0 END) AS male_noncatchment,
	SUM(CASE WHEN NOT gender AND NOT in_catchment THEN 1 ELSE 0 END) AS female_noncatchment,
	SUM(CASE WHEN NOT in_catchment THEN 1 ELSE 0 END) AS total_noncatchment
	
 FROM (
	SELECT DISTINCT hf."ID" AS hfid, 
		CASE WHEN "TYPE_ID" >= 3 THEN hf."NAME" ELSE NULL END AS h_name,
		CASE WHEN "TYPE_ID" = 2 THEN hf."NAME" ELSE (SELECT "NAME" FROM "HEALTH_FACILITY" WHERE "ID" = hf."PARENT_ID") END AS d_name,
		CASE WHEN "TYPE_ID" = 1 THEN hf."NAME" WHEN "TYPE_ID" = 2 THEN (SELECT "NAME" FROM "HEALTH_FACILITY" WHERE "ID" = hf."PARENT_ID") ELSE
			(SELECT "NAME" FROM "HEALTH_FACILITY" WHERE "ID" = (SELECT "PARENT_ID" FROM "HEALTH_FACILITY" WHERE "ID" = hf."PARENT_ID")) END AS r_name,
		to_char(ve."VACCINATION_DATE", 'MON-YYYY') as report_period,
		c."GENDER" AS gender,
		c."ID" AS child_id,
		ve."VACCINATION_STATUS" AS vaccinated,
		va."OUTREACH" AS outreach,
		c."HEALTHCENTER_ID" = ve."HEALTH_FACILITY_ID" AS in_catchment
	FROM "VACCINATION_EVENT" ve
		INNER JOIN "HEALTH_FACILITY" hf ON (hf."ID" = ve."HEALTH_FACILITY_ID")
		INNER JOIN "VACCINATION_APPOINTMENT" va ON (va."ID" = ve."APPOINTMENT_ID")
		INNER JOIN "CHILD" c ON (c."ID" = ve."CHILD_ID")
	WHERE ve."SCHEDULED_DATE" < CURRENT_TIMESTAMP
) AS I 
GROUP BY hfid, report_period;
-- CROSSTAB FOR VACCINATION DATES
CREATE OR REPLACE VIEW child_vaccination_date_crosstab AS
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
		"OPV0" DATE,
		"OPV 1" DATE,
		"OPV 2" DATE,
		"OPV 3" DATE,
		"PCV 1" DATE,
		"PCV 2" DATE,
		"PCV 3" DATE,
		"Rota 1" DATE,
		"Rota 2" DATE,
		"TT1" DATE,
		"TT2" DATE,
		"TT3" DATE,
		"TT4" DATE,
		"TT5" DATE
	) ;
	
-- DEFAULTERS
CREATE OR REPLACE VIEW child_defaulters AS 
SELECT 
	row_number() OVER () AS row_num,
	C."ID", 
	C."FIRSTNAME1",
	C."FIRSTNAME2",
	C."LASTNAME1",
	C."LASTNAME2",
	C."GENDER",
	C."BIRTHDATE",
	C."MOTHER_FIRSTNAME",
	C."MOTHER_LASTNAME",
	C."PHONE",
	C."MOBILE",
	ve."SCHEDULED_DATE",
	D."FULLNAME",
	P."NAME" AS place_name,
	to_char(ve."SCHEDULED_DATE", 'MON-YYYY') AS report_period,
	hf."ID" as hf_id,
	nv."NAME" AS nv_reason
	FROM "VACCINATION_EVENT" ve 
	INNER JOIN "CHILD" c ON (ve."CHILD_ID" = c."ID")
	INNER JOIN "DOSE" d ON (ve."DOSE_ID" = d."ID")
	INNER JOIN "VACCINATION_APPOINTMENT" va ON (ve."APPOINTMENT_ID" = va."ID")
	INNER JOIN "PLACE" p ON (p."ID" = c."DOMICILE_ID")
	INNER JOIN "HEALTH_FACILITY" hf ON (hf."ID" = ve."HEALTH_FACILITY_ID")
	LEFT JOIN "NONVACCINATION_REASON" nv ON (ve."NONVACCINATION_REASON_ID" = nv."ID")
WHERE 
	"VACCINATION_STATUS" = FALSE AND ve."IS_ACTIVE" = TRUE AND c."IS_ACTIVE" = TRUE
	AND va."SCHEDULED_DATE" < CURRENT_TIMESTAMP;
	

ALTER TABLE "WEIGHT_TALLY" ADD EBF INT;
ALTER TABLE "WEIGHT_TALLY" ADD RF INT;

--CREATE INDEX "VACCINATION_EVENT_ACTIVE" ON "VACCINATION_EVENT"("IS_ACTIVE");
CREATE INDEX "VACCINATION_EVENT_VACCINE_DATE" ON "VACCINATION_EVENT"("VACCINATION_DATE");
CREATE INDEX "VACCINATION_EVENT_SCHEDULE_DATE" ON "VACCINATION_EVENT"("SCHEDULED_DATE");
CREATE INDEX "VACCINATION_EVENT_STATUS" ON "VACCINATION_EVENT"("VACCINATION_STATUS");
CREATE INDEX "HEALTH_FACILITY_PARENT_ID" ON "HEALTH_FACILITY"("PARENT_ID")

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

-- FOR TALLY TABLE
INSERT INTO "DOSE" ("SCHEDULED_VACCINATION_ID", "AGE_DEFINITION_ID", "DOSE_NUMBER", "FULLNAME", "IS_ACTIVE", "MODIFIED_ON", "MODIFIED_BY") VALUES (41, 31, 1, 'TT1', FALSE, CURRENT_TIMESTAMP, 1);
INSERT INTO "DOSE" ("SCHEDULED_VACCINATION_ID", "AGE_DEFINITION_ID", "DOSE_NUMBER", "FULLNAME", "IS_ACTIVE", "MODIFIED_ON", "MODIFIED_BY") VALUES (41, 31, 2, 'TT2', FALSE, CURRENT_TIMESTAMP, 1);
INSERT INTO "DOSE" ("SCHEDULED_VACCINATION_ID", "AGE_DEFINITION_ID", "DOSE_NUMBER", "FULLNAME", "IS_ACTIVE", "MODIFIED_ON", "MODIFIED_BY") VALUES (41, 31, 3, 'TT3', FALSE, CURRENT_TIMESTAMP, 1);
INSERT INTO "DOSE" ("SCHEDULED_VACCINATION_ID", "AGE_DEFINITION_ID", "DOSE_NUMBER", "FULLNAME", "IS_ACTIVE", "MODIFIED_ON", "MODIFIED_BY") VALUES (41, 31, 4, 'TT4', FALSE, CURRENT_TIMESTAMP, 1);
INSERT INTO "DOSE" ("SCHEDULED_VACCINATION_ID", "AGE_DEFINITION_ID", "DOSE_NUMBER", "FULLNAME", "IS_ACTIVE", "MODIFIED_ON", "MODIFIED_BY") VALUES (41, 31, 5, 'TT5', FALSE, CURRENT_TIMESTAMP, 1);

-- ADD ACTION ID TO THE PARAMETER TABLE
ALTER TABLE "REPORT_PARAMETER_INPUT_TYPE" ALTER COLUMN "COMPLETE_SOURCE" TYPE TEXT;
ALTER TABLE "REPORT_PARAMETER_INPUT_TYPE" ADD "DEFAULT" VARCHAR(128); 
ALTER TABLE "REPORT_PARAMETERS" ADD "ACTION_ID" INTEGER NOT NULL DEFAULT 482;
ALTER TABLE "REPORT_PARAMETERS" ALTER COLUMN "ACTION_ID" DROP DEFAULT;
ALTER TABLE "REPORT_PARAMETERS" ADD CONSTRAINT "FK_REPORT_PARAMETERS_ACTION" FOREIGN KEY ("ACTION_ID") REFERENCES "ACTIONS"("ID");
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
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (15, 'ANTIGEN', 'select', 'SELECT "ID","NAME" FROM (SELECT "ID","NAME","NAME" AS S FROM "SCHEDULED_VACCINATION" UNION SELECT NULL, ''All'', ''AAAAAAA'') Q ORDER BY s', '1', 'System.Int32', null);
INSERT INTO "REPORT_PARAMETER_INPUT_TYPE" VALUES (16, 'REPORT_PERIOD', 'select', 'SELECT DISTINCT TO_CHAR(TO_TIMESTAMP(I::TEXT, ''MM''),''MON'') || ''-'' || Y AS KEY, TO_CHAR(TO_TIMESTAMP(I::TEXT, ''MM''),''MON'') || ''-'' || Y AS DISP, I, Y FROM generate_series(1,12) I, (SELECT DISTINCT EXTRACT(YEAR FROM "SCHEDULED_DATE") AS Y FROM "VACCINATION_EVENT") D WHERE Y BETWEEN 2010 AND EXTRACT(YEAR FROM CURRENT_TIMESTAMP) AND (Y < EXTRACT(YEAR FROM CURRENT_TIMESTAMP) OR I <= EXTRACT(MONTH FROM CURRENT_TIMESTAMP))  ORDER BY Y DESC, I DESC', '1', 'System.String', null);

-- REPORTS
INSERT INTO "REPORT" VALUES (1, 'Adverse_Effects', 'Non-Vaccinations', 482, 2, 'A list of non-vaccination events due to adverse reactions, refusal, etc.', 'R');
INSERT INTO "REPORT" VALUES (2, 'All_Children', 'All Children', 482, 2, 'A list of all children by facility, district, etc.', 'R');
INSERT INTO "REPORT" VALUES (4, 'defaulter_report', 'Defaulters', 482, 2, NULL, 'R');
INSERT INTO "REPORT" VALUES (6, 'Child_Vaccinations', 'Child Vaccinations', 482, 2, NULL, 'R');

-- MONTHLY REPORTS
INSERT INTO "REPORT" VALUES (8, 'DVDMT_Report', 'DVDMT Worksheet', 481, 3, NULL, 'R');
INSERT INTO "REPORT" VALUES (14, 'drp_divo_overview', 'Forecasted Consumption', 481, 3, NULL, 'R');
--INSERT INTO "REPORT" VALUES (15, 'Facility_Coverage_Admin', 'Facility Coverage (targets)', 482, 3, 'Review the coverage rates of facilities based on coverage targets', 'R');
INSERT INTO "REPORT" VALUES (17, 'Facility_Coverage_Actual', 'Facility Coverage', 482, 3, 'Review the coverage rates of facilities as a function of those who were expected to show up (appointment data) and administrative targets', 'R');
INSERT INTO "REPORT" VALUES (20, 'facility_droupout', 'Facility Dropout', 482, 3, 'Review the dropout rates of facilities as a function of those who received the very first dose of a series but not the last', 'R');

-- VACCINATIONS
INSERT INTO "REPORT" VALUES (5, 'Vaccines_Administered', 'Health Facility Consumption', 481, 1, NULL, 'R');
INSERT INTO "REPORT" VALUES (9, 'Facility_Balance_Report', 'Facility Balances', 481, 1, NULL, 'R');
INSERT INTO "REPORT" VALUES (13, 'Pack_Order_Report', 'Facility Pick/Pack Report', 481, 1, NULL, 'R');

-- FORMS
INSERT INTO "REPORT" VALUES (11, 'Vaccine_Report_Barcode', 'Monthly Register', 481, 4, NULL, 'F');
INSERT INTO "REPORT" VALUES (12, 'Vaccine_Report_Temp', 'Monthly Register (Temp Id)', 481, 4, NULL, 'F');
INSERT INTO "REPORT" VALUES (19, 'New_Vaccination_Register', 'Monthly Register (v9)', 481, 4, 'Prints a list of children due for vaccines at a particular facility', 'F');

-- PARAMETERS- ALL CHILDREN REPORT
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
INSERT INTO "REPORT_PARAMETERS" SELECT 6, 'reportPeriod', 'Report Period', 'Identifies the reporting period for which the report should be generated', true, 16, "ID", 6 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';

INSERT INTO "REPORT_PARAMETERS" SELECT 8, 'reportPeriod', 'Report Period', 'Identifies the reporting period for which the DVDMT should be generated', true, 16, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 8, 'facilityId', 'Health Facility', 'Identifies the current facility for the child', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 8, 'districtId', 'District', 'Identifies the district (nb: facilities are not filtered when this value changes)', true, 13, "ID", 1 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 8, 'regionId', 'Region', 'Identifies the region (nb: district is not filtered when this value changes)', true, 14, "ID", 0 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';


INSERT INTO "REPORT_PARAMETERS" SELECT 11, 'Month', 'Month', 'Identifies the month which should be generated for the monthly register', true, 6, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 11, 'Year', 'Year', 'Identifies the year which should be generated for the monthly register', true, 7, "ID", 5 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 11, 'healthFacility', 'Health Facility', 'Identifies the current facility for the  monthly register', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';

INSERT INTO "REPORT_PARAMETERS" SELECT 12, 'Month', 'Month', 'Identifies the month which should be generated for the  monthly register', true, 6, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 12, 'Year', 'Year', 'Identifies the year which should be generated for the  monthly register', true, 7, "ID", 5 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 12, 'healthFacility', 'Health Facility', 'Identifies the current facility for the  monthly register', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';

INSERT INTO "REPORT_PARAMETERS" SELECT 19, 'month', 'Month', 'Identifies the month which should be generated for the  monthly register', true, 6, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 19, 'year', 'Year', 'Identifies the year which should be generated for the  monthly register', true, 7, "ID", 5 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 19, 'facilityId', 'Health Facility', 'Identifies the current facility for the  monthly register', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';

INSERT INTO "REPORT_PARAMETERS" SELECT 15, 'dateFrom', 'Date (From)', 'Identifies the month which should be generated for the facility balance report', true, 1, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 15, 'dateTo', 'Date (To)', 'Identifies the year which should be generated for the facility balance report', true, 1, "ID", 5 FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 15, 'antigenId', 'Antigen', 'Identifies the antigen which should be displayed', true, 15, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 15, 'facilityId', 'Health Facility', 'Identifies the current facility for the facility balance report', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 15, 'districtId', 'District', 'Identifies the district (nb: facilities are not filtered when this value changes)', true, 13, "ID", 1 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 15, 'regionId', 'Region', 'Identifies the region (nb: district is not filtered when this value changes)', true, 14, "ID", 0 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';

INSERT INTO "REPORT_PARAMETERS" SELECT 17, 'reportPeriod', 'Reporting Period', '', true, 16, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 17, 'antigenId', 'Antigen', 'Identifies the antigen which should be displayed', true, 15, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'VaccinatorReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 17, 'facilityId', 'Health Facility', 'Identifies the current facility for the facility balance report', true, 2, "ID", 2 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 17, 'districtId', 'District', 'Identifies the district (nb: facilities are not filtered when this value changes)', true, 13, "ID", 1 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 17, 'regionId', 'Region', 'Identifies the region (nb: district is not filtered when this value changes)', true, 14, "ID", 0 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';

INSERT INTO "REPORT_PARAMETERS" SELECT 9, 'facilityId', 'Health Facility', 'Identifies the current facility for the facility balance report', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 9, 'districtId', 'District', 'Identifies the district (nb: facilities are not filtered when this value changes)', true, 13, "ID", 1 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 9, 'regionId', 'Region', 'Identifies the region (nb: district is not filtered when this value changes)', true, 14, "ID", 0 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 9, 'reportPeriod', 'Report Period', 'Identifies the reporting period for which the report should be generated', true, 16, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';

INSERT INTO "REPORT_PARAMETERS" SELECT 4, 'facilityId', 'Health Facility', 'Identifies the current facility for the facility balance report', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 4, 'districtId', 'District', 'Identifies the district (nb: facilities are not filtered when this value changes)', true, 13, "ID", 1 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 4, 'regionId', 'Region', 'Identifies the region (nb: district is not filtered when this value changes)', true, 14, "ID", 0 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 4, 'reportPeriod', 'Report Period', 'Identifies the reporting period for which the report should be generated', true, 16, "ID", 4 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';



INSERT INTO "REPORT_PARAMETERS" SELECT 20, 'facilityId', 'Health Facility', 'Identifies the current facility for the facility balance report', true, 2, "ID", 3 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 20, 'districtId', 'District', 'Identifies the district (nb: facilities are not filtered when this value changes)', true, 13, "ID", 1 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 20, 'regionId', 'Region', 'Identifies the region (nb: district is not filtered when this value changes)', true, 14, "ID", 0 FROM "ACTIONS" WHERE "NAME" = 'RegionalReportParms';
INSERT INTO "REPORT_PARAMETERS" SELECT 20, 'reportPeriod', 'Year', 'Identifies the year which should be generated', true, 7, "ID", 5 FROM "ACTIONS" WHERE "NAME" = 'DistrictReportParms';

CREATE TABLE dropout_doses (
	id SERIAL NOT NULL,
	start_dose_id INT NOT NULL,
	end_dose_id INT NOT NULL,
	display TEXT NOT NULL,
	CONSTRAINT PK_dropout_doses PRIMARY KEY (id),
	CONSTRAINT FK_start_dose_id FOREIGN KEY (start_dose_id) REFERENCES "DOSE"("ID"),
	CONSTRAINT FK_end_dose_id FOREIGN KEY (end_dose_id) REFERENCES "DOSE"("ID")
);

INSERT INTO dropout_doses (START_DOSE_ID, END_DOSE_ID, DISPLAY) VALUES (61, 72, 'BCG > MEASLES 1');
INSERT INTO dropout_doses (START_DOSE_ID, END_DOSE_ID, DISPLAY) VALUES (61, 77, 'BCG > MR1');
INSERT INTO dropout_doses (START_DOSE_ID, END_DOSE_ID, DISPLAY) VALUES (66, 68, 'PENTA1 > PENTA3');
	
CREATE OR REPLACE FUNCTION get_facility_dropout(hfid_in IN INTEGER, report_period_in IN VARCHAR) RETURNS TABLE(
	hfid INTEGER,
	dose_name text,
	mon text,
	numerator bigint,
	denominator bigint
)
AS 
$$
BEGIN
RETURN QUERY
WITH RECURSIVE facility(hfid) AS (
			SELECT "ID" as hfid , "CODE" as hfcode, "PARENT_ID", "NAME", "TYPE_ID" FROM "HEALTH_FACILITY" WHERE "ID" = hfid_in
			UNION
			SELECT "ID", "CODE" as hfcode, facility.hfid, "HEALTH_FACILITY"."NAME", "HEALTH_FACILITY"."TYPE_ID" FROM facility, "HEALTH_FACILITY" WHERE "HEALTH_FACILITY"."PARENT_ID" = facility.hfid
		)
SELECT
	hfid_in as hfid,
	dr.display as dose_name,
	to_char(ve."SCHEDULED_DATE", 'MON') as month,
	SUM(CASE WHEN
		(SELECT "VACCINATION_STATUS" FROM "VACCINATION_EVENT" ive WHERE ive."CHILD_ID" = ve."CHILD_ID" AND ive."DOSE_ID" = dr.START_DOSE_ID LIMIT 1)
		AND NOT ve."VACCINATION_STATUS"
		THEN
			1
		ELSE
			0
		END) AS numerator,
	(SUM(CASE WHEN (SELECT "VACCINATION_STATUS" FROM "VACCINATION_EVENT" ive WHERE ive."CHILD_ID" = ve."CHILD_ID" AND ive."DOSE_ID" = dr.START_DOSE_ID LIMIT 1) THEN 1 ELSE 0 END)) AS denominator
	FROM "VACCINATION_EVENT" ve
	INNER JOIN DROPOUT_DOSES dR ON (dr.END_DOSE_ID = ve."DOSE_ID")
	INNER JOIN "DOSE" d ON (ve."DOSE_ID" = d."ID")
	INNER JOIN facility f ON (f.hfid = ve."HEALTH_FACILITY_ID")
	WHERE
	ve."SCHEDULED_DATE" < CURRENT_TIMESTAMP
	AND ve."SCHEDULED_DATE" BETWEEN (report_period_in || '-01-01')::DATE AND (report_period_in || '-12-31')::DATE
	GROUP BY month, dr.display;
END $$ LANGUAGE PLPGSQL;

CREATE OR REPLACE VIEW facility_balance_source AS
select id::TEXT || gtin || lot || date, id, gtin, lot, expiry, name, date, coalesce(adjustment_reason, transaction_name) as tx, qty from 
	dvdmt_facility_stock_source 
WHERE transaction_name in ('Stock Count') or adjustment_reason in ('Mabadiliko ya VVM','Kuisha muda wa matumizi','Zimevunjika') order by 1, tx;

CREATE OR REPLACE VIEW facility_balance_crosstab AS 
SELECT * FROM crosstab('SELECT * FROM facility_balance_source ORDER BY 1, tx', 
	'select * from unnest(array[''Stock Count'',''Zimevunjika'',''Mabadiliko ya VVM'',''Kuisha muda wa matumizi'']) ORDER BY 1') AS ct (

	rowid TEXT,
	id INT,
	gtin TEXT,
	lot TEXT,
	expiry DATE,
	name TEXT,
	date TEXT,
	"Kuisha muda wa matumizi" INT,
"Mabadiliko ya VVM" INT,
"Stock Count" INT,
"Zimevunjika" INT

	);
