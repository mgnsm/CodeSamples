using AspNetCoreWebApiCustomTypesSample.ModelBinders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;

namespace AspNetCoreWebApiCustomTypesSample
{
    public class DateBindingProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(Date))
            {
                return new BinderTypeModelBinder(typeof(DateModelBinder));
            }

            return null;
        }
    }
}