namespace HE.Investments.DocumentService.Models;

public sealed class DownloadFileData : IDisposable
{
    public DownloadFileData(string name, Stream content)
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
