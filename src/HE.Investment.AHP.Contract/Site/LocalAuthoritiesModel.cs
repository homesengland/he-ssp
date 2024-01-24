namespace HE.Investment.AHP.Contract.Site;

public class LocalAuthoritiesModel
{
    public IList<LocalAuthorityModel>? Items { get; set; }

    public string? Phrase { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalItems { get; set; }

    public string SiteId { get; set; }

    public string? LocalAuthorityId { get; set; }

    public string? LocalAuthorityName { get; set; }
}
