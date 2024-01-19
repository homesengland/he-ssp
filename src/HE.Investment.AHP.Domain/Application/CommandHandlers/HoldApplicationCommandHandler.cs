using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class HoldApplicationCommandHandler : IRequestHandler<HoldApplicationCommand, OperationResult>
{
    private readonly IAccountUserContext _accountUserContext;
    private readonly IApplicationRepository _applicationRepository;

    public HoldApplicationCommandHandler(
        IApplicationRepository applicationRepository,
        IAccountUserContext accountUserContext)
    {
        _accountUserContext = accountUserContext;
        _applicationRepository = applicationRepository;
    }

    public async Task<OperationResult> Handle(HoldApplicationCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var application = await _applicationRepository.GetById(request.Id, account, cancellationToken);

        var holdReason = request.HoldReason.IsProvided()
            ? new HoldReason(request.HoldReason!)
            : null;

        await application.Hold(_applicationRepository, holdReason, account.SelectedOrganisationId(), cancellationToken);
        await _applicationRepository.DispatchEvents(application, cancellationToken);

        return OperationResult.Success();
    }
}
