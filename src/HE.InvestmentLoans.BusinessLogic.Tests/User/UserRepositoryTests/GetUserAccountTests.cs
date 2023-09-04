using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.UserRepositoryTests;
public class GetUserAccountTests : TestBase<LoanUserRepository>
{
    [Fact]
    public async Task ShouldReturnContactRolesDto_WhenUserGlobalIdAndEmailAreCorrect()
    {
        // given
        var contactRolesDto = ContactServiceMockTestBuilder
            .New()
            .Register(this)
            .ContactRolesFromMock;

        ContactServiceMockTestBuilder
            .New()
            .ReturnContactRolesDto(contactRolesDto)
            .Register(this);

        // when
        var result = await TestCandidate.GetUserAccount(UserGlobalId.From(contactRolesDto.externalId), contactRolesDto.email);

        // then
        result!.externalId.Should().Be(contactRolesDto.externalId);
        result.email.Should().Be(contactRolesDto.email);
        result.contactRoles.Should().BeEquivalentTo(contactRolesDto.contactRoles);
    }

    [Fact]
    public async Task ShouldThrowNotFoundException_WhenUserGlobalIdDoesNotExist()
    {
        // given
        var contactRolesDto = ContactServiceMockTestBuilder
            .New()
            .Register(this)
            .ContactRolesFromMock;

        ContactServiceMockTestBuilder
            .New()
            .ReturnContactRolesDto(contactRolesDto)
            .Register(this);

        var wrongUserGlobalId = "Wrong user global id";

        // when
        var action = () => TestCandidate.GetUserAccount(UserGlobalId.From(wrongUserGlobalId), contactRolesDto.email);

        // then
        await action.Should().ThrowExactlyAsync<NotFoundException>().WithMessage($"*{wrongUserGlobalId}*");
    }
}
