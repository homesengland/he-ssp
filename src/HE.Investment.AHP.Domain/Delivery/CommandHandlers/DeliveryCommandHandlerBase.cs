using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public abstract class DeliveryCommandHandlerBase
{
    private readonly ILogger _logger;

    protected DeliveryCommandHandlerBase(ILogger logger)
    {
        _logger = logger;
    }

    protected IList<ErrorItem> PerformWithValidation(params Action[] actions)
    {
        var errors = new List<ErrorItem>();
        foreach (var action in actions)
        {
            try
            {
                action();
            }
            catch (DomainValidationException domainValidationException)
            {
                _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
                errors.AddRange(domainValidationException.OperationResult.Errors);
            }
        }

        return errors;
    }
}

[SuppressMessage("Maintainability Rules", "SA1402", Justification = "Generic version of the same command handler.")]
public abstract class DeliveryCommandHandlerBase<TCommand> : DeliveryCommandHandlerBase, IRequestHandler<TCommand, OperationResult>
    where TCommand : IDeliveryCommand
{
    private readonly IDeliveryPhaseRepository _repository;

    private readonly IConsortiumUserContext _accountUserContext;

    protected DeliveryCommandHandlerBase(IDeliveryPhaseRepository repository, IConsortiumUserContext accountUserContext, ILogger logger)
        : base(logger)
    {
        _accountUserContext = accountUserContext;
        _repository = repository;
    }

    public async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var deliveryPhases = await _repository.GetByApplicationId(request.ApplicationId, account, cancellationToken);
        Perform(deliveryPhases, request);

        await _repository.Save(deliveryPhases, account, cancellationToken);
        return OperationResult.Success();
    }

    protected abstract void Perform(DeliveryPhasesEntity deliveryPhases, TCommand request);
}
