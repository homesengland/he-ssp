using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Site.Commands;

public interface IProvideSiteDetailsCommand : IRequest<OperationResult>
{
    public FrontDoorSiteId SiteId { get; }
}
