using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Projects.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.CommandHandlers;

public class ProvideLandOwnershipCommandHandlerTests : TestBase<ProvideLandOwnershipCommandHandler>
{
    public ProvideLandOwnershipCommandHandlerTests()
    {
        AccountUserContextTestBuilder.New().Register(this);
    }

    [Fact]
    public async Task SetLandOwnership_WhenLandOwnershipIsProvided()
    {
        // given
        var applicationProjects = ApplicationProjectsBuilder
            .New()
            .WithDefaultProject()
            .Build();

        var project = applicationProjects.Projects.Single();
        var projectId = project.Id;

        Given(ApplicationProjectsRepositoryBuilder
            .New()
            .ForProject(projectId)
            .ReturnsOneProject(project));

        // when
        var result = await TestCandidate.Handle(
            new ProvideLandOwnershipCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, CommonResponse.Yes),
            CancellationToken.None);

        // then
        result.HasValidationErrors.Should().BeFalse();

        project.LandOwnership!.ApplicantHasFullOwnership.Should().BeTrue();
    }
}
