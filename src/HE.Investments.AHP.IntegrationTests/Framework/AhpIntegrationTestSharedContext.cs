using HE.Investment.AHP.WWW;
using HE.Investments.IntegrationTestsFramework;
using Xunit;

namespace HE.Investments.AHP.IntegrationTests.Framework;

[CollectionDefinition(nameof(AhpIntegrationTestSharedContext))]
public class AhpIntegrationTestSharedContext : ICollectionFixture<IntegrationTestFixture<Program>>
{
}
