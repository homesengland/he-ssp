using System.Threading;
using He.AspNetCore.Mvc.Gds.Components.Extensions;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects;
using DeliveryPhaseId = HE.Investment.AHP.Contract.Delivery.DeliveryPhaseId;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class CreateDeliveryPhaseCommandHandler : DeliveryCommandHandlerBase, IRequestHandler<CreateDeliveryPhaseCommand, OperationResult<DeliveryPhaseId>>
{
    private readonly IDeliveryPhaseRepository _deliveryPhaseRepository;
    private readonly IAccountUserContext _accountUserContext;

    public CreateDeliveryPhaseCommandHandler(
        IDeliveryPhaseRepository repository,
        IApplicationRepository applicationRepository,
        IAccountUserContext accountUserContext,
        ILogger<CreateDeliveryPhaseCommandHandler> logger)
        : base(logger)
    {
        _accountUserContext = accountUserContext;
        _deliveryPhaseRepository = repository;
    }

    public async Task<OperationResult<DeliveryPhaseId>> Handle(CreateDeliveryPhaseCommand request, CancellationToken cancellationToken)
    {
        var operationResult = OperationResult.New();

        var account = await _accountUserContext.GetSelectedAccount();

        var deliveryPhases = await _deliveryPhaseRepository.GetByApplicationId(new ApplicationId.ApplicationId(request.ApplicationId), account, cancellationToken);
        var newDeliveryPhase = deliveryPhases.CreateDeliveryPhase(request.DeliveryPhaseName.IsNotNullOrEmpty() ? new DeliveryPhaseName(request.DeliveryPhaseName) : null);
        operationResult.CheckErrors();
        var deliveryPhaseId = operationResult.CatchResult(() => _deliveryPhaseRepository.Save(newDeliveryPhase, account.SelectedOrganisationId(), cancellationToken).Result);
        operationResult.CheckErrors();

        return OperationResult.Success(new DeliveryPhaseId(deliveryPhaseId.Value));
    }
}
