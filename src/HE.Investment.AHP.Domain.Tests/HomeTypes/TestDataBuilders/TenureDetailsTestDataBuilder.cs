using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;

public class TenureDetailsTestDataBuilder
{
    private MarketValue? _marketValue;

    private MarketRentPerWeek? _marketRent;

    private RentPerWeek? _prospectiveRent;

    private InitialSale? _initialSale;

    private YesNoType _exemptFromTheRightToSharedOwnership = YesNoType.Undefined;

    private MoreInformation? _exemptionJustification;

    public TenureDetailsTestDataBuilder WithMarketValue(MarketValue marketValue)
    {
        _marketValue = marketValue;
        return this;
    }

    public TenureDetailsTestDataBuilder WithMarketRent(MarketRentPerWeek marketRentPerWeek)
    {
        _marketRent = marketRentPerWeek;
        return this;
    }

    public TenureDetailsTestDataBuilder WithProspectiveRent(RentPerWeek prospectiveRent)
    {
        _prospectiveRent = prospectiveRent;
        return this;
    }

    public TenureDetailsTestDataBuilder WithInitialSale(InitialSale initialSale)
    {
        _initialSale = initialSale;
        return this;
    }

    public TenureDetailsTestDataBuilder WithExemptFromTheRightToSharedOwnership(YesNoType exemptFromTheRightToSharedOwnership)
    {
        _exemptFromTheRightToSharedOwnership = exemptFromTheRightToSharedOwnership;
        return this;
    }

    public TenureDetailsTestDataBuilder WithExemptionJustification(string exemptionJustification)
    {
        _exemptionJustification = new MoreInformation(exemptionJustification);
        return this;
    }

    public TenureDetailsSegmentEntity Build()
    {
        return new TenureDetailsSegmentEntity(
                marketValue: _marketValue,
                marketRentPerWeek: _marketRent,
                rentPerWeek: _prospectiveRent,
                initialSale: _initialSale,
                exemptFromTheRightToSharedOwnership: _exemptFromTheRightToSharedOwnership,
                exemptionJustification: _exemptionJustification);
    }
}
