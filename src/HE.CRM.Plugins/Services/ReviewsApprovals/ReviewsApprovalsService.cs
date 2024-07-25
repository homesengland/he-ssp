using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.Implementations;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Plugins.Services.LocalAuthority;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Services.ReviewsApprovals
{
    public class ReviewsApprovalsService : CrmService, IReviewsApprovalsService
    {

        private readonly IReviewApprovalRepository _reviewApprovalRepository;
        private readonly IIspRepository _ispRepository;

        public ReviewsApprovalsService(CrmServiceArgs args) : base(args)
        {
            _reviewApprovalRepository = CrmRepositoriesFactory.Get<IReviewApprovalRepository>();
            _ispRepository = CrmRepositoriesFactory.GetSystem<IIspRepository>();
        }

        public void UpdateIspRelatedToApprovalsService(invln_reviewapproval target, invln_reviewapproval postImage)
        {
            TracingService.Trace("UpdateIspRelatedToApprovalsService");
            if (postImage != null && postImage.invln_ispid != null && postImage.invln_status != null)
            {
                var isp = _ispRepository.GetById(postImage.invln_ispid.Id, invln_ISP.Fields.invln_ApprovalLevelNew);

                var ispToUpdate = new invln_ISP()
                {
                    Id = postImage.invln_ispid.Id
                };
                var reviewApprovals = _reviewApprovalRepository.GetReviewApprovalsForIsp(postImage.invln_ispid, null)
                    .ToList();
                if (reviewApprovals.Count == 0)
                {
                    return;
                }

                switch (postImage.invln_status.Value)
                {
                    case (int)invln_StatusReviewApprovalSet.Rejected:
                    {
                        ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Rejected);
                        ispToUpdate.invln_DateApproved = null;
                        _ispRepository.Update(ispToUpdate);
                        break;
                    }
                    case (int)invln_StatusReviewApprovalSet.Pending:
                    {
                        CheckAndChangeStatusToPending(ispToUpdate, reviewApprovals);
                        break;
                    }
                    case (int)invln_StatusReviewApprovalSet.SentBack:
                    {
                        CheckAndChangeStatusToPending(ispToUpdate, reviewApprovals);
                        break;
                    }
                    case (int)invln_StatusReviewApprovalSet.Approved:
                    {
                        CheckAndChangeToAproved(reviewApprovals, ispToUpdate, isp);
                        break;
                    }
                    case (int)invln_StatusReviewApprovalSet.Reviewed:
                    {
                        CheckAndChangeToAproved(reviewApprovals, ispToUpdate, isp);
                        break;
                    }
                    case (int)invln_StatusReviewApprovalSet.NotRequired:
                    {
                        var rejected = reviewApprovals.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Rejected);
                        if (rejected.Any())
                        {
                            ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Rejected);
                            _ispRepository.Update(ispToUpdate);
                            break;
                        }
                        var pendingOrSendBack = reviewApprovals.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Pending ||
                                             x.invln_status.Value == (int)invln_StatusReviewApprovalSet.SentBack);
                        if (pendingOrSendBack.Any())
                        {
                            ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Pending);
                            _ispRepository.Update(ispToUpdate);
                            break;
                        }

                        var approved = reviewApprovals.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved ||
                                             x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed || x.invln_status.Value == (int)invln_StatusReviewApprovalSet.NotRequired).ToList();
                        if (approved.Count == reviewApprovals.Count)
                        {
                            ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Approved);
                            _ispRepository.Update(ispToUpdate);
                            break;
                        }
                        break;
                    }
                }
            }
        }

        private void CheckAndChangeToAproved(List<invln_reviewapproval> reviewApprovals, invln_ISP ispToUpdate, invln_ISP isp)
        {
            TracingService.Trace("CheckAndChangeToAproved");
            var mostRecentRAList = FindMostRecentReviewApprovalRecords(reviewApprovals);

            TracingService.Trace("Check Rejected");
            var rejected = mostRecentRAList.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Rejected);
            if (rejected.Any())
            {
                ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Rejected);
                ispToUpdate.invln_DateApproved = null;
                _ispRepository.Update(ispToUpdate);
                return;
            }
            TracingService.Trace("Check Send Back");
            var pendingOrSendBack = mostRecentRAList.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Pending ||
                                                         x.invln_status.Value == (int)invln_StatusReviewApprovalSet.SentBack);
            if (pendingOrSendBack.Any())
            {
                ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Pending);
                _ispRepository.Update(ispToUpdate);
                return;
            }
            TracingService.Trace("Get Teams");
            var teamRepository = CrmRepositoriesFactory.GetSystem<ITeamRepository>();
            var desTeam = teamRepository.GetTeamByName("DES team");
            var hoFTeam = teamRepository.GetTeamByName("Hof Team");

            var lastDesApprowals = mostRecentRAList
            .Where(x => x.OwnerId.Id == desTeam.Id)
            .OrderByDescending(x => x.CreatedOn.Value).FirstOrDefault();

            var lastHoFApprowals = mostRecentRAList
                .FirstOrDefault(x => x.OwnerId.Id == hoFTeam.Id && x.CreatedOn.Value >= lastDesApprowals.CreatedOn.Value);
            if (lastHoFApprowals != null
                && (lastDesApprowals.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved || lastDesApprowals.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed)
                && (lastHoFApprowals.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved)
                && isp.invln_ApprovalLevelNew.Value == (int)invln_ApprovalLevel.HoF)
            {
                TracingService.Trace("Approved");
                ispToUpdate.invln_DateApproved = DateTime.UtcNow;
                ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Approved);
                _ispRepository.Update(ispToUpdate);
            }

            if (lastHoFApprowals != null
                && (lastDesApprowals.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved || lastDesApprowals.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed)
                && (lastHoFApprowals.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed)
                && isp.invln_ApprovalLevelNew.Value == (int)invln_ApprovalLevel.HoF)
            {
                TracingService.Trace("Reviewed");
                ispToUpdate.invln_DateApproved = DateTime.UtcNow;
                ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.InReview);
                _ispRepository.Update(ispToUpdate);
            }

            if (mostRecentRAList.All(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved || x.invln_status.Value == (int)invln_StatusReviewApprovalSet.NotRequired))
            {
                TracingService.Trace("Approved or not Required");
                ispToUpdate.invln_DateApproved = DateTime.UtcNow;
                ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Approved);
                _ispRepository.Update(ispToUpdate);
            }
        }

        private void CheckAndChangeStatusToPending(invln_ISP isp, List<invln_reviewapproval> reviewApprovals)
        {
            var mostRecentRAList = FindMostRecentReviewApprovalRecords(reviewApprovals);

            var rejected = mostRecentRAList.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Rejected);
            if (rejected.Any())
            {
                isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Rejected);
                isp.invln_DateApproved = null;
                _ispRepository.Update(isp);
            }
            else
            {
                isp.invln_DateApproved = null;
                isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Pending);
                _ispRepository.Update(isp);
            }
        }

        private List<invln_reviewapproval> FindMostRecentReviewApprovalRecords(List<invln_reviewapproval> reviewApprovals)
        {
            TracingService.Trace(reviewApprovals.Count.ToString());

            var lastDesApprowals = reviewApprovals
                    .Where(x => x.invln_reviewerapprover.Value == (int)invln_reviewerapproverset.DESReview)
                    .OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            TracingService.Trace(lastDesApprowals.CreatedOn.ToString());

            var mostRecentRAList = reviewApprovals.Where(x =>
            x.CreatedOn.Value >= lastDesApprowals.CreatedOn.Value).ToList();
            TracingService.Trace(mostRecentRAList.Count.ToString());
            return mostRecentRAList;
        }
    }
}
