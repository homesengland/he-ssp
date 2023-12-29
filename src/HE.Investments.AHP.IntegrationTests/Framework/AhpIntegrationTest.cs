using HE.Investment.AHP.WWW;
using HE.Investments.IntegrationTestsFramework;
using Xunit;

namespace HE.Investments.AHP.IntegrationTests.Framework;

[Collection(nameof(AhpIntegrationTestSharedContext))]
public class AhpIntegrationTest : IntegrationTestBase<Program>
{
    protected AhpIntegrationTest(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }
}
