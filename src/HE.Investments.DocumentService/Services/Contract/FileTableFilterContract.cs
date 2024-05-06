namespace HE.Investments.DocumentService.Services.Contract;

internal sealed class FileTableFilterContract
{
    public string ListTitle { get; set; }

    public string ListAlias { get; set; }

    public List<string> FolderPaths { get; set; }

    public string? PagingInfo { get; set; }
}
