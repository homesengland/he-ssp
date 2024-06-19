using System.Threading.Tasks;
using HE.Base.Services;

namespace HE.CRM.Common.Api.Auth
{
    public interface IApiTokenProvider : ICrmService
    {
        Task<string> GetToken();
    }
}
