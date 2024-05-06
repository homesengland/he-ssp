namespace HE.Investments.DocumentService.Services.Contract;

internal sealed class FileTableRowContract
{
    public int Id { get; set; }

    public string FileName { get; set; }

    public string FolderPath { get; set; }

    public int Size { get; set; }

    public string Editor { get; set; }

    public DateTime Modified { get; set; }

    public string? Metadata { get; set; }
}
