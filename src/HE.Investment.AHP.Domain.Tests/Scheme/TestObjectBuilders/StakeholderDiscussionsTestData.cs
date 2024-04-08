using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Scheme.TestObjectBuilders;

public static class StakeholderDiscussionsTestData
{
    public static StakeholderDiscussions StakeholderDiscussions => new(new StakeholderDiscussionsDetails("Report"), new LocalAuthoritySupportFileContainer(null));
}
