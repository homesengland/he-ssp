using System;
using System.Collections.Generic;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Services.Application
{
    public interface IApplicationService : ICrmService
    {
        Guid SetApplication(string applicationSerialized, string organisationId, string contactId, string fieldsToUpdate = null);
        List<AhpApplicationDto> GetApplication(string organisationId, string contactId, string fieldsToRetrieve = null, string applicationId = null);
        bool CheckIfApplicationExists(string serializedApplication);
        void CheckIfApplicationWithNewNameExists(invln_scheme target, invln_scheme preImage);
    }
}
