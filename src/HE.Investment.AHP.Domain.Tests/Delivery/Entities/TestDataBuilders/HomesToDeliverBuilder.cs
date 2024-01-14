using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class HomesToDeliverBuilder
{
    private string _homeTypeId = "1-bf";

    private int _totalHomes = 10;

    public HomesToDeliverBuilder WithHomeTypeId(string homeTypeId)
    {
        _homeTypeId = homeTypeId;
        return this;
    }

    public HomesToDeliverBuilder WithTotalHomes(int totalHomes)
    {
        _totalHomes = totalHomes;
        return this;
    }

    public HomesToDeliver Build()
    {
        return new HomesToDeliver(
            new HomeTypeId(_homeTypeId),
            new HomeTypeName("1 bed flat"),
            _totalHomes);
    }
}
