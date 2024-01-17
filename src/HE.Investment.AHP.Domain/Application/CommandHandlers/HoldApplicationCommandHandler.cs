using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class HoldApplicationCommandHandler : IRequestHandler<HoldApplicationCommand, OperationResult>
{
    private readonly IAccountUserContext _accountUserContext;
    private readonly IApplicationRepository _applicationRepository;
    private readonly IEventDispatcher _eventDispatcher;

    public HoldApplicationCommandHandler(
        IApplicationRepository applicationRepository,
        IAccountUserContext accountUserContext,
        IEventDispatcher eventDispatcher)
    {
        _accountUserContext = accountUserContext;
        _applicationRepository = applicationRepository;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<OperationResult> Handle(HoldApplicationCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var application = await _applicationRepository.GetById(request.Id, account, cancellationToken);

        var changeStatusReason = request.ChangeStatusReason.IsProvided()
            ? new ChangeStatusReason(request.ChangeStatusReason!)
            : null;
        application.ProvideChangeStatusReason(changeStatusReason);

        application.Hold();

        await _applicationRepository.Save(application, account.SelectedOrganisationId(), cancellationToken);

        await _eventDispatcher.Publish(new ApplicationHasBeenPutOnHoldEvent(), cancellationToken);

        return OperationResult.Success();
    }
}
