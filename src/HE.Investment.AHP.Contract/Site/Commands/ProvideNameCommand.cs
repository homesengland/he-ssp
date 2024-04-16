using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Shared.Project;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideNameCommand(SiteId? SiteId, FrontDoorProjectId? FrontDoorProjectId, FrontDoorSiteId? FrontDoorSiteId, string? Name)
    : IRequest<OperationResult<SiteId>>;
