using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace HE.CRM.Common.Api.Auth
{
    internal sealed class BearerTokenAuthorizationHandler : DelegatingHandler
    {
        private readonly ITokenProvider _tokenProvider;

        public BearerTokenAuthorizationHandler(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _tokenProvider.GetToken());

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
