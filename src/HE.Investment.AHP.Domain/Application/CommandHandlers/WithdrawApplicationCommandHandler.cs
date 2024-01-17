using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class WithdrawApplicationCommandHandler : IRequestHandler<WithdrawApplicationCommand, OperationResult>
{
    private readonly IAccountUserContext _accountUserContext;
    private readonly IApplicationRepository _applicationRepository;

    public WithdrawApplicationCommandHandler(
        IApplicationRepository applicationRepository,
        IAccountUserContext accountUserContext)
    {
        _accountUserContext = accountUserContext;
        _applicationRepository = applicationRepository;
    }

    public async Task<OperationResult> Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var application = await _applicationRepository.GetById(request.Id, account, cancellationToken);

        var withdrawReason = new WithdrawReason(request.WithdrawReason);
        application.ProvideWithdrawReason(withdrawReason);

        application.Withdraw();

        await _applicationRepository.Withdraw(application, account.SelectedOrganisationId(), cancellationToken);

        return OperationResult.Success();
    }
}
