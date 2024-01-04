using HE.Investment.AHP.WWW;
using HE.Investments.AHP.IntegrationTests.FillApplication;
using HE.Investments.IntegrationTestsFramework;
using Xunit;

namespace HE.Investments.AHP.IntegrationTests.Framework;

[Collection(nameof(AhpIntegrationTestSharedContext))]
public class AhpIntegrationTest : IntegrationTestBase<Program>
{
    protected AhpIntegrationTest(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        var applicationData = GetSharedDataOrNull<ApplicationData>(nameof(ApplicationData));
        if (applicationData is null)
        {
            applicationData = new ApplicationData();
            SetSharedData(nameof(ApplicationData), applicationData);
        }

        ApplicationData = applicationData;
    }

    public ApplicationData ApplicationData { get; }
}
