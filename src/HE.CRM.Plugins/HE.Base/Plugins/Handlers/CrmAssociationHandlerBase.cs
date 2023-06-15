using Microsoft.Xrm.Sdk.Client;
using HE.Base.Plugins.Common;
using HE.Base.Plugins.Common.PluginContexts;
using HE.Base.Repositories;
using HE.Base.Services;
using System;
using HE.Base.Log;

namespace HE.Base.Plugins.Handlers
{
    public abstract class CrmAssociationHandlerBase<TContext> : CrmHandlerBase<TContext>, ICrmWorkHandler
        where TContext : OrganizationServiceContext
    {
        protected new EntityReferencePluginExecutionContext ExecutionData { get; private set; }

        public abstract void DoWork();

        public override void Initialize(IServiceProvider serviceProvider, ICrmRepositoriesFactory crmRepositoriesFactory, ICrmServicesFactory crmServicesFactory, IBaseLogger logger, HandlerCache cache)
        {
            base.Initialize(serviceProvider, crmRepositoriesFactory, crmServicesFactory, logger, cache);
            this.ExecutionData = new EntityReferencePluginExecutionContext(base.ExecutionData.Context);
        }
    }
}
