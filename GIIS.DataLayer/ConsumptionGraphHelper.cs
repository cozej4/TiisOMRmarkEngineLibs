//*******************************************************************************
//Copyright 2015 TIIS - Tanzania Immunization Information System
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
 //******************************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace GIIS.DataLayer
{
    public class ConsumptionGraphHelper
    {
        public static string GetUrl(DataTable dt, string title)
        {
            string vaccines = GetVaccines(dt);
            int max = GetMaxValue(dt);
            string colors = CreateRandomColor(dt.Rows.Count);
            string size = CreateGraphSize(dt.Rows.Count, max);
            string data = CreateData(dt);

            string url = string.Format(@"http://chart.apis.google.com/chart
               ?chxl=0:|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec
               &chxp=0,1,2,3,4,5,6,7,8,9,10,11,12
               &chxr=0,1,12|1,0,{5}
               &chxt=x,y
               &chs=700x350
               &cht=lxy
               &chco={0}
               &chds={1}
               &chm=B,{0},0,0,0
               &chd=t:{2}
               &chdl={3}
               &chls=1|1
               &chtt={4}", colors, size, data, vaccines, title, max);

            return url;
        }

        private static string GetVaccines(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return "";

            string vaccines = "";

            foreach (DataRow dr in dt.Rows)
            {
                vaccines += (dr[0].ToString() + "|");
            }

            return vaccines.Substring(0, vaccines.Length - 1);
        }

        private static int GetMaxValue(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return 0;

            int max = -1;

            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 1; i <= 12; i++)
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

        private static string CreateGraphSize(int rows, int max)
        {
            if (rows == 0)
                return "";

            string size = "";

            for (int i = 0; i < rows; i++)
            {
                size += string.Format("1,12,0,{0},", max);
            }

            return size.Substring(0, size.Length - 1);
        }

        private static string CreateData(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return "";

            string chartsData = "";

            foreach (DataRow dr in dt.Rows)
            {
                string setx = "1,2,3,4,5,6,7,8,9,10,11,12|";
                string sety = "";

                for (int index = 1; index <= 12; index++)
                {
                    if (dr[index] != null && !string.IsNullOrEmpty(dr[index].ToString()))
                        sety += (dr[index].ToString() + ",");
                    else
                        sety += "0,";
                }

                chartsData += (setx + sety.Substring(0, sety.Length - 1) + "|");
            }

            return chartsData.Substring(0, chartsData.Length - 1);
        }
    }
}
