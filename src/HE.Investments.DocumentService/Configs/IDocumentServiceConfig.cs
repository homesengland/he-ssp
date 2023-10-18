namespace HE.Investments.DocumentService.Configs;

public interface IDocumentServiceConfig
{
    public string Url { get; set; }

    public string ListAlias { get; set; }

    public string ListTitle { get; set; }

    public int MaxFileSizeInMegabytes { get; set; }
}
