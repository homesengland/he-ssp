using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;

public class TenureDetailsTestDataBuilder
{
    private MarketValue? _marketValue;

    private MarketRent? _marketRent;

    private ProspectiveRent? _prospectiveRent;

    private InitialSale? _initialSale;

    private YesNoType _exemptFromTheRightToSharedOwnership = YesNoType.Undefined;

    private MoreInformation? _exemptionJustification;

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
                marketRent: _marketRent,
                prospectiveRent: _prospectiveRent,
                initialSale: _initialSale,
                exemptFromTheRightToSharedOwnership: _exemptFromTheRightToSharedOwnership,
                exemptionJustification: _exemptionJustification);
    }
}
