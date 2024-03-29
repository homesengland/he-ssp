using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.FrontDoor.Domain.Site.Utilities;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Site.UtilitiesTests.LocalAuthorityDivisionTests;

public class IsLocalAuthorityNotAllowedForLoanApplicationTests
{
    public static IEnumerable<object[]> NotAllowedLocalAuthority =>
        Enumerable.Range(1, 10)
            .Select(x => new object[] { $"E080000{x:00}" })
            .Concat(Enumerable.Range(1, 10).Select(x => new object[] { $"80000{x:00}" }));

    [Theory]
    [SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "False positive returned by analyzer")]
    [MemberData(nameof(NotAllowedLocalAuthority))]
    public void ShouldReturnTrue_WhenLocalAuthorityIsNotAllowedForLoanApplication(string localAuthorityCode)
    {
        // given && when
        var result = LocalAuthorityDivision.IsLocalAuthorityNotAllowedForLoanApplication(localAuthorityCode);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("E08000011")]
    [InlineData("8000011")]
    public void ShouldReturnFalse_WhenLocalAuthorityIsAllowedForLoanApplication(string localAuthorityCode)
    {
        // given && when
        var result = LocalAuthorityDivision.IsLocalAuthorityNotAllowedForLoanApplication(localAuthorityCode);

        // then
        result.Should().BeFalse();
    }
}
