using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace TimHanewich.Sql
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
                
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    //Get the column name
                    string column = ColumnNames[i];

                    //First, check if this column is null. If it is null, add null.
                    bool IsNull = dr.IsDBNull(i);
                    if (IsNull)
                    {
                        ThisObj.Add(column, null);
                    }
                    else //It is not null, so add the value somehow.
                    {
                        //Get the value.
                        string colVal = dr.GetValue(i).ToString();
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
                }
                ToReturn.Add(ThisObj);
            }

            return JsonConvert.SerializeObject(ToReturn.ToArray());
        }

        public static string ToSymbol(this ComparisonOperator op)
        {
            switch (op)
            {
                case ComparisonOperator.Equals:
                    return "=";
                case ComparisonOperator.GreaterThan:
                    return ">";
                case ComparisonOperator.LessThan:
                    return "<";
                case ComparisonOperator.GreaterThanOrEqual:
                    return ">=";
                case ComparisonOperator.LessThanOrEqual:
                    return "<=";
                case ComparisonOperator.Not:
                    return "!=";
                default:
                    throw new Exception("A comparison operator does not exist for enum value '" + op.ToString () + "'");
            }
        }

        public static string ToSymbol(this OrderDirection od)
        {
            if (od == OrderDirection.Ascending)
            {
                return "asc";
            }
            else if (od == OrderDirection.Descending)
            {
                return "desc";
            }
            else
            {
                throw new Exception("A symbol does not exist for OrderDirection '" + od.ToString() + "'");
            }
        }
    
        public static async Task<string> FindPrimaryKeyAsync(this SqlConnection sqlcon, string table_name)
        {
            string cmd = "SELECT column_name as PRIMARYKEYCOLUMN FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME AND KU.table_name='" + table_name +"' ORDER BY KU.TABLE_NAME,KU.ORDINAL_POSITION";
            
            //Is it open?
            bool WasClosed = false;
            if (sqlcon.State != ConnectionState.Open)
            {
                WasClosed = true;
                await sqlcon.OpenAsync();
            }

            SqlCommand sqlcmd = new SqlCommand(cmd, sqlcon);
            SqlDataReader dr = await sqlcmd.ExecuteReaderAsync();

            //Get the value to return
            string ToReturn = null;
            if (dr.HasRows == true)
            {
                await dr.ReadAsync();
                ToReturn = dr.GetString(0);
                dr.Close();
            }


            //If it was closed originally, close it
            if (WasClosed)
            {
                sqlcon.Close();
            }

            return ToReturn;
        }
    }
}