using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;

namespace HE.CRM.Plugins.Services.ReviewsApprovals
{

    public interface IReviewsApprovalsService : ICrmService
    {
        void UpdateIspRelatedToApprovalsService(invln_reviewapproval target, invln_reviewapproval postImage);
    }
}
