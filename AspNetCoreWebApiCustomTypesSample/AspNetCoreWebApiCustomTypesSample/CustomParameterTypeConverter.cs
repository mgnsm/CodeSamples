using System;
using System.ComponentModel;

namespace AspNetCoreWebApiCustomTypesSample
{
    public class CustomParameterTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == typeof(string);
    }
}