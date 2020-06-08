using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceSettingsValidations
{
    /// <summary>
    /// Composite validation model which aggregate validation errors from nested objects
    /// </summary>
    public class CompositeValidationResult : ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        /// <summary>
        /// Collection of validation results from nested objects
        /// </summary>
        public IEnumerable<ValidationResult> Results => _results;

        public CompositeValidationResult(string errorMessage) : base(errorMessage) { }

        /// <summary>
        /// Method add single validation result to collection of validation results
        /// </summary>
        /// <param name="validationResult">Single validation result</param>
        public void AddResult(ValidationResult validationResult)
        {
            _results.Add(validationResult);
        }
    }
}