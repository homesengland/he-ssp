using System.Threading.Tasks;

namespace HE.CRM.Common.Api.Auth
{
    public interface ITokenProvider
    {
        Task<string> GetToken();
    }
}
