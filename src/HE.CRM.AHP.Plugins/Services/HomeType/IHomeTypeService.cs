using System;
using System.Collections.Generic;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Services.HomeType
{
    public interface IHomeTypeService : ICrmService
    {
        List<HomeTypeDto> GetApplicaitonHomeTypes(string applicationId);
        void SetHomeType(string homeType, string fieldsToSet = null);
        HomeTypeDto GetHomeType(string homeTypeId, string fieldsToRetrieve = null);
    }
}
