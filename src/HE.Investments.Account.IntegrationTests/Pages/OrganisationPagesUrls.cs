namespace HE.Investments.Account.IntegrationTests.Pages;

public static class OrganisationPagesUrls
{
    public const string Search = "/organisation/search";

    public static string SearchResult(string organisationName) => $"/organisation/search/result?searchPhrase={organisationName.Replace(" ", "%20")}";

    public static string Confirm(string organisationId) => $"/organisation/{organisationId}/confirm";
}
