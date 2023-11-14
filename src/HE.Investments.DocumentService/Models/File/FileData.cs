namespace HE.Investments.DocumentService.Models.File;

public sealed class FileData : IDisposable
{
    public FileData(string name, Stream content)
    {
        Name = name;
        Content = content;
    }

    public string Name { get; }

    public Stream Content { get; }

    public void Dispose()
    {
        Content.Dispose();
    }
}
