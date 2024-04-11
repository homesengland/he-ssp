using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using HE.Investments.TestsUtils.Helpers;

namespace HE.Investments.Account.WWW.Tests;

public class ArchitectureTests
{
    public static IEnumerable<object[]> AssemblyTestData
    {
        get
        {
            yield return new object[] { typeof(Program).Assembly };
            yield return new object[] { typeof(Domain.Config.AccountModule).Assembly };
            yield return new object[] { typeof(Contract.User.UserProfileDetails).Assembly };
            yield return new object[] { typeof(Api.Contract.User.AccountDetails).Assembly };
            yield return new object[] { typeof(Shared.AccountAccessContext).Assembly };
        }
    }

    [Theory]
    [SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "False positive returned by analyzer")]
    [MemberData(nameof(AssemblyTestData))]
    public void ShouldNotAllowForbiddenReferences(Assembly assembly)
    {
        // given & when
        var notAllowedProjectReferences = ArchitectureTestHelper.GetNotAllowedProjectReferences(assembly);

        // then
        notAllowedProjectReferences.Should().BeEmpty($"only Common and Account projects can be referenced in {assembly.GetName().Name}");
    }
}
