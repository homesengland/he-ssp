using DataverseModel;
using FakeItEasy;
using FakeXrmEasy;
using HE.CRM.AHP.Plugins.Plugins.CustomApi;
using HE.Base.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HE.CRM.AHP.Plugins.Tests.CustomApi
{
    [TestClass]
    public class GetAbsoluteAhpApplicationFileLocationPluginTests
    {
        private XrmFakedContext fakedContext;

        private XrmFakedPluginExecutionContext pluginContext;

        [TestInitialize]
        public void Initialize()
        {
            fakedContext = new XrmFakedContext();
            pluginContext = fakedContext.GetDefaultPluginContext();
        }

        [TestMethod]
        public void GetAbsolutepluginWithConfigutation_Test()
        {
            var siteLocationId = Guid.NewGuid();
            var parentLocataionId = Guid.NewGuid();
            var mainLocationId = Guid.NewGuid();

            var application = new invln_scheme()
            {
                Id = Guid.NewGuid(),
            };

            fakedContext.Initialize(new List<Entity>()
            {
                application,
                new SharePointDocumentLocation()
                {
                    Id = siteLocationId,
                    RegardingObjectId = application.Id.ToEntityReference<invln_scheme>(),
                    RelativeUrl = "bla3",
                    ParentSiteOrLocation = parentLocataionId.ToEntityReference<SharePointDocumentLocation>()
                },
                new SharePointDocumentLocation()
                {
                    Id = parentLocataionId,
                    RelativeUrl = "bla2",
                    ParentSiteOrLocation = mainLocationId.ToEntityReference<SharePointSite>()
                },
                new SharePointSite()
                {
                    Id = mainLocationId,
                    AbsoluteURL = "/bla1"
                },
            });
            pluginContext.InputParameters = new ParameterCollection
                {
                    {invln_getabsoluteahpapplicationfilelocationRequest.Fields.invln_applicationid,
                                                                                application.Id.ToString() }
                };

            fakedContext.ExecutePluginWithConfigurations<GetAbsoluteAhpApplicationFileLocationPlugin>(pluginContext, "", "");
            var outputResponce = pluginContext.OutputParameters[invln_getabsoluteahpapplicationfilelocationResponse.Fields.invln_documentlocation].ToString();
            Assert.AreEqual("/bla1/bla2/bla3", outputResponce);
        }

        [TestMethod]
        public void GetAbsolutepluginWithConfigutation_Test2()
        {
            var siteLocationId = Guid.NewGuid();
            var parentLocataionId = Guid.NewGuid();
            var mainLocationId = Guid.NewGuid();

            var application = new invln_scheme()
            {
                Id = Guid.NewGuid(),
            };

            fakedContext.Initialize(new List<Entity>()
            {
                application,
                new SharePointDocumentLocation()
                {
                    Id = siteLocationId,
                    RegardingObjectId = application.Id.ToEntityReference<invln_scheme>(),
                    RelativeUrl = "bla3", ParentSiteOrLocation = parentLocataionId.ToEntityReference<SharePointDocumentLocation>()
                },
                new SharePointDocumentLocation()
                {
                    Id = parentLocataionId,
                    RelativeUrl = "bla2",
                },
                new SharePointSite() { Id = mainLocationId, AbsoluteURL = "/bla1"
                },
            });
            pluginContext.InputParameters = new ParameterCollection
                {
                    {invln_getabsoluteahpapplicationfilelocationRequest.Fields.invln_applicationid, application.Id.ToString() }
                };

            fakedContext.ExecutePluginWithConfigurations<GetAbsoluteAhpApplicationFileLocationPlugin>(pluginContext, "", "");
            var outputResponce = pluginContext.OutputParameters[invln_getabsoluteahpapplicationfilelocationResponse.Fields.invln_documentlocation].ToString();
            Assert.AreEqual("bla3", outputResponce);
        }
    }
}
