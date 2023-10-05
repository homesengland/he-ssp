using HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.CommandHandlers;
public class ProvideLandOwnershipCommandHandlerTests : TestBase<ProvideLandOwnershipCommandHandler>
{
    public ProvideLandOwnershipCommandHandlerTests()
    {
        LoanUserContextTestBuilder.New().Register(this);
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
            .For(LoanApplicationIdTestData.LoanApplicationIdOne)
            .Returns(applicationProjects));

        // when
        var result = await TestCandidate.Handle(new ProvideLandOwnershipCommand(LoanApplicationIdTestData.LoanApplicationIdOne, projectId, CommonResponse.Yes), CancellationToken.None);

        // then
        result.HasValidationErrors.Should().BeFalse();

        project.LandOwnership!.ApplicantHasFullOwnership.Should().BeTrue();
    }
}
