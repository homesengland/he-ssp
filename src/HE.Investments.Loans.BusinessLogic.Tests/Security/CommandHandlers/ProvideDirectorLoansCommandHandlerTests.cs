using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.BusinessLogic.Security.CommandHandler;
using HE.Investments.Loans.BusinessLogic.Tests.Security.TestObjectBuilder;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Security.Commands;
using HE.Investments.Loans.Contract.Security.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Security.CommandHandlers;
public class ProvideDirectorLoansCommandHandlerTests : TestBase<ProvideDirectorLoansCommandHandler>
{
    public ProvideDirectorLoansCommandHandlerTests()
    {
        AccountUserContextTestBuilder.New().Register(this);
    }

    [Fact]
    public async Task ShouldRemoveDirectLoans_WhenNoAnswerProvided()
    {
        var entity = SecurityEntityTestBuilder
            .New()
            .WithDirectorLoans(new DirectorLoans(true))
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(entity)
            .BuildMockAndRegister(this);

        var command = new ProvideDirectorLoansCommand(LoanApplicationIdTestData.LoanApplicationIdOne, null!);

        await TestCandidate.Handle(command, CancellationToken.None);

        entity.DirectorLoans.Should().BeNull();
    }

    [Fact]
    public async Task ShouldRemoveDirectLoansSubordinate_WhenNegativeAnswerProvided()
    {
        var entity = SecurityEntityTestBuilder
            .New()
            .WithDirectorLoans(new DirectorLoans(true))
            .WithDirectorLoansSubordinate(new DirectorLoansSubordinate(true, string.Empty))
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(entity)
            .BuildMockAndRegister(this);

        var command = new ProvideDirectorLoansCommand(LoanApplicationIdTestData.LoanApplicationIdOne, CommonResponse.No);

        await TestCandidate.Handle(command, CancellationToken.None);

        entity.DirectorLoansSubordinate.Should().BeNull();
    }

    [Theory]
    [InlineData(CommonResponse.Yes, true)]
    [InlineData(CommonResponse.No, false)]
    public async Task ShouldSetDirectLoans_WhenCorrectAnswerIsProvided(string requestData, bool directorLoansExist)
    {
        var entity = SecurityEntityTestBuilder
            .New()
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(entity)
            .BuildMockAndRegister(this);

        var command = new ProvideDirectorLoansCommand(LoanApplicationIdTestData.LoanApplicationIdOne, requestData);

        await TestCandidate.Handle(command, CancellationToken.None);

        entity.DirectorLoans.Exists.Should().Be(directorLoansExist);
    }

    [Fact]
    public async Task ShouldFail_WhenIncorrectAnswerIsProvided()
    {
        var entity = SecurityEntityTestBuilder
            .New()
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(entity)
            .BuildMockAndRegister(this);

        var command = new ProvideDirectorLoansCommand(LoanApplicationIdTestData.LoanApplicationIdOne, "incorrect answer");

        Func<Task<OperationResult>> testDelegate = () => TestCandidate.Handle(command, CancellationToken.None);

        await testDelegate.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task ShouldChangeStatusToInProgress_WhenDirectLoansIsProvidedAndSecurityWasNotStarted()
    {
        var entity = SecurityEntityTestBuilder
            .New()
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(entity)
            .BuildMockAndRegister(this);

        var command = new ProvideDirectorLoansCommand(LoanApplicationIdTestData.LoanApplicationIdOne, CommonResponse.No);

        await TestCandidate.Handle(command, CancellationToken.None);

        entity.Status.Should().Be(SectionStatus.InProgress);
    }

    [Fact]
    public async Task ShouldChangeStatusToInProgress_WhenDirectLoansWasChangedAndSecurityWasCompleted()
    {
        var entity = SecurityEntityTestBuilder
            .New()
            .WithDirectorLoans(new DirectorLoans(true))
            .ThatIsCompleted()
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(entity)
            .BuildMockAndRegister(this);

        var command = new ProvideDirectorLoansCommand(LoanApplicationIdTestData.LoanApplicationIdOne, CommonResponse.No);

        await TestCandidate.Handle(command, CancellationToken.None);

        entity.Status.Should().Be(SectionStatus.InProgress);
    }

    [Fact]
    public async Task ShouldNotChangeStatus_WhenDirectLoansWasNotChangedAndSecurityWasCompleted()
    {
        var entity = SecurityEntityTestBuilder
            .New()
            .WithDirectorLoans(new DirectorLoans(true))
            .ThatIsCompleted()
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(entity)
            .BuildMockAndRegister(this);

        var command = new ProvideDirectorLoansCommand(LoanApplicationIdTestData.LoanApplicationIdOne, CommonResponse.Yes);

        await TestCandidate.Handle(command, CancellationToken.None);

        entity.Status.Should().Be(SectionStatus.Completed);
    }
}
