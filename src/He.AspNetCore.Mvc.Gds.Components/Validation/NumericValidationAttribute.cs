using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace He.AspNetCore.Mvc.Gds.Components.Validation
{
    /// <summary>
    /// Attribute for validating a Numeric input
    ///  - field must be a number
    ///  - field must be 0 or more.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NumericValidationAttribute : ValidationAttribute
    {
        /// <summary>Initializes a new instance of the <see cref="NumericValidationAttribute" /> class.</summary>
        /// <param name="allowEmpty">if set to <c>true</c> [allow empty].</param>
        public NumericValidationAttribute(bool allowEmpty = false)
        {
            this.AllowEmpty = allowEmpty;
        }

        private bool AllowEmpty { get; set; }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;

            if (this.AllowEmpty && string.IsNullOrEmpty(value?.ToString()))
            {
                return validationResult;
            }

            var errorFields = new List<string> { validationContext?.MemberName };

            // Check value is positive, and valid as an int type (not above Int32.MaxValue)
            var isInt = int.TryParse(value?.ToString(), out var res);

            if (!isInt || res < 0)
            {
                validationResult = new ValidationResult(
                    this.ErrorMessage,
                    errorFields);
            }

            return validationResult;
        }
    }
}
