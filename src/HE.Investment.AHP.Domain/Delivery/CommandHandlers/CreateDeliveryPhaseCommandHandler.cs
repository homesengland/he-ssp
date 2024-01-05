using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class CreateDeliveryPhaseCommandHandler : DeliveryCommandHandlerBase, IRequestHandler<CreateDeliveryPhaseCommand, DeliveryPhaseId>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IDeliveryPhaseRepository _deliveryPhaseRepository;
    private readonly IAccountUserContext _accountUserContext;

    public CreateDeliveryPhaseCommandHandler(
        IDeliveryPhaseRepository repository,
        IApplicationRepository applicationRepository,
        IAccountUserContext accountUserContext,
        ILogger<CreateDeliveryPhaseCommandHandler> logger)
        : base(logger)
    {
        _applicationRepository = applicationRepository;
        _accountUserContext = accountUserContext;
        _deliveryPhaseRepository = repository;
    }

    public async Task<DeliveryPhaseId> Handle(CreateDeliveryPhaseCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var deliveryPhases = await _deliveryPhaseRepository.GetByApplicationId(new ApplicationId.ApplicationId(request.ApplicationId), account, cancellationToken);

        var applicationBasicInfo = await _applicationRepository.GetApplicationBasicInfo(
            new ApplicationId.ApplicationId(request.ApplicationId),
            account,
            CancellationToken.None);

        var deliveryPhase = new DeliveryPhaseEntity(applicationBasicInfo, request.DeliveryPhaseName, Investments.Common.Contract.SectionStatus.NotStarted);
        deliveryPhases.Add(deliveryPhase);

        await _deliveryPhaseRepository.Save(deliveryPhases, account.SelectedOrganisationId(), cancellationToken);

        return new DeliveryPhaseId(deliveryPhase.Id.ToString());
    }
}
