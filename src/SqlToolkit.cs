using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;

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

        public static string ReadSqlToJson(SqlDataReader dr)
        {
            List<string> ColumnNames = new List<string>();
            for (int i = 0; i < dr.FieldCount; i++)
            {
                ColumnNames.Add(dr.GetName(i));
            }

            //Extract
            List<JObject> ToReturn = new List<JObject>();
            while (dr.Read())
            {
                JObject ThisObj = new JObject();
                foreach (string column in ColumnNames)
                {
                    //Get the value.
                    string colVal = dr.GetValue(column).ToString();
                    bool Added = false;

                    //Is it an integer?
                    int colValNumInt = 0;
                    try
                    {
                        colValNumInt = Convert.ToInt32(colVal);
                        ThisObj.Add(column, colValNumInt);
                        Added = true;
                    }
                    catch
                    {

                    }

                    //Is it numeric? If so, add the number version
                    float colValNumFlo = 0;
                    try
                    {
                        colValNumFlo = Convert.ToSingle(colVal);
                        ThisObj.Add(column, colValNumFlo);
                        Added = true;
                    }
                    catch
                    {
                        
                    }

                    //If it has still not been added, add it as a string
                    if (Added == false)
                    {
                        ThisObj.Add(column, colVal);
                    }

                }
                ToReturn.Add(ThisObj);
            }

            return JsonConvert.SerializeObject(ToReturn.ToArray());
        }
    }
}