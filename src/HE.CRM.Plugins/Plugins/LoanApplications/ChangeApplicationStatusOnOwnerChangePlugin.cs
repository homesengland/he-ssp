using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.Plugins.Handlers.LoanApplications;

namespace HE.CRM.Plugins.Plugins.LoanApplications
{
    [CrmPluginRegistration(
       MessageNameEnum.Update,
       invln_Loanapplication.EntityLogicalName,
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "ownerid",
       "HE.CRM.Plugins.Plugins.LoanApplications.ChangeApplicationStatusOnOwnerChangePlugin: Update of Loan Application",
       1,
       IsolationModeEnum.Sandbox,
       Id = "08CC9FF8-2C59-46A0-9209-DBD392558BC0",
       Image1Name = "PreImage",
       Image1Attributes = "ownerid,statuscode,invln_externalstatus",
       Image1Type = ImageTypeEnum.PreImage,
       Image2Name = "PostImage",
       Image2Attributes = "ownerid,statuscode,invln_externalstatus",
       Image2Type = ImageTypeEnum.PostImage)]

    public class ChangeApplicationStatusOnOwnerChangePlugin : PluginBase<DataverseContext>, IPlugin
    {
        public ChangeApplicationStatusOnOwnerChangePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<ChangeApplicationStatusOnOwnerChangeHandler>());
        }
    }
}
