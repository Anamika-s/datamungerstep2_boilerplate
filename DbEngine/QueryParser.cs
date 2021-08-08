using System;
using System.Collections;
using System.Collections.Generic;
namespace DbEngine
{
    public class QueryParser
    {
        private QueryParameter queryParameter;
        public QueryParser()
        {
            queryParameter = new QueryParameter();
        }

        /*
	 * this method will parse the queryString and will return the object of
	 * QueryParameter class
	 */

        public QueryParameter parseQuery(string queryString)
        {
            // ADDED CODE HERE
            queryParameter.setFileName(getFile(queryString));
            queryParameter.setBaseQuery(getBase(queryString));
            queryParameter.setOrderByFields(GetOrderByFields(queryString));
            queryParameter.setGroupByFields(GetGroupByFields(queryString));
            queryParameter.setLogicalOperators(GetLogicalOperators(queryString));
            queryParameter.setFields(GetFields(queryString));
            queryParameter.setAggregateFunctions(GetAggregateFunctions(queryString));
            queryParameter.setRestrictions(GetRestrictions(queryString));


            return queryParameter;
        }

        public String getBase(String queryString)
        {
            String[] splitString = queryString.Split(" where | group by "); //to split the given array into where and group by
            return splitString[0]; // return the splitted new array

        }
        public String getFile(String queryString)
        {
            String queryLowerCase = queryString.ToLower(); //to convert the original string to LowerCase
            String[] result_String = queryLowerCase.Split("from"); //to split til from to get the file name
            String[] result_String1 = result_String[1].Split(" "); //to split and extract the first word from the second array
            String result = result_String1[1]; //return type is String, store the array[] to a String variable and return the String
            return result;
        }
        /*
	 * extract the selected fields from the query string. Please note that we will
	 * need to extract the field(s) after "select" clause followed by a space from
	 * the query string. For eg: select city,win_by_runs from data/ipl.csv from the
	 * query mentioned above, we need to extract "city" and "win_by_runs". Please
	 * note that we might have a field containing name "from_date" or "from_hrs".
	 * Hence, consider this while parsing.
	 */
        private List<string> GetFields(string queryString)
        {
            queryString = queryString.ToLower();
            String[] spiltFrom = queryString.Split(" from"); // to split the array with from
            String[] splitSelect = spiltFrom[0].Split("select "); // to split the array with select
            String[] resultString = splitSelect[1].Split(","); // to split the array with (,)

            List<String> fieldsList = new List<String>(); //to create a new arraylist
            for (int i = 0; i < resultString.Length; i++) // to loop through the resultString array
                fieldsList.Add(resultString[i]); //add the array to the list

            return fieldsList; //to return the fieldlist array
        }


        /*
	 * extract the conditions from the query string(if exists). for each condition,
	 * we need to capture the following: 1. Name of field 2. condition 3. value
	 * 
	 * For eg: select city,winner,team1,team2,player_of_match from data/ipl.csv
	 * where season >= 2008 or toss_decision != bat
	 * 
	 * here, for the first condition, "season>=2008" we need to capture: 1. Name of
	 * field: season 2. condition: >= 3. value: 2008 Also use trim() where ever
	 * required
	 * 
	 * the query might contain multiple conditions separated by OR/AND operators.
	 * Please consider this while parsing the conditions .
	 * 
	 */
        public String ConditionsPartQueryExtractor(String queryString)
        {
            if (queryString.Contains(" where "))
            {  // to check if query string contains 'where'
               //queryString = queryString.toLowerCase();
                String[] trimFrom = queryString.Split("where ");
                queryString = trimFrom[1];
                String[] splitOrderby = queryString.Split(" order by | group by ");
                queryString = splitOrderby[0];
                return queryString;
            }
            else
                return queryString;
        }
        private List<FilterCondition> GetRestrictions(string queryString)
        {
            List<FilterCondition> list = new List<FilterCondition>();
            if (queryString.Contains("where"))
            {
                queryString = ConditionsPartQueryExtractor(queryString);
                String[] trimOrderby = queryString.Split(" order by | group by ");
                queryString = trimOrderby[0];
                String[] finalStr = { queryString };
                //System.out.println(queryString);
                if (queryString.Contains("and") || queryString.Contains("or")) ;
                {
                    String strLeft, strRight;

                    finalStr = queryString.Split(" and | or ");

                    for (int i = 0; i < finalStr.Length; i++)
                    {
                        if (finalStr[i].Contains("="))
                        {
                            String[] tokens1 = finalStr[i].Split("=");
                            FilterCondition res1 = new FilterCondition(tokens1[0].Trim().Replace("'", ""), tokens1[1].Trim().Replace("'", ""), "=");

                            list.Add(res1);

                        }
                        else
                        {
                            String[] tokens1 = finalStr[i].Split(" ");
                            FilterCondition res1 = new FilterCondition(tokens1[0].Trim().Replace("'", ""), tokens1[2].Trim().Replace("'", ""), tokens1[1].Trim().Replace("'", ""));

                            list.Add(res1);
                        }

                    }

                }

                return list;
            }
            else
                return null;
        }

        /*
	 * extract the logical operators(AND/OR) from the query, if at all it is
	 * present. For eg: select city,winner,team1,team2,player_of_match from
	 * data/ipl.csv where season >= 2008 or toss_decision != bat and city =
	 * bangalore
	 * 
	 * the query mentioned above in the example should return a List of Strings
	 * containing [or,and]
	 */

