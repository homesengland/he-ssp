using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Handlers.ISPs;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.ISPs
{
    [CrmPluginRegistration(
       MessageNameEnum.Create,
       invln_ISP.EntityLogicalName,
       StageEnum.PreOperation,
       ExecutionModeEnum.Synchronous,
       "",
       "HE.CRM.Plugins.Plugins.Isps.PopulateFieldsOnCreatePlugin: Create of Isp",
       1,
       IsolationModeEnum.Sandbox,
       Id = "39062a9c-37eb-4673-89aa-13eba1e4a33e")]
    public class PopulateFieldsOnCreatePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public PopulateFieldsOnCreatePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<PopulateFieldsOnCreateHandler>());
        }
        #endregion
    }
}
