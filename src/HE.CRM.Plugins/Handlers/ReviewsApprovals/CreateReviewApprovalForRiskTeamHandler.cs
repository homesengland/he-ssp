using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Common.Extensions;
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
            return ValueChanged(invln_reviewapproval.Fields.invln_status)
                && (CurrentState.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved || CurrentState.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed);
        }

        public override void DoWork()
        {
            var teamRepository = CrmRepositoriesFactory.GetSystem<ITeamRepository>();
            TracingService.Trace("Get ISP");
            var isp = _ispRepository.GetById(CurrentState.invln_ispid, new string[] { invln_ISP.Fields.invln_ApprovalLevelNew });
            if (isp.invln_ApprovalLevelNew != null && isp.invln_ApprovalLevelNew.Value != (int)invln_ApprovalLevel.CRO && isp.invln_ApprovalLevelNew.Value != (int)invln_ApprovalLevel.CRODelegatedAuthority)
            {
                return;
            }

            var reviewApprovals = _reviewApprovalRepository.GetReviewApprovalsForIsp(CurrentState.invln_ispid, null).ToList();
            var desTeam = teamRepository.GetTeamByName("DES team");
            var hoFTeam = teamRepository.GetTeamByName("HoF Team");
            var riskTeam = teamRepository.GetTeamByName("Risk team");
            TracingService.Trace("Get latest Des RA");
            var latestDesApproval = reviewApprovals.Where(x => x.OwnerId.Id == desTeam.Id).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            var latestRA = reviewApprovals.Where(x => x.CreatedOn >= latestDesApproval.CreatedOn).ToList();

            TracingService.Trace("Get latest HoF");
            var hoFra = latestRA.Where(x => x.OwnerId.Id == hoFTeam.Id).FirstOrDefault();

            if (latestRA.Any(x => x.OwnerId.Id == riskTeam.Id))
                return;

            if (hoFra == null)
                return;

            if (ConditionForCROApproval(isp, latestDesApproval, hoFra))
            {
                TracingService.Trace("Create RA for CRO");
                CreateRA(isp, riskTeam, "CRO approval of ISP", new OptionSetValue((int)invln_reviewerapproverset.CROApproval));
            }

            if (ConditionForCRODelegatedApproval(isp, latestDesApproval, hoFra))
            {
                TracingService.Trace("Create RA for CRO Delegated Authority");
                CreateRA(isp, riskTeam, "CRO Delegated Authority approval of ISP", new OptionSetValue((int)invln_reviewerapproverset.CRODelegatedAuthorityApproval));
            }

        }

        private void CreateRA(invln_ISP isp, Team riskTeam, string descryption, OptionSetValue reviewer)
        {
            _reviewApprovalRepository.Create(
                new invln_reviewapproval()
                {
                    invln_ApprovalFor = new OptionSetValue((int)invln_ApprovalFor.ISP),
                    invln_Description = descryption,//"CRO approval of ISP",
                    invln_reviewerapprover = reviewer,//new OptionSetValue((int)invln_reviewerapproverset.CROApproval),
                    invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending),
                    invln_name = descryption,//"CRO approval of ISP",
                    OwnerId = riskTeam.ToEntityReference(),
                    invln_ispid = isp.ToEntityReference()
                }
                );
        }

        private static bool ConditionForCRODelegatedApproval(invln_ISP isp, invln_reviewapproval latestDesApproval, invln_reviewapproval hoFra)
        {
            return isp.invln_ApprovalLevelNew.Value == (int)invln_ApprovalLevel.CRODelegatedAuthority &&
                            ((hoFra.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved || hoFra.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed)
                            || (latestDesApproval.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved || latestDesApproval.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed));
        }

        private static bool ConditionForCROApproval(invln_ISP isp, invln_reviewapproval latestDesApproval, invln_reviewapproval hoFra)
        {
            return isp.invln_ApprovalLevelNew.Value == (int)invln_ApprovalLevel.CRO &&
                            ((hoFra.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved || hoFra.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed)
                            || (latestDesApproval.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved || latestDesApproval.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed));
        }
    }
}
