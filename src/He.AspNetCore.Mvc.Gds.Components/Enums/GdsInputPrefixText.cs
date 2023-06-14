using System.ComponentModel.DataAnnotations;

namespace He.AspNetCore.Mvc.Gds.Components.Enums
{
    /// <summary>
    /// Enum to facilitate rendering of the 'Prefix' to input fields e.g. 'L'.
    /// </summary>
    public enum GdsInputPrefixText
    {
        /// <summary>
        /// Pounds.
        /// </summary>
        [Display(Name= "£")]
        Pounds = 1,

        /// <summary>
        /// Pounds.
        /// </summary>
        [Display(Name = "%")]
        Percentage = 2,
    }
}
