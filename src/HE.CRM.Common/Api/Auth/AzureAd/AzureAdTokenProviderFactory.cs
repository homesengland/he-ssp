using HE.Base.Services;

namespace HE.CRM.Common.Api.Auth.AzureAd
{
    public sealed class AzureAdTokenProviderFactory : CrmService, IAzureAdTokenProviderFactory
    {
        public AzureAdTokenProviderFactory(CrmServiceArgs args)
            : base(args)
        {
        }

        public ITokenProvider Create(AzureAdAuthConfig config)
        {
            return new AzureAdTokenProvider(Logger, config);
        }
    }
}
