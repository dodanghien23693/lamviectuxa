using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSoftSeo.Library
{
    public static class Ultilites
    {
        public static string GetStringDate(DateTime datetime){
            string day = datetime.Day.ToString();
            string month = datetime.Month.ToString();
            string year = datetime.Year.ToString();
            if (day.Length == 1)
            {
                day = "0" + day;
            }
            if (month.Length == 1)
            {

                month = "0" + month;

            }

            string date = year + "-" + month + "-" + day;
            return date;
        }
    }
}