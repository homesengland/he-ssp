namespace HE.Investments.Account.Shared.Routing;

public static class OrganisationAccountEndpoints
{
    public const string Controller = "organisation";

    public const string SearchOrganisation = $"{Controller}/{SearchOrganisationSuffix}";

    public const string SearchOrganisationSuffix = "search";
}
