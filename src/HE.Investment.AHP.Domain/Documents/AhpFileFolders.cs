namespace HE.Investment.AHP.Domain.Documents;

public static class AhpFileFolders
{
    public static readonly Func<string, string> LocalAuthoritySupportFilesFolder =
        rootDirectory => $"{rootDirectory}/internal/Local Authority Support";

    public static readonly Func<string, string, string> DesignFilesFolder =
        (rootDirectory, homeTypeId) => $"{rootDirectory}/Home Types/{homeTypeId}/internal/Design Files";

    public static readonly Func<string, IEnumerable<string>> ApplicationFolders = rootDirectory => new[]
    {
        LocalAuthoritySupportFilesFolder(rootDirectory),
        $"{rootDirectory}/external/Local Authority Support",
        $"{rootDirectory}/Home Types",
    };

    public static readonly Func<string, string, IEnumerable<string>> HomeTypeFolders = (rootDirectory, homeTypeId) => new[]
    {
        DesignFilesFolder(rootDirectory, homeTypeId),
        $"{rootDirectory}/Home Types/{homeTypeId}/external/Design Files",
    };
}
