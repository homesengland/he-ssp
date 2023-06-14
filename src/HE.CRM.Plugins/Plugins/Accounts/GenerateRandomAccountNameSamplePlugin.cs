using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.CRM.Plugins.Handlers.Accounts;

namespace HE.CRM.Plugins.Plugins.Accounts
{
    [CrmPluginRegistration(
       "Create",
       "account",
       StageEnum.PreOperation,
       ExecutionModeEnum.Synchronous,
       "",
       "HE.CRM.Plugins.Plugins.Accounts.GenerateRandomAccountSampleName: Create of account",
       1,
       IsolationModeEnum.Sandbox,
       Id = "417FFBDE-E263-4B1F-8221-91D7AA7DA669")]
    public class GenerateRandomAccountNameSamplePlugin : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GenerateRandomAccountNameSampleHandler>());
        }
    }
}
