using System;
using DataverseModel;
using HE.Base.Repositories;
using HE.Base.Services;
using HE.CRM.Common.Services.Interfaces;

namespace HE.CRM.Common.Services
{
    public class EnvironmentVariableSecretValueService : CrmService, IEnvironmentVariableSecretValueService
    {
        private readonly ICrmEntityRepository<EnvironmentVariableValue, DataverseContext> _envValRepo;

        public EnvironmentVariableSecretValueService(CrmServiceArgs args) : base(args)
        {
            _envValRepo = CrmRepositoriesFactory.GetSystemBase<EnvironmentVariableValue, DataverseContext>();
        }

        public string GetEnvironmentVariableValue(string environmentVariableName)
        {
            var retrieveRequest = new RetrieveEnvironmentVariableSecretValueRequest
            {
                EnvironmentVariableName = environmentVariableName
            };

            try
            {
                var retrievedResponse = _envValRepo.Execute<RetrieveEnvironmentVariableSecretValueRequest, RetrieveEnvironmentVariableSecretValueResponse>(retrieveRequest);
                return retrievedResponse.EnvironmentVariableSecretValue;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occured {nameof(EnvironmentVariableSecretValueService)}.{nameof(GetEnvironmentVariableValue)}: {environmentVariableName}");
                Logger.Error(ex.Message);
                throw;
            }
        }
    }
}
