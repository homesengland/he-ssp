using HE.Base.Repositories;
using DataverseModel;
using System.Collections.Generic;
using System;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IHeProjectRepository : ICrmEntityRepository<he_Pipeline, DataverseContext>
    {
        List<he_Pipeline> GetHeProject(string organisationCondition, string contactExternalIdFilter, string frontDoorProjectFilters, string recordStatusCondition);
        bool CheckIfHeProjectWithGivenNameExists(string frontDoorProjectName, Guid organisationId);
    }
}
