using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Delivery.QueryHandlers;

public class GetDeliveryPhasesQueryHandler : IRequestHandler<GetDeliveryPhasesQuery, ApplicationDeliveryPhases>
{
    private readonly IDeliveryPhaseRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public GetDeliveryPhasesQueryHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<ApplicationDeliveryPhases> Handle(GetDeliveryPhasesQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var deliveryPhases = await _repository.GetByApplicationId(new ApplicationId(request.ApplicationId), account, cancellationToken);

        return new ApplicationDeliveryPhases(
            deliveryPhases.ApplicationName.Name,
            3, // TODO: fetch/calculate in entity
            deliveryPhases.DeliveryPhases.OrderByDescending(x => x.CreatedOn).Select(Map).ToList());
    }

    private static DeliveryPhaseDetails Map(IDeliveryPhaseEntity deliveryPhase)
    {
        return new DeliveryPhaseDetails(
            deliveryPhase.Application.Name.Name,
            deliveryPhase.Id.Value,
            deliveryPhase.Name.Value,
            12,  // TODO: fetch from CRM
            null,
            null,
            null);
    }
}
