using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Application.TestData;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Extensions;
using HE.Investments.TestsUtils;
using Microsoft.Crm.Sdk.Messages;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.TestObjectBuilders;

public class FinancialDetailsEntityBuilder
{
    private readonly FinancialDetailsEntity _item;

    private FinancialDetailsEntityBuilder(ApplicationBasicInfo? applicationBasicInfo)
    {
        applicationBasicInfo ??= ApplicationBasicInfoTestData.AffordableRentInDraftState;
        _item = new(
            applicationBasicInfo,
            new SiteBasicInfo(
                applicationBasicInfo.SiteId,
                new SiteName("Site name"),
                new LandAcquisitionStatus(SiteLandAcquisitionStatus.FullOwnership),
                SiteUsingModernMethodsOfConstruction.Yes));
    }

    public static FinancialDetailsEntityBuilder New(ApplicationBasicInfo? applicationBasicInfo = null) => new(applicationBasicInfo);

    public FinancialDetailsEntityBuilder WithOtherApplicationCosts(string expectedWorksCosts, string expectedOnCosts)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.OtherApplicationCosts),
            new OtherApplicationCosts(
                expectedWorksCosts.IsProvided() ? new ExpectedWorksCosts(expectedWorksCosts) : null,
                expectedOnCosts.IsProvided() ? new ExpectedOnCosts(expectedOnCosts) : null));

        return this;
    }

    public FinancialDetailsEntityBuilder WithLandValue(string landValue)
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

    public FinancialDetailsEntityBuilder WithSchemaFunding(int schemaFunding)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.SchemeFunding),
            new SchemeFunding(schemaFunding, 5));

        return this;
    }

    public FinancialDetailsEntityBuilder WithLandStatus(string purchasePrice)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.LandStatus),
            new LandStatus(new PurchasePrice(purchasePrice), null));

        return this;
    }

    public FinancialDetailsEntityBuilder WithSiteBasicInfo(SiteLandAcquisitionStatus status)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.SiteBasicInfo),
            _item.SiteBasicInfo with { LandAcquisitionStatus = new LandAcquisitionStatus(status) });

        return this;
    }
}
