using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Consortium;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Consortium
{
    public class GetConsortiumHandler : CrmActionHandlerBase<invln_getconsortiumRequest, DataverseContext>
    {
        private string ConsortiumId => ExecutionData.GetInputParameter<string>(invln_getconsortiumRequest.Fields.invln_consortiumid);
        private string MemberOrganisationId => ExecutionData.GetInputParameter<string>(invln_getconsortiumRequest.Fields.invln_memberorganisationid);

        private readonly IConsortiumRepository _consortiumRepository;
        private readonly IConsortiumMemberRepository _consortiumMemberRepository;

        public GetConsortiumHandler(IConsortiumRepository consortiumRepository, IConsortiumMemberRepository consortiumMemberRepository)
        {
            _consortiumRepository = consortiumRepository;
            _consortiumMemberRepository = consortiumMemberRepository;
        }

        public override bool CanWork()
        {
            return ConsortiumId != null && ConsortiumId != string.Empty
                && MemberOrganisationId != null && MemberOrganisationId != string.Empty;
        }

        public override void DoWork()
        {
            if (Guid.TryParse(ConsortiumId, out var consortiumId))
            {
                var consortium = _consortiumRepository.GetById(consortiumId, new string[] {
                    invln_Consortium.Fields.Id,
                    invln_Consortium.Fields.invln_LeadPartner,
                    invln_Consortium.Fields.invln_Name,
                    invln_Consortium.Fields.invln_Programme
                }
                );
                if (consortium != null)
                {
                    var consortiumMembers = _consortiumMemberRepository.GetByAttribute(invln_ConsortiumMember.Fields.invln_Consortium, ConsortiumId,
                                                                        new string[] { invln_ConsortiumMember.Fields.invln_Name, invln_ConsortiumMember.Fields.Id,
                                                                    invln_ConsortiumMember.Fields.StatusCode, invln_ConsortiumMember.Fields.invln_Partner }).ToList();
                    var result = ConsortiumMapper.MapRegularEntityToDto(consortium, consortiumMembers);
                    if (!IsorganizationMemeberExist(consortium, consortiumMembers))
                    {
                        return;
                    }
                    ExecutionData.SetOutputParameter(invln_getconsortiumResponse.Fields.invln_consortium, JsonSerializer.Serialize(result));
                }
            }
        }

        private bool IsorganizationMemeberExist(invln_Consortium consortium, List<invln_ConsortiumMember> consortiumMembers)
        {
            if (consortium.invln_LeadPartner.Id.ToString().ToLower() == MemberOrganisationId.ToLower())
            {
                return true;
            }

            if (consortiumMembers.Count > 0)
            {
                return consortiumMembers.Any(x => x.invln_Partner.Id.ToString().ToLower() == MemberOrganisationId.ToLower());
            }
            return false;
        }
    }
}
