using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.Contract;

namespace HE.CRM.Plugins.Handlers.Contract
{
    public class RejectAddingWhenRelatedLoanInApprovedStatusHandler : CrmEntityHandlerBase<invln_contract, DataverseContext>
    {
        #region Fields

        private invln_contract target => ExecutionData.Target;
        private invln_contract preImage => ExecutionData.Context.PreEntityImages.ContainsKey(nameof(ExecutionData.PreImage)) ? ExecutionData.PreImage : null;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return target != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IContractService>().RejectAddingWhenRelatedLoanInApprovedStatus(target, preImage);
        }

        #endregion
    }
}
