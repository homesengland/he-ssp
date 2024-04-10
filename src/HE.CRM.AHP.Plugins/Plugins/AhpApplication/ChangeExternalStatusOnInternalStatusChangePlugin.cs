using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.AHPApplication;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.AhpApplication
{
    [CrmPluginRegistration(
       MessageNameEnum.Update,
       invln_scheme.EntityLogicalName,
       StageEnum.PreOperation,
       ExecutionModeEnum.Synchronous,
       "statuscode," + invln_scheme.Fields.invln_Site,
       "HE.CRM.AHP.Plugins.Plugins.AhpApplication.ChangeExternalStatusOnInternalStatusChangePlugin: Update of AHP Application",
       1,
       IsolationModeEnum.Sandbox,
       Id = "3FF1ED1C-DB8E-4D07-8966-232C74189FD5",
       Image1Name = "PreImage", Image1Attributes = "",
       Image1Type = ImageTypeEnum.PreImage)]
    public class ChangeExternalStatusOnInternalStatusChangePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors

        public ChangeExternalStatusOnInternalStatusChangePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        #endregion Constructors

        #region Base Methods Overrides

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<ChangeExternalStatusOnInternalStatusChangeHandler>());
            registeredHandlers.Add(handlerFactory.GetHandler<UpdateLocalAuthorityWhenSiteIsChanged>());
            registeredHandlers.Add(handlerFactory.GetHandler<RecalculateDeliveryphaseHandler>());
        }

        #endregion Base Methods Overrides
    }
}
