namespace HE.Investments.Account.Shared.Routing;

public static class OrganisationAccountEndpoints
{
    public const string Controller = "organisation";

    public const string SearchOrganization = $"{Controller}/{SearchOrganizationSuffix}";

    public const string SearchOrganizationSuffix = "search";
}
