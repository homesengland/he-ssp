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
        private string LeadPartner => ExecutionData.GetInputParameter<string>(invln_setconsortiumRequest.Fields.invln_leadpartner);
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
            return UserId != null && LeadPartner != null && ProgrammeId != null && ConsortiumName != null;
        }

        public override void DoWork()
        {
            var contact = _contactRepository.GetContactViaExternalId(UserId);
            if (Guid.TryParse(UserId, out var leadPartnerId) && Guid.TryParse(UserId, out var programmeId))
            {
                var consortium = new invln_Consortium()
                {
                    invln_Name = ConsortiumName,
                    invln_LeadPartner = new EntityReference(Account.EntityLogicalName, leadPartnerId),
                    invln_Programme = new EntityReference(invln_programme.EntityLogicalName, programmeId),
                    invln_Createdby = contact.ToEntityReference()
                };

                var consortiumId = _consortiumRepository.Create(consortium);
                ExecutionData.SetOutputParameter(invln_setconsortiumResponse.Fields.invln_consortiumid, consortiumId);
            }
        }
    }
}
