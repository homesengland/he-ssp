using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Handlers.ISPs;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using DataverseModel;

namespace HE.CRM.Plugins.Plugins.ISPs
{
    [CrmPluginRegistration(
       MessageNameEnum.Create,
       invln_ISP.EntityLogicalName,
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "",
       "HE.CRM.Plugins.Plugins.Isps.CreateConditionRecordsOnCreate: Create of Isp",
       1,
       IsolationModeEnum.Sandbox,
       Id = "61f741fd-290b-41eb-88fa-9ebcb2e615f5")]
    public class CreateConditionRecordsOnCreate : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public CreateConditionRecordsOnCreate(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CreateConditionRecordsOnHandler>());
        }
        #endregion
    }
}
