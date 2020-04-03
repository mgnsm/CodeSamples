using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AspNetCoreWebApiCustomTypesSample.Converters
{
    public class DateJsonConverter : JsonConverter<Date>
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(Date);

        public override Date Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            DateTime dateTime = reader.GetDateTime();
            return new Date(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        public override void Write(Utf8JsonWriter writer, Date value, JsonSerializerOptions options)
        {
            const string Format = "yyyy-MM-dd";
            writer.WriteStringValue(new DateTime(value.Year, value.Month, value.Day).ToString(Format));
        }
    }
}