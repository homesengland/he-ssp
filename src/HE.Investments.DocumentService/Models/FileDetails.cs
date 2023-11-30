namespace HE.Investments.DocumentService.Models;

public record FileDetails<TMetadata>(int Id, string FileName, string FolderPath, int Size, string Editor, DateTime Modified, TMetadata? Metadata)
    where TMetadata : class;
