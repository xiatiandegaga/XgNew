namespace Cloud.Utilities.Json
{
    /// <summary>
    /// 大数字序列化器
    /// </summary>
    public class LongJsonConverter
    {
        //public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => JValue.ReadFrom(reader).Value<long>();

        //public override bool CanConvert(Type objectType)=> typeof(long).Equals(objectType);

        //public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)=> serializer.Serialize(writer, value.ToString());
    }
}
