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
            : base(new HttpClientHandler())
        {
            _tokenProvider = tokenProvider;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenProvider.GetToken());

            return base.SendAsync(request, cancellationToken);
        }
    }
}
