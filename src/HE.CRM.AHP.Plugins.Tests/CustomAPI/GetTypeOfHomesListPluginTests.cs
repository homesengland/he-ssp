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
    public class GetTypeOfHomesListPluginTests
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
            var homeTypeId = Guid.NewGuid();
            var appId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            fakedContext.Initialize(new List<Entity>()
            {
                { new invln_HomeType() { Id = homeTypeId, invln_application = appId.ToEntityReference<invln_HomeType>() } },
                { new invln_scheme() { Id = appId, invln_organisationid = organizationId.ToEntityReference<Account>(), invln_contactid =  userId.ToEntityReference<Contact>()} },
                { new Account() { Id = organizationId} },
                { new Contact() { Id = userId, invln_externalid = userId.ToString()} }
            });

            //   var request = new invln_changeahpapplicationstatusRequest();
            pluginContext.InputParameters = new ParameterCollection
            {
                { invln_gettypeofhomeslistRequest.Fields.invln_applicationid, appId.ToString()},
                { invln_gettypeofhomeslistRequest.Fields.invln_organisationid, organizationId.ToString()},
                { invln_gettypeofhomeslistRequest.Fields.invln_userid, userId.ToString()},
                { invln_gettypeofhomeslistRequest.Fields.invln_fieldstoretrieve, null}
            };

            fakedContext.ExecutePluginWithConfigurations<GetTypeOfHomesListPlugin>(pluginContext, "", "");
            var outputResponce = pluginContext.OutputParameters[invln_gettypeofhomeslistResponse.Fields.invln_hometypeslist].ToString();
            Assert.IsNotNull(outputResponce);
            Assert.AreNotEqual("[]", outputResponce);
        }
    }
}
