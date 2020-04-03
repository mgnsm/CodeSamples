using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Writers;
using System;

namespace AspNetCoreWebApiCustomTypesSample
{
    public class CustomOpenApiDateTime : IOpenApiPrimitive
    {
        public CustomOpenApiDateTime(DateTime value)
        {
            Value = value;
        }

        public AnyType AnyType { get; } = AnyType.Primitive;

        public string Format { get; set; } = "yyyy-MM-dd";

        public PrimitiveType PrimitiveType { get; } = PrimitiveType.DateTime;

        public DateTime Value { get; }

        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion) => writer.WriteValue(Value.ToString(Format));
    }
}