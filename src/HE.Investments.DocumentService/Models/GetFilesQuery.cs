namespace HE.Investments.DocumentService.Models;

public record GetFilesQuery(string ListTitle, string ListAlias, IReadOnlyCollection<string> FolderPaths);
