using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace He.AspNetCore.Mvc.Gds.Components.Validation
{
    /// <summary>
    /// Attribute for validating a Numeric input is within a range.
    /// </summary>
    public class NumericRangeValidationAttribute : NumericValidationAttribute
    {
        private readonly bool _allowEmpty;
        private readonly int _minValue;
        private readonly int _maxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericRangeValidationAttribute"/> class.
        /// </summary>
        /// <param name="minValue">The min value of range.</param>
        /// <param name="maxValue">The max value of range.</param>
        /// <param name="allowEmpty">Allowing empty value.</param>
        public NumericRangeValidationAttribute(int minValue, int maxValue, bool allowEmpty = false)
        {
            this._minValue = minValue;
            this._maxValue = maxValue;
            this._allowEmpty = allowEmpty;
        }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var validationResult = ValidationResult.Success;

            if (this._allowEmpty && string.IsNullOrEmpty(value?.ToString()))
            {
                return validationResult;
            }

            validationResult = base.IsValid(value, validationContext);

            if (validationResult == ValidationResult.Success)
            {
                _ = int.TryParse(value?.ToString(), out var res);
                if (res < this._minValue || res > this._maxValue)
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
