using FluentAssertions;
using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Domain.UserOrganisation.CommandHandlers;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.UserOrganisation.CommandHandlers;

public class InviteUserToOrganisationCommandHandlerTests : TestBase<InviteUserToOrganisationCommandHandler>
{
    [Fact]
    public async Task ShouldReturnValidationErrors_WhenUserDataIsNotValid()
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
}
