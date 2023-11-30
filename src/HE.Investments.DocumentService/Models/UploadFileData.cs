namespace HE.Investments.DocumentService.Models;

public sealed class UploadFileData<TMetadata>
{
    public UploadFileData(string name, TMetadata metadata, Stream content)
    {
        Name = name;
        Metadata = metadata;
        Content = content;
    }

    public string Name { get; }

    public TMetadata Metadata { get; }

    public Stream Content { get; }
}
