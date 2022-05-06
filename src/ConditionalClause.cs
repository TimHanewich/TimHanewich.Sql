using System;

namespace TimHanewich.Sql
{
    public class ConditionalClause
    {
        public string ColumnName {get; set;}
        public ComparisonOperator Operator {get; set;}
        public string Value {get; set;}
        public bool UseQuotes {get; set;}

        public ConditionalClause()
        {

        }
        
        public ConditionalClause(string column, ComparisonOperator op, string value, bool quotes)
        {
            ColumnName = column;
            Operator = op;
            Value = value;
            UseQuotes = quotes;
        }
    }
}