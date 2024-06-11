namespace HE.Investments.Api.Auth;

public interface IApiTokenProvider
{
    Task<string> GetToken();
}
