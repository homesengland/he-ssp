using Xunit;

namespace HE.InvestmentLoans.IntegrationTests.IntegrationFramework;

[CollectionDefinition(nameof(IntegrationTestSharedContext))]
public class IntegrationTestSharedContext : ICollectionFixture<IntegrationTestFixture<Program>>
{
}
