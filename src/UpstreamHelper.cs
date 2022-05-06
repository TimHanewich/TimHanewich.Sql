using System;
using System.Collections.Generic;

namespace TimHanewich.Sql
{
    public class UpstreamHelper
    {
        //Private vars - used for both insert and update
        private List<ColumnNameValuePair> _ColumnNameValuePairs;
        private string TableName;

        //Private vars - update only
        private List<ConditionalClause> _WhereClauses;
        

        public UpstreamHelper(string table_name)
        {
            TableName = table_name;
            _ColumnNameValuePairs = new List<ColumnNameValuePair>();
            _WhereClauses = new List<ConditionalClause>();
        }

        public void Add(ColumnNameValuePair cnvp)
        {
            _ColumnNameValuePairs.Add(cnvp);
        }

        //Adds to the columns that will be inserted or updated (data in)
        public void Add(string column_name, string value, bool use_quotations = false)
        {
            ColumnNameValuePair cnvp = new ColumnNameValuePair();
            cnvp.ColumnName = column_name;
            cnvp.Value = value;
            cnvp.UseQuotations = use_quotations;
            _ColumnNameValuePairs.Add(cnvp);
        }

        //Adds a condition to which records should be changed
        public void AddWhereClause(ConditionalClause clause)
        {
            _WhereClauses.Add(clause);
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
    
        public string ToInsert()
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
    
        public string ToUpdate()
        {
            string cmd = "";
            cmd = "update " + TableName + " set ";

            //Create the properties section
            List<string> SetProps = new List<string>();
            foreach (ColumnNameValuePair cnvp in _ColumnNameValuePairs)
            {
                string quotePart = "";
                if (cnvp.UseQuotations)
                {
                    quotePart = "'";
                }
                SetProps.Add(cnvp.ColumnName + " = " + quotePart + cnvp.Value + quotePart);
            }
            string setPart = "";
            foreach (string s in SetProps)
            {
                setPart = setPart + s + ",";
            }
            setPart = setPart.Substring(0, setPart.Length-1); //Remove the last trailing comma
            cmd = cmd + setPart; //Add the set part

            //Create the where section
            if (_WhereClauses.Count > 0)
            {
                string WhereSection = "where ";
                foreach (ConditionalClause cc in _WhereClauses)
                {
                    string ThisWhereClauseStr = cc.ColumnName + " ";

                    //Set the operator
                    if (cc.Operator == ComparisonOperator.Equals)
                    {
                        ThisWhereClauseStr = ThisWhereClauseStr + "=";
                    }
                    else if (cc.Operator == ComparisonOperator.GreaterThan)
                    {
                        ThisWhereClauseStr = ThisWhereClauseStr + ">";
                    }
                    else if (cc.Operator == ComparisonOperator.LessThan)
                    {
                        ThisWhereClauseStr = ThisWhereClauseStr + "<";
                    }
                    else if (cc.Operator == ComparisonOperator.GreaterThanOrEqual)
                    {
                        ThisWhereClauseStr = ThisWhereClauseStr + ">=";
                    }
                    else if (cc.Operator == ComparisonOperator.LessThanOrEqual)
                    {
                        ThisWhereClauseStr = ThisWhereClauseStr + "<=";
                    }
                    else if (cc.Operator == ComparisonOperator.Not)
                    {
                        ThisWhereClauseStr = ThisWhereClauseStr + "!=";
                    }

                    //Set the value
                    string quotePart = "";
                    if (cc.UseQuotes)
                    {
                        quotePart = "'";
                    }
                    ThisWhereClauseStr = ThisWhereClauseStr + " " + quotePart + cc.Value + quotePart;

                    //Append it
                    WhereSection = WhereSection + ThisWhereClauseStr + " and ";
                }
                WhereSection = WhereSection.Substring(0, WhereSection.Length - 5); //Remove the final trailing " and "
                cmd = cmd + " " + WhereSection;
            }
            

            return cmd;


        }

    }
}