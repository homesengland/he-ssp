using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class ProvideApplicationStatusCommandHandler : IRequestHandler<ProvideApplicationStatusCommand, OperationResult>
{
    private readonly IAccountUserContext _accountUserContext;
    private readonly IApplicationRepository _applicationRepository;
    private readonly ILogger<ProvideApplicationStatusCommandHandler> _logger;
    private readonly IEventDispatcher _eventDispatcher;

    public ProvideApplicationStatusCommandHandler(
        IApplicationRepository applicationRepository,
        IAccountUserContext accountUserContext,
        ILogger<ProvideApplicationStatusCommandHandler> logger,
        IEventDispatcher eventDispatcher)
    {
        _accountUserContext = accountUserContext;
        _applicationRepository = applicationRepository;
        _logger = logger;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<OperationResult> Handle(ProvideApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var application = await _applicationRepository.GetById(request.Id, account, cancellationToken);

        try
        {
            var changeStatusReason = request.ChangeStatusReason.IsProvided()
                ? new ChangeStatusReason(request.ChangeStatusReason!)
                : null;
            application.ProvideChangeStatusReason(changeStatusReason);

            application.ProvideApplicationStatus(request.Status);

            await _eventDispatcher.Publish(new ApplicationStatusHasBeenChangedEvent(request.Status), cancellationToken);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        await _applicationRepository.Save(application, account.SelectedOrganisationId(), cancellationToken);

        return OperationResult.Success();
    }
}
