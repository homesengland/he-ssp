namespace HE.Investments.Common.Validators;

[Flags]
public enum MessageOptions
{
    None = 0,

    /// <summary>
    ///     Value indicating that error message should not contain proper example value.
    /// </summary>
    HideExample = 1,

    /// <summary>
    ///     Value indicating that field is required for calculation and error message should contain such information.
    /// </summary>
    IsCalculation = 2,

    /// <summary>
    ///     Value indicating that field accepts money value so error message contains pounds/pence reference.
    /// </summary>
    IsMoney = 4,
}
