namespace DbEngine
{
    /*
 * This class is used for storing name of field, condition and value for 
 * each conditions
 * generate properties for this class,
 * Also override toString method
 * */
    public class FilterCondition
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        // Write logic for constructor
        public FilterCondition(string propertyName,string propertyValue, string condition)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

    }
}
