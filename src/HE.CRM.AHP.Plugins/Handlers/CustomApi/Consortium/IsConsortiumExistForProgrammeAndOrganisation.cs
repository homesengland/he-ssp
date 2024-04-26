using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Consortium
{
    public class IsConsortiumExistForProgrammeAndOrganisation : CrmActionHandlerBase<invln_IsConsortiumExistForProgrammeAndOrganisationRequest, DataverseContext>
    {
        private string ProgrammeId => ExecutionData.GetInputParameter<string>(invln_IsConsortiumExistForProgrammeAndOrganisationRequest.Fields.invln_programmeid);
        private string OrganizationId => ExecutionData.GetInputParameter<string>(invln_IsConsortiumExistForProgrammeAndOrganisationRequest.Fields.invln_organisationid);

        private readonly IConsortiumRepository _consortiumRepository;
        private readonly IConsortiumMemberRepository _consortiumMemberRepository;

        public IsConsortiumExistForProgrammeAndOrganisation(IConsortiumRepository consortiumRepository,
                            IConsortiumMemberRepository consortiumMemberRepository)
        {
            _consortiumRepository = consortiumRepository;
            _consortiumMemberRepository = consortiumMemberRepository;
        }

        public override bool CanWork()
        {
            return ProgrammeId != null && OrganizationId != null &&
                    ProgrammeId != string.Empty && OrganizationId != string.Empty;
        }

        public override void DoWork()
        {
            if (Guid.TryParse(ProgrammeId, out var programmeId) && Guid.TryParse(OrganizationId, out var organizationId))
            {
                var isExist = _consortiumRepository.GetByProgrammeAndOrganization(programmeId, organizationId);
                ExecutionData.SetOutputParameter(invln_IsConsortiumExistForProgrammeAndOrganisationResponse.Fields.invln_isconsortiumexist, isExist);
            }
        }
    }
}
