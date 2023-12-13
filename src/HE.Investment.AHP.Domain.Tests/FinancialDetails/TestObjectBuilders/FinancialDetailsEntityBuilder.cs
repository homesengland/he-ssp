using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Application.TestData;
using HE.Investments.Common.Extensions;
using HE.Investments.TestsUtils;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.TestObjectBuilders;

public class FinancialDetailsEntityBuilder
{
    private readonly FinancialDetailsEntity _item;

    private FinancialDetailsEntityBuilder(ApplicationBasicInfo? applicationBasicInfo)
    {
        _item = new(applicationBasicInfo ?? ApplicationBasicInfoTestData.AffordableRentInDraftState);
    }

    public static FinancialDetailsEntityBuilder New(ApplicationBasicInfo? applicationBasicInfo = null) => new(applicationBasicInfo);

    public FinancialDetailsEntityBuilder WithOtherApplicationCosts(decimal? expectedWorksCosts, decimal? expectedOnCosts)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.OtherApplicationCosts),
            new OtherApplicationCosts(
                expectedWorksCosts.IsProvided() ? new ExpectedWorksCosts(expectedWorksCosts!.Value) : null,
                expectedOnCosts.IsProvided() ? new ExpectedOnCosts(expectedOnCosts!.Value) : null));

        return this;
    }

    public FinancialDetailsEntityBuilder WithLandValue(decimal landValue)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.LandValue),
            new LandValue(new CurrentLandValue(landValue), true));

        return this;
    }

    public FinancialDetailsEntity Build() => _item;

    public FinancialDetailsEntityBuilder WithExpectedContributions(ExpectedContributionsToScheme expectedContributionsToScheme)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            expectedContributionsToScheme,
            nameof(expectedContributionsToScheme.ApplicationTenure),
            _item.ApplicationBasicInfo.Tenure);

        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.ExpectedContributions),
            expectedContributionsToScheme);

        return this;
    }

    public FinancialDetailsEntityBuilder WithGrants(PublicGrants publicGrants)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.PublicGrants),
            publicGrants);

        return this;
    }

    public FinancialDetailsEntityBuilder WithLandStatus(decimal purchasePrice)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.LandStatus),
            new LandStatus(new PurchasePrice(purchasePrice), null));

        return this;
    }
}
