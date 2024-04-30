namespace HE.Investments.DocumentService.Services.Contract;

internal sealed class TableResultContract
{
    public IList<FileTableRowContract> Items { get; set; }

    public int TotalCount { get; set; }

    public string? PagingInfo { get; set; }
}
