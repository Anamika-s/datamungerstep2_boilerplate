using System;
namespace DbEngine
{
    /* This class is used for storing name of field, aggregate function for 
 * each aggregate function
 * generate properties for this class,
 * Also override toString method
 * */
    public class AggregateFunction
    {
        String field = "";
        String functions = "";
        // Write logic for constructor
        public AggregateFunction(string field, string function)
        {
            this.field = field;
            this.functions = function;
        }
        public String toString()
        {
            return "AggregateFunction{" +
                    "field='" + field + '\'' +
                    ", functions='" + functions + '\'' +
                    '}';
        }
    }
}
