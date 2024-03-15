using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Shared.Project;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Site.Commands;

public interface IProvideSiteDetailsCommand : IRequest<OperationResult>
{
    public FrontDoorProjectId ProjectId { get; }

    public FrontDoorSiteId SiteId { get; }
}
