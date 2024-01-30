namespace HE.Investment.AHP.Contract.Site.Commands.PlanningDetails;

public interface IProvideSitePlanningDetailsCommand
{
    public SiteId SiteId { get; init; }
}
