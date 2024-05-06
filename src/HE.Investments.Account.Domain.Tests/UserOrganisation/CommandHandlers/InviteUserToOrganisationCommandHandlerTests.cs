using FluentAssertions;
using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Domain.Tests.UserOrganisation.TestDataBuilders;
using HE.Investments.Account.Domain.UserOrganisation.CommandHandlers;
using HE.Investments.Account.Domain.UserOrganisation.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.UserOrganisation.CommandHandlers;

public class InviteUserToOrganisationCommandHandlerTests : TestBase<InviteUserToOrganisationCommandHandler>
{
    [Fact]
    public async Task ShouldReturnValidationErrors_WhenUserDataIsNotProvided()
    {
        // given
        var command = new InviteUserToOrganisationCommand(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            null);

        // when
        var result = await TestCandidate.Handle(command, CancellationToken.None);

        // then
        result.Should().NotBeNull();
        result.HasValidationErrors.Should().BeTrue();
        result.Errors.Should()
            .HaveCount(5)
            .And.Contain(new ErrorItem("FirstName", "Enter the first name"))
            .And.Contain(new ErrorItem("LastName", "Enter the last name"))
            .And.Contain(new ErrorItem("EmailAddress", "Enter the email address"))
            .And.Contain(new ErrorItem("JobTitle", "Enter the job title"))
            .And.Contain(new ErrorItem("Role", "Select the role"));
    }

    [Fact]
    public async Task ShouldReturnSuccessResult_WhenUserDataProvided()
    {
        // given
        var command = new InviteUserToOrganisationCommand("John", "Paul", "john.paul@he.gov.uk", "Pope", UserRole.Enhanced);
        var userContext = CreateAndRegisterDependencyMock<IAccountUserContext>();
        var organisationUsersRepository = CreateAndRegisterDependencyMock<IOrganisationUsersRepository>();

        userContext.Setup(x => x.GetSelectedAccount()).ReturnsAsync(UserAccountTestData.UserAccountOne);
        organisationUsersRepository.Setup(x => x.GetOrganisationUsers(It.IsAny<OrganisationId>(), CancellationToken.None))
            .ReturnsAsync(new OrganisationUsersEntityTestDataBuilder().Build());

        // when
        var result = await TestCandidate.Handle(command, CancellationToken.None);

        // then
        result.Should().NotBeNull();
        result.HasValidationErrors.Should().BeFalse();
    }
}
