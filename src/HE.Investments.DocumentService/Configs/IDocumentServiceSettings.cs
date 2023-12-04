namespace HE.Investments.DocumentService.Configs;

public interface IDocumentServiceSettings
{
    public string Url { get; }

    public bool UseMock { get; }
}
