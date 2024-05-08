using System;
using DataverseModel;
using HE.Base.Common.Extensions;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Consortium
{
    public class RequestAddingMemberToConsortiumHandler : CrmActionHandlerBase<invln_requestaddingmembertoconsortiumRequest, DataverseContext>
    {
        private string OrganizationId => ExecutionData.GetInputParameter<string>(invln_requestaddingmembertoconsortiumRequest.Fields.invln_organisationid);
        private string ConsortiumId => ExecutionData.GetInputParameter<string>(invln_requestaddingmembertoconsortiumRequest.Fields.invln_consortiumid);

        private readonly IConsortiumMemberRepository _memberRepository;
        private readonly IAccountRepository _accountRepository;

        public RequestAddingMemberToConsortiumHandler(IConsortiumMemberRepository memberRepository,
                                                        IAccountRepository accountRepository)
        {
            _memberRepository = memberRepository;
            _accountRepository = accountRepository;
        }

        public override bool CanWork()
        {
            TracingService.Trace($"RequestAddingMemberToConsortiumHandler Can Work");
            return ConsortiumId != null && OrganizationId != null && ConsortiumId != string.Empty && OrganizationId != string.Empty;
        }

        public override void DoWork()
        {
            TracingService.Trace($"RequestAddingMemberToConsortiumHandler Do Work");

            var account = _accountRepository.GetById(new Guid(OrganizationId), new string[] { Account.Fields.Name });

            var cm = new invln_ConsortiumMember
            {
                invln_Name = account.Name,
                invln_Consortium = new Guid(ConsortiumId).ToEntityReference<invln_Consortium>(),
                invln_Partner = new Guid(OrganizationId).ToEntityReference<Account>(),
                StatusCode = new OptionSetValue((int)invln_ConsortiumMember_StatusCode.Additionsubmitted)
            };
            _ = _memberRepository.Create(cm);
        }
    }
}
