using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;
using System.Globalization;

namespace AspNetCoreWebApiCustomTypesSample.ModelBinders
{
    public class DateModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            string modelName = bindingContext?.ModelName;
            if (!string.IsNullOrEmpty(modelName))
            {
                ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
                if (valueProviderResult != ValueProviderResult.None)
                {
                    bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

                    if (!DateTime.TryParse(valueProviderResult.FirstValue, 
                        CultureInfo.InvariantCulture, 
                        DateTimeStyles.None, 
                        out DateTime dateTime))
                    {
                        bindingContext.ModelState.TryAddModelError(modelName, "Invalid date.");
                        return Task.CompletedTask;
                    }
                    else
                    {
                        bindingContext.Result = 
                            ModelBindingResult.Success(new Date(dateTime.Year, dateTime.Month, dateTime.Day));
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}