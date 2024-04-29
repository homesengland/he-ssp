using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Consortium
{
    public class GetConsortiumsByOrganisationHandler : CrmActionHandlerBase<invln_getconsortiumsRequest, DataverseContext>
    {
        private string OrganisationId => ExecutionData.GetInputParameter<string>(invln_getconsortiumsRequest.Fields.invln_organisationid);

        private readonly IConsortiumRepository _consortiumRepository;

        public GetConsortiumsByOrganisationHandler(IConsortiumRepository consortiumRepository)
        {
            _consortiumRepository = consortiumRepository;
        }

        public override bool CanWork()
        {
            return OrganisationId != null;
        }

        public override void DoWork()
        {
            var listOfConsortiums = new List<invln_Consortium>();
            listOfConsortiums.AddRange(_consortiumRepository.GetByAttribute(invln_Consortium.Fields.invln_LeadPartner, new Guid(OrganisationId),
                new string[] { invln_Consortium.Fields.Id, invln_Consortium.Fields.invln_Programme, invln_Consortium.Fields.invln_LeadPartner,
                             invln_Consortium.Fields.StatusCode, invln_Consortium.Fields.invln_Name})
                            .ToList());
            var consortiesWhereAccountIsAMemer = _consortiumRepository.GetByLeadPartnerInConsoriumMember(OrganisationId);
            listOfConsortiums.AddRange(consortiesWhereAccountIsAMemer);
            if (listOfConsortiums.Count == 0)
                return;
            var listOfConsortiumsDto = new List<ConsortiumDto>();
            foreach (var consortium in listOfConsortiums)
            {
                listOfConsortiumsDto.Add(ConsortiumMapper.MapRegularEntityToDto(consortium));
            }
            ExecutionData.SetOutputParameter(invln_getconsortiumsResponse.Fields.invln_consortiums, JsonSerializer.Serialize(listOfConsortiumsDto));
        }
    }
}
