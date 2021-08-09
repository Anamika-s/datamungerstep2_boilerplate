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
        String function= "";
		// Write logic for constructor
		public AggregateFunction(String field, String function)
		{
			 
			this.function = function;
			this.field = field;
		}

		public void setFunction(String function)
		{
			this.function = function;
		}

		public void setField(String field)
		{
			this.field = field;
		}

		public String getFunction()
		{
			// TODO Auto-generated method stub
			return function;
		}

		public String getField()
		{
			// TODO Auto-generated method stub
			return field;
		}
	}
}
