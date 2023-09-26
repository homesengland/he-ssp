using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IEnvironmentVariableRepository : ICrmEntityRepository<EnvironmentVariableValue, DataverseContext>
    {
        string GetEnvironmentVariableValue(string variableName);
    }
}
