using System;
using System.Collections.Generic;
using System.Text;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using DataverseModel;
using System.Diagnostics.Eventing.Reader;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class EnvironmentVariableRepository : CrmEntityRepository<EnvironmentVariableValue, DataverseContext>, IEnvironmentVariableRepository
    {
        #region Constructors

        public EnvironmentVariableRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        #endregion

        #region Interface Implementation

        public string GetEnvironmentVariableValue(string variableName)
        {
            if (!string.IsNullOrEmpty(variableName))
            {
                var varRequest = new RetrieveEnvironmentVariableValueRequest()
                {
                    DefinitionSchemaName = variableName
                };

                var result = (RetrieveEnvironmentVariableValueResponse)this.service.Execute(varRequest);

                return result?.Value;
            }

            return null;
        }

        #endregion
    }
}
