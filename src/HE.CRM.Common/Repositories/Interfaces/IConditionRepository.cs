using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;
using HE.Base.Repositories;
using DataverseModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IConditionRepository : ICrmEntityRepository<invln_Conditions, DataverseContext>
    {
        List<invln_Conditions> GetBespokeConditionsForLoanApplication(Guid loanId);
    }
}
