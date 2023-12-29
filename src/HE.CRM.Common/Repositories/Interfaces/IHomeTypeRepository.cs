using System;
using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;

namespace HE.CRM.Common.Repositories.Interfaces
{

    public interface IHomeTypeRepository : ICrmEntityRepository<invln_HomeType, DataverseContext>
    {
        List<invln_HomeType> GetHomeTypesForNullableUserAndOrganisationRelatedToApplication(string applicationId, string userId, string organisationId, string attributes = null);
        invln_HomeType GetHomeTypeForNullableUserAndOrganisationByIdAndApplicationId(string homeTypeId, string applicationId, string userId, string organisationId, string attributes = null);
        bool CheckIfGivenHomeTypeIsAssignedToGivenOrganisationAndApplication(Guid homeTypeId, Guid organisationId, Guid applicationId);
    }
}
