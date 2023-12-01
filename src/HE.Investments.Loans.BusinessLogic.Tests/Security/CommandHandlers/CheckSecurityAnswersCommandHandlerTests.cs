using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using HE.Investments.Loans.BusinessLogic.Security.CommandHandler;
using HE.Investments.Loans.BusinessLogic.Tests.Security.TestObjectBuilder;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Security.Commands;
using HE.Investments.Loans.Contract.Security.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Security.CommandHandlers;
public class CheckSecurityAnswersCommandHandlerTests : TestBase<CheckSecurityAnswersCommandHandler>
{
    private ConfirmSecuritySectionCommand _command;

    public CheckSecurityAnswersCommandHandlerTests()
    {
        AccountUserContextTestBuilder
            .New()
            .Register(this);
    }

    [Fact]
    public async Task ShouldFail_WhenDebentureIsNotProvided()
    {
        var security = SecurityEntityTestBuilder
            .New()
            .WithDirectorLoans(new DirectorLoans(false))
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(security)
            .BuildMockAndRegister(this);

        _command = new ConfirmSecuritySectionCommand(LoanApplicationIdTestData.LoanApplicationIdOne, CommonResponse.Yes);

        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        result.HasValidationErrors.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldFail_WhenDirectorLoansAreNotProvided()
    {
        var security = SecurityEntityTestBuilder
            .New()
            .WithDebenture(new Debenture("holder", true))
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(security)
            .BuildMockAndRegister(this);

        _command = new ConfirmSecuritySectionCommand(LoanApplicationIdTestData.LoanApplicationIdOne, CommonResponse.Yes);

        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        result.HasValidationErrors.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldFail_WhenDirectorLoansArePresentButDirectorLoanSubordinationIsNot()
    {
        var security = SecurityEntityTestBuilder
            .New()
            .WithDebenture(new Debenture("holder", true))
            .WithDirectorLoans(new DirectorLoans(true))
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(security)
            .BuildMockAndRegister(this);

        _command = new ConfirmSecuritySectionCommand(LoanApplicationIdTestData.LoanApplicationIdOne, CommonResponse.Yes);

        var result = await TestCandidate.Handle(_command, CancellationToken.None);

        result.HasValidationErrors.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldChangeStatus_WhenAllDataIsProvided()
    {
        var security = SecurityEntityTestBuilder
            .New()
            .WithDebenture(new Debenture("holder", true))
            .WithDirectorLoans(new DirectorLoans(false))
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(security)
            .BuildMockAndRegister(this);

        _command = new ConfirmSecuritySectionCommand(LoanApplicationIdTestData.LoanApplicationIdOne, CommonResponse.Yes);

        await TestCandidate.Handle(_command, CancellationToken.None);

        security.Status.Should().Be(SectionStatus.Completed);
    }

    [Fact]
    public async Task ShouldChangeStatusFromCompletedToInProgress_WhenAnswerIsNoAndSectionWasCompleted()
    {
        var security = SecurityEntityTestBuilder
            .New()
            .ThatIsCompleted()
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(security)
            .BuildMockAndRegister(this);

        _command = new ConfirmSecuritySectionCommand(LoanApplicationIdTestData.LoanApplicationIdOne, CommonResponse.No);

        await TestCandidate.Handle(_command, CancellationToken.None);

        security.Status.Should().Be(SectionStatus.InProgress);
    }
}
