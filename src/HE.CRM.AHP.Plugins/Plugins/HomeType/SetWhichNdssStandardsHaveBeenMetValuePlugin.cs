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
      "HE.CRM.Plugins.Plugins.HomeType.SetWhichNdssStandardsHaveBeenMetValuePlugin: Create of Home Type",
      1,
      IsolationModeEnum.Sandbox,
      Id = "bdce071e-0b04-4db0-9664-6c7da22adfa6")]
    [CrmPluginRegistration(
      MessageNameEnum.Update,
      invln_HomeType.EntityLogicalName,
      StageEnum.PreOperation,
      ExecutionModeEnum.Synchronous,
      "invln_whichndssstandardshavebeenmet",
      "HE.CRM.Plugins.Plugins.HomeType.SetWhichNdssStandardsHaveBeenMetValuePlugin: Update of Home Type",
      1,
      IsolationModeEnum.Sandbox,
      Id = "aadeac61-ae7f-4714-96a1-5b18e87c4b0e")]
    public class SetWhichNdssStandardsHaveBeenMetValuePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SetWhichNdssStandardsHaveBeenMetValuePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SetWhichNdssStandardsHaveBeenMetValueHandler>());
            registeredHandlers.Add(handlerFactory.GetHandler<CalculatePercentageValueOfNDSSStandardHandler>());
        }
        #endregion
    }
}
