using System.Reflection;

namespace HE.Investments.TestsUtils.Helpers;

public static class ArchitectureTestHelper
{
    private static readonly List<string> AllowedCommonProjects =
    [
        "HE.Investments.Common",
        "HE.Investments.DocumentService",
        "HE.Investments.Organisation",
        "HE.Investments.Account.Shared",
        "HE.Investments.Account.Api.Contract",
        "HE.Investments.FrontDoor.Shared",
        "HE.Investments.Programme.Contract"
    ];

    private static readonly List<string> AllowedSubdomainProjects =
    [
        "HE.Investments.Programme.Domain"
    ];

    public static IList<string> GetNotAllowedProjectReferences(Assembly assembly)
    {
        var currentAssembly = new ReferencedAssembly(assembly.GetName());

        return assembly.GetReferencedAssemblies()
            .Select(x => new ReferencedAssembly(x))
            .Where(x => x.IsInvestmentAssembly)
            .Where(x => !(x.IsTheSameDomainProject(currentAssembly)
                          || x.IsAllowedCommonProject
                          || (currentAssembly.IsWww && x.IsAllowedSubdomainProject)))
            .Select(x => x.Name)
            .ToList();
    }

    private class ReferencedAssembly
    {
        private const string ProjectNamePrefix = "HE.Investment";

        public ReferencedAssembly(AssemblyName assemblyName)
        {
            Name = assemblyName.Name!;
        }

        public string Name { get; }

        public bool IsInvestmentAssembly => Name.StartsWith(ProjectNamePrefix, StringComparison.InvariantCultureIgnoreCase);

        public bool IsAllowedCommonProject => AllowedCommonProjects.Exists(NameStartsWith);

        public bool IsAllowedSubdomainProject => AllowedSubdomainProjects.Exists(NameStartsWith);

        public bool IsWww => Name.Split('.')[^1] == "WWW";

        public bool IsTheSameDomainProject(ReferencedAssembly other)
        {
            var otherProjectPrefix = string.Join(".", other.Name.Split(".").Take(3));
            var otherProjectAlternativePrefix = otherProjectPrefix.Replace("Investment", "Investments");

            return NameStartsWith(otherProjectPrefix) || NameStartsWith(otherProjectAlternativePrefix);
        }

        private bool NameStartsWith(string other) => Name.StartsWith(other, StringComparison.InvariantCultureIgnoreCase);
    }
}
