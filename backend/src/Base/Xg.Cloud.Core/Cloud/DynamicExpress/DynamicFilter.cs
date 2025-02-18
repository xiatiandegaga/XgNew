namespace Cloud.DynamicExpress
{
    public class DynamicFilter
    {
        public DynamicFilter() { }

        public string PropertyName { get; set; }
        public Operation Op { get; set; }
        public string Value { get; set; }
    }
}
