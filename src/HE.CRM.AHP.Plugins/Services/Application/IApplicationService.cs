using System;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Services.Application
{
    public interface IApplicationService : ICrmService
    {
        Guid SetApplication(string applicationSerialized, string fieldsToUpdate = null);
        AhpApplicationDto GetApplication(string applicationId, string fieldsToRetrieve = null);
    }
}
