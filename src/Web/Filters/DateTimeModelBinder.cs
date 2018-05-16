using System;
using System.Globalization;
using System.Web.Mvc;

namespace BigBallz.Filters
{
    public class DateTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);

            if (!string.IsNullOrWhiteSpace(value?.AttemptedValue) && value.AttemptedValue.Contains("T"))
            {
                var dateTime = DateTime.ParseExact(value.AttemptedValue, "yyyy-MM-ddTHH-mm", CultureInfo.InvariantCulture);
                return dateTime;
            }

            return value?.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
        }
    }
}