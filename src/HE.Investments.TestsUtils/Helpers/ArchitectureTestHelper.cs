using System.Reflection;

namespace HE.Investments.TestsUtils.Helpers;

public static class ArchitectureTestHelper
{
    private const string ProjectNamePrefix = "HE.Investment";

    private static readonly List<string> AllowedCommonProjects =
    [
        "HE.Investments.Common",
        "HE.Investments.DocumentService",
        "HE.Investments.Organisation",
        "HE.Investments.Account.Shared",
        "HE.Investments.Account.Api.Contract",
        "HE.Investments.FrontDoor.Shared"
    ];

    public static IList<string> GetNotAllowedProjectReferences(Assembly assembly)
    {
        var investmentsProjects = assembly.GetReferencedAssemblies()
            .Select(x => x.Name!)
            .Where(x => x.StartsWith(ProjectNamePrefix, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
        var currentProjectPrefix = string.Join(".", assembly.GetName().Name!.Split(".").Take(3));
        var currentProjectAlternativePrefix = currentProjectPrefix.Replace("Investment", "Investments");

        return investmentsProjects
            .Where(x => AllowedCommonProjects.TrueForAll(y => !x.StartsWith(y, StringComparison.InvariantCultureIgnoreCase)))
            .Where(x => !x.StartsWith(currentProjectPrefix, StringComparison.InvariantCultureIgnoreCase)
                        && !x.StartsWith(currentProjectAlternativePrefix, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
}
