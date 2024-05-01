using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class OrganisationBasicInfoBuilder
{
    private readonly OrganisationId _organisationId = new("00000000-0000-0000-0000-000000000001");
    private bool _isUnregisteredBody;

    public OrganisationBasicInfoBuilder WithUnregisteredBody()
    {
        _isUnregisteredBody = true;

        return this;
    }

    public OrganisationBasicInfo Build()
    {
        return new OrganisationBasicInfo(_organisationId, "AccountOne", "1234", "London", _isUnregisteredBody);
    }
}
