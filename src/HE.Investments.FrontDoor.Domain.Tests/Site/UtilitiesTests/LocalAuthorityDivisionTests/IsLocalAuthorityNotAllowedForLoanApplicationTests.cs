using FluentAssertions;
using HE.Investments.FrontDoor.Domain.Site.Utilities;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Site.UtilitiesTests.LocalAuthorityDivisionTests;

public class IsLocalAuthorityNotAllowedForLoanApplicationTests
{
    [Theory]
    [InlineData("E08000001")]
    [InlineData("E08000002")]
    [InlineData("E08000003")]
    [InlineData("E08000004")]
    [InlineData("E08000005")]
    [InlineData("E08000006")]
    [InlineData("E08000007")]
    [InlineData("E08000008")]
    [InlineData("E08000009")]
    [InlineData("E08000010")]
    public void ShouldReturnTrue_WhenLocalAuthorityIsNotAllowedForLoanApplication(string localAuthorityCode)
    {
        // given && when
        var result = LocalAuthorityDivision.IsLocalAuthorityNotAllowedForLoanApplication(localAuthorityCode);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("E08000011")]
    [InlineData("E08000012")]
    [InlineData("E08000013")]
    [InlineData("E08000014")]
    [InlineData("E08000015")]
    [InlineData("E08000016")]
    public void ShouldReturnFalse_WhenLocalAuthorityIsAllowedForLoanApplication(string localAuthorityCode)
    {
        // given && when
        var result = LocalAuthorityDivision.IsLocalAuthorityNotAllowedForLoanApplication(localAuthorityCode);

        // then
        result.Should().BeFalse();
    }
}
