using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceSettingsValidations
{
    /// <summary>
    /// Validate object attribute used to mark nested objects as validable
    /// </summary>
    public class ValidateObjectAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validate object method
        /// </summary>
        /// <param name="value">Object to validate</param>
        /// <param name="validationContext">Validation context</param>
        /// <returns>ValidationResult</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is IEnumerable list))
            {
                var results = ValidateObject(value);
                return CreateValidationResult(results, $"Validation for {validationContext.DisplayName} failed!");
            }
            else
            {
                var aggregatedValidationResults = new List<ValidationResult>();
                int arrayIndex = 0;
                foreach (var item in list)
                {
                    var itemValidationResults = ValidateObject(item);
                    itemValidationResults.ForEach(itemR => itemR.ErrorMessage = $"{validationContext.DisplayName}[{arrayIndex}]:{itemR.ErrorMessage}");
                    aggregatedValidationResults.AddRange(itemValidationResults);
                    arrayIndex++;
                }
                return CreateValidationResult(aggregatedValidationResults, $"Validation for IEnumerable {validationContext.DisplayName} failed!");
            }
        }

        private List<ValidationResult> ValidateObject(object objectToValidate)
        {
            List<ValidationResult> objectValidationResults = new List<ValidationResult>();
            var context = new ValidationContext(objectToValidate, null, null);
            Validator.TryValidateObject(objectToValidate, context, objectValidationResults, true);
            return objectValidationResults;
        }

        private ValidationResult CreateValidationResult(List<ValidationResult> validationResults, string validationErrorMessage)
        {
            if (validationResults.Count == 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                var compositeResults = new CompositeValidationResult(validationErrorMessage);
                validationResults.ForEach(compositeResults.AddResult);
                return compositeResults;
            }
        }
    }
}