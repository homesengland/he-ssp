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
        #endregion

        #region Constructors

        public ReviewsApprovalsService(CrmServiceArgs args) : base(args)
        {
            _reviewApprovalRepository = CrmRepositoriesFactory.Get<IReviewApprovalRepository>();
            _ispRepository = CrmRepositoriesFactory.Get<IIspRepository>();
        }

        public void UpdateIspRelatedToApprovalsService(invln_reviewapproval target, invln_reviewapproval postImage)
        {
            if (postImage != null && postImage.invln_ispid != null && postImage.invln_status != null)
            {
                var isp = new invln_ISP()
                {
                    Id = postImage.invln_ispid.Id
                };
                var reviewApprovals = _reviewApprovalRepository.GetReviewApprovalsForIsp(postImage.invln_ispid, null);
                switch(postImage.invln_status.Value)
                {
                    case (int)invln_StatusReviewApprovalSet.Rejected:
                    {
                        isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Rejected);
                        this._ispRepository.Update(isp);
                        break;
                    }
                    case (int)invln_StatusReviewApprovalSet.Pending:
                    {
                        var rejected = reviewApprovals.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Rejected);
                        if (rejected.Any())
                        {
                            isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Rejected);
                            this._ispRepository.Update(isp);
                        }
                        else
                        {
                            isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Pending);
                            this._ispRepository.Update(isp);
                        }
                        break;
                    }
                    case (int)invln_StatusReviewApprovalSet.SentBack:
                    {
                        var rejected = reviewApprovals.Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.Rejected);
                        if (rejected.Any())
                        {
                            isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Rejected);
                            this._ispRepository.Update(isp);
                        }
                        else
                        {
                            isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Pending);
                            this._ispRepository.Update(isp);
                        }
                        break;
                    }
                    case (int)invln_StatusReviewApprovalSet.Approved:
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
                        if (!pendingOrSendBack.Any())
                        {
                            isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Pending);
                            this._ispRepository.Update(isp);
                            break;
                        }
                        isp.invln_DateApproved = DateTime.UtcNow;
                        isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Approved);
                        this._ispRepository.Update(isp);
                        break;
                    }
                    case (int)invln_StatusReviewApprovalSet.Reviewed:
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
                        if (!pendingOrSendBack.Any())
                        {
                            isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Pending);
                            this._ispRepository.Update(isp);
                            break;
                        }
                        isp.invln_DateApproved = DateTime.UtcNow;
                        isp.invln_ApprovalStatus = new OptionSetValue((int)invln_ApprovalStatus.Approved);
                        this._ispRepository.Update(isp);
                        break;
                    }
                }
            }
        }
        #endregion
    }
}
