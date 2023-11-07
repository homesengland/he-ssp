namespace HE.Investments.Common.Errors;

public static class ValidationErrorMessages
{
    public static string MustBeNumber(string displayName) => $"The {displayName} must be a number";

    public static string InvalidLength(string displayName, int maxDigits) => $"The {displayName} must be {maxDigits} digits or less";
}
