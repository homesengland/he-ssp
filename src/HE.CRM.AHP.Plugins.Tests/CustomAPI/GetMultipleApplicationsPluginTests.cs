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
    public class GetMultipleApplicationsPluginTests
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
            var contactId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var appId_1 = Guid.NewGuid();
            var appId_2 = Guid.NewGuid();

            fakedContext.Initialize(new List<Entity>()
            {
                new Contact() { Id = contactId, invln_externalid = contactId.ToString()},
                new Account() { Id= accountId },
                new invln_scheme() { Id = appId_1, invln_organisationid = accountId.ToEntityReference<Account>(), invln_contactid = contactId.ToEntityReference<Contact>()},
                new invln_scheme() { Id = appId_2, invln_organisationid = accountId.ToEntityReference<Account>(), invln_contactid = contactId.ToEntityReference<Contact>()},
            });

            //   var request = new invln_changeahpapplicationstatusRequest();
            pluginContext.InputParameters = new ParameterCollection
            {
                { invln_getmultipleahpapplicationsRequest.Fields.invln_appfieldstoretrieve, null},
                { invln_getmultipleahpapplicationsRequest.Fields.inlvn_userid, contactId.ToString()},
                { invln_getmultipleahpapplicationsRequest.Fields.invln_organisationid, accountId.ToString()}
            };

            fakedContext.ExecutePluginWithConfigurations<GetMultipleApplicationsPlugin>(pluginContext, "", "");
            var outputResponce = pluginContext.OutputParameters[invln_getmultipleahpapplicationsResponse.Fields.invln_ahpapplications].ToString();
            Assert.IsNotNull(outputResponce);
            Assert.AreNotEqual("[]", outputResponce);
        }
    }
}
