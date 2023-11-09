using Microsoft.Xrm.Sdk.Client;
using HE.Base.DependencyInjection;
using HE.Base.Plugins.Common;
using HE.Base.Repositories;
using HE.Base.Services;
using System;
using HE.Base.Log;

namespace HE.Base.Plugins.Handlers
{
    /// <summary>
    /// Handlers factory generates and initializes handlers for crm plugins
    /// </summary>
    /// <typeparam name="TContext">Type of a Crm organization service context.</typeparam>
    public class CrmHandlerFactory<TContext>
        where TContext : OrganizationServiceContext
    {
        private readonly HandlerCache cache;
        private readonly ICrmRepositoriesFactory crmRepositoriesFactory;
        private readonly ICrmServicesFactory crmServicesFactory;
        private readonly IServiceProvider serviceProvider;
        private readonly IContainer container;
        private readonly IBaseLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrmHandlerFactory{TContext}"/> class.
        /// </summary>
        /// <param name="container">DI container instance.</param>
        /// <param name="cache">Handlers plugin cache object.</param>
        public CrmHandlerFactory(IContainer container, HandlerCache cache)
        {
            this.cache = cache;
            this.container = container;
            this.logger = container.Resolve<IBaseLogger>();
            this.serviceProvider = container.Resolve<IServiceProvider>(); ;
            this.crmRepositoriesFactory = container.Resolve<ICrmRepositoriesFactory>();
            this.crmServicesFactory = container.Resolve<ICrmServicesFactory>();
        }

        /// <summary>
        /// Creates and initialize a plugin handler. Everytime new handler is generated.
        /// </summary>
        /// <typeparam name="THandler">The type of the plugin handler. Must inherit <see cref="ICrmHandler"/> interface.</typeparam>
        /// <returns>Initialized plugin handler of a given type under <see cref="ICrmHandler"/> interface type.</returns>
        public ICrmHandler GetHandler<THandler>()
            where THandler : ICrmHandler
        {
            var logic = container.Resolve<THandler>();

            logic.Initialize(serviceProvider, crmRepositoriesFactory, crmServicesFactory, logger, cache);

            return logic;
        }
    }
}
