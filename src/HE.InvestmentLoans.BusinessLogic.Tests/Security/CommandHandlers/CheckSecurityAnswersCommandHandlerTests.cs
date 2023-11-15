using HE.InvestmentLoans.BusinessLogic.Security.CommandHandler;
using HE.InvestmentLoans.BusinessLogic.Tests.Security.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Security.Commands;
using HE.InvestmentLoans.Contract.Security.ValueObjects;
using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Security.CommandHandlers;
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
