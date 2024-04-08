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
    public class ChangeAhpApplicationHandlerTests
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
        public void ChangeAhpApplicationHandler_CustomApi_Tests()
        {
            var schemaID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var accountId = Guid.NewGuid();

            fakedContext.Initialize(new List<Entity>()
            {
                new invln_scheme() { Id = schemaID, invln_organisationid = accountId.ToEntityReference<Account>(), invln_contactid = contactId.ToEntityReference<Contact>()},
                new Contact() { Id = contactId, invln_externalid = contactId.ToString()},
                new Account() { Id= accountId }
            });

            try
            {
                var request = new invln_changeahpapplicationstatusRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {invln_changeahpapplicationstatusRequest.Fields.invln_applicationid, schemaID.ToString() },  //invln_scheme
                    {invln_changeahpapplicationstatusRequest.Fields.invln_userid, contactId.ToString() },         //contact
                    {invln_changeahpapplicationstatusRequest.Fields.invln_organisationid, accountId.ToString() }, //account
                    {invln_changeahpapplicationstatusRequest.Fields.invln_newapplicationstatus, (int)invln_scheme_StatusCode.Approved },
                };

                fakedContext.ExecutePluginWithConfigurations<ChangeAhpApplicationStatusPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
            }

            var schema = fakedContext.CreateQuery<invln_scheme>().FirstOrDefault();
            Assert.IsNotNull(schema);
            Assert.AreEqual(expected: (int)invln_scheme_StatusCode.Approved, schema.invln_ExternalStatus.Value);
        }
    }
}
