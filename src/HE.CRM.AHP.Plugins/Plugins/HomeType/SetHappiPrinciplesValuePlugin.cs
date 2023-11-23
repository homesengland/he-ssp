using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.HomeType;

namespace HE.CRM.AHP.Plugins.Plugins.HomeType
{
    [CrmPluginRegistration(
      MessageNameEnum.Create,
      invln_HomeType.EntityLogicalName,
      StageEnum.PreOperation,
      ExecutionModeEnum.Synchronous,
      "",
      "HE.CRM.Plugins.Plugins.HomeType.SetHappiPrinciplesValue: Create of Home Type",
      1,
      IsolationModeEnum.Sandbox,
      Id = "b7be7d66-7f22-4913-934f-7235af854992")]
    [CrmPluginRegistration(
      MessageNameEnum.Update,
      invln_HomeType.EntityLogicalName,
      StageEnum.PreOperation,
      ExecutionModeEnum.Synchronous,
      "invln_happiprinciples",
      "HE.CRM.Plugins.Plugins.HomeType.SetHappiPrinciplesValue: Update of Home Type",
      1,
      IsolationModeEnum.Sandbox,
      Id = "34c06d17-aee3-46de-9ab9-dac98c3b120c")]
    public class SetHappiPrinciplesValuePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SetHappiPrinciplesValuePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SetHappiPrinciplesValueHandler>());
        }
        #endregion
    }
}
