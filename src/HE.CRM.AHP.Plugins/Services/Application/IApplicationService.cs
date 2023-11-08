using System;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Services.Application
{
    public interface IApplicationService : ICrmService
    {
        Guid SetApplication(string applicationSerialized, string organisationId, string contactId, string fieldsToUpdate = null);
        AhpApplicationDto GetApplication(string applicationId, string organisationId, string contactId, string fieldsToRetrieve = null);
    }
}
