using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Handlers.ISPs;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Plugins.ISPs
{
    [CrmPluginRegistration(
   MessageNameEnum.Update,
   invln_ISP.EntityLogicalName,
   StageEnum.PostOperation,
   ExecutionModeEnum.Synchronous,
   "",
   "HE.CRM.Plugins.Plugins.Isps.PostAssign: Assign of Isp",
   1,
   IsolationModeEnum.Sandbox,
   Id = "9404d9e5-cdf5-4045-b8ae-b57c681b6a3a",
   Image1Name = "PreImage",
   Image1Attributes =
    invln_ISP.Fields.OwnerId + "," +
    invln_ISP.Fields.invln_Loanapplication,
   Image1Type = ImageTypeEnum.PreImage)]
    public class PostAssign : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SendEmailEndAddNotificationOnOwnerChangeHandler>());
        }
    }
}
