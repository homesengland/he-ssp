namespace HE.Investment.AHP.WWW.Views.Project.Const;

public static class ProjectPageTitles
{
    public static string ApplicationList(string projectName) => $"{projectName}";

    public static string AllocationList(string programmeName) => $"{programmeName} allocations";

    public static string Start(string programmeFullName, string programmeShortName) => $"Apply for {programmeFullName} ({programmeShortName})";
}
