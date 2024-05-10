using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Consortium
{
    public class RemoveMemberFromConsortiumHandler : CrmActionHandlerBase<invln_requesttoremovememberRequest, DataverseContext>
    {
        private string OrganizationId => ExecutionData.GetInputParameter<string>(invln_requesttoremovememberRequest.Fields.invln_organizationid);
        private string ConsortiumId => ExecutionData.GetInputParameter<string>(invln_requesttoremovememberRequest.Fields.invln_consortiumid);

        private readonly IConsortiumMemberRepository _consortiumMemberRepository;

        public RemoveMemberFromConsortiumHandler(IConsortiumMemberRepository consortiumMemberRepository)
        {
            _consortiumMemberRepository = consortiumMemberRepository;
        }

        public override bool CanWork()
        {
            return !string.IsNullOrEmpty(OrganizationId) && !string.IsNullOrEmpty(ConsortiumId);
        }

        public override void DoWork()
        {
            var consortium = _consortiumMemberRepository.GetMemberByOrganizstationIdAndConsortiumId(OrganizationId, ConsortiumId);
            if (consortium != null)
            {
                consortium.StatusCode = new OptionSetValue((int)invln_ConsortiumMember_StatusCode.Removalsubmitted);
                _consortiumMemberRepository.Update(consortium);
            }
        }
    }
}
