using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.Domain.Application.Mappers;
using HE.Investment.AHP.Domain.Delivery.Mappers;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investment.AHP.Domain.Delivery.QueryHandlers;

public class GetDeliveryPhasesQueryHandler : IRequestHandler<GetDeliveryPhasesQuery, ApplicationDeliveryPhases>
{
    private readonly IDeliveryPhaseRepository _repository;

    private readonly IConsortiumUserContext _accountUserContext;

    public GetDeliveryPhasesQueryHandler(IDeliveryPhaseRepository repository, IConsortiumUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<ApplicationDeliveryPhases> Handle(GetDeliveryPhasesQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var deliveryPhases = await _repository.GetByApplicationId(request.ApplicationId, account, cancellationToken);

        return new ApplicationDeliveryPhases(
            ApplicationBasicInfoMapper.Map(deliveryPhases.Application),
            deliveryPhases.UnusedHomeTypesCount,
            account.SelectedOrganisation().IsUnregisteredBody,
            deliveryPhases.DeliveryPhases.OrderByDescending(x => x.CreatedOn).Select(DeliveryPhaseEntityMapper.ToDeliveryPhaseBasicDetails).ToList());
    }
}
