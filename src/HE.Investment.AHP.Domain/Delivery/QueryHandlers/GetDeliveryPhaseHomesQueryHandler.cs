using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.Investment.AHP.Domain.Delivery.QueryHandlers;

public class GetDeliveryPhaseHomesQueryHandler : IRequestHandler<GetDeliveryPhaseHomesQuery, DeliveryPhaseHomes>
{
    private readonly IDeliveryPhaseRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public GetDeliveryPhaseHomesQueryHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<DeliveryPhaseHomes> Handle(GetDeliveryPhaseHomesQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var deliveryPhases = await _repository.GetByApplicationId(request.ApplicationId, userAccount, cancellationToken);
        var deliveryPhase = deliveryPhases.GetById(request.DeliveryPhaseId);

        // AB#66085 do not return those used fully in other phases. Return null, when used did not set to 0 explicitly.
        return new DeliveryPhaseHomes(
            request.DeliveryPhaseId,
            deliveryPhase.Name.Value,
            deliveryPhases.ApplicationName.Name,
            deliveryPhases.GetHomesToDeliverInPhase(request.DeliveryPhaseId)
                .Select(x => new HomeTypesToDeliver(x.HomesToDeliver.HomeTypeId, x.HomesToDeliver.HomeTypeName.Value, x.ToDeliver))
                .ToList());
    }
}
