using System.ComponentModel.DataAnnotations;

namespace He.AspNetCore.Mvc.Gds.Components.Enums
{
    /// <summary>
    /// Enum to facilitate rendering of the 'Suffix' to input fields e.g. 'per month'.
    /// </summary>
    public enum GdsInputSuffixText
    {
        /// <summary>
        /// Per month.
        /// </summary>
        [Display(Name = "a month")]
        PerMonth = 1,

        /// <summary>
        /// Per year.
        /// </summary>
        [Display(Name = "a year")]
        PerYear,

        /// <summary>
        /// Percent.
        /// </summary>
        [Display(Name = "%")]
        Percent,

        /// <summary>
        /// Number of years.
        /// </summary>
        [Display(Name = "years")]
        NumberOfYears,

        /// <summary>
        /// Peer week.
        /// </summary>
        [Display(Name = "per week")]
        PerWeek,
        /// <summary>
        /// Square meters.
        /// </summary>
        [Display(Name = "square meters")]
        SquareMeters,
    }
}
