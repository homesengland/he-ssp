using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Services.Application;

namespace HE.CRM.AHP.Plugins.Handlers.CustomApi.Allocation
{
    public class CreateAllocationHandler : CrmActionHandlerBase<invln_createahpallocationRequest, DataverseContext>
    {
        #region Fields

        private Guid ApplicationId => ExecutionData.GetInputParameter<Guid>(invln_createahpallocationRequest.Fields.invln_applicationid);

        #endregion

        public override bool CanWork()
        {
            return Guid.Empty != ApplicationId;
        }

        public override void DoWork()
        {
            Logger.Trace($"{nameof(CreateAllocationHandler)}.{nameof(DoWork)}");

            Logger.Trace($"ApplicationId: {ApplicationId}");

            var allocationId = CrmServicesFactory.Get<IAllocationService>().CreateAllocation(ApplicationId);

            ExecutionData.SetOutputParameter(invln_createahpallocationResponse.Fields.invln_allocationid, allocationId);
        }
    }
}
