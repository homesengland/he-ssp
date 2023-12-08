using System;
using System.Collections.Generic;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Services.HomeType
{
    public interface IHomeTypeService : ICrmService
    {
        List<HomeTypeDto> GetApplicaitonHomeTypes(string applicationId, string userId, string organisationId, string fieldsToRetrieve = null);
        Guid SetHomeType(string homeType, string userId, string organisationId, string applicationId, string fieldsToSet = null);
        HomeTypeDto GetHomeType(string homeTypeId, string applicationId, string userId, string organisationId, string fieldsToRetrieve = null);
        void DeleteHomeType(string homeTypeId, string userId, string organisationId, string applicationId);
        void SetHappiPrinciplesValue(invln_HomeType target);
        void SetWhichNdssStandardsHaveBeenMetValue(invln_HomeType target);
        void CreateDocumentLocation(invln_HomeType target);
    }
}
