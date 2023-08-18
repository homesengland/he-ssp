using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_searchorganizationbynameandcompanyhousename",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.SearchOrganizationByNameAndCompanyHouseName: invln_searchorganizationbynameandcompanyhousename",
    1,
    IsolationModeEnum.Sandbox,
    Id = "82f5dc99-e79f-4607-83da-eeba85732dd2")]
    public class SearchOrganizationByNameAndCompanyHouseNamePlugin : PluginBase<DataverseContext>, IPlugin
    {
        public SearchOrganizationByNameAndCompanyHouseNamePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SearchOrganizationByNameAndCompanyHouseNameHandler>());
        }
    }
}
