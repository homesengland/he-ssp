using HE.Investments.FrontDoor.WWW;
using HE.Investments.IntegrationTestsFramework;
using Xunit;

namespace HE.Investments.FrontDoor.IntegrationTests.Framework;

[CollectionDefinition(nameof(FrontDoorIntegrationTestSharedContext))]
public class FrontDoorIntegrationTestSharedContext : ICollectionFixture<FrontDoorIntegrationTestFixture>
{
}
