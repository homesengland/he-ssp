using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace He.AspNetCore.Mvc.Gds.Components.Validation
{
    /// <summary>
    /// Marks the current field as error if the linked field exceeds max length constraint.
    /// </summary>
    public class MaxLengthLinkedFieldAttribute : ValidationAttribute
    {
        private readonly long maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxLengthLinkedFieldAttribute"/> class.
        /// </summary>
        /// <param name="maxLength">The maximum allowed length for the linked field.</param>
        public MaxLengthLinkedFieldAttribute(long maxLength)
        {
            this.maxLength = maxLength;
        }

        /// <summary>
        /// Gets or sets the linked field.
        /// </summary>
        public string LinkedField { get; set; }

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var validationResult = ValidationResult.Success;

            var linkedField = validationContext?.ObjectType.GetProperty(this.LinkedField);

            if (linkedField != null)
            {
                var linkedFieldString = linkedField.GetValue(validationContext.ObjectInstance) as string;

                if (linkedFieldString?.Length > this.maxLength)
                {
                    validationResult = new ValidationResult(this.ErrorMessage, new List<string>() { validationContext.MemberName });
                }
            }

            return validationResult;
        }
    }
}