        private List<string> GetLogicalOperators(string queryString)
        {
            if (queryString.Contains("where"))
            {
                List<String> finalResList = new List<String>();

                queryString = ConditionsPartQueryExtractor(queryString);
                String[] strArr = queryString.Split(" ");
                for (int i = 0; i < strArr.Length; i++)
                {
                    if (strArr[i].Equals("and"))
                    {
                        finalResList.Add("and");
                    }
                    else if (strArr[i].Equals("or"))
                    {
                        finalResList.Add("or");
                    }
                }
                return finalResList;
            }
            else
                return null;
        }

        /*
             * extract the aggregate functions from the query. The presence of the aggregate
             * functions can determined if we have either "min" or "max" or "sum" or "count"
             * or "avg" followed by opening braces"(" after "select" clause in the query
             * string. in case it is present, then we will have to extract the same. For
             * each aggregate functions, we need to know the following: 1. type of aggregate
             * function(min/max/count/sum/avg) 2. field on which the aggregate function is
             * being applied
             * 
             * Please note that more than one aggregate function can be present in a query
             * 
             * 
             */
        private List<AggregateFunction> GetAggregateFunctions(string queryString)
        {
            List<AggregateFunction> aggregateList = new List<AggregateFunction>();

            String[] trimSelect = queryString.Split("select ");
            String[] trimFrom = trimSelect[1].Split(" from");
            String[] trimAggregate = trimFrom[0].Split(",");
            ArrayList list = new ArrayList();
            int k = 0;
            for (int i = 0; i < trimAggregate.Length; i++)
            { // to loop through the Aggregate Array

                if (trimAggregate[i].Contains("sum(") || trimAggregate[i].Contains("count(") || trimAggregate[i].Contains("min(") || trimAggregate[i].Contains("max(") || trimAggregate[i].Contains("avg("))
                { // check the condition if it has sum,min,max,avg,count
                    String[] trimLeft = trimAggregate[i].Split("(");
                    if (trimLeft[0].Contains("sum"))
                    {
                        String[] trimRight = { trimLeft[1].Remove(trimLeft[1].Length - 1, 1) };
                        //String[] trimRight = trimLeft[0].Split("\\)");
                        AggregateFunction obj = new AggregateFunction(trimRight[0], "sum");
                        aggregateList.Add(obj);
                    }
                    else if (trimLeft[0].Contains("count"))
                    {
                        String[] trimRight = { trimLeft[1].Remove(trimLeft[1].Length - 1, 1) };
                        AggregateFunction obj = new AggregateFunction(trimRight[0], "count");
                        aggregateList.Add(obj);
                    }
                    else if (trimLeft[0].Contains("min"))
                    {
                        String[] trimRight = { trimLeft[1].Remove(trimLeft[1].Length - 1, 1) };
                        //String[] trimRight = trimLeft[0].Split("\\)");
                        AggregateFunction obj = new AggregateFunction(trimRight[0], "min");
                        aggregateList.Add(obj);
                    }
                    else if (trimLeft[0].Contains("max"))
                    {
                        //string temp = trimLeft[1].Split("\\)").ToString();

                        String[] trimRight = { trimLeft[1].Remove(trimLeft[1].Length - 1, 1) };
                        //String[] trimRight = trimLeft[0].Split("\\)");
                        AggregateFunction obj = new AggregateFunction(trimRight[0], "max");
                        aggregateList.Add(obj);
                    }
                    else if (trimLeft[0].Contains("avg"))
                    {
                        String[] trimRight = { trimLeft[1].Remove(trimLeft[1].Length - 1, 1) };
                        AggregateFunction obj = new AggregateFunction(trimRight[0], "avg");
                        aggregateList.Add(obj);
                    }
                }

            }

            return aggregateList;
        }

    //      /*
    //* extract the order by fields from the query string. Please note that we will
    //* need to extract the field(s) after "order by" clause in the query, if at all
    //* the order by clause exists. For eg: select city,winner,team1,team2 from
    //* data/ipl.csv order by city from the query mentioned above, we need to extract
    //* "city". Please note that we can have more than one order by fields.
    //*/
    private List<string> GetOrderByFields(string queryString)
        {

            String[] conditionQuery = queryString.ToLower().Split("from");
            int countOrderBy = conditionQuery[1].Trim().IndexOf(" order by ");
            //	System.out.println(conditionQuery[1].trim());

            if (countOrderBy == -1)
                return null;
            String[] splitedCondition = conditionQuery[1].Trim().Split(" order by ");
            String[] orderByFields = splitedCondition[1].Trim().Split(",");

            List<String> splitLst = new List<string>();
            for (int i = 0; i < orderByFields.Length; i++)
            {
                splitLst.Add(orderByFields[i].Trim());
            }
            return splitLst;
        }

        /*
	 * extract the group by fields from the query string. Please note that we will
	 * need to extract the field(s) after "group by" clause in the query, if at all
	 * the group by clause exists. For eg: select city,max(win_by_runs) from
	 * data/ipl.csv group by city from the query mentioned above, we need to extract
	 * "city". Please note that we can have more than one group by fields.
	 */
        private List<string> GetGroupByFields(string queryString)
        {
            String[] conditionQuery = queryString.ToLower().Split(" from ");

            int countGroupBy = conditionQuery[1].Trim().IndexOf(" group by ");
            //	System.out.println(conditionQuery[1].trim());

            if (countGroupBy == -1)
                return null;
            String[] splitedCondition = conditionQuery[1].Trim().Split(" group by ");

            String[] splitOrderBy= splitedCondition[1].Trim().Split(" order by ");

            String[] groupByFields = splitOrderBy[0].Trim().Split(",");

            List<String> splitLst = new List<string>();
            for (int i = 0; i < groupByFields.Length; i++)
            {
                splitLst.Add(groupByFields[i].Trim());
            }
            return splitLst;
        }
    }
}
