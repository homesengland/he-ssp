using System.Diagnostics;
using HE.Investment.AHP.WWW;
using HE.Investments.AHP.IntegrationTests.Crm;
using HE.Investments.AHP.IntegrationTests.Order01StartAhpProjectWithSite.Data;
using HE.Investments.AHP.IntegrationTests.Order02FillSite.Data;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data.Allocation;
using HE.Investments.AHP.IntegrationTests.Utils;
using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.IntegrationTests.Utils;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Auth;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace HE.Investments.AHP.IntegrationTests.Framework;

[Collection(nameof(AhpIntegrationTestSharedContext))]
public class AhpIntegrationTest : IntegrationTestBase<Program>
{
    private readonly ITestOutputHelper _output;

    private readonly AhpIntegrationTestFixture _fixture;

    protected AhpIntegrationTest(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture)
    {
        SetApplicationData();
        SetAllocationData();
        SetSiteData();
        SetProjectData();
        InitStopwatch();
        fixture.CheckUserLoginData();
        fixture.MockUserAccount();
        _output = output;
        _fixture = fixture;
        LoginData = fixture.LoginData;
        InFrontDoor = fixture.ServiceProvider.GetRequiredService<FrontDoorDataManipulator>();
        SetUserOrganisationData();
    }

    public ApplicationData ApplicationData { get; private set; }

    public AllocationData AllocationData { get; private set; }

    public SiteData SiteData { get; private set; }

    public AhpProjectData ProjectData { get; private set; }

    public Stopwatch Stopwatch { get; private set; }

    protected AhpCrmContext AhpCrmContext => _fixture.AhpCrmContext;

    protected ILoginData LoginData { get; }

    protected UserOrganisationData UserOrganisationData { get; private set; }

    protected FrontDoorDataManipulator InFrontDoor { get; }

    protected AhpDataManipulator AhpDataManipulator => _fixture.AhpDataManipulator;

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();

        _output.WriteLine($"Elapsed time: {Stopwatch.Elapsed.TotalSeconds} sec");
    }

    public async Task ChangeApplicationStatus(string applicationId, ApplicationStatus applicationStatus)
    {
        await AhpCrmContext.ChangeApplicationStatus(applicationId, applicationStatus, LoginData);
    }

    private void SetApplicationData()
    {
        ApplicationData = ReturnSharedData<ApplicationData>();
    }

    private void SetAllocationData()
    {
        AllocationData = ReturnSharedData<AllocationData>();
    }

    private void SetSiteData()
    {
        SiteData = ReturnSharedData<SiteData>();
    }

    private void SetProjectData()
    {
        ProjectData = ReturnSharedData<AhpProjectData>();
    }

    private void InitStopwatch()
    {
        Stopwatch = ReturnSharedData<Stopwatch>(data => data.Start());
    }

    private void SetUserOrganisationData()
    {
        UserOrganisationData = ReturnSharedData<UserOrganisationData>(data => data.SetOrganisationId(LoginData.OrganisationId));
    }
}
