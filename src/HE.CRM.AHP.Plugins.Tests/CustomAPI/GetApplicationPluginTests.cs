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
    public class GetApplicationPluginTests
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
            var appId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();

            fakedContext.Initialize(new List<Entity>()
            {
                new invln_scheme()
                {
                    Id = appId,
                    invln_organisationid = organizationId.ToEntityReference<Account>(),
                    invln_contactid = userId.ToEntityReference<Contact>()
                },
                new Contact()
                {
                    Id = userId,
                    invln_externalid = userId.ToString()
                },
                new Account()
                {
                    Id= organizationId
                }
            });

            pluginContext.InputParameters = new ParameterCollection
            {
                { invln_getahpapplicationRequest.Fields.invln_applicationid, appId.ToString()},
                { invln_getahpapplicationRequest.Fields.invln_appfieldstoretrieve, null},
                { invln_getahpapplicationRequest.Fields.invln_userid, userId.ToString()},
                { invln_getahpapplicationRequest.Fields.invln_organisationid, organizationId.ToString()}
            };

            fakedContext.ExecutePluginWithConfigurations<GetApplicationPlugin>(pluginContext, "", "");
            var outputResponce = pluginContext.OutputParameters[invln_getahpapplicationResponse.Fields.invln_retrievedapplicationfields].ToString();
            Assert.IsNotNull(outputResponce);
            Assert.AreNotEqual(string.Empty, outputResponce);
        }
    }
}
