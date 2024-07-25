using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Handlers.ReviewsApprovals
{
    public class CreateReviewApprovalForRiskTeamHandler : CrmEntityHandlerBase<invln_reviewapproval, DataverseContext>
    {
        public readonly IIspRepository _ispRepository;
        public readonly IReviewApprovalRepository _reviewApprovalRepository;

        public CreateReviewApprovalForRiskTeamHandler(IIspRepository ispRepository, IReviewApprovalRepository reviewApprovalRepository)
        {
            _ispRepository = ispRepository;
            _reviewApprovalRepository = reviewApprovalRepository;
        }

        public override bool CanWork()
        {
            return ValueChanged(invln_reviewapproval.Fields.invln_status) && (CurrentState.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved || CurrentState.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed);
        }

        public override void DoWork()
        {
            var teamRepository = CrmRepositoriesFactory.GetSystem<ITeamRepository>();

            var isp = _ispRepository.GetById(CurrentState.invln_ispid, new string[] { invln_ISP.Fields.invln_ApprovalLevelNew });
            if (isp.invln_ApprovalLevelNew.Value != (int)invln_ApprovalLevel.CRO && isp.invln_ApprovalLevelNew.Value != (int)invln_ApprovalLevel.CRODelegatedAuthority)
            {
                return;
            }

            var reviewApprovals = _reviewApprovalRepository.GetReviewApprovalsForIsp(CurrentState.invln_ispid, null).ToList();
            var desTeam = teamRepository.GetTeamByName("DES team");
            var hoFTeam = teamRepository.GetTeamByName("Hof Team");

            var latestDesApproval = reviewApprovals.Where(x => x.OwnerId.Id == desTeam.Id).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            var latestRA = reviewApprovals.Where(x => x.CreatedOn >= latestDesApproval.CreatedOn);

            var hoFra = latestRA.Where(x => x.OwnerId.Id == hoFTeam.Id).FirstOrDefault();

            if (isp.invln_ApprovalLevelNew.Value == (int)invln_ApprovalLevel.CRO &&
                ((hoFra.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved || hoFra.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed)
                || (latestDesApproval.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved || latestDesApproval.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed)))
            {
                _reviewApprovalRepository.Create(
                    new invln_reviewapproval()
                    {
                        invln_ApprovalFor = new OptionSetValue((int)invln_app)
                    }
                    )
            }

        }
    }
}
