using HE.Base.Services;

namespace HE.CRM.Common.Services.Interfaces
{
    public interface IEnvironmentVariableSecretValueService : ICrmService
    {
        string GetEnvironmentVariableValue(string environmentVariableName);
    }
}
