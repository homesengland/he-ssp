using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Contract.Site.Commands;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Site.CommandHandlers;

public class CreateSiteCommandHandler : IRequestHandler<CreateSiteCommand, OperationResult<FrontDoorSiteId>>
{
    public Task<OperationResult<FrontDoorSiteId>> Handle(CreateSiteCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new OperationResult<FrontDoorSiteId>());
    }
}
