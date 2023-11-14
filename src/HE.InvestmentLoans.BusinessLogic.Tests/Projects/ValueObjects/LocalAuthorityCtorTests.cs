using HE.InvestmentLoans.Contract.Projects.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.ValueObjects;

public class LocalAuthorityCtorTests
{
    [Fact]
    public void ShouldCreateLocalAuthority()
    {
        // given
        var id = "1";
        var name = "Liverpool";

        // when
        var result = LocalAuthority.New(id, name);

        // then
        result.Id.ToString().Should().Be(id);
        result.Name.Should().Be(name);
    }
}
