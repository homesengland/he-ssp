using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_getorganizationdetails",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.GetOrganizationDetailsPlugin: invln_getorganizationdetails",
    1,
    IsolationModeEnum.Sandbox,
    Id = "302988EC-B2D8-4F48-8CE3-547590BDF49C")]
    public class GetOrganizationDetailsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetOrganizationDetailsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetOrganizationDetailsHandler>());
        }
        #endregion
    }
}
