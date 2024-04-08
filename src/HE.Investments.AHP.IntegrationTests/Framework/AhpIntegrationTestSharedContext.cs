using Xunit;

namespace HE.Investments.AHP.IntegrationTests.Framework;

[CollectionDefinition(nameof(AhpIntegrationTestSharedContext))]
public class AhpIntegrationTestSharedContext : ICollectionFixture<AhpIntegrationTestFixture>
{
}
