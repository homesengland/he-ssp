using AngleSharp.Io.Dom;

namespace HE.Investments.IntegrationTestsFramework.Data;

public sealed class FileEntry : IFile
{
    public FileEntry(string fileName, string type, Stream content)
    {
        Name = fileName;
        Type = type;
        Body = content;
        LastModified = DateTime.Now;
    }

    public Stream Body { get; }

    public bool IsClosed => Body.CanRead == false;

    public DateTime LastModified { get; }

    public int Length => (int)Body.Length;

    public string Name { get; }

    public string Type { get; }

    public IBlob Slice(int start = 0, int end = int.MaxValue, string? contentType = null)
    {
        var ms = new MemoryStream();
        Body.Position = start;
        var buffer = new byte[Math.Max(0, Math.Min(end, Body.Length) - start)];
        var bytesRead = Body.Read(buffer, 0, buffer.Length);
        ms.Write(buffer, 0, bytesRead);
        Body.Position = 0;
        return new FileEntry(Name, Type, ms);
    }

    public void Close()
    {
        Body.Close();
    }

    public void Dispose()
    {
        Body.Dispose();
    }
}
