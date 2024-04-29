using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Consortium
{
    public class SetConsortiumHandler : CrmActionHandlerBase<invln_setconsortiumRequest, DataverseContext>
    {
        private string UserId => ExecutionData.GetInputParameter<string>(invln_setconsortiumRequest.Fields.invln_userid);
        private string LeadPartnerId => ExecutionData.GetInputParameter<string>(invln_setconsortiumRequest.Fields.invln_leadpartnerid);
        private string ProgrammeId => ExecutionData.GetInputParameter<string>(invln_setconsortiumRequest.Fields.invln_programmeId);
        private string ConsortiumName => ExecutionData.GetInputParameter<string>(invln_setconsortiumRequest.Fields.invln_consortiumname);

        private readonly IContactRepository _contactRepository;
        private readonly IConsortiumRepository _consortiumRepository;

        public SetConsortiumHandler(IAccountRepository accountRepository, IProgrammeRepository programmeRepository, IContactRepository contactRepository, IConsortiumRepository consortiumRepository)
        {
            _contactRepository = contactRepository;
            _consortiumRepository = consortiumRepository;
        }


        public override bool CanWork()
        {
            return UserId != null && LeadPartnerId != null && ProgrammeId != null && ConsortiumName != null;
        }

        public override void DoWork()
        {
            TracingService.Trace($"Get Contact");
            var contact = _contactRepository.GetContactViaExternalId(UserId);
            TracingService.Trace($"contact with id: {contact.Id}");
            if (contact != null && ProgrammeId != null)
            {
                TracingService.Trace($"PrePare Data ");
                var consortium = new invln_Consortium()
                {
                    invln_Name = ConsortiumName,
                    invln_LeadPartner = new EntityReference(Account.EntityLogicalName, new Guid(LeadPartnerId)),
                    invln_Programme = new EntityReference(invln_programme.EntityLogicalName, new Guid(ProgrammeId)),
                    invln_Createdby = contact.ToEntityReference()
                };
                TracingService.Trace($"Create Consortium");
                var consortiumId = _consortiumRepository.Create(consortium);
                TracingService.Trace($"consortium Id: {consortiumId}");
                ExecutionData.SetOutputParameter(invln_setconsortiumResponse.Fields.invln_consortiumid, consortiumId.ToString());
            }
        }
    }
}
