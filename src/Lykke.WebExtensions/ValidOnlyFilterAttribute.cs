using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lykke.WebExtensions
{
    public class ValidOnlyFilterAttribute : ActionFilterAttribute
    {
        private readonly string _parameterName;

        public ValidOnlyFilterAttribute(string parameterName = null)
        {
            _parameterName = parameterName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ValidateParameters(
                !string.IsNullOrWhiteSpace(_parameterName)
                    ? filterContext.ActionArguments.Where(x => x.Key == _parameterName)
                    : filterContext.ActionArguments, filterContext);
        }

        private static void ValidateParameters(IEnumerable<KeyValuePair<string, object>> parameters, ActionExecutingContext filterContext)
        {
            var results = new List<ValidationResult>();
            var isValid = true;
            foreach (var parameter in parameters)
            {
                var parameterValue = parameter.Value;
                var context = new ValidationContext(parameterValue, null, null);
                isValid &= Validator.TryValidateObject(parameterValue, context, results, true);
            }
            if (!isValid)
            {
                filterContext.Result = new BadRequestWithMessageResult(GetErrorMessage(results));
            }
        }

        public static string GetErrorMessage(ICollection<ValidationResult> results)
        {
            var errorList = new List<string>(results.Count);
            errorList.AddRange(
                from entry
                in results
                where !string.IsNullOrWhiteSpace(entry.ErrorMessage)
                select $"{string.Join(", ", entry.MemberNames)}: {entry.ErrorMessage}");
            return string.Join(" ", errorList);
        }
    }
}
