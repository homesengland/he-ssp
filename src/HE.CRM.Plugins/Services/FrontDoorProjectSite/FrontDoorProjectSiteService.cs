using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.interfaces;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Model.CrmSerializedParameters;
using HE.CRM.Plugins.Services.FrontDoorProject;
using HE.CRM.Plugins.Services.GovNotifyEmail;
using HE.CRM.Plugins.Services.LoanApplication;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Linq;

namespace HE.CRM.Plugins.Services.FrontDoorProjectSite
{
    public class FrontDoorProjectSiteService : CrmService, IFrontDoorProjectSiteService
    {
        #region Fields
        private readonly IFrontDoorProjectSiteRepository _frontDoorProjectSiteRepository;
        #endregion

        #region Constructors
        public FrontDoorProjectSiteService(CrmServiceArgs args) : base(args)
        {
            _frontDoorProjectSiteRepository = CrmRepositoriesFactory.Get<IFrontDoorProjectSiteRepository>();
        }
        #endregion

        public FrontDoorProjectSiteDto GetSingleFrontDoorProjectSite(string frontDoorSiteId)
        {
            var frontDoorProjectSite = _frontDoorProjectSiteRepository.GetSingleFrontDoorProjectSite(frontDoorSiteId);
            var frontDoorProjectSiteDto = FrontDoorProjectSiteMapper.MapFrontDoorProjectSiteToDto(frontDoorProjectSite);
            return frontDoorProjectSiteDto;
        }

        public List<FrontDoorProjectSiteDto> GetMultipleSiteRelatedToFrontDoorProjectForGivenAccountAndContact(string frontDoorProjectId, string organisationId, string externalContactId)
        {
            List<FrontDoorProjectSiteDto> frontDoorProjectSiteDtoList = new List<FrontDoorProjectSiteDto>();
            var frontDoorProjectSiteList = _frontDoorProjectSiteRepository.GetMultipleSiteRelatedToFrontDoorProjectForGivenAccountAndContact(frontDoorProjectId, organisationId, externalContactId);

            foreach(var frontDoorProjectSite in frontDoorProjectSiteList)
            {
                var frontDoorProjectSiteDto = FrontDoorProjectSiteMapper.MapFrontDoorProjectSiteToDto(frontDoorProjectSite);
                frontDoorProjectSiteDtoList.Add(frontDoorProjectSiteDto);
            }

            return frontDoorProjectSiteDtoList;
        }
    }
}
