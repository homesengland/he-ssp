namespace HE.Investment.AHP.Contract.Site.Commands;

public interface IProvideSiteDetailsCommand
{
    public SiteId SiteId { get; init; }
}
