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
using HE.Common.IntegrationModel.PortalIntegrationModel;
using System.Text.Json;

namespace HE.CRM.AHP.Plugins.Tests.CustomApi
{
    [TestClass]
    public class CheckIfApplicationWithGivenNameExistsPluginTests
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
            var schemaId = Guid.NewGuid();
            var aplication = new AhpApplicationDto()
            {
                id = schemaId.ToString(),
                name = "abc_cba"
            };
            var organizationId = Guid.NewGuid();
            fakedContext.Initialize(new List<Entity>()
            {
                new invln_scheme()
                {
                    Id = schemaId,
                    invln_schemename = "abc_cba",
                    invln_organisationid = organizationId.ToEntityReference<Account>()
                },
                new Account() { Id = organizationId}
            });

            try
            {
                var request = new invln_checkifapplicationwithgivennameexistsRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {invln_checkifapplicationwithgivennameexistsRequest.Fields.invln_application,
                                                                                JsonSerializer.Serialize(aplication)},
                    {invln_checkifapplicationwithgivennameexistsRequest.Fields.invln_organisationid,
                                                                                organizationId.ToString() }
                };

                fakedContext.ExecutePluginWithConfigurations<CheckIfApplicationWithGivenNameExistsPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
            }
            var outputResponce = pluginContext.OutputParameters[invln_checkifapplicationwithgivennameexistsResponse.Fields.invln_applicationexists].ToString().ToLower() == "true";
            Assert.IsTrue(pluginContext.OutputParameters.Contains(invln_checkifapplicationwithgivennameexistsResponse.Fields.invln_applicationexists));
            Assert.IsTrue(outputResponce);
        }
    }
}
