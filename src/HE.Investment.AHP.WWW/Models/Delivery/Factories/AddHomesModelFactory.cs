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
        return await Create(applicationId, deliveryPhaseId, new Dictionary<string, string?>(), cancellationToken);
    }

    public async Task<AddHomesModel> Create(
        string applicationId,
        string deliveryPhaseId,
        AddHomesModel modelWithError,
        CancellationToken cancellationToken)
    {
        return await Create(applicationId, deliveryPhaseId, modelWithError.HomesToDeliver ?? new Dictionary<string, string?>(), cancellationToken);
    }

    private static string? GetHomesToDeliverOrDefault(IDictionary<string, string?> homesToDeliver, string homeTypeId, int? usedHomes)
    {
        return homesToDeliver.TryGetValue(homeTypeId, out var toDeliver) ? toDeliver : usedHomes.ToString();
    }

    private async Task<AddHomesModel> Create(
        string applicationId,
        string deliveryPhaseId,
        IDictionary<string, string?> homesToDeliver,
        CancellationToken cancellationToken)
    {
        var deliveryPhaseHomes = await _mediator.Send(
            new GetDeliveryPhaseHomesQuery(AhpApplicationId.From(applicationId), DeliveryPhaseId.From(deliveryPhaseId)),
            cancellationToken);

        return new AddHomesModel(
            applicationId,
            deliveryPhaseHomes.ApplicationName,
            deliveryPhaseHomes.DeliveryPhaseName,
            deliveryPhaseHomes.HomeTypesToDeliver.ToDictionary(x => x.HomeTypeId.Value, x => x.HomeTypeName),
            deliveryPhaseHomes.HomeTypesToDeliver.ToDictionary(x => x.HomeTypeId.Value, x => GetHomesToDeliverOrDefault(homesToDeliver, x.HomeTypeId.Value, x.UsedHomes)));
    }
}
