using HE.Investment.AHP.Contract.HomeTypes;

namespace HE.Investment.AHP.Domain.Documents;

public static class AhpFileFolders
{
    public static readonly Func<string, string> LocalAuthoritySupportFilesFolder =
        rootDirectory => $"{rootDirectory}/external/Local Authority Support";

    public static readonly Func<string, HomeTypeId, string> DesignFilesFolder =
        (rootDirectory, homeTypeId) => $"{rootDirectory}/Home Types/{homeTypeId}/external/Design Files";

    public static readonly Func<string, IEnumerable<string>> ApplicationFolders = rootDirectory =>
    [
        LocalAuthoritySupportFilesFolder(rootDirectory),
        $"{rootDirectory}/internal/Local Authority Support",
        $"{rootDirectory}/Home Types",
    ];

    public static readonly Func<string, HomeTypeId, IEnumerable<string>> HomeTypeFolders = (rootDirectory, homeTypeId) =>
    [
        DesignFilesFolder(rootDirectory, homeTypeId),
        $"{rootDirectory}/Home Types/{homeTypeId}/internal/Design Files",
    ];
}
