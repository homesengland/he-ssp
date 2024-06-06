using HE.Investments.Organisation.Contract;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.WWW.Tests.TestDataBuilders;

public class OrganisationDetailsBuilder : TestObjectBuilder<OrganisationDetailsBuilder, OrganisationDetails>
{
    public OrganisationDetailsBuilder()
        : base(new OrganisationDetails("Test organisation", string.Empty, string.Empty, string.Empty, null, null))
    {
    }

    protected override OrganisationDetailsBuilder Builder => this;

    public OrganisationDetailsBuilder WithName(string value) => SetProperty(x => x.Name, value);
}
