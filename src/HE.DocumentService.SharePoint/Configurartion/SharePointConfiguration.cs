namespace HE.DocumentService.SharePoint.Configurartion;

public class SharePointConfiguration : ISharePointConfiguration
{
    public string ClientId { get; }

    public string ClientSecret { get; }

    public string SiteUrl { get; }

    public string WhiteListOfFileExtensions { get; }

    /// <summary>
    /// SP_FileMaxSize the value in bytes
    /// </summary>
    public int FileMaxSize { get; }

    public List<string> AllowedFileExtensions => WhiteListOfFileExtensions.Split(',').ToList();
}