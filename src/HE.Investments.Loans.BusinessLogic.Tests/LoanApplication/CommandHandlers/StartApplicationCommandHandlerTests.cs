using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.LoanApplication.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Entities;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Contract.Application.Commands;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.CommandHandlers;

public class StartApplicationCommandHandlerTests : TestBase<StartApplicationCommandHandler>
{
    [Fact]
    public async Task ShouldReturnFailedResult_WhenApplicationNameIsInvalid()
    {
        // given
        AccountUserContextTestBuilder
            .New()
            .Register(this);

        // when
        var action = () => TestCandidate.Handle(new StartApplicationCommand(string.Empty), CancellationToken.None);

        // then
        await action.Should().ThrowAsync<DomainValidationException>().WithMessage("Enter the name for your application");
    }

    [Fact]
    public async Task ShouldReturnFailedResult_WhenApplicationWithTheSameNameAlreadyExistsWithinOrganisation()
    {
        // given
        var applicationName = new LoanApplicationName("My application");
        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        CreateAndRegisterDependencyMock<ILoanApplicationRepository>()
            .Setup(x => x.IsExist(applicationName, userAccount, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // when
        var result = await TestCandidate.Handle(new StartApplicationCommand(applicationName.Value), CancellationToken.None);

        // then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1).And.ContainSingle(x => x.AffectedField == nameof(LoanApplicationName));
        result.ReturnedData.Should().BeNull();
    }

    [Fact]
    public async Task ShouldCreateLoanApplicationAndReturnId_WhenApplicationIsValid()
    {
        // given
        var applicationName = new LoanApplicationName("My application");
        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var repository = CreateAndRegisterDependencyMock<ILoanApplicationRepository>();
        repository.Setup(x => x.IsExist(applicationName, userAccount, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        repository.Setup(x => x.Save(
                It.Is<LoanApplicationEntity>(y => y.Name == applicationName && y.UserAccount == userAccount),
                It.IsAny<UserProfileDetails>(),
                It.IsAny<CancellationToken>()))
            .Callback<LoanApplicationEntity, UserProfileDetails, CancellationToken>((x, _, _) => x.SetId(LoanApplicationIdTestData.LoanApplicationIdOne));

        // when
        var result = await TestCandidate.Handle(new StartApplicationCommand(applicationName.Value), CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
        result.ReturnedData.Should().Be(LoanApplicationIdTestData.LoanApplicationIdOne);
    }
}
