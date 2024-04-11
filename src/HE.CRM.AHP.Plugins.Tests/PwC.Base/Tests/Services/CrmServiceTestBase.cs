using FakeXrmEasy;
using HE.Base.DependencyInjection;
using HE.Base.Log;
using HE.Base.Repositories;
using HE.Base.Services;
using Microsoft.Xrm.Sdk;
using Moq;
using System;

namespace PwC.Base.Tests.Services
{
    /// <summary>
    /// Base crm service test abstraact class. Contains methods and properties for fast crm services unit test development
    /// Every crm service test should inherit from this class.
    /// </summary>
    /// <typeparam name="TService">The type of the crm service.</typeparam>
    public class CrmServiceTestBase<TService>
        where TService : ICrmService
    {
        /// <summary>
        /// The crm service
        /// </summary>
        protected TService service;

        /// <summary>
        /// The Crm repositories factory based on fake organization service
        /// </summary>
        protected ICrmRepositoriesFactory repositoriesFactory;

        /// <summary>
        /// The organization service factory mock which returns faked organization service context.
        /// </summary>
        protected Mock<IOrganizationServiceFactory> organizationServiceFactoryMock;

        /// <summary>
        /// Faked crm context to mock data and messages executions
        /// </summary>
        protected XrmFakedContext fakedContext;

        /// <summary>
        /// Tracing service implementation
        /// </summary>
        protected ITracingService tracingService;

        /// <summary>
        /// Tracing service mock
        /// </summary>
        protected Mock<ITracingService> tracingServiceMock;

        /// <summary>
        /// The crm repositoreis factory mock injected into CrmHandlerFactory
        /// </summary>
        protected Mock<CrmRepositoriesFactory> crmRepositoriesFactoryMock;

        /// <summary>
        /// The crm service factory mock injected into CrmHandlerFactory
        /// </summary>
        protected Mock<CrmServicesFactory> crmServiceFactoryMock;

        /// <summary>
        /// Crm plugin service provider mock to inject all required mocks into handler
        /// </summary>
        protected Mock<IServiceProvider> serviceProviderMock;

        /// <summary>
        /// Faked crm plugin context gathered from faked crm context
        /// </summary>
        protected XrmFakedPluginExecutionContext fakedPluginContext;

        /// <summary>
        /// DI container
        /// </summary>
        protected IContainer container;

        /// <summary>
        /// Initializes all required test elements. Should be invoked in TestInitialize function.
        /// </summary>
        public virtual void InitializeTest(IContainer container = null)
        {
            this.container = container = container ?? new Container(); // Default implementation

            fakedContext = new XrmFakedContext();
            organizationServiceFactoryMock = new Mock<IOrganizationServiceFactory>();
            organizationServiceFactoryMock.Setup(of => of.CreateOrganizationService(It.IsAny<Guid?>())).Returns(fakedContext.GetOrganizationService());
            container.RegisterSingleton(() => organizationServiceFactoryMock.Object);

            fakedPluginContext = fakedContext.GetDefaultPluginContext();

            serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(s => s.GetService(typeof(IPluginExecutionContext))).Returns(fakedPluginContext);
            serviceProviderMock.Setup(s => s.GetService(typeof(ITracingService))).Returns(fakedContext.GetFakeTracingService());
            serviceProviderMock.Setup(s => s.GetService(typeof(IOrganizationServiceFactory))).Returns(organizationServiceFactoryMock.Object);

            // classes mock used to use default implementations
            tracingServiceMock = new Mock<ITracingService>();
            crmRepositoriesFactoryMock = new Mock<CrmRepositoriesFactory>(container);
            crmRepositoriesFactoryMock.CallBase = true;
            crmServiceFactoryMock = new Mock<CrmServicesFactory>(container);
            crmServiceFactoryMock.CallBase = true;

            repositoriesFactory = crmRepositoriesFactoryMock.Object;
            tracingService = tracingServiceMock.Object;

            // Container configuration
            container.RegisterSingleton(serviceProviderMock.Object);
            container.RegisterSingleton<ICrmRepositoriesFactory>(() => crmRepositoriesFactoryMock.Object);
            container.RegisterSingleton<ICrmServicesFactory>(() => crmServiceFactoryMock.Object);
            container.RegisterSingleton<ITracingService>(() => tracingServiceMock.Object);
            container.Register<CrmServiceArgs, CrmServiceArgs>();
            container.Register<CrmRepositoryArgs, CrmRepositoryArgs>();
            container.Register<IBaseLogger>(() => new BaseLogger(tracingServiceMock.Object, new LogSettings() { Level = LogLevel.Trace }));
        }

        /// <summary>
        /// Assets all required elements to ACT in the test, initializes service object. Should be invoked just before ACT (invoked on service) in the test.
        /// </summary>
        public virtual void Asset()
        {
            ICrmServicesFactory serviceFactory = new CrmServicesFactory(container);
            service = serviceFactory.Get<TService>();
        }
    }
}
