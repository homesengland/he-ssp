using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Common.Tests.ObjectBuilders;
using HE.Investments.Loans.Contract.Projects.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;
using static HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData.ProjectDateTestData;
using static HE.Investments.Loans.Common.Tests.TestData.PoundsTestData;
using static HE.Investments.Loans.Common.Tests.TestData.SourceOfValuationTestData;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.CommandHandlers;

public class ProvideAdditionalDataCommandHandlerTests : TestBase<ProvideAdditionalDataCommandHandler>
{
    public ProvideAdditionalDataCommandHandlerTests()
    {
        AccountUserContextTestBuilder.New().Register(this);
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
        var action = () => TestCandidate.Handle(
            new ProvideAdditionalDetailsCommand(
                LoanApplicationIdTestData.LoanApplicationIdOne,
                projectId,
                null,
                string.Empty,
                string.Empty,
                string.Empty),
            CancellationToken.None);

        // then
        await action.Should()
            .ThrowAsync<DomainValidationException>()
            .WithMessage(
                "Enter when you purchased this site" +
                $"\n{ValidationErrorMessage.IncorrectProjectCost}" +
                $"\n{ValidationErrorMessage.IncorrectProjectValue}" +
                $"\n{ValidationErrorMessage.EnterMoreDetails}");
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
                new DateDetails(day, month, year),
                CorrectAmountAsString,
                CorrectAmountAsString,
                AnySourceAsString),
            CancellationToken.None);

        // then
        result.HasValidationErrors.Should().BeFalse();

        project.AdditionalDetails!.Cost.Value.Should().Be(CorrectAmount);
        project.AdditionalDetails!.CurrentValue.Value.Should().Be(CorrectAmount);
        project.AdditionalDetails!.PurchaseDate.Value.Should().Be(CorrectDate.Value);
        project.AdditionalDetails!.PurchaseDate.Value.Should().Be(CorrectDate.Value);
    }

    private void GivenCurrentDate(DateTime date)
    {
        Given(DateTimeProviderTestBuilder.New().ReturnsAsNow(date));
    }
}
