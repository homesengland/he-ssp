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

        return new DeliveryPhaseHomes(
            request.DeliveryPhaseId,
            deliveryPhase.Name.Value,
            deliveryPhases.Application.Name.Value,
            deliveryPhases.GetHomesToDeliverInPhase(request.DeliveryPhaseId)
                .Select(x => new HomeTypesToDeliver(x.HomesToDeliver.HomeTypeId, x.HomesToDeliver.HomeTypeName.Value, x.ToDeliver))
                .ToList());
    }
}
