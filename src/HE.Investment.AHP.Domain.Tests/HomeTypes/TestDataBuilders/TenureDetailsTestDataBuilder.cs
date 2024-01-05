using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;

public class TenureDetailsTestDataBuilder
{
    private MarketValue _marketValue = new(1000);

    private MarketRent _marketRent = new(500);

    private ProspectiveRent _prospectiveRent = new(200.2m);

    private InitialSale _initialSale = new(50);

    public TenureDetailsTestDataBuilder WithMarketValue(MarketValue marketValue)
    {
        _marketValue = marketValue;
        return this;
    }

    public TenureDetailsTestDataBuilder WithMarketRent(MarketRent marketRent)
    {
        _marketRent = marketRent;
        return this;
    }

    public TenureDetailsTestDataBuilder WithProspectiveRent(ProspectiveRent prospectiveRent)
    {
        _prospectiveRent = prospectiveRent;
        return this;
    }

    public TenureDetailsTestDataBuilder WithInitialSale(InitialSale initialSale)
    {
        _initialSale = initialSale;
        return this;
    }

    public TenureDetailsSegmentEntity Build()
    {
        return new TenureDetailsSegmentEntity(
                marketValue: _marketValue,
                marketRent: _marketRent,
                prospectiveRent: _prospectiveRent,
                initialSale: _initialSale);
    }
}
