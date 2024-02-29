public class DataModel
{
    public string DataModelName { get; set; }
    public string DataModelTag { get; set; }
    public string StartTime { get; set; }
    public List<Field> Fields { get; set; }
}

public class Field
{
    public string FieldName { get; set; }
    public string DataType { get; set; }
    public double MinValue { get; set; }
    public double MaxValue { get; set; }
}