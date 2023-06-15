using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace He.AspNetCore.Mvc.Gds.Components.Validation
{
    /// <summary>
    /// Attribute for validating a Numeric input is within a range.
    /// </summary>
    public class NumericRangeValidationAttribute : NumericValidationAttribute
    {
        private readonly bool allowEmpty;
        private readonly int minValue;
        private readonly int maxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericRangeValidationAttribute"/> class.
        /// </summary>
        /// <param name="minValue">The min value of range.</param>
        /// <param name="maxValue">The max value of range.</param>
        /// <param name="allowEmpty">Allowing empty value.</param>
        public NumericRangeValidationAttribute(int minValue, int maxValue, bool allowEmpty = false)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.allowEmpty = allowEmpty;
        }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;

            if (this.allowEmpty && string.IsNullOrEmpty(value?.ToString()))
            {
                return validationResult;
            }

            validationResult = base.IsValid(value, validationContext);

            if (validationResult == ValidationResult.Success)
            {
                var isInt = int.TryParse(value?.ToString(), out var res);
                if (res < this.minValue || res > this.maxValue)
                {
                    validationResult = new ValidationResult(
                        this.ErrorMessage,
                        new List<string> { validationContext?.MemberName });
                }
            }

            return validationResult;
        }
    }
}
