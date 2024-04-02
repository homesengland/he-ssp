using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Handlers.SiteDetails;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.SiteDetails
{
    [CrmPluginRegistration(
       MessageNameEnum.Create,
       invln_SiteDetails.EntityLogicalName,
       StageEnum.PreOperation,
       ExecutionModeEnum.Synchronous,
       "",
       "HE.CRM.Plugins.Plugins.SiteDetails.FulfillRegionOnLocalAuthorityChange: Create of Site Details",
       1,
       IsolationModeEnum.Sandbox,
       Id = "a2886447-fec5-40e6-88c3-994a88855d41")]
    [CrmPluginRegistration(
       MessageNameEnum.Update,
       invln_SiteDetails.EntityLogicalName,
       StageEnum.PreOperation,
       ExecutionModeEnum.Synchronous,
       "invln_localauthorityid",
       "HE.CRM.Plugins.Plugins.SiteDetails.FulfillRegionOnLocalAuthorityChange: Update of Site Details",
       1,
       IsolationModeEnum.Sandbox,
       Id = "6580fbf7-1cda-4bff-b78f-75f0947a4fd9",
       Image1Name = "PreImage",
       Image1Attributes = "",
       Image1Type = ImageTypeEnum.PreImage)]
    public class PreUpdate : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors

        public PreUpdate(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        #endregion Constructors

        #region Base Methods Overrides

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<FulfillRegionOnLocalAuthorityChangeHandler>());
            registeredHandlers.Add(handlerFactory.GetHandler<UpdateLoansApplicationAfterStatusChange>());
        }

        #endregion Base Methods Overrides
    }
}
