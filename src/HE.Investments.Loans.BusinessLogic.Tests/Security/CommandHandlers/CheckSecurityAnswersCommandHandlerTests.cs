using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Security.CommandHandler;
using HE.Investments.Loans.BusinessLogic.Tests.Security.TestObjectBuilder;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
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
        // given
        var security = SecurityEntityTestBuilder
            .New()
            .WithDirectorLoans(new DirectorLoans(false))
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(security)
            .BuildMockAndRegister(this);

        _command = new ConfirmSecuritySectionCommand(LoanApplicationIdTestData.LoanApplicationIdOne, CommonResponse.Yes);

        // when
        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        // then
        await action.Should().ThrowAsync<DomainValidationException>().WithMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public async Task ShouldFail_WhenDirectorLoansAreNotProvided()
    {
        // given
        var security = SecurityEntityTestBuilder
            .New()
            .WithDebenture(new Debenture("holder", true))
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(security)
            .BuildMockAndRegister(this);

        _command = new ConfirmSecuritySectionCommand(LoanApplicationIdTestData.LoanApplicationIdOne, CommonResponse.Yes);

        // when
        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        // then
        await action.Should().ThrowAsync<DomainValidationException>().WithMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public async Task ShouldFail_WhenDirectorLoansArePresentButDirectorLoanSubordinationIsNot()
    {
        // given
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

        // when
        var action = () => TestCandidate.Handle(_command, CancellationToken.None);

        // then
        await action.Should().ThrowAsync<DomainValidationException>().WithMessage(ValidationErrorMessage.CheckAnswersOption);
    }

    [Fact]
    public async Task ShouldChangeStatus_WhenAllDataIsProvided()
    {
        // given
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

        // when
        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        security.Status.Should().Be(SectionStatus.Completed);
    }

    [Fact]
    public async Task ShouldChangeStatusFromCompletedToInProgress_WhenAnswerIsNoAndSectionWasCompleted()
    {
        // given
        var security = SecurityEntityTestBuilder
            .New()
            .ThatIsCompleted()
            .Build();

        SecurityRepositoryTestBuilder
            .New()
            .ReturnsSecurityEntity(security)
            .BuildMockAndRegister(this);

        _command = new ConfirmSecuritySectionCommand(LoanApplicationIdTestData.LoanApplicationIdOne, CommonResponse.No);

        // when
        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        security.Status.Should().Be(SectionStatus.InProgress);
    }
}
