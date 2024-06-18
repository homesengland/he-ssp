using System.Threading.Tasks;

namespace HE.CRM.Common.Api.Auth
{
    public interface IApiTokenProvider
    {
        Task<string> GetToken();
    }
}
