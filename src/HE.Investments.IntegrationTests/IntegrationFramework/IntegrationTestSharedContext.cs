using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.WWW;
using Xunit;

namespace HE.Investments.Loans.IntegrationTests.IntegrationFramework;

[CollectionDefinition(nameof(IntegrationTestSharedContext))]
public class IntegrationTestSharedContext : ICollectionFixture<IntegrationTestFixture<Program>>
{
}
