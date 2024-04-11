using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using HE.Investments.TestsUtils.Helpers;
using Xunit;

namespace HE.Investments.FrontDoor.WWW.Tests;

public class ArchitectureTests
{
    public static IEnumerable<object[]> AssemblyTestData
    {
        get
        {
            yield return new object[] { typeof(Program).Assembly };
            yield return new object[] { typeof(Domain.Config.DomainModule).Assembly };
            yield return new object[] { typeof(Contract.Site.SiteDetails).Assembly };
            yield return new object[] { typeof(Shared.Config.FrontDoorSharedModule).Assembly };
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
        notAllowedProjectReferences.Should().BeEmpty($"only Common and FrontDoor projects can be referenced in {assembly.GetName().Name}");
    }
}
