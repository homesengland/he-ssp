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
       "HE.CRM.Plugins.Plugins.Isps.SetFieldsOnSentForApprovalChangePlugin: Create of Isp",
       1,
       IsolationModeEnum.Sandbox,
       Id = "fc06f2b3-c285-4f49-a8df-fbb79d9464a4")]
    [CrmPluginRegistration(
       MessageNameEnum.Update,
       invln_ISP.EntityLogicalName,
       StageEnum.PreOperation,
       ExecutionModeEnum.Synchronous,
       "invln_sendforapproval",
       "HE.CRM.Plugins.Plugins.Isps.SetFieldsOnSentForApprovalChangePlugin: Update of Isp",
       1,
       IsolationModeEnum.Sandbox,
       Id = "4b90f53b-ee13-4a50-abff-271ea8c12a92")]
    public class SetFieldsOnSentForApprovalChangePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SetFieldsOnSentForApprovalChangePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SetFieldsOnSentForApprovalChangeHandler>());
        }
        #endregion
    }
}
