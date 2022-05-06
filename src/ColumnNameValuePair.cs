using System;

namespace TimHanewich.Sql
{
    public class ColumnNameValuePair
    {
        public string ColumnName {get; set;}
        public string Value {get; set;}
        public bool UseQuotations {get; set;}
    }
}