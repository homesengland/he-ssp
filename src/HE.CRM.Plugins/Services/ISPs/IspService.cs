using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using HE.CRM.Plugins.Services.LoanApplication;

namespace HE.CRM.Plugins.Services.ISPs
{
    public class IspService : CrmService, IIspService
    {
        #region Constructors

        public IspService(CrmServiceArgs args) : base(args)
        {
        }

        public void SetFieldsOnSentForApprovalChange(invln_ISP target)
        {
            if(target.invln_SendforApproval == true)
            {
                target.invln_DateSubmitted = DateTime.UtcNow;
                target.invln_DateSentforApproval = DateTime.UtcNow;
            }
        }

        #endregion
    }
}
