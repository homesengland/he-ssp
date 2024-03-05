namespace HE.DocumentService.SharePoint.Configuration;

public interface ISharePointConfiguration
{
    public string ClientId { get; set; }

    public string ClientSecret { get; set; }

    public string SiteUrl { get; set; }

    /// <summary>
    /// SP_FileMaxSize the value in bytes.
    /// </summary>
    public int FileMaxSize { get; set; }

    public int RetryCount { get; set; }
}
