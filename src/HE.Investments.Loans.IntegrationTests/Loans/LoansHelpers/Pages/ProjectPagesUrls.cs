namespace HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;

internal static class ProjectPagesUrls
{
    public const string StartSuffix = "/project/start";

    public const string NameSuffix = "/name";

    public const string StartDateSuffix = "/start-date";

    public const string ManyHomesSuffix = "/many-homes";

    public const string TypeHomesSuffix = "/type-homes";

    public const string ProjectTypeSuffix = "/type";

    public const string PlanningReferenceNumberExistsSuffix = "/planning-ref-number-exists";

    public const string PlanningReferenceNumberSuffix = "/planning-ref-number";

    public const string PlanningPermissionStatusSuffix = "/planning-permission-status";

    public const string LocationSuffix = "/location";

    public const string OwnershipSuffix = "/ownership";

    public const string LocalAuthoritySearchSuffix = "/local-authority/search";

    public const string LocalAuthorityNotFoundSuffix = "/local-authority/not-found";

    public const string LocalAuthoritySearchResultSuffix = "/local-authority/search/result";

    public const string LocalAuthorityConfirmSuffix = "/confirm";

    public const string AdditionalDetailsSuffix = "/additional-details";

    public const string GrantFundingExistsSuffix = "/grant-funding-exists";

    public const string GrantFundingSuffix = "/grant-funding";

    public const string ChargesDebtSuffix = "/charges-debt";

    public const string AffordableHomesSuffix = "/affordable-homes";

    public const string CheckAnswersSuffix = "/check-answers";

    public const string DeleteSuffix = "/delete";

    public static string Start(string organisationId, string applicationId)
    {
        return $"{organisationId}/application/{applicationId}{StartSuffix}";
    }

    public static string Name(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{NameSuffix}";
    }

    public static string StartDate(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{StartDateSuffix}";
    }

    public static string ManyHomes(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{ManyHomesSuffix}";
    }

    public static string TypeHomes(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{TypeHomesSuffix}";
    }

    public static string ProjectType(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{ProjectTypeSuffix}";
    }

    public static string PlanningReferenceNumberExists(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{PlanningReferenceNumberExistsSuffix}";
    }

    public static string PlanningReferenceNumber(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{PlanningReferenceNumberSuffix}";
    }

    public static string PlanningPermissionStatus(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{PlanningPermissionStatusSuffix}";
    }

    public static string Location(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{LocationSuffix}";
    }

    public static string Ownership(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{OwnershipSuffix}";
    }

    public static string LocalAuthoritySearch(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{LocalAuthoritySearchSuffix}";
    }

    public static string AdditionalDetails(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{AdditionalDetailsSuffix}";
    }

    public static string GrantFundingExists(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{GrantFundingExistsSuffix}";
    }

    public static string GrantFunding(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{GrantFundingSuffix}";
    }

    public static string ChargesDebt(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{ChargesDebtSuffix}";
    }

    public static string AffordableHomes(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{AffordableHomesSuffix}";
    }

    public static string CheckAnswers(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{CheckAnswersSuffix}";
    }

    public static string Delete(string organisationId, string applicationId, string projectId)
    {
        return $"{organisationId}/application/{applicationId}/project/{projectId}{DeleteSuffix}";
    }
}
