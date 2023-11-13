namespace HE.InvestmentLoans.Contract.Projects.ViewModels;
public class LocalAuthoritiesViewModel
{
    public IList<LocalAuthorityViewModel>? Items { get; set; }

    public string? Phrase { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalItems { get; set; }

    public Guid ApplicationId { get; set; }

    public Guid ProjectId { get; set; }

    public string? LocalAuthorityId { get; set; }
}
