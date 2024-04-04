using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class RequestToEditApplicationCommandHandler : ApplicationCommandHandlerBase, IRequestHandler<RequestToEditApplicationCommand, OperationResult>
{
    public RequestToEditApplicationCommandHandler(IApplicationRepository applicationRepository, IAccountUserContext accountUserContext)
        : base(applicationRepository, accountUserContext)
    {
    }

    public async Task<OperationResult> Handle(RequestToEditApplicationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            application =>
            {
                application.RequestToEdit(new RequestToEditReason(request.RequestToEditReason));

                return Task.CompletedTask;
            },
            request.Id,
            cancellationToken);
    }
}
