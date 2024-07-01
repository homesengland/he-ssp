using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface ISecretVariableRepository : ICrmEntityRepository<invln_SecretVariable, DataverseContext>
    {
        IEnumerable<invln_SecretVariable> GetMultiple(params string[] variableNames);
    }
}
