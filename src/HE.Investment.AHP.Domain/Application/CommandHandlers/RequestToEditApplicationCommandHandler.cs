using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class RequestToEditApplicationCommandHandler : ChangeApplicationStatusBaseCommandHandler, IRequestHandler<RequestToEditApplicationCommand, OperationResult>
{
    public RequestToEditApplicationCommandHandler(IApplicationRepository applicationRepository, IAccountUserContext accountUserContext)
        : base(applicationRepository, accountUserContext)
    {
    }

    public async Task<OperationResult> Handle(RequestToEditApplicationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async (applicationRepository, application, organisationId) =>
            {
                var requestToEditReason = new RequestToEditReason(request.RequestToEditReason);

                await application.RequestToEdit(applicationRepository, requestToEditReason, organisationId, cancellationToken);
                await applicationRepository.DispatchEvents(application, cancellationToken);
            },
            request.Id,
            cancellationToken);
    }
}
