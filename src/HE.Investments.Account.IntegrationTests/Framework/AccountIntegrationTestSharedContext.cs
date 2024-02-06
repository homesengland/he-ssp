using HE.Investments.Account.WWW;
using HE.Investments.IntegrationTestsFramework;
using Xunit;

namespace HE.Investments.Account.IntegrationTests.Framework;

[CollectionDefinition(nameof(AccountIntegrationTestSharedContext))]
public class AccountIntegrationTestSharedContext : ICollectionFixture<IntegrationTestFixture<Program>>
{
}
