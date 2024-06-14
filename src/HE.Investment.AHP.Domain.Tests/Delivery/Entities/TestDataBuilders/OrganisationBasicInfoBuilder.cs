using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class OrganisationBasicInfoBuilder : TestObjectBuilder<OrganisationBasicInfoBuilder, OrganisationBasicInfo>
{
    public OrganisationBasicInfoBuilder()
        : base(new OrganisationBasicInfo(
            new("00000000-0000-0000-0000-000000000001"),
            "AccountOne",
            "1234",
            "Main street",
            "London",
            "Postal code",
            false))
    {
    }

    protected override OrganisationBasicInfoBuilder Builder => this;

    public OrganisationBasicInfoBuilder WithUnregisteredBody() => SetProperty(x => x.IsUnregisteredBody, true);

    public OrganisationBasicInfoBuilder WithId(OrganisationId value) => SetProperty(x => x.OrganisationId, value);

    public OrganisationBasicInfoBuilder WithName(string value) => SetProperty(x => x.RegisteredCompanyName, value);
}
