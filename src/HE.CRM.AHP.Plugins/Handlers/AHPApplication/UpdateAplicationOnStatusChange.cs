using System.Runtime.Remoting.Messaging;
using DataverseModel;
using HE.Base.Plugins.Handlers;

namespace HE.CRM.AHP.Plugins.Handlers.AHPApplication
{
    public class UpdateAplicationOnStatusChange : CrmEntityHandlerBase<invln_scheme, DataverseContext>
    {
        public override bool CanWork()
        {
            return ValueChanged(invln_scheme.Fields.StatusCode) && CurrentState.StatusCode.Value == (int)invln_scheme_StatusCode.ReferredBackToApplicant;
        }

        public override void DoWork()
        {
            if (CurrentState.invln_representationsandwarrantiesconfirmation != true)
                return;
            ExecutionData.Target.invln_representationsandwarrantiesconfirmation = false;
        }
    }
}
