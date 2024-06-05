using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Application.TestData;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils;

namespace HE.Investment.AHP.Domain.Tests.Scheme.TestObjectBuilders;

public class SchemeEntityBuilder
{
    private readonly SchemeEntity _item;

    public SchemeEntityBuilder()
    {
        _item = new SchemeEntity(
            ApplicationBasicInfoTestData.SharedOwnershipInDraftState,
            SectionStatus.NotStarted,
            SchemeFunding.Empty(),
            ApplicationPartners.ConfirmedPartner(OrganisationId.From("cd7e3fb6-bff0-43ee-b65c-54db77d81f4c")),
            new AffordabilityEvidence(string.Empty),
            new SalesRisk(null),
            new HousingNeeds(string.Empty, string.Empty),
            StakeholderDiscussionsTestData.StakeholderDiscussions);
    }

    public static SchemeEntityBuilder NewNotStarted() => new();

    public SchemeEntityBuilder WithSchemeFunding(SchemeFunding funding)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.Funding),
            funding);

        return this;
    }

    public SchemeEntityBuilder WithApplicationDetails(ApplicationBasicInfo application)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.Application),
            application);

        return this;
    }

    public SchemeEntityBuilder WithSectionStatus(SectionStatus status)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.Status),
            status);

        return this;
    }

    public SchemeEntityBuilder WithAffordabilityEvidence(AffordabilityEvidence affordabilityEvidence)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.AffordabilityEvidence),
            affordabilityEvidence);

        return this;
    }

    public SchemeEntityBuilder WithSalesRisk(SalesRisk salesRisk)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.SalesRisk),
            salesRisk);

        return this;
    }

    public SchemeEntityBuilder WithHousingNeeds(HousingNeeds housingNeeds)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.HousingNeeds),
            housingNeeds);

        return this;
    }

    public SchemeEntity Build() => _item;
}
