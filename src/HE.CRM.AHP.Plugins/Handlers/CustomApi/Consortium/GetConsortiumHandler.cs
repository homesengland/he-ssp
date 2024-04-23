using System;
using System.Collections.Generic;
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
        private string consortiumId => ExecutionData.GetInputParameter<string>(invln_getconsortiumRequest.Fields.invln_consortiumid);

        private readonly IConsortiumRepository _consortiumRepository;
        private readonly IConsortiumMemberRepository _consortiumMemberRepository;

        public GetConsortiumHandler(IConsortiumRepository consortiumRepository, IConsortiumMemberRepository consortiumMemberRepository)
        {
            _consortiumRepository = consortiumRepository;
            _consortiumMemberRepository = consortiumMemberRepository;
        }

        public override bool CanWork()
        {
            return consortiumId != null && consortiumId != string.Empty;
        }

        public override void DoWork()
        {
            var consortium = _consortiumRepository.GetConsortiumById(consortiumId, new ColumnSet(true));
            if (consortium != null)
            {
                var consortiumMembers = _consortiumMemberRepository.GetByAttribute(invln_ConsortiumMember.Fields.invln_Consortium, consortiumId,
                                                                    new string[] { invln_ConsortiumMember.Fields.invln_Name, invln_ConsortiumMember.Fields.Id,
                                                                    invln_ConsortiumMember.Fields.StatusCode, invln_ConsortiumMember.Fields.invln_Partner }).ToList();
                var result = ConsortiumMapper.MapReguralEntityToDto(consortium, consortiumMembers);
                var serialized = JsonSerializer.Serialize(result);
                ExecutionData.SetOutputParameter(invln_getconsortiumResponse.Fields.invln_consortium, serialized);
            }
        }
    }
}
