using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Site.Commands;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Site.CommandHandlers;

public class ProvideSiteNameCommandHandler : IRequestHandler<ProvideSiteNameCommand, OperationResult>
{
    public Task<OperationResult> Handle(ProvideSiteNameCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(OperationResult.Success());
    }
}
