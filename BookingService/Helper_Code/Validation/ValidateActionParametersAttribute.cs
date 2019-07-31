using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace BookingService.Helper_Code.Validation
{
    /// <summary>
    /// Validates all the validation attributes placed directly on action method parameters.
    /// </summary>
    /// <remarks>
    /// The framework by default doesn't evaluate the validation attributes put directly on action method parameters. It only evaluates the attributes put on the properties of the model types.
    /// This filter validates the attributes placed directly on action method parameters, and adds all the validation errors to the ModelState collection.
    /// </remarks>
    public class ValidateActionParametersAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext context)
        {
            var descriptor = context.ActionDescriptor as HttpActionDescriptor;
            if (descriptor != null)
            {
                var parameters = descriptor.GetParameters();

                foreach (var parameter in parameters)
                {
                    var argument = context.ActionArguments.ContainsKey(parameter.ParameterName) ?
                        context.ActionArguments[parameter.ParameterName] : null;

                    EvaluateValidationAttributes(parameter, argument, context.ModelState);
                }
            }

            base.OnActionExecuting(context);
        }

        private void EvaluateValidationAttributes(HttpParameterDescriptor parameter, object argument, ModelStateDictionary modelState)
        {
            var validationAttributes = parameter.GetCustomAttributes<ValidationAttribute>();

            foreach (ValidationAttribute validationAttribute in validationAttributes)
            {
                if (!validationAttribute.IsValid(argument))
                {
                    if (parameter.GetCustomAttributes<FromBodyAttribute>().Count > 0 && String.IsNullOrEmpty(argument?.ToString()))
                    {
                        modelState.AddModelError(parameter.ParameterName, "Request body must be not empty");
                    }
                    else
                    {
                        modelState.AddModelError(parameter.ParameterName, validationAttribute.FormatErrorMessage(parameter.ParameterName));
                    }
                }
            }
        }
    }
}