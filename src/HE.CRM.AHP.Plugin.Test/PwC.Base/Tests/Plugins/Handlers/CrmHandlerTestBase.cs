using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Moq;
using PwC.Base.DependencyInjection;
using PwC.Base.Log;
using HE.Base.Plugins.Attributes;
using HE.Base.Plugins.Common;
using HE.Base.Plugins.Common.Constants;
using HE.Base.Plugins.Handlers;
using PwC.Base.Repositories;
using PwC.Base.Services;
using System;

namespace PwC.Base.Tests.Plugins.Handlers
{
    /// <summary>
    /// Base handler test abstraact class. Contains methods and properties for fast plugins handlers unit test development
    /// Every handler test should inherit from this class.
    /// </summary>
    /// <typeparam name="THandler">The type of the crm plugin handler. Must implement ICrmHandler interface</typeparam>
    /// <typeparam name="TContext">The type of the context inherited from OrganizationServiceCotnext.</typeparam>
    public abstract class CrmHandlerTestBase<THandler, TContext>
        where THandler : ICrmHandler
        where TContext : OrganizationServiceContext
    {
        /// <summary>
        /// Crm Handler factory object with preinitialized fake context and mocks.
        /// </summary>
        protected CrmHandlerFactory<TContext> handlerFactory;

        /// <summary>
        /// Faked crm context to mock data and messages executions
        /// </summary>
        protected XrmFakedContext fakedContext;

        /// <summary>
        /// Faked crm plugin context gathered from faked crm context
        /// </summary>
        protected XrmFakedPluginExecutionContext fakedPluginContext;

        /// <summary>
        /// Crm plugin service provider mock to inject all required mocks into handler
        /// </summary>
        protected Mock<IServiceProvider> serviceProviderMock;

        /// <summary>
        /// The organization service factory mock injected into service provider gathered from crm faked context
        /// </summary>
        protected Mock<IOrganizationServiceFactory> organizationServiceFactoryMock;

        /// <summary>
        /// The crm service factory mock injected into CrmHandlerFactory
        /// </summary>
        protected Mock<CrmServicesFactory> crmServiceFactoryMock;

        /// <summary>
        /// The crm repositoreis factory mock injected into CrmHandlerFactory
        /// </summary>
        protected Mock<CrmRepositoriesFactory> crmRepositoriesFactoryMock;

        /// <summary>
        /// Test class handler
        /// </summary>
        protected THandler handler;

        /// <summary>
        /// Tracing service mock
        /// </summary>
        protected Mock<ITracingService> tracingServiceMock;

        /// <summary>
        /// DI container mock
        /// </summary>
        protected IContainer container;

        /// <summary>
        /// Initializes all required test elements. Should be invoked in TestInitialize function.
        /// </summary>
        public virtual void InitializeTest(IContainer container = null)
        {
            this.container = container = container ?? new Container(); // Default implementation

            serviceProviderMock = new Mock<IServiceProvider>();
            organizationServiceFactoryMock = new Mock<IOrganizationServiceFactory>();
            container.RegisterSingleton(() => organizationServiceFactoryMock.Object);

            fakedContext = new XrmFakedContext();
            fakedPluginContext = fakedContext.GetDefaultPluginContext();
            organizationServiceFactoryMock.Setup(of => of.CreateOrganizationService(It.IsAny<Guid?>())).Returns(fakedContext.GetOrganizationService());

            tracingServiceMock = new Mock<ITracingService>();

            serviceProviderMock.Setup(s => s.GetService(typeof(IPluginExecutionContext))).Returns(fakedPluginContext);
            serviceProviderMock.Setup(s => s.GetService(typeof(ITracingService))).Returns(tracingServiceMock.Object);
            serviceProviderMock.Setup(s => s.GetService(typeof(IOrganizationServiceFactory))).Returns(organizationServiceFactoryMock.Object);

            // classes mock used to use default implementations
            crmRepositoriesFactoryMock = new Mock<CrmRepositoriesFactory>(container);
            crmRepositoriesFactoryMock.CallBase = true;
            crmServiceFactoryMock = new Mock<CrmServicesFactory>(container);
            crmServiceFactoryMock.CallBase = true;

            // Container configuration
            container.RegisterSingleton(serviceProviderMock.Object);
            container.RegisterSingleton<ICrmRepositoriesFactory>(() => crmRepositoriesFactoryMock.Object);
            container.RegisterSingleton<ICrmServicesFactory>(() => crmServiceFactoryMock.Object);
            container.RegisterSingleton<ITracingService>(() => tracingServiceMock.Object);
            container.Register<IOrganizationService>(() => organizationServiceFactoryMock.Object.CreateOrganizationService(Guid.Empty));
            container.Register<CrmServiceArgs, CrmServiceArgs>();
            container.Register<CrmRepositoryArgs, CrmRepositoryArgs>();
            container.Register<IBaseLogger>(() => new BaseLogger(tracingServiceMock.Object, new LogSettings() { Level = LogLevel.Trace }));
        }

        /// <summary>
        /// Assets all required elements to ACT in the test, initializes handler object. Should be invoked just before ACT (invoked on handler) in the test.
        /// </summary>
        /// <param name="messageName">Execution pipeline message name. Can be passed from messagesnameshelper class</param>
        /// <param name="messageStage">Execution pipeline message stage. Can be passed from stages enum</param>
        /// <param name="cache">Handler cache. It's passed across handlers within specified plugin</param>
        public virtual void Asset(string messageName, int messageStage, HandlerCache cache = null)
        {
            fakedPluginContext.MessageName = messageName;
            fakedPluginContext.Stage = messageStage;
            handlerFactory = new CrmHandlerFactory<TContext>((HE.Base.DependencyInjection.IContainer)container, cache ?? new HandlerCache());
            handler = (THandler)handlerFactory.GetHandler<THandler>();
        }

        /// <summary>
        /// Assets all required elements to ACT in the test, initializes handler object. Should be invoked just before ACT (invoked on handler) in the test.
        /// </summary>
        /// <param name="messageName">Execution pipeline message name. Can be passed from messagesnameshelper class</param>
        /// <param name="messageStage">Execution pipeline message stage.</param>
        /// <param name="cache">Handler cache. It's passed across handlers within specified plugin</param>
        public virtual void Asset(string messageName, CrmProcessingStepStages messageStage, HandlerCache cache = null)
        {
            Asset(messageName, (int)messageStage, cache);
        }

        /// <summary>
        /// Assets all required elements to ACT in the test, initializes handler object. Should be invoked just before ACT (invoked on handler) in the test.
        /// </summary>
        /// <param name="message">Execution pipeline message</param>
        /// <param name="messageStage">Execution pipeline message stage.</param>
        /// <param name="cache">Handler cache. It's passed across handlers within specified plugin</param>
        public virtual void Asset(CrmMessage message, CrmProcessingStepStages messageStage, HandlerCache cache = null)
        {
            Asset(message.ToString(), (int)messageStage, cache);
        }

        /// <summary>
        /// Checks handler filters and run canwork
        /// </summary>
        /// <returns>True if filters and canwork returns true</returns>
        protected bool CanWorkWithFilters()
        {
            var fulfilled = FilterAttributesHelper.AreFiltersFulfilled(typeof(THandler), this.fakedPluginContext);
            return fulfilled && handler.CanWork();
        }
    }
}
