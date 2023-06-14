using Microsoft.Xrm.Sdk;
using HE.Base.Log;
using HE.Base.Repositories;

namespace HE.Base.Services
{
    /// <summary>
    /// Basic Crm Service implementation abstract class. Each CrmService should inherit from this CrmService class.
    /// </summary>
    /// <seealso cref="HE.Base.Services.ICrmService" />
    public abstract class CrmService : ICrmService
    {
        /// <summary>
        /// Crm Repositories factory for gathering entity repositories
        /// </summary>
        protected readonly ICrmRepositoriesFactory CrmRepositoriesFactory;

        /// <summary>
        /// Crm Services factory
        /// </summary>
        protected readonly ICrmServicesFactory CrmServicesFactory;

        /// <summary>
        /// Tracing service
        /// </summary>
        protected readonly ITracingService TracingService;

        /// <summary>
        /// Logger
        /// </summary>
        protected readonly IBaseLogger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrmService"/> class.
        /// </summary>
        /// <param name="crmRepositoriesFactory">The CRM repositories factory.</param>
        /// <param name="tracingService">The tracing service implementation</param>
        public CrmService(CrmServiceArgs args)
        {
            this.CrmRepositoriesFactory = args.CrmRepositoriesFactory;
            this.CrmServicesFactory = args.CrmServicesFactory;
            this.TracingService = args.TracingService;
            this.Logger = args.Logger;
        }

        /// <summary>
        /// Trace and logs execution time of selected code block by using statement
        /// </summary>
        /// <returns>Disposable trace log context</returns>
        protected TraceLogContext TraceExecution()
        {
            return TraceLogContext.Create(new BaseLogger(TracingService, new LogSettings { Level = LogLevel.Info }));
        }
    }
}
