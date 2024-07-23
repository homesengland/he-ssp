namespace HE.Investments.DocumentService.Models;

public sealed class UploadFileData<TMetadata>
{
    public UploadFileData(string name, TMetadata metadata, Stream content, string partitionId)
    {
        Name = name;
        Metadata = metadata;
        Content = content;
        PartitionId = partitionId;
    }

    public string Name { get; }

    public TMetadata Metadata { get; }

    public Stream Content { get; }

    public string PartitionId { get; }
}
