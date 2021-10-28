using System;

namespace TimHanewich.SqlHelper
{
    public static class SqlToolkit
    {
        public static string ToSqlDateTimeString(this DateTime dt)
        {
            string ToReturn = "";
            ToReturn = dt.Year.ToString("0000") + dt.Month.ToString("00") + dt.Day.ToString("00");
            ToReturn = ToReturn + " " + dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString() + "." + dt.Millisecond.ToString();
            return ToReturn;
        }

        public static string ToSqlDateString(this DateTime dt)
        {
            string ToReturn = "";
            ToReturn = dt.Year.ToString("0000") + dt.Month.ToString("00") + dt.Day.ToString("00");
            return ToReturn;
        }
    }
}