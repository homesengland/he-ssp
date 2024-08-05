using HE.DocumentService.SharePoint.Configuration;
using HE.DocumentService.SharePoint.Interfaces;
using Microsoft.SharePoint.Client;
using PnP.Framework;

namespace HE.DocumentService.SharePoint.Services;

public class SharePointContext : ISharePointContext, IDisposable
{
    private readonly Lazy<ClientContext> _context;

    public SharePointContext(ISharePointConfiguration spConfig)
    {
        _context = new Lazy<ClientContext>(
            () => new AuthenticationManager().GetACSAppOnlyContext(spConfig.SiteUrl, spConfig.ClientId, spConfig.ClientSecret));
    }

    public ClientContext Context => _context.Value;

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing && _context.IsValueCreated)
        {
            _context.Value.Dispose();
        }
    }
}
