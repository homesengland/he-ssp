namespace HE.Investments.Common.CRM.Services;

public interface ICrmApiTokenProvider
{
    Task<string> GetToken();
}
