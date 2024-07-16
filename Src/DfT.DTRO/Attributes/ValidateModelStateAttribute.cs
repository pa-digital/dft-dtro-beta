using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DfT.DTRO.Attributes;

public class ValidateModelStateAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
        if (descriptor != null)
        {
            foreach (var parameter in descriptor.MethodInfo.GetParameters())
            {
                object args = null;
                if (context.ActionArguments.ContainsKey(parameter.Name))
                {
                    args = context.ActionArguments[parameter.Name];
                }

                ValidateAttributes(parameter, args, context.ModelState);
            }
        }

        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }

    private void ValidateAttributes(ParameterInfo parameter, object args, ModelStateDictionary modelState)
    {
        foreach (var attributeData in parameter.CustomAttributes)
        {
            var attributeInstance = parameter.GetCustomAttribute(attributeData.AttributeType);

            var validationAttribute = attributeInstance as ValidationAttribute;
            if (validationAttribute != null)
            {
                var isValid = validationAttribute.IsValid(args);
                if (!isValid)
                {
                    modelState.AddModelError(parameter.Name, validationAttribute.FormatErrorMessage(parameter.Name));
                }
            }
        }
    }
}
