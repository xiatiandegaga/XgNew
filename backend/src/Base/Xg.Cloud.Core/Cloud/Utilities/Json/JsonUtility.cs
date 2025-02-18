using System;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Cloud.Utilities.Json
{
    public static class JsonUtility
    {
        //static JsonUtility()
        //{
        //    JsonSerializerSettings setting = new JsonSerializerSettings();
        //    JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
        //    {
        //        setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
        //        setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        //        setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
        //        //setting.Converters.Add(new LongJsonConverter());
        //        return setting;
        //    });
        //}


        //public static string Serialize(object obj)
        //{
        //    return JsonConvert.SerializeObject(obj);
        //}

        //public static string SerializeByConverter(object obj, params JsonConverter[] converters)
        //{
        //    return JsonConvert.SerializeObject(obj, converters);
        //}

        //public static T Deserialize<T>(string input)
        //{
        //    return JsonConvert.DeserializeObject<T>(input);
        //}

        //public static T DeserializeByConverter<T>(string input, params JsonConverter[] converter)
        //{
        //    return JsonConvert.DeserializeObject<T>(input, converter);
        //}

        //public static T DeserializeBySetting<T>(string input, JsonSerializerSettings settings)
        //{
        //    return JsonConvert.DeserializeObject<T>(input, settings);
        //}

        public static readonly JsonSerializerOptions jsonSerializerOptions = new();
        static JsonUtility()
        {
            InitJsonOptions(jsonSerializerOptions);
        }


        public static string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, jsonSerializerOptions);
        }

        public static T Deserialize<T>(string input)
        {
            if (string.IsNullOrEmpty(input))
                return default(T);
            return JsonSerializer.Deserialize<T>(input, jsonSerializerOptions);
        }

        public static byte[] SerializeByte(object obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj, jsonSerializerOptions);
        }

        public static T DeserializeByte<T>(byte[] input)
        {
            return JsonSerializer.Deserialize<T>(input, jsonSerializerOptions);
        }

        public static void  InitJsonOptions(JsonSerializerOptions options)
        {
            options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            options.PropertyNameCaseInsensitive = true;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Converters.Add(new TextJsonConvert.DateTimeConverter());
            options.Converters.Add(new TextJsonConvert.DateTimeNullableConverter());
            options.Converters.Add(new TextJsonConvert.LongConverter());
            options.Converters.Add(new TextJsonConvert.Int32Converter());
            options.Converters.Add(new TextJsonConvert.DecimalConverter());
            options.Converters.Add(new TextJsonConvert.StringConverter());
        }

    }

}
