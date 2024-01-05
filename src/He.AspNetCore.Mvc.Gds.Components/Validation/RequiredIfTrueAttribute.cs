using System;
using System.ComponentModel.DataAnnotations;

namespace He.AspNetCore.Mvc.Gds.Components.Validation
{
    /// <summary>
    /// Attribute for making a field required if another boolean field is true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfTrueAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredIfTrueAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public RequiredIfTrueAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        private string PropertyName { get; set; }

        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="validationContext">The context.</param>
        /// <returns>ValidationResult.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext?.ObjectInstance;
            var type = instance.GetType();

            _ = bool.TryParse(type?.GetProperty(this.PropertyName)?.GetValue(instance)?.ToString(), out var propertyValue);

            if (propertyValue && string.IsNullOrEmpty(value?.ToString()))
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
