using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServiceSettingsValidations
{
    /// <summary>
    /// Service settings validator extension
    /// </summary>
    public static class SettingsValidatorExtension
    {
        /// <summary>
        /// Extension method to validate
        /// </summary>
        /// <param name="configuration">Configuration object contains all service configurations</param>
        /// <param name="settingsTypes">Service settings models</param>
        public static void ValidateServiceSettings(this IConfiguration configuration, IEnumerable<Type> settingsTypes)
        {
            var allConfigErrors = new List<string>();
            foreach (var type in settingsTypes)
            {
                var section = type.Name;
                var configurationValue = Activator.CreateInstance(type);
                configuration.GetSection(section).Bind(configurationValue);
                var configurationErrors = configurationValue.ValidationErrors();
                if (configurationErrors.Any())
                {
                    allConfigErrors.AddRange(configurationErrors);
                }
            }
            if (allConfigErrors.Any())
            {
                var message = $"Find {allConfigErrors.Count} configuration errors: {allConfigErrors.Aggregate((a, b) => a + "," + b)}";
                throw new ApplicationException(message);
            }
        }

        /// <summary>
        /// Method extend each object by validation errors
        /// </summary>
        /// <param name="this">Any object</param>
        /// <returns>List of validation errors</returns>
        public static List<string> ValidationErrors(this object @this)
        {
            var context = new ValidationContext(@this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(@this, context, results, true);
            var validationErrorMessages = new List<string>();
            foreach (var validationResult in results)
            {
                GetValidationErrors(validationResult, validationErrorMessages);
            }
            return validationErrorMessages;
        }

        /// <summary>
        /// Method recursively retrieves validation error messages 
        /// </summary>
        /// <param name="valResult">Single validation result</param>
        /// <param name="valErrorMessages">Collection of validation error messages</param>
        private static void GetValidationErrors(ValidationResult valResult, ICollection<string> valErrorMessages)
        {
            valErrorMessages.Add(valResult.ErrorMessage);
            if (!(valResult is CompositeValidationResult result))
            {
                return;
            }
            foreach (var ss in ((CompositeValidationResult)result).Results)
            {
                GetValidationErrors(ss, valErrorMessages);
            }
        }
    }
}