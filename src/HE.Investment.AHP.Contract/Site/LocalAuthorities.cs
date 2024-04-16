using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.Contract.Site;

public class LocalAuthorities
{
    public string? Phrase { get; set; }

    public PaginationResult<LocalAuthority>? Page { get; set; }

    public string SiteId { get; set; }

    public string? LocalAuthorityCode { get; set; }

    public string? LocalAuthorityName { get; set; }
}
