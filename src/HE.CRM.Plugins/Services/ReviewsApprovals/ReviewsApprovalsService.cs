using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Plugins.Services.LocalAuthority;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Services.ReviewsApprovals
{
    public class ReviewsApprovalsService : CrmService, IReviewsApprovalsService
    {
        #region Fields

        private readonly IReviewApprovalRepository _reviewApprovalRepository;

        private readonly IIspRepository _ispRepository;

        #endregion Fields

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
                var isp = new invln_ISP()
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
                        isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Rejected);
                        isp.invln_DateApproved = null;
                        this._ispRepository.Update(isp);
                        break;
                    }
                    case (int)invln_StatusReviewApprovalSet.Pending:
                    {
                        CheckAndChangeStatusToPending(isp, reviewApprovals);
                        break;
                    }
                    case (int)invln_StatusReviewApprovalSet.SentBack:
                    {
                        CheckAndChangeStatusToPending(isp, reviewApprovals);
                        break;
                    }
                    case (int)invln_StatusReviewApprovalSet.Approved:
                    {
                        CheckAndChangeToAproved(reviewApprovals, isp);
                        break;
                    }
                    case (int)invln_StatusReviewApprovalSet.Reviewed:
                    {
                        CheckAndChangeToAproved(reviewApprovals, isp);
                        break;
                    }
                    case (int)invln_StatusReviewApprovalSet.NotRequired:
                    {
                        var rejected = reviewApprovals.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Rejected);
                        if (rejected.Any())
                        {
                            isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Rejected);
                            this._ispRepository.Update(isp);
                            break;
                        }
                        var pendingOrSendBack = reviewApprovals.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Pending ||
                                             x.invln_status.Value == (int)invln_StatusReviewApprovalSet.SentBack);
                        if (pendingOrSendBack.Any())
                        {
                            isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Pending);
                            this._ispRepository.Update(isp);
                            break;
                        }

                        var approved = reviewApprovals.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved ||
                                             x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed || x.invln_status.Value == (int)invln_StatusReviewApprovalSet.NotRequired).ToList();
                        if (approved.Count == reviewApprovals.Count)
                        {
                            isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Approved);
                            this._ispRepository.Update(isp);
                            break;
                        }
                        break;
                    }
                }
            }
        }

        private void CheckAndChangeToAproved(List<invln_reviewapproval> reviewApprovals, invln_ISP isp)
        {
            var mostRecentRAList = FindMostRecentReviewApprovalRecords(reviewApprovals);

            var rejected = mostRecentRAList.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Rejected);
            if (rejected.Any())
            {
                isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Rejected);
                isp.invln_DateApproved = null;
                this._ispRepository.Update(isp);
                return;
            }
            var pendingOrSendBack = mostRecentRAList.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Pending ||
                                                         x.invln_status.Value == (int)invln_StatusReviewApprovalSet.SentBack);
            if (pendingOrSendBack.Any())
            {
                isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Pending);
                this._ispRepository.Update(isp);
                return;
            }

            var lastDesApprowals = reviewApprovals
                        .Where(x => x.invln_reviewerapprover == new OptionSetValue((int)invln_reviewerapproverset.DESReview))
                        .OrderByDescending(x => x.CreatedOn).FirstOrDefault();

            var lastHoFApprowals = reviewApprovals
            .Where(x => x.invln_reviewerapprover == new OptionSetValue((int)invln_reviewerapproverset.ho))
            .OrderByDescending(x => x.CreatedOn).FirstOrDefault();

            if (lastDesApprowals.invln_status.Value == (int)invln_StatusReviewApprovalSet.Approved
                || lastDesApprowals.invln_status.Value == (int)invln_StatusReviewApprovalSet.Reviewed)

                isp.invln_DateApproved = DateTime.UtcNow;
            isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Approved);
            this._ispRepository.Update(isp);
        }

        private void CheckAndChangeStatusToPending(invln_ISP isp, List<invln_reviewapproval> reviewApprovals)
        {
            var mostRecentRAList = FindMostRecentReviewApprovalRecords(reviewApprovals);

            var rejected = mostRecentRAList.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Rejected);
            if (rejected.Any())
            {
                isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Rejected);
                isp.invln_DateApproved = null;
                this._ispRepository.Update(isp);
            }
            else
            {
                isp.invln_DateApproved = null;
                isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Pending);
                this._ispRepository.Update(isp);
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
