using System;
using DataverseModel;
using HE.Base.Plugins.Handlers;

namespace HE.CRM.AHP.Plugins.Handlers.AHPApplication
{
    public class CheckUniqueApplicationNameHandler : CrmValidationHandlerBase<invln_scheme, DataverseContext>
    {
        public override string ViolationMessage => "Application with new name already exists.";

        public override bool CanWork()
        {
            return ExecutionData.Target != null && !string.IsNullOrEmpty(ExecutionData.Target.invln_schemename);
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
