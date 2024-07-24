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

        #region Constructors

        public ReviewsApprovalsService(CrmServiceArgs args) : base(args)
        {
            _reviewApprovalRepository = CrmRepositoriesFactory.Get<IReviewApprovalRepository>();
            _ispRepository = CrmRepositoriesFactory.GetSystem<IIspRepository>();
        }

        public void UpdateIspRelatedToApprovalsService(invln_reviewapproval target, invln_reviewapproval postImage)
        {
            if (postImage != null && postImage.invln_ispid != null && postImage.invln_status != null)
            {
                var isp = _ispRepository.GetById(postImage.invln_ispid.Id, invln_ISP.Fields.invln_ApprovalLevelNew);

                var ispToUpdate = new invln_ISP()
                {
                    Id = postImage.invln_ispid.Id
                };
                var reviewApprovals = _reviewApprovalRepository.GetReviewApprovalsForIsp(postImage.invln_ispid, null)
                    .ToList().Where(x =>
                    x.invln_reviewedapprovedbyid != null
                    && x.invln_reviewerapprovercomments != null)
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
                        this._ispRepository.Update(ispToUpdate);
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
                            this._ispRepository.Update(ispToUpdate);
                            break;
                        }
                        var pendingOrSendBack = reviewApprovals.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Pending ||
                                             x.invln_status.Value == (int)invln_StatusReviewApprovalSet.SentBack);
                        if (pendingOrSendBack.Any())
                        {
                            ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Pending);
                            this._ispRepository.Update(ispToUpdate);
                            break;
                        }

                        var approved = reviewApprovals.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved ||
                                             x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed || x.invln_status.Value == (int)invln_StatusReviewApprovalSet.NotRequired).ToList();
                        if (approved.Count == reviewApprovals.Count)
                        {
                            ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Approved);
                            this._ispRepository.Update(ispToUpdate);
                            break;
                        }
                        break;
                    }
                }
            }
        }

        private void CheckAndChangeToAproved(List<invln_reviewapproval> reviewApprovals, invln_ISP ispToUpdate, invln_ISP isp)
        {
            var mostRecentRAList = FindMostRecentReviewApprovalRecords(reviewApprovals);

            var rejected = mostRecentRAList.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Rejected);
            if (rejected.Any())
            {
                ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Rejected);
                ispToUpdate.invln_DateApproved = null;
                this._ispRepository.Update(ispToUpdate);
                return;
            }
            var pendingOrSendBack = mostRecentRAList.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Pending ||
                                                         x.invln_status.Value == (int)invln_StatusReviewApprovalSet.SentBack);
            if (pendingOrSendBack.Any())
            {
                ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Pending);
                this._ispRepository.Update(ispToUpdate);
                return;
            }

            var teamRepository = CrmRepositoriesFactory.GetSystem<ITeamRepository>();
            var desTeam = teamRepository.GetTeamByName("DES Team");
            var hoFTeam = teamRepository.GetTeamByName("Hof Team");

            var lastDesApprowals = mostRecentRAList
            .Where(x => x.OwnerId.Id == desTeam.Id)
            .OrderByDescending(x => x.CreatedOn).FirstOrDefault();

            var lastHoFApprowals = mostRecentRAList
                .FirstOrDefault(x => x.OwnerId.Id == hoFTeam.Id && x.CreatedOn >= lastDesApprowals.CreatedOn);

            if (lastHoFApprowals != null
                && (lastDesApprowals.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved || lastDesApprowals.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed)
                && (lastHoFApprowals.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved)
                && isp.invln_ApprovalLevelNew.Value == (int)invln_ApprovalLevel.HoF)
            {
                ispToUpdate.invln_DateApproved = DateTime.UtcNow;
                ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Approved);
                _ispRepository.Update(ispToUpdate);
            }

            if (lastHoFApprowals != null
                && (lastDesApprowals.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved || lastDesApprowals.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed)
                && (lastHoFApprowals.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed)
                && isp.invln_ApprovalLevelNew.Value == (int)invln_ApprovalLevel.HoF)
            {
                ispToUpdate.invln_DateApproved = DateTime.UtcNow;
                ispToUpdate.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.InReview);
                _ispRepository.Update(ispToUpdate);
            }
            if (mostRecentRAList.All(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved || x.invln_status.Value == (int)invln_StatusReviewApprovalSet.NotRequired))
            {
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

        private static List<invln_reviewapproval> FindMostRecentReviewApprovalRecords(List<invln_reviewapproval> reviewApprovals)
        {
            var lastDesApprowals = reviewApprovals
                    .Where(x => x.invln_reviewerapprover == new OptionSetValue((int)invln_reviewerapproverset.DESReview))
                    .OrderByDescending(x => x.CreatedOn).FirstOrDefault();

            List<OptionSetValue> opsvList = new List<OptionSetValue>()
            {
                new OptionSetValue((int)invln_reviewerapproverset.DESReview),
                new OptionSetValue((int)invln_reviewerapproverset.HoFReview),
                new OptionSetValue((int)invln_reviewerapproverset.HoFApproval),
                new OptionSetValue((int)invln_reviewerapproverset.RiskApproval),
                new OptionSetValue((int)invln_reviewerapproverset.CRODelegatedAuthorityApproval),
                new OptionSetValue((int)invln_reviewerapproverset.CROApproval),
                new OptionSetValue((int)invln_reviewerapproverset.IPEApproval)
            };

            var mostRecentRAList = new List<invln_reviewapproval>();
            foreach (var opsv in opsvList)
            {
                var ra = reviewApprovals.FirstOrDefault(x =>
                x.CreatedOn >= lastDesApprowals.CreatedOn
                && x.invln_reviewerapprover == opsv);
                mostRecentRAList.Add(ra);
            };
            return mostRecentRAList;
        }

        #endregion Constructors
    }
}
