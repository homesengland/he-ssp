using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HE.CRM.Common.Api
{
    public interface IApiHttpClient
    {
        Task<TResponse> SendAsync<TResponse>(string relativeUrl, HttpMethod method, CancellationToken cancellationToken)
            where TResponse : class;

        Task<TResponse> SendAsync<TRequest, TResponse>(
            TRequest request,
            string relativeUrl,
            HttpMethod method,
            CancellationToken cancellationToken)
            where TResponse : class;
    }
}
