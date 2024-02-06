namespace HE.DocumentService.SharePoint.Configurartion;

public class SharePointConfiguration : ISharePointConfiguration
{
    public string ClientId { get; set; }

    public string ClientSecret { get; set; }

    public string SiteUrl { get; set; }

    public string WhiteListOfFileExtensions { get; set; }

    /// <summary>
    /// SP_FileMaxSize the value in bytes
    /// </summary>
    public int FileMaxSize { get; set; }

    public List<string> AllowedFileExtensions() => WhiteListOfFileExtensions.Split(',').ToList();
}
