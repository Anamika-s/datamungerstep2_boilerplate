using System;
using System.Collections.Generic;
namespace DbEngine
{
    /* 
 * This class will contain the elements of the parsed Query String such as conditions,
 * logical operators,aggregate functions, file name, fields group by fields, order by
 * fields, Query Type
 * */
    public class QueryParameter
    {
        public string QueryString { get; set; }
        public List<string> LogicalOperators { get; set; }
        public string FileName { get; set; }
        public string BaseQuery { get; set; }
        public string Query_Type { get; set; }
        public List<string> Fields { get; set; }
        public List<string> GroupByFields { get; set; }
        public List<string> OrderByFields { get; set; }
        public List<FilterCondition> Restrictions { get; set; }
        public List<AggregateFunction> AggregateFunctions { get; set; }
		/*File name setter*/
		public void setFileName(String fileName)
		{

			this.FileName = fileName;
		}
		/*file name setter*/
		public String getFileName() { return FileName; }

		/*Base query setter*/
		public void setBaseQuery(String baseQuery)
		{
			this.BaseQuery = baseQuery;
		}
		/*Base Query getter*/
		public String getBaseQuery() { return BaseQuery; }

		/* Set fields */
		public void setFields(List<string> fields)
		{
			this.Fields = fields;
		}

		/* get fields */
		public List<String> getFields()
		{
			return Fields;
		}


		/*Group By Setter*/
		public void setGroupByFields(List<String> groupByFields)
		{
			this.GroupByFields = groupByFields;
		}

		/*Group By getter*/
		public List<String> getGroupByFields() { return GroupByFields; }

		/*Order By setter*/
		public void setOrderByFields(List<String> orderByFields)
		{
			this.OrderByFields = orderByFields;
		}

		/*OrderBy getter*/
		public List<String> getOrderByFields()
		{
			return OrderByFields;
		}

		/*Logical operator Setter*/
		public void setLogicalOperators(List<string> logicalOperators)
		{
			this.LogicalOperators = logicalOperators;
		}

		/*Logical operator getter*/
		public List<String> getLogicalOperators() { return LogicalOperators; }

		/*Aggregate function Setter*/
		public void setAggregateFunctions(List<AggregateFunction> aggregateFunctions)
		{
			this.AggregateFunctions = aggregateFunctions;
		}

		/* Aggregate function getter*/
		public List<AggregateFunction> getAggregateFunctions()
		{
			return AggregateFunctions;
		}

        ///*Restriction function Setter */
        public void setRestrictions(List<FilterCondition> RestrictionFunctions)
        {
            this.Restrictions = RestrictionFunctions;
        }
        /*Restriction function getter */

        public List<FilterCondition> getRestrictions()
        {
            return Restrictions;
        }
    }
}
