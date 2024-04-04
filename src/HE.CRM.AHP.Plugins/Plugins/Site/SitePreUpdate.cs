using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.Site;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.Site
{
    [CrmPluginRegistration(
        MessageNameEnum.Update,
        invln_Sites.EntityLogicalName,
        StageEnum.PreOperation,
        ExecutionModeEnum.Synchronous,
        "invln_localauthority",
        "HE.CRM.Plugins.Plugins.Site.PreOpreation: Site Update",
        1,
        IsolationModeEnum.Sandbox,
        Id = "d5e2f900-7190-4030-86b4-d95a9d3065ea",
        Image1Name = "PreImage",
        Image1Attributes = invln_Sites.Fields.invln_LocalAuthority,
        Image1Type = ImageTypeEnum.PreImage)]
    public class SitePreUpdate : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<UpdateLocalAuthorityOnSiteHandler>());
        }
    }
}
