using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Site.Commands;

public record ProvideSiteNameCommand(FrontDoorProjectId ProjectId, FrontDoorSiteId SiteId, string? Name) : IRequest<OperationResult>;
