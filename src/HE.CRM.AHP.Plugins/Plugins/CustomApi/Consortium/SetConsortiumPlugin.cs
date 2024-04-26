using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.Consortium
{
    [CrmPluginRegistration(
        "invln_setconsortium",
        "none",
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "",
        "HE.CRM.Plugins.Plugins.CustomApi.Consortium.SetConsortiumPlugin: invln_setconsortium",
        1,
        IsolationModeEnum.Sandbox,
        Id = "ab252deb-112b-4e50-845e-cf6108654225")]
    public class SetConsortiumPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
        }
    }
}
