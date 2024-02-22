namespace HE.Investment.AHP.Domain.Application.Constants;

public static class ApplicationValidationErrors
{
    public const string MissingConfirmation = "Select that you confirm the following before submitting";

    public static string EnterChangeStatusReason(string displayName) => $"Enter why you {displayName} this application";
}
