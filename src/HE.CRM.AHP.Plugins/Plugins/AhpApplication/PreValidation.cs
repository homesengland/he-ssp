using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.AHPApplication;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.AhpApplication
{
    public class PreValidation : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            //registeredHandlers.Add(handlerFactory.GetHandler<CheckUniqueApplicationNameHandler>());
        }
    }
}
