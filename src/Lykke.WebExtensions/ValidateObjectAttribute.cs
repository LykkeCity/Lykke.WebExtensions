using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lykke.WebExtensions
{
    public class ValidateObjectAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(value, null, null);

            Validator.TryValidateObject(value, context, results, true);

            if (results.Count != 0)
            {
                var compositeResults = new CompositeValidationResult($"Validation for {validationContext.DisplayName} failed!");
                results.ForEach(compositeResults.AddResult);

                return compositeResults;
            }

            return ValidationResult.Success;
        }
    }
}