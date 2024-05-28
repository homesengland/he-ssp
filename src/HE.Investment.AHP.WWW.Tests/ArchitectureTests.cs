using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using HE.Investments.TestsUtils.Helpers;

namespace HE.Investment.AHP.WWW.Tests;

public class ArchitectureTests
{
    public static IEnumerable<object[]> AssemblyTestData
    {
        get
        {
            yield return [typeof(Program).Assembly];
            yield return [typeof(Domain.Config.DomainModule).Assembly];
            yield return [typeof(Contract.Application.AhpApplicationId).Assembly];
            yield return [typeof(HE.Investments.Programme.Domain.Config.DomainModule).Assembly];
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
        notAllowedProjectReferences.Should().BeEmpty($"only Common and AHP projects can be referenced in {assembly.GetName().Name}");
    }
}
