using System.Net.Http.Headers;
using DataverseModel;
using HE.Base.Common.Extensions;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Implementations;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Plugins.Services.ISPs;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Handlers.ISPs
{
    public class CreateRecordsOnSentForApprovalChangeHandler : CrmEntityHandlerBase<invln_ISP, DataverseContext>
    {
        private readonly IReviewApprovalRepository _reviewApprovalRepository;
        private readonly IIspRepository _ispRepository;

        public CreateRecordsOnSentForApprovalChangeHandler(IReviewApprovalRepository reviewApprovalRepository, IIspRepository ispRepository)
        {
            _reviewApprovalRepository = reviewApprovalRepository;
            _ispRepository = ispRepository;
        }

        public override bool CanWork()
        {
            TracingService.Trace("Can Work - CreateRecordsOnSentForApprovalChangeHandler");
            return ValueChanged(invln_ISP.Fields.invln_SendforApproval) && CurrentState.invln_SendforApproval == true;
        }

        public override void DoWork()
        {
            var oldRA = _reviewApprovalRepository.GetByAttribute(invln_reviewapproval.Fields.invln_ispid, CurrentState.invln_ISPId.Value, new string[] { invln_reviewapproval.Fields.Id });

            foreach (var ra in oldRA)
            {
                ra.invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.NotRequired);
                _reviewApprovalRepository.Update(ra);
            }

            invln_ISP toUpdate = new invln_ISP();
            toUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Pending);
            toUpdate.Id = CurrentState.Id;
            _ispRepository.Update(toUpdate);
            TracingService.Trace("Create Team repo");

            var teamRepository = CrmRepositoriesFactory.GetSystem<ITeamRepository>();
            TracingService.Trace("Get DES Team");
            var desTeam = teamRepository.GetTeamByName("DES Team");

            TracingService.Trace("Create DES Approval");
            _reviewApprovalRepository.Create(new invln_reviewapproval()
            {
                invln_ApprovalFor = new OptionSetValue((int)invln_ApprovalFor.ISP),
                invln_Description = "DES team review of ISP",
                invln_reviewerapprover = new OptionSetValue((int)invln_reviewerapproverset.DESReview),
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending),
                invln_name = "DES team review of ISP",
                invln_ispid = CurrentState.Id.ToEntityReference<invln_ISP>(),
                OwnerId = desTeam.ToEntityReference(),
            });

            if (CurrentState.invln_ApprovalLevelNew == null)
            {
                return;
            }

            if (CurrentState.invln_ApprovalLevelNew.Value == (int)invln_ApprovalLevel.HoF)
            {
                TracingService.Trace("Create HoF Approval");
                var hofTeam = teamRepository.GetTeamByName("HoF Team");
                _reviewApprovalRepository.Create(new invln_reviewapproval()
                {
                    invln_ApprovalFor = new OptionSetValue((int)invln_ApprovalFor.ISP),
                    invln_Description = "HoF team approval of ISP",
                    invln_reviewerapprover = new OptionSetValue((int)invln_reviewerapproverset.HoFApproval),
                    invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending),
                    invln_name = "HoF team approval of ISP",
                    invln_ispid = CurrentState.Id.ToEntityReference<invln_ISP>(),
                    OwnerId = hofTeam.ToEntityReference(),
                });
            }
            else if (CurrentState.invln_ApprovalLevelNew.Value == (int)invln_ApprovalLevel.CRODelegatedAuthority || CurrentState.invln_ApprovalLevelNew.Value == (int)invln_ApprovalLevel.CRO)
            {
                TracingService.Trace("Create CRO Approval");
                var hoFTeam = teamRepository.GetTeamByName("HoF Team");
                _reviewApprovalRepository.Create(new invln_reviewapproval()
                {
                    invln_ApprovalFor = new OptionSetValue((int)invln_ApprovalFor.ISP),
                    invln_Description = "HoF team approval of ISP",
                    invln_reviewerapprover = new OptionSetValue((int)invln_reviewerapproverset.HoFReview),
                    invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending),
                    invln_name = "HoF team approval of ISP",
                    invln_ispid = CurrentState.Id.ToEntityReference<invln_ISP>(),
                    OwnerId = hoFTeam.ToEntityReference(),
                });
            }
        }
    }
}
