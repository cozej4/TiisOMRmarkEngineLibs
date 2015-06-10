using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GIIS.DataLayer
{
    public class VaccineCoverageReportGrahpHelper
    {
        public static string GetUrl(DataTable dt, string title)
        {
            string vaccines = GetVaccines(dt);
            string vaccineLegend = GetVaccinesLegend(dt.Rows.Count);
            //string vaccinesCodes = GetVaccinesCodes(dt.Rows.Count);
            //int max = GetMaxValue(dt);
            //string size = CreateGraphSize(dt.Rows.Count, max);
            string data = CreateData(dt);

            string url = string.Format(@"http://chart.apis.google.com/chart
		   ?chxl=0:|5|10|15|20|25|30|35|40|45|50|55|60|65|70|75|80|85|90|95|100|1:|{0} 
		   &chxr=0,5,100
		   &chxt=x,y
		   &chs=600x500
		   &cht=bhg
		   &chbh=20,5,5
		   &chco=1589FF
		   &chds=5,100
		   &chd=t:{1}
		   &chls=1|1
		   &chtt={2}", vaccines, data, title, vaccineLegend);
//            string url = string.Format(@"http://chart.apis.google.com/chart
//		   ?chxl=0:|85|90|95|100|1:|{0} 
//		   &chxr=0,85,100
//		   &chxt=x,y
//		   &chs=600x500
//		   &cht=bhg
//		   &chbh=20,5,5
//		   &chco=1589FF
//		   &chds=85,100
//		   &chd=t:{1}
//		   &chls=1|1
//		   &chtt={2}", vaccines, data, title, vaccineLegend);

            return url;
        }

        private static string CreateData(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return "";

            string chartsData = "";
            //string x = GetVaccinesCodes(dt.Rows.Count);

            //string setx = string.Format("{0}|", x);
            string sety = "";

            foreach (DataRow dr in dt.Rows)
            {
                if (dr[5] != null && !string.IsNullOrEmpty(dr[5].ToString()))
                    sety += (dr[5].ToString().Replace("%", "") + ",");
                else
                    sety += "0,";
            }

            chartsData += (sety.Substring(0, sety.Length - 1) + "|");

            return chartsData.Substring(0, chartsData.Length - 1);
        }

        private static string CreateGraphSize(int rows, int max)
        {
            if (rows == 0)
                return "";

            string size = "";

            for (int i = 0; i < 1; i++)
            {
                size += string.Format("0,{0},0,{1},", rows - 1, 100);
            }

            return size.Substring(0, size.Length - 1);
        }

        private static int GetMaxValue(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return 0;

            int max = -1;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr[4] != null)
                    if (int.Parse(dr[4].ToString()) > max)
                        max = int.Parse(dr[4].ToString());
            }

            return max;
        }

        private static string GetVaccines(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return "";

            string vaccines = "";

            // int i = 0;
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                vaccines += (dt.Rows[i][2].ToString() + "|");
            }
            //foreach (DataRow dr in dt.Rows)
            //{
            //    vaccines += (Convert.ToChar(i + 65) + " = " + dr[2].ToString() + "|");
            //    i++;
            //}

            return vaccines.Substring(0, vaccines.Length - 1);
        }

        private static string GetVaccinesCodes(int rows)
        {
            if (rows == 0)
                return "";

            string vaccinesCodes = "";

            for (int i = 0; i < rows; i++)
            {
                vaccinesCodes += (i + ",");
            }

            return vaccinesCodes.Substring(0, vaccinesCodes.Length - 1);
        }

        private static string GetVaccinesLegend(int rows)
        {
            if (rows == 0)
                return "";

            string vaccines = "";

            for (int i = 0; i < rows; i++)
            {
                vaccines += (Convert.ToChar(i + 65) + "|");
            }

            return vaccines.Substring(0, vaccines.Length - 1);
        }
    }
}
