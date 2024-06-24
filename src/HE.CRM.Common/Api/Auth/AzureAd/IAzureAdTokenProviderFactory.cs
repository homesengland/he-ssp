using HE.Base.Services;

namespace HE.CRM.Common.Api.Auth.AzureAd
{
    public interface IAzureAdTokenProviderFactory : ICrmService
    {
        ITokenProvider Create(AzureAdAuthConfig config);
    }
}
