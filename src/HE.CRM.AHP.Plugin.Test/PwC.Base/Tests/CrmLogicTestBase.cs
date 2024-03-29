using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Moq;
using PwC.Base.DependencyInjection;
using PwC.Base.Log;
using PwC.Base.Repositories;
using PwC.Base.Services;
using System;

namespace PwC.Base.Tests
{
    /// <summary>
    /// Test base class for non-handler classes
    /// </summary>
    public class CrmLogicTestBase
    {
        /// <summary>
        /// Faked crm context to mock data and messages executions
        /// </summary>
        protected XrmFakedContext fakedContext;

        /// <summary>
        /// Faked crm plugin context gathered from faked crm context
        /// </summary>
        protected XrmFakedPluginExecutionContext fakedPluginContext;

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
        /// Tracing service mock
        /// </summary>
        protected Mock<XrmFakedTracingService> tracingServiceMock;

        /// <summary>
        /// DI container
        /// </summary>
        protected IContainer container;

        /// <summary>
        /// Initializes all required test elements. Should be invoked in TestInitialize function.
        /// </summary>
        public void InitializeTest(IContainer container)
        {
            this.container = container = container ?? new Container(); // Default implementation

            organizationServiceFactoryMock = new Mock<IOrganizationServiceFactory>();
            fakedContext = new XrmFakedContext();
            fakedPluginContext = fakedContext.GetDefaultPluginContext();
            organizationServiceFactoryMock.Setup(of => of.CreateOrganizationService(It.IsAny<Guid?>())).Returns(fakedContext.GetOrganizationService());

            // classes mock used to use default implementations
            tracingServiceMock = new Mock<XrmFakedTracingService>();
            tracingServiceMock.CallBase = true;
            crmRepositoriesFactoryMock = new Mock<CrmRepositoriesFactory>(container);
            crmRepositoriesFactoryMock.CallBase = true;
            crmServiceFactoryMock = new Mock<CrmServicesFactory>(container);
            crmServiceFactoryMock.CallBase = true;

            // Container configuration
            container.RegisterSingleton(() => organizationServiceFactoryMock.Object);
            container.RegisterSingleton<ICrmRepositoriesFactory>(() => crmRepositoriesFactoryMock.Object);
            container.RegisterSingleton<ICrmServicesFactory>(() => crmServiceFactoryMock.Object);
            container.RegisterSingleton<ITracingService>(() => tracingServiceMock.Object);
            container.Register<CrmServiceArgs, CrmServiceArgs>();
            container.Register<CrmRepositoryArgs, CrmRepositoryArgs>();
            container.Register<IBaseLogger>(() => new BaseLogger(tracingServiceMock.Object, new LogSettings() { Level = LogLevel.Trace }));
        }
    }
}
