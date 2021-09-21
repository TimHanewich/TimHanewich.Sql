using System;
using System.Collections.Generic;

namespace TimHanewich.SqlHelper
{
    public class InsertHelper
    {
        //Private vars
        private List<ColumnNameValuePair> _ColumnNameValuePairs;
        private string TableName;
        
        public InsertHelper(string table_name)
        {
            TableName = table_name;
            _ColumnNameValuePairs = new List<ColumnNameValuePair>();
        }

        public void Add(string column_name, string value, bool use_quotations = false)
        {
            ColumnNameValuePair cnvp = new ColumnNameValuePair();
            cnvp.ColumnName = column_name;
            cnvp.Value = value;
            cnvp.UseQuotations = use_quotations;
            _ColumnNameValuePairs.Add(cnvp);
        }

        public void Remove(string column_name)
        {
            List<ColumnNameValuePair> ToRemove = new List<ColumnNameValuePair>();
            foreach (ColumnNameValuePair cnvp in _ColumnNameValuePairs)
            {
                if (cnvp.ColumnName == column_name)
                {
                    ToRemove.Add(cnvp);
                }
            }

            foreach (ColumnNameValuePair cnvp in ToRemove)
            {
                _ColumnNameValuePairs.Remove(cnvp);
            }
        }

        public override string ToString()
        {
            //Assemble the column names
            string col_names = "";
            foreach (ColumnNameValuePair cnvp in _ColumnNameValuePairs)
            {
                col_names = col_names + cnvp.ColumnName + ",";
            }
            col_names = col_names.Substring(0, col_names.Length - 1);

            //Assemble the values
            string vals = "";
            foreach (ColumnNameValuePair cnvp in _ColumnNameValuePairs)
            {
                string ToAppend = cnvp.Value;
                if (cnvp.UseQuotations)
                {
                    ToAppend = "'" + ToAppend + "'";
                }
                vals = vals + ToAppend + ",";
            }
            vals = vals.Substring(0, vals.Length - 1);

            //Assemble
            string cmd = "insert into " + TableName + " (" + col_names + ") values (" + vals + ")";
            return cmd;
        }

        private class ColumnNameValuePair
        {
            public string ColumnName {get; set;}
            public string Value {get; set;}
            public bool UseQuotations {get; set;}
        }


    }
}