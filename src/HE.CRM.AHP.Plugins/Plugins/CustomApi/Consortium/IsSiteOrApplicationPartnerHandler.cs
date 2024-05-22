using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.Consortium;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.Consortium
{
    [CrmPluginRegistration(
    "invln_issiteorapplicationpartner",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.Consortium.IsSiteOrApplicationPartnerHandler: invln_issiteorapplicationpartner",
    1,
    IsolationModeEnum.Sandbox,
    Id = "37046931-d895-4a47-bf14-f855836ad5aa")]
    public class IsSiteOrApplicationPartnerHandler : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<IsSiteOrApplicationPartnerPlugin>());
        }
    }
}
