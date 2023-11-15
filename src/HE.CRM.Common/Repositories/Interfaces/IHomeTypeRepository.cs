using System;
using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IHomeTypeRepository : ICrmEntityRepository<invln_HomeType, DataverseContext>
    {
        List<invln_HomeType> GetHomeTypesRelatedToApplication(Guid applicationId);
        invln_HomeType GetHomeTypeByIdAndApplicationId(string homeTypeId, string applicationId, string attributes = null);
    }
}
