using HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Tests.ObjectBuilders;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;
using static HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData.ProjectDateTestData;
using static HE.InvestmentLoans.Common.Tests.TestData.PoundsTestData;
using static HE.InvestmentLoans.Common.Tests.TestData.SourceOfValuationTestData;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.CommandHandlers;

public class ProvideAdditionalDataCommandHandlerTests : TestBase<ProvideAdditionalDataCommandHandler>
{
    public ProvideAdditionalDataCommandHandlerTests()
    {
        LoanUserContextTestBuilder.New().Register(this);
    }

    [Fact]
    public async Task ShouldReturnAllValidationResults_WhenNoDataWasProvided()
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

        GivenCurrentDate(CorrectDateTime);

        // when
        var result = await TestCandidate.Handle(
            new ProvideAdditionalDetailsCommand(
                LoanApplicationIdTestData.LoanApplicationIdOne,
                projectId,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty),
            CancellationToken.None);

        // then
        result.Errors.Should()
            .ContainsErrorMessages(
                ValidationErrorMessage.NoPurchaseDate,
                ValidationErrorMessage.IncorrectProjectCost,
                ValidationErrorMessage.IncorrectProjectValue,
                ValidationErrorMessage.EnterMoreDetails);
    }

    [Fact]
    public async Task ShouldSetAllData_WhenCorrectDataWasProvided()
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

        GivenCurrentDate(CorrectDateTime);

        // when
        var (year, month, day) = CorrectDateAsStrings;

        var result = await TestCandidate.Handle(
            new ProvideAdditionalDetailsCommand(
                LoanApplicationIdTestData.LoanApplicationIdOne,
                projectId,
                year,
                month,
                day,
                CorrectAmountAsString,
                CorrectAmountAsString,
                AnySourceAsString),
            CancellationToken.None);

        // then
        result.HasValidationErrors.Should().BeFalse();

        project.AdditionalDetails!.Cost.Value.Should().Be(CorrectAmount);
        project.AdditionalDetails!.CurrentValue.Value.Should().Be(CorrectAmount);
        project.AdditionalDetails!.PurchaseDate.Date.Should().Be(CorrectDate);
        project.AdditionalDetails!.PurchaseDate.Date.Should().Be(CorrectDate);
    }

    private void GivenCurrentDate(DateTime date)
    {
        Given(DateTimeProviderTestBuilder.New().ReturnsAsNow(date));
    }
}
