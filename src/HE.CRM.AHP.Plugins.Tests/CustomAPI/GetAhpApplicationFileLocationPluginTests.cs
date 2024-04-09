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
    public class GetAhpApplicationFileLocationPluginTests
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
        public void Test_1()
        {
            var siteLocationId = Guid.NewGuid();
            var parentLocataionId = Guid.NewGuid();
            var mainLocationId = Guid.NewGuid();

            var app = new invln_scheme()
            {
                Id = Guid.NewGuid(),
            };

            fakedContext.Initialize(new List<Entity>()
            {
                { app},
                { new SharePointDocumentLocation() { Id = siteLocationId, RegardingObjectId = app.Id.ToEntityReference<invln_scheme>(), RelativeUrl = "bla3", ParentSiteOrLocation = parentLocataionId.ToEntityReference<SharePointDocumentLocation>() } },
                { new SharePointDocumentLocation() { Id = parentLocataionId, RelativeUrl = "bla2", ParentSiteOrLocation = mainLocationId.ToEntityReference<SharePointSite>() } },
                { new SharePointSite() { Id = mainLocationId, AbsoluteURL = "/bla1" } },
            });

            pluginContext.InputParameters = new ParameterCollection
            {
                { invln_getahpapplicationdocumentlocationRequest.Fields.invln_applicationid, app.Id.ToString()}
            };

            fakedContext.ExecutePluginWithConfigurations<GetAhpApplicationFileLocationPlugin>(pluginContext, "", "");
            var outputResponce = pluginContext.OutputParameters[invln_getahpapplicationdocumentlocationResponse.Fields.invln_documentlocation].ToString();
            Assert.AreEqual("bla3", outputResponce);
        }
    }
}
