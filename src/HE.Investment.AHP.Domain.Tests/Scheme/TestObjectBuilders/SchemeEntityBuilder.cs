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
            SchemeFunding.Empty(),
            SectionStatus.NotStarted,
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

    public SchemeEntity Build() => _item;
}
