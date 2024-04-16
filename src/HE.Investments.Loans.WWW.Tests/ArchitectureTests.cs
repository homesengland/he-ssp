using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using HE.Investments.TestsUtils.Helpers;

namespace HE.Investments.Loans.WWW.Tests;

public class ArchitectureTests
{
    public static IEnumerable<object[]> AssemblyTestData
    {
        get
        {
            yield return new object[] { typeof(Program).Assembly };
            yield return new object[] { typeof(BusinessLogic.Config.BusinessLogicModule).Assembly };
            yield return new object[] { typeof(Common.Utils.Constants.FundingPurposeString).Assembly };
            yield return new object[] { typeof(Contract.LoanApplicationErrorCodes).Assembly };
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
        notAllowedProjectReferences.Should().BeEmpty($"only Common and Loans projects can be referenced in {assembly.GetName().Name}");
    }
}
