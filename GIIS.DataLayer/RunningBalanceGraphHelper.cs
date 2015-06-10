using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace GIIS.DataLayer
{
    public class RunningBalanceGraphHelper
    {
        public static string GetUrl(DataTable dt, string title, DateTime firstDate, DateTime lastDate, string hf)
        {
            string vaccines = hf;
            int max = GetMaxValue(dt, firstDate, lastDate);
            string colors = CreateRandomColor(1);
            string size = CreateGraphSize(1, max, firstDate, lastDate);
            string data = CreateData(dt, firstDate, lastDate);
            string arange = String.Format("1,0,{0}", max);
            if (max <= 10)
            {
                arange = String.Format("1,0,{0},1", max);
            }

            string s = "";
            string index = "";
            int j = 0;
            for (DateTime i = firstDate; i <= lastDate; i = i.AddMonths(1))
            {
                int m = i.Month;
                switch (m)
                {
                    case 1:
                        s += "1Jan" + "|";
                        break;
                    case 2:
                        s += "1Feb" + "|";
                        break;
                    case 3:
                        s += "1Mar" + "|";
                        break;
                    case 4:
                        s += "1Apr" + "|";
                        break;
                    case 5:
                        s += "1May" + "|";
                        break;
                    case 6:
                        s += "1Jun" + "|";
                        break;
                    case 7:
                        s += "1Jul" + "|";
                        break;
                    case 8:
                        s += "1Aug" + "|";
                        break;
                    case 9:
                        s += "1Sep" + "|";
                        break;
                    case 10:
                        s += "1Oct" + "|";
                        break;
                    case 11:
                        s += "1Nov" + "|";
                        break;
                    case 12:
                        s += "1Dec" + "|";
                        break;


                }

                index += (j + ",");
                j = i.DayOfYear;

            }
            
            string url = string.Format(@"http://chart.apis.google.com/chart
               ?chxl=0:|{5}
               &chxr={4}
               &chxt=x,y
               &chs=700x400
               &cht=lxy
               &chco={0}
               &chd=t:{2}
               &chds={1}
               &chm=B,{0},0,0,0
               &chls=1|1
               &chtt={3}", colors, size, data,  title, arange, s.Substring(0, s.Length - 1));

            return url;
        }

        private static int GetMaxValue(DataTable dt, DateTime firstDate, DateTime lastDate)
        {
            if (dt.Rows.Count == 0)
                return 0;

            int max = -1;
            int n = dt.Columns.Count - 1;//(lastDate - firstDate).Days;
            DataRow dr = dt.Rows[8];            
            {
                for (int i = 1; i <= n; i++)
                {
                    if (dr[i] != null && !string.IsNullOrEmpty(dr[i].ToString()))
                        if (int.Parse(dr[i].ToString()) > max)
                            max = int.Parse(dr[i].ToString());
                }
            }

            return max;
        }

        private static string CreateRandomColor(int rows)
        {
            if (rows == 0)
                return "";

            string colors = "";

            for (int i = 0; i < rows; i++)
            {
                Random randonGen = new Random();
                Color randomColor = Color.FromArgb(randonGen.Next(255), randonGen.Next(255), randonGen.Next(255));
                colors += (ColorTranslator.ToHtml(randomColor).Substring(1, 6) + ",");
                Thread.Sleep(100);
            }

            return colors.Substring(0, colors.Length - 1);
        }

        private static string CreateGraphSize(int rows, int max, DateTime firstDate, DateTime lastDate)
        {
            if (rows == 0)
                return "";

            string size = "";

            for (int i = 0; i < rows; i++)
            {
                size += string.Format("1,{1},0,{0},", max, (lastDate - firstDate).Days + 1);
            }

            return size.Substring(0, size.Length - 1);
        }

        private static string CreateData(DataTable dt, DateTime firstDate, DateTime lastDate)
        {
            if (dt.Rows.Count == 0)
                return "";

            string chartsData = "";
            int n = (lastDate - firstDate).Days;

            string j = "";
            for (int i = 1; i <= n + 1; i++)
            {
                j += (i + ",");
            }

            DataRow dr = dt.Rows[8];

            string setx = j.Substring(0, j.Length - 1) + "|";
            string sety = "";

            for (int index = 1; index <= n + 1; index++)
            {
                if (dr[index] != null && !string.IsNullOrEmpty(dr[index].ToString()))
                    sety += (dr[index].ToString() + ",");
                else
                    sety += "0,";
            }

            chartsData += (setx + sety.Substring(0, sety.Length - 1) + "|");

            return chartsData.Substring(0, chartsData.Length - 1);
        }
    }
}