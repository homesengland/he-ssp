using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Queries;
using MediatR;

namespace HE.Investment.AHP.WWW.Models.Delivery.Factories;

internal class AddHomesModelFactory
{
    private readonly IMediator _mediator;

    public AddHomesModelFactory(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<AddHomesModel> Create(
        string applicationId,
        string deliveryPhaseId,
        CancellationToken cancellationToken)
    {
        var deliveryPhaseHomes = await _mediator.Send(
            new GetDeliveryPhaseHomesQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)),
            cancellationToken);

        return new AddHomesModel(
            applicationId,
            deliveryPhaseHomes.ApplicationName,
            deliveryPhaseHomes.DeliveryPhaseName,
            deliveryPhaseHomes.HomeTypesToDeliver.ToDictionary(x => x.HomeTypeId.Value, x => x.HomeTypeName),
            deliveryPhaseHomes.HomeTypesToDeliver.ToDictionary(x => x.HomeTypeId.Value, x => x.UsedHomes.ToString()));
    }
}
