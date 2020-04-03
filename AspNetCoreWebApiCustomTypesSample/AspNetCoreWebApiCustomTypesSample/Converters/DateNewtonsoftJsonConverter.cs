using Newtonsoft.Json;
using System;
using System.Globalization;

namespace AspNetCoreWebApiCustomTypesSample.Converters
{
    public class DateNewtonsoftJsonConverter : JsonConverter<Date>
    {
        private const string Format = "yyyy-MM-dd";

        public override Date ReadJson(JsonReader reader, Type objectType, Date existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            DateTime dateTime = DateTime.ParseExact((string)reader.Value, Format, CultureInfo.InvariantCulture);
            return new Date(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        public override void WriteJson(JsonWriter writer, Date value, JsonSerializer serializer)
        {
            writer.WriteValue(new DateTime(value.Year, value.Month, value.Day).ToString(Format));
        }
    }
}