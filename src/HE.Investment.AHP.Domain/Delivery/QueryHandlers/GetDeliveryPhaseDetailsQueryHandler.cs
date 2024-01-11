using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.Domain.Delivery.Mappers;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.Investment.AHP.Domain.Delivery.QueryHandlers;

public class GetDeliveryPhaseDetailsQueryHandler : IRequestHandler<GetDeliveryPhaseDetailsQuery, DeliveryPhaseDetails>
{
    private readonly IDeliveryPhaseRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public GetDeliveryPhaseDetailsQueryHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<DeliveryPhaseDetails> Handle(GetDeliveryPhaseDetailsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var deliveryPhase = await _repository.GetById(
            request.ApplicationId,
            new DeliveryPhaseId(request.DeliveryPhaseId),
            userAccount,
            cancellationToken);

        return DeliveryPhaseEntityMapper.ToDeliveryPhaseDetails(deliveryPhase);
    }
}
