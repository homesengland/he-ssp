using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.ValueObjects;

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
        result.Code.ToString().Should().Be(id);
        result.Name.Should().Be(name);
    }
}
